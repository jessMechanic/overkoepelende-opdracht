using CardGame.Models;
using CardGame.Models.Matches;
using Microsoft.AspNetCore.SignalR;

namespace CardGame.Controllers
{


    public class MatchHub : Hub
    {
        private static  HttpClient _client = new HttpClient();
            
        
        public async Task JoinRoom(string roomid, Guid playerId)
        {     
        HttpResponseMessage response = await _client.PatchAsJsonAsync(NetworkInfo.ApiPath + $"/Match/join/{roomid}", playerId);
            Console.WriteLine(response);
        if (response.IsSuccessStatusCode)
        {
         await  Groups.AddToGroupAsync(Context.ConnectionId, roomid.ToString());
                AnouncePlayerJoined(playerId.ToString(), roomid.ToString());
                await Clients.Caller.SendAsync("RoomConnectResult", true);
                return;
        }
            await Clients.Caller.SendAsync("RoomConnectResult", false);
        }

        public Task LeaveRoom(Guid roomid)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomid.ToString());
        }

        public async void AnouncePlayerJoined(string client,string roomid)
        {
            await Clients.Group(roomid).SendAsync("joined", client,roomid);
        }
    }
    }
