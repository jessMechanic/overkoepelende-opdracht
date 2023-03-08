using CardGame.Models.Matches;

namespace CardGame.Controllers
{
    public static class MatchController
    {
        private static Dictionary<Guid,Match> Matches  = new Dictionary<Guid,Match>();

        public static Match Get(Guid id)
        {
            return Matches[id];
        }


    }
}
