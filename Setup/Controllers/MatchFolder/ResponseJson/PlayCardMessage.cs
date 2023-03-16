namespace CardsWithoutCommonSense.Controllers.MatchFolder.ResponseJson
{
    public class PlayCardMessage : ClientMessage
    {
        public string RoomId { get ; set ; }
        public string PlayerId { get; set ; }
        public int Card { get; set ; }

    }
}
