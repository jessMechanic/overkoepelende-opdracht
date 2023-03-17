namespace CardGame.Controllers.MatchFolder.ResponseJson
{
    public interface IClientMessage
    {
        public Guid RoomId { get; set; }
        public Guid PlayerId { get; set; }
    
    }
}
