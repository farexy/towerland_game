using Assets.Scripts.Models.Stats;

namespace Assets.Scripts.Network.Models
{
    public class StatsResponseModel
    {
        public UnitStats[] UnitStats { get; set; }
        public TowerStats[] TowerStats { get; set; }
        public DefenceCoeff[] DefenceCoeffs { get; set; }
        public Skill[] Skills { get; set; }
    }
}