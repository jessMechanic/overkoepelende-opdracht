namespace CardGame.Controllers.MatchFolder.ResponseJson
{
    public interface IServerResponse
    {
        public Guid RoomId { get; set; }
        public string Message { get; set; }
    }
}
