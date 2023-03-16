using CardsWithoutCommonSense.Models;
using CardsWithoutCommonSense.Models.Matches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Net.Http.Json;

namespace CardsWithoutCommonSense.Controllers.MatchFolder
{


    public class MatchHub : Hub
    {
        private static HttpClient _client = new HttpClient();
        public Dictionary<string, Match> matches = new Dictionary<string, Match>();

        public async Task JoinRoom(string roomid, string player)
        {
            player = player.TrimEnd('"').TrimStart('"');
            HttpResponseMessage response = await _client.PostAsJsonAsync(NetworkInfo.ApiPath + $"/Match/join/{roomid}", Guid.Parse(player));
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomid.ToString());
                AnouncePlayerJoined(player.ToString(), roomid.ToString());
                await Clients.Caller.SendAsync("RoomConnectResult", true);
                return;
            }
            await Clients.Caller.SendAsync("RoomConnectResult", false);
        }

        public async Task PlayCard(string roomid,string json)
        {

        }


        public Task LeaveRoom(Guid roomid)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomid.ToString());
        }

        public async void AnouncePlayerJoined(string client, string roomid)
        {
            await Clients.Group(roomid).SendAsync("joined", client, roomid);
        }
    }
}
