using CardGame.Models.Matches;

namespace CardApi.Models.Matches
{

    public enum CreatureEffect
    {
        None,
        Sturdy,
        Glass_Cannon,


    }
    public enum DefenceEffect
    {
        None,
        WellBuild,
        Briddle,

    }
    public class Effect
    {
        public int EffectName { get; set; }
        public string Discriptor { get; set; }
        public Effect(int effectName)
        {
            EffectName = effectName;
        }



    }
}
