using CardGame.Models.Matches;

namespace CardGame.Controllers.MatchFolder.ResponseJson
{
    public class CardDefineResponse : IServerResponse
    {

        public Guid RoomId { get; set; }
        public string Message { get; set; }
        public bool IsGlobal { get; set; }
        public Card CardDef { get; set; }
        public int Position { get; set; }
        public  Guid PlayerSide { get; set; }

        public CardDefineResponse(Guid roomId, string message, bool global, Card cardDef, Guid playerSide,int pos)
        {
            RoomId  = roomId;
            Message  = message;
            IsGlobal     = global;
            CardDef  = cardDef;
            PlayerSide = playerSide;
            Position     = pos;
        }
    }
}
