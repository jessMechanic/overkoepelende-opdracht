using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CardGame.Models;
using CardGame.Models.Matches;

namespace CardGameTests.BusinesLogic
{


    [TestClass]
    public class MatchTests
    {
        [TestMethod]
        public void DrawCard_ReturnsCorrectCardIndex()
        {
            // Arrange
            var match = new Match(Guid.NewGuid(), Guid.NewGuid());         //making a match with fake players
            var playerId = match.player1.Id;                                                              //save the id of player1 for later

            // Act
            var cardIndex = match.DrawCard(playerId);

            // Assert
            Assert.AreEqual(1, match.GetHandCount(playerId));
            Assert.AreEqual(0, cardIndex);
        }

        [TestMethod]
        public void PlayCard_CardIsPlayed()
        {
            // Arrange
            var match = new Match(Guid.NewGuid(), Guid.NewGuid());
            var playerId = match.player1.Id;
            var cardIndex = match.DrawCard(playerId);

            // Act
            var played = match.PlayCard(playerId, cardIndex);

            // Assert
            Assert.IsTrue(played);                                                                                                     //check if player is ready after play
            Assert.AreEqual(match.GetHandCount(playerId), 0);                               //check if cards isnt present in hand
            Assert.AreEqual(match.GetCardsInPlayCount(playerId), 1);            //check if card is present on the field
        }

            
       

        [TestMethod]
        public void GetCardsInPlayCount_ReturnsCorrectCount()
        {
            // Arrange
            var match = new Match(Guid.NewGuid(), Guid.NewGuid());
            var player = match.player1;
            match.DrawCard(player.Id);
            match.DrawCard(player.Id);

            // Act
            int result = match.GetHandCount(player.Id);

            // Assert
            Assert.AreEqual(2, result);                                                                                          //checks if the two added cards are added to the hand
        }

       

        [TestMethod]
        public void AddPlayer2_SetsPlayer2()
        {
            // Arrange
            var match = new Match(Guid.NewGuid(),Guid.Empty);
            var playerId = Guid.NewGuid();

            // Act
            match.AddPlayer2(playerId);

            // Assert
            Assert.AreEqual(playerId, match.player2.Id);
        }

        [TestMethod]
        public void getPlayer_ReturnsCorrectPlayer()
        {
            // Arrange
            var match = new Match(Guid.NewGuid(), Guid.NewGuid());
            var expectedPlayer = match.player1;

            // Act
            var result = match.getPlayer(expectedPlayer.Id);

            // Assert
            Assert.AreEqual(expectedPlayer, result);
        }

     

        [TestMethod]
        public void PlayCard_ReturnsTrueWhenSuccessful()
        {
            // Arrange
            var match = new Match(Guid.NewGuid(), Guid.NewGuid());
            var playerId = match.player1.Id;
            int cardIndex =   match.DrawCard(playerId);
          
            // Act
            bool result = match.PlayCard(playerId, cardIndex);

            // Assert
            Assert.IsTrue(result);
        }
    }
}

