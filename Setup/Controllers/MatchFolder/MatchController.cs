using CardsWithoutCommonSense.Models.Matches;

namespace CardsWithoutCommonSense.Controllers .MatchFolder
{
    public static class MatchController
    {
        private static Dictionary<Guid, Match> Matches  = new Dictionary<Guid,Match>();

        public static Match Get(Guid id)
        {
            return Matches[id];
        }


    }
}
