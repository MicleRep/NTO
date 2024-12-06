
using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        public bool isFirstSession = true;
        public List<Market.UnitD> units=new List<Market.UnitD>();
        public int _coins,_unN,_unP,_level,_opened,_hpCost,_steelCost;
        public float _aiHP,_difficulty,_steel,_hp;
    }
}
