using CardGame.Models;
using CardGame.Models.Matches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Net.Http.Json;
using CardGame.Controllers.MatchFolder.ResponseJson;

namespace CardGame.Controllers.MatchFolder
{


    public class MatchHub : Hub
    {
        private static HttpClient _client = new HttpClient();

        #region connection
        public async Task JoinRoom(string roomId, string player)
        {
            player = player.TrimEnd('"').TrimStart('"');
            HttpResponseMessage response = await _client.PostAsJsonAsync(NetworkInfo.ApiPath + $"/Match/join/{roomId}", Guid.Parse(player));
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                AnouncePlayerJoined(player.ToString(), roomId.ToString());
                await Clients.Caller.SendAsync("RoomConnectResult", true);

                MatchController.Join(Guid.Parse(roomId), Guid.Parse(player));

                return;
            }
            await Clients.Caller.SendAsync("RoomConnectResult", false);
        }
        public Task LeaveRoom(Guid roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public async void AnouncePlayerJoined(string client, string roomId)
        {
            await Clients.Group(roomId).SendAsync("joined", client, roomId);
        }
        #endregion connection

        #region GameResponses
        ///<summory>
        ///gives the information of a card to the whole room ( now consisting of two players) 
        ///used for syncing of cards on the battle field
        /// </summory>
        public async void DefineCardRoomWide(Guid roomId, Card card)
        {
            // still implement
            await Clients.Group(roomId.ToString()).SendAsync("DefineCard");
        }


        ///<summory>
        ///gives the information of a card to the a single player
        ///used for showing the cards in the hand of a player 
        /// </summory>
        public async void DefineCard(Guid roomId, String player, Card card)
        {
            await Clients.Client(player).SendAsync("DefineCard", card);
        }
        #endregion GameResponses

        #region GameEndPoint


        ///<summory>
        ///allows the player to request a card
        ///will later be refactored to only be usable server side
        ///</summory>
        public async Task DrawCard(Guid roomId, string json)
        {
            Match match = getMatch(roomId);
            if (match == null)
                throw new Exception("not found somewhere was a miss sync");

            IClientMessage? clientMessage = JsonSerializer.Deserialize<IClientMessage>(json);
            if (clientMessage == null)
            {
                throw new Exception("invalid json");
            }
            int cardIndex = match.DrawCard(clientMessage.PlayerId);


            Card? card = match.GetCard(cardIndex);
            if (card == null)
            {
                throw new Exception("card is null");
            }
            DefineCard(roomId, Context.ConnectionId, card);

        }


        ///<summory>
        ///gives the player the ability to play a card from theyre hand to the battlefield
        ///needed to ad the sync functie so it apears on other players battlefields
        /// </summory>
        public async Task PlayCard(string roomId, string json)
        {
            PlayCardMessage? mes = JsonSerializer.Deserialize<PlayCardMessage>(json);
            getMatch(Guid.Parse(roomId)).PlayCard(mes.PlayerId, mes.Card);
        }

        #endregion GameEndPoint

        #region Utility
        private Match getMatch(Guid roomId)
        {
            return MatchController.Get(roomId);
        }
        private void error(string message)
        {
            ///// TO DO create global error function to send to the client (wich can be used to make fun easter eggs
        }

        #endregion Utility
    }
}
