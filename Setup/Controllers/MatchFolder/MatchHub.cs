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
                Match match = MatchController.Get(Guid.Parse(roomId));
                MatchPlayer matchPlayer = match.getPlayer(Guid.Parse(player));
                matchPlayer.ConnectionString = Context.ConnectionId;
                if(matchPlayer.Hand.Count > 0)
                {
                    foreach (int item in matchPlayer.Hand)
                    {
                        DefineCard(Guid.Parse(roomId), Context.ConnectionId, match.GetCard(item));
                    }
                }
                
                
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
            await Clients.Group(roomId.ToString()).SendAsync("DefineCardRoomWide", json);
        }



        ///<summory>
        ///gives the information of a card to the a single player
        ///used for showing the cards in the hand of a player 
        /// </summory>

        public async void DefineCard(Guid roomId, string ConnectionString, Card card)
        {
            string json = JsonSerializer.Serialize(new CardDefineResponse(roomId, "DefineCard", true, card, Guid.Empty, 0));
            await Clients.Client(ConnectionString).SendAsync("DefineCard", card);
        }


        public async void ReDefinePlayingField(Guid roomId)
        {
         Match match = MatchController.Get(roomId);
            await Clients.Group(roomId.ToString()).SendAsync("Reset");

            foreach (int item in  match.player1.Cards)
            {
                DefineCardRoomWide(roomId, match.player1.Id, match.GetCard(item), 0);
               
            }
            
          foreach (int item in match.player2.Cards)
            {
                DefineCardRoomWide(roomId, match.player2.Id, match.GetCard(item), 0);

            }
            match.player1.Ready=false; match.player2.Ready=false;
        }

        public async void AnnounceWinner(Guid roomId,Guid winner)
        {
            await Clients.Group(roomId.ToString()).SendAsync("AnnounceWinner",winner);
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
            MatchPlayer player = match.getPlayer(clientMessage.PlayerId);
            if (player != null)
            {
                if (player.Hand.Count < 4)
                {
                    int cardIndex = match.DrawCard(clientMessage.PlayerId);
                       Console.WriteLine($"{cardIndex}");

                    Card? card = match.GetCard(cardIndex);
                    card.Index = cardIndex;
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
            if (!match.PlayCard(mes.PlayerId, mes.Card ))
            {
                return false;
            }
            MatchPlayer player = match.getPlayer(mes.PlayerId);
            if (player.Ready) { Console.WriteLine("player is ready"); return false; }
            Stack<int> playercards = player.Cards;
            player.Ready= true;
            
            DefineCardRoomWide(mes.RoomId, mes.PlayerId, match.GetCard(playercards.Peek()), playercards.Count);   
             if(match.checkReady())
            {
                ReDefinePlayingField(mes.RoomId);
               
            }
            return true;


        }

       

        public async Task Ready(string json)
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
            MatchPlayer player = match.getPlayer(clientMessage.PlayerId);
            if(player != null)
            {

                player.Ready = true;

                if (match.checkReady())
            {
                ReDefinePlayingField(clientMessage.RoomId);
            }  }

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
