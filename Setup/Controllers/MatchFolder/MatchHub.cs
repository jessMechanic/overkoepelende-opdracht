using CardGame.Models;
using CardGame.Models.Matches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Net.Http.Json;
using CardGame.Controllers.MatchFolder.ResponseJson;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace CardGame.Controllers.MatchFolder
{


    public class MatchHub : Hub
    {
        private static HttpClient _client = new HttpClient();

        #region connection
        public async Task JoinRoom(string roomId, string player)
        {
            if (player == null)
            {
                return;
            }
            player = player.TrimEnd('"').TrimStart('"');
            HttpResponseMessage response = await _client.PatchAsJsonAsync(NetworkInfo.ApiPath + $"/Match/join/{roomId}", Guid.Parse(player));
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                AnouncePlayerJoined(player.ToString(), roomId.ToString());
                await Clients.Caller.SendAsync("RoomConnectResult", true);

                MatchController.Join(Guid.Parse(roomId), Guid.Parse(player));
                MatchController.Get(Guid.Parse(roomId)).getPlayer(Guid.Parse(player)).ConnectionString = Context.ConnectionId;
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
        public async void DefineCardRoomWide(Guid roomId, Guid player, Card card, int position)
        {
            string json = JsonSerializer.Serialize(new CardDefineResponse(roomId, "DefineCardRoomWide", true, card, player, position));
            await Clients.Group(roomId.ToString()).SendAsync("DefineCard", json);
        }


        ///<summory>
        ///gives the information of a card to the a single player
        ///used for showing the cards in the hand of a player 
        /// </summory>

        public async void DefineCard(Guid roomId, string ConnectionString, Card card)
        {
            string json = JsonSerializer.Serialize(new CardDefineResponse(roomId, "DefineCardRoomWide", true, card, Guid.Empty, 0));
            await Clients.Client(ConnectionString).SendAsync("DefineCard", card);
        }
        #endregion GameResponses

        #region GameEndPoint


        ///<summory>
        ///allows the ConnectionString to request a card
        ///will later be refactored to only be usable server side
        ///</summory>
        public async Task<int> DrawCard( string json)
        {
        
            ClientMessage? clientMessage = JsonSerializer.Deserialize<ClientMessage>(json);
            if (clientMessage == null)
            {
                throw new Exception("invalid json");
            }        
            Match match = getMatch(clientMessage.RoomId);
            if (match == null)
                throw new Exception("not found somewhere was a miss sync");
            Console.WriteLine(json);
            //   if (match.AllowDraw) { 
            if (match.getPlayer(clientMessage.PlayerId) != null)
            {
                if (match.getPlayer(clientMessage.PlayerId).Hand.Count < 6)
                {
                    int cardIndex = match.DrawCard(clientMessage.PlayerId);


                    Card? card = match.GetCard(cardIndex);
                    if (card == null)
                    {
                        throw new Exception("card is null");
                    }
                    DefineCard(clientMessage.RoomId, Context.ConnectionId, card);
                    return cardIndex;
                }
            }
            //         }
            return -1;

        }


        ///<summory>
        ///gives the ConnectionString the ability to play a card from theyre hand to the battlefield
        ///needed to ad the sync functie so it apears on other players battlefields
        /// </summory>
        public async Task<bool> PlayCard( string json)
        {

            PlayCardMessage? mes = JsonSerializer.Deserialize<PlayCardMessage>(json);
            Console.WriteLine(json);
            if (mes == null)
            {
                Console.WriteLine("badJson");
                return false;
            }
            Match? match = getMatch(mes.RoomId);
            if (match == null) { return false; }
            if (!match.PlayCard(mes.PlayerId, mes.Card))
            {
                return false;
            }
            Stack<int> playercards = match.getPlayer(mes.PlayerId).Cards;
            DefineCardRoomWide(mes.RoomId, mes.PlayerId, match.GetCard(playercards.Peek()), playercards.Count);
            return true;


        }



        public async Task Ready(Guid roomId, string json)
        {
          
            ClientMessage? clientMessage = JsonSerializer.Deserialize<ClientMessage>(json);
            if (clientMessage == null)
            {
                return;
            }  
            Match match = getMatch(clientMessage.RoomId);
            if (match == null)
                throw new Exception("not found somewhere was a miss sync");
            Console.WriteLine(json);
            match.getPlayer(clientMessage.PlayerId).Ready = true;

        }
        #endregion GameEndPoint

        #region Utility
        private Match getMatch(Guid roomId)
        {
            return MatchController.Get(roomId);
        }
        private void Log(string connectionString, string message)
        {
            ///// TO DO create IsGlobal error function to send to the client (wich can be used to make fun easter eggs
            Clients.Client(connectionString).SendAsync("Log", message);
        }

        #endregion Utility
    }
}
