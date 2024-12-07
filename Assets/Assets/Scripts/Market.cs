using System;
using TMPro;
using UnityEngine;

public class Market : MonoBehaviour
{
    public static int _coins=0;
    [SerializeField] float _plusSteel,_plusHP;
    public int _hpCost,_steelCost;
    [SerializeField] TMP_Text _steelCostT,_hpCostT,_coinsT,_hpValueT;
    [SerializeField] GameObject _land,_air;
    [SerializeField] GameObject[] _unitsIcons;
    [SerializeField] Main _main;
    [SerializeField] Saver _saver;
    public Unit[] _units;
    public static float _maxHp=10,_steel=0.25f;
    UpgradeD _updD;

    void Start()
    {
        UnitsIcons();
        _main.start();
    }

    //Unit
    public void By(int i)
    {
        if(_units[i]._data._cost<=_coins)
        {
            _saver.SendLog("By", "item"+i);
            _coins-=_units[i]._data._cost;
            _units[i]._data._unlocked=true;
            _units[i]._obj._byButton.SetActive(false);
            _units[i]._obj._upButton.SetActive(true);
            UpdCoinsT();
            UnitsIcons();
            _main.Save();
        }
    }

    public void Upgrade(int i)
    {
        _updD=_units[i]._data._upgrade;
        if(_updD._cost<=_coins)
        {
            _coins-=_updD._cost;
            _units[i]._data._hp+=_updD._hp;
            _units[i]._data._damage+=_updD._damage;
            _units[i]._data._upLevel++;
            _units[i]._obj._levelT.text=(_units[i]._data._upLevel+1).ToString();
            _units[i]._data._upgrade._cost+=_updD._plusPerLevel;
            _units[i]._obj._upCostT.text=_units[i]._data._upgrade._cost.ToString();
            UpdCoinsT();
            _main.Save();
            _saver.SendLog("Upgrade", "item"+i);
        }
    }

    //Upgrades
    public void HPUpgrade()
    {
        if(_coins>=_hpCost)
        {
            _saver.SendLog("Upgrade", "hp");
            _maxHp+=_plusHP;
            _coins-=_hpCost;
            _hpCost+=30;
            _hpCostT.text=_hpCost.ToString();
            UpdCoinsT();
            _main.Save();
            _hpValueT.text=_maxHp.ToString();
        }
    }
    public void SteelUpgrade()
    {
        if(_coins>=_steelCost)
        {
            _steel+=_plusSteel;
            _coins-=_steelCost;
            _steelCost+=30;
            _steelCostT.text=_steelCost.ToString();
            UpdCoinsT();
            _main.Save();
        }
    }

    //Other
    public void ReloadButtons()
    {
        for(int i=0;i<_units.Length;i++)
        {
            if(_units[i]._data._unlocked)
            {
                if(_units[i]._obj._byButton!=null)
                {
                    _units[i]._obj._byButton.SetActive(false);
                    _units[i]._obj._upButton.SetActive(true);
                    _units[i]._obj._levelT.text=(_units[i]._data._upLevel+1).ToString();
                    _units[i]._obj._upCostT.text=_units[i]._data._upgrade._cost.ToString();
                }
            }
        }   
        _hpValueT.text=_maxHp.ToString();
    }

    public void UnitsIcons()
    {
        int j=0;
        for(int i=0;i<_unitsIcons.Length;i++)
        {
            _unitsIcons[i].SetActive(_units[i]._data._unlocked);
            if(_units[i]._data._unlocked)
            {
                _unitsIcons[i].transform.localPosition=new Vector3(j*160,0,0);
                j++;
            }
        }
    }

    public void Land()
    {
        _air.SetActive(false);
        _land.SetActive(true);
    }

    public void Air()
    {
        _air.SetActive(true);
        _land.SetActive(false);
    }
    public void Plus100()
    {
        _coins+=100;
        UpdCoinsT();
        _main.Save();
    }
    [Serializable]
    public struct Unit
    {
        public UnitD _data;
        public UnitO _obj;
    }

    [Serializable]
    public struct UnitD
    {
        public int _cost,_upLevel;
        public UpgradeD _upgrade;
        public bool _unlocked;
        public float _damage,_hp;
    }
    [Serializable]
    public struct UnitO
    {
        public GameObject _byButton,_upButton;
        public TMP_Text _levelT,_upCostT;
    }
    [Serializable]
    public struct UpgradeD
    {
        public int _cost,_plusPerLevel;
        public float _hp,_damage;
    }

    public void UpdCoinsT()
    {
        _coinsT.text=_coins.ToString();
    }
}
