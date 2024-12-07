using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Main : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] Obj[] _units;
    [SerializeField] Demo[] _demos;
    [Header("Other")]
    [SerializeField] Camera _cam;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] TMP_Text _coinsText;
    [Header("Win or Loose")]
    [SerializeField] GameObject _winMenu;
    [SerializeField] GameObject _looseMenu,_markrketMenu,_fight;
    [SerializeField] TMP_Text _rewardT,_coinsT;
    [Header("AI Controll")]
    [SerializeField] int _level;
    [SerializeField] AI _ai;
    [SerializeField] Uint _base;
    [SerializeField] Image _baseBar,_aiBar;
    [SerializeField] int _startPoints;
    [Header("Save&Load")]
    [SerializeField] Market _market;
    [SerializeField] List<Market.Unit> _unitsS;
    [SerializeField] TMP_Text _hpCostT;
    [SerializeField] GameStop _gameStop;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _playSound;
    [SerializeField] ParticleSystem _spawnParticle;
    [SerializeField] GameObject[] _terrains;
    [SerializeField] Saver _saver;
    [Header("Bomber")]
    public Transform _bomberTarget;
    [SerializeField] float _bomberCooldown;
    [SerializeField] Transform _bomberSpawnPoint;
    [SerializeField] Image _readyBomberI;
    public float _coins;
    int _unit;
    GameObject _demo;
    RaycastHit hit;
    protected bool _noSpawn;
    bool _bomberReady;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SpawnUnit());
        }
        if(_demo!=null)
        {
            Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition),out hit,700,_groundLayer);
            if(hit.collider!=null)
            {
                _demo.transform.position=hit.point+_demos[_unit]._offset;
            }
        }
    }
    public virtual void SelectUnit(int i)
    {
        _unit = i;
        Destroy(_demo);
        Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition),out hit,700,_groundLayer);
        _demo=Instantiate(_demos[_unit]._demo,hit.point+_demos[_unit]._offset,Quaternion.identity);
        StartCoroutine(NoSpawn());
    }
    public IEnumerator SpawnUnit()
    {
        yield return new WaitForSeconds(0.2f);
        if(!_noSpawn&_coins>=_units[_unit]._cost)
        {
            Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition),out hit,700,_groundLayer);
            if(hit.point!=new Vector3(0,0,0))
            {
                StartCoroutine(OnEducation());
                Instantiate(_spawnParticle,Instantiate(_units[_unit]._weapon,hit.point+_demos[_unit]._offset,Quaternion.identity).transform.position,Quaternion.identity).Play();
                if(_units[_unit]._spawnSound)
                {
                    _source.PlayOneShot(_units[_unit]._spawnSound);
                }
                _coins-=_units[_unit]._cost;
                _coinsText.text=Mathf.FloorToInt(_coins).ToString();
            }
        }
    }
    public void BomberSpawn()
    {
        Debug.Log("TrySpawn  "+_bomberReady);
        if(_bomberReady)
        {
            Debug.Log("Spawn");
            // StartCoroutine(BomberCooldown());
            Instantiate(_units[9]._weapon, _bomberSpawnPoint.position, Quaternion.identity);
        }
    }
    public virtual IEnumerator BomberCooldown()
    {
        _bomberReady=false;
        for(int i=0;i<_bomberCooldown*25+1;i++)
        {
            yield return new WaitForSeconds(0.04f);
            _readyBomberI.fillAmount=1-(i/(_bomberCooldown*25));
        }
        _bomberReady=true;
        Debug.Log(_bomberReady);
    }
    protected virtual IEnumerator OnEducation()
    {
        yield return null;
    }

    public IEnumerator NoSpawn()
    {
        _noSpawn=true;
        yield return new WaitForSeconds(0.25f);
        _noSpawn=false;
    }

    void Start()
    {
        _coins+=_startPoints;
        _coinsText.text=_coins.ToString();
        // StartCoroutine(BomberCooldown());
    }

    public void start()
    {
        // Load
    }

    void OnEnable()
    {
        StartCoroutine(Enabled());
    }
    IEnumerator Enabled()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            _coins+=Market._steel;
            _coinsText.text=Mathf.FloorToInt(_coins).ToString();
        }
    }

    void AddReward(int multiple)
    {
        Market._coins+=(50+_level*10)*multiple;
    }

    public void Win()
    {
        _saver.SendLog("Win", "level");
        //Destroy Units
        foreach(Uint unit in FindObjectsOfType<Uint>())
        {
            if(unit.GetComponent<Bases>()==null)
            {
                Destroy(unit.gameObject);
            }
        }

        //Add level and difficulty
        _level++;
        _ai._unProgress++;
        Debug.Log(_ai._unlockNow);
        Debug.Log(_ai._objects[_ai._unlockNow]._toUnlock);
        if(_ai._unProgress>=_ai._objects[_ai._unlockNow]._toUnlock)
        {
            _ai._unProgress=0;
            _ai._unlockNow++;
            _ai._opened++;
            _ai._opened=Mathf.Min(_ai._opened, _ai._objects.Length);
        }
        _ai._maxHP+=1;
        _ai._difficulty=_level*0.02f+0.2f;
        _rewardT.text=(50+_level*10).ToString();
    }

    public void Fight()
    {
        _saver.SendLog("Fight", "");
        _fight.SetActive(true);
        _markrketMenu.SetActive(false);
        _coins=0;
        _coinsText.text=_coins.ToString();
        _base._hp=Market._maxHp;
        _ai.GetComponent<Uint>()._hp=_ai._maxHP;
        _baseBar.fillAmount=1;
        _aiBar.fillAmount=1;
        foreach(GameObject o in _terrains)
        {
            o.SetActive(false);
        }
        _terrains[_level%_terrains.Length].SetActive(true);
        StartCoroutine(FightS());
    }

    IEnumerator FightS()
    {
        yield return new WaitForSeconds(0.1f);
        _source.PlayOneShot(_playSound);
    }

    public void Collect()
    {
        AddReward(1);
        Menu();
    }

    public void X2()
    {
        AddReward(2);
        Menu();
    }

    public void Menu()
    {
        _saver.SendLog("Go to menu", "");
        Time.timeScale=1;
        _looseMenu.SetActive(false);
        _winMenu.SetActive(false);
        _coinsT.text=Market._coins.ToString();
        _fight.SetActive(false);
        _markrketMenu.SetActive(true);
        Save();
    }

    [Serializable]
    public struct Demo
    {
        public GameObject _demo;
        public Vector3 _offset;
    }
    [Serializable]
    public struct Obj
    {
        public GameObject _weapon;
        public AudioClip _spawnSound;
        public int _cost;
    }
    [Serializable]
    public struct Data
    {
        public bool _purchared;
        public int _level;
    }
    [Serializable]
    public struct ByData
    {
        public int _cost;
        public TMP_Text _costT;
        public GameObject _buB,_upB;
    }

    //Save and Load
    [ContextMenu("Save")]
    public virtual void Save()
    {
        _saver.savesData.units=new List<Market.UnitD>();
        foreach(Market.Unit u in _market._units)
        {
            _saver.savesData.units.Add(u._data);
        }
        _saver.savesData._unP=_ai._unProgress;
        _saver.savesData._level=_level;
        _saver.savesData._unN=_ai._unlockNow;
        _saver.savesData._coins=Market._coins;
        _saver.savesData._opened=_ai._opened;
        _saver.savesData._aiHP=_ai._maxHP;
        _saver.savesData._difficulty=_ai._difficulty;
        _saver.savesData._steelCost=_market._steelCost;
        _saver.savesData._hpCost=_market._hpCost;
        _saver.savesData._steel=Market._steel;
        _saver.savesData._hp=Market._maxHp;
        _saver.Save();
    }
    public virtual void Load()
    {
        for(int i=0;i<_market._units.Length;i++)
        {
            _market._units[i]._data=_saver.savesData.units[i];
        }
        _ai._unProgress = _saver.savesData._unP;
        _level=_saver.savesData._level;
        _ai._unlockNow = _saver.savesData._unN;
        Market._coins=_saver.savesData._coins;
        _ai._opened=_saver.savesData._opened;
        _ai._maxHP=_saver.savesData._aiHP;
        _ai._difficulty=_saver.savesData._difficulty;
        _market._steelCost=_saver.savesData._steelCost;
        _market._hpCost=_saver.savesData._hpCost;
        Market._steel=_saver.savesData._steel;
        Market._maxHp=_saver.savesData._hp;
        _hpCostT.text=_market._hpCost.ToString();
        _market.ReloadButtons();
        _market.UnitsIcons();
        _market.UpdCoinsT();
    }
}