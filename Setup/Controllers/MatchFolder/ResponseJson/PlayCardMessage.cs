namespace CardGame.Controllers.MatchFolder.ResponseJson
{
    public class PlayCardMessage : ClientMessage
    {
        public Guid RoomId { get ; set ; }
        public Guid PlayerId { get; set ; }
        public int Card { get; set ; }

    }
}
