using CardGame.Models.Matches;

namespace CardGame.Controllers.MatchFolder
{
    public static class MatchController
    {
        private static Dictionary<Guid, Match> Matches = new Dictionary<Guid, Match>();

        public static Match Get(Guid id)
        {
            return Matches[id];
        }
        public static bool Join(Guid id, Guid player)
        {
            if (Matches.ContainsKey(id))
            {
                Matches[id].AddPlayer2(player);
                return true;
            }
            else
            {
                try
                {
                    Matches.Add(id, new(player, Guid.Empty));
                    return true;
                }
                catch (Exception e)
                { return false; }
            }
        }
        public static bool Delete(Guid id)
        {
            try
            {
                Matches.Remove(id);
                return true;
            }
            catch (Exception e)
            { return false; }
        }
    }
}
