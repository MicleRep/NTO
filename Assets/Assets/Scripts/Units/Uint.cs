using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Uint : MonoBehaviour
{
    [SerializeField] bool _mowing,_hasRatioAttack,_GizmosVisability;
    public float _hp;
    [SerializeField] int _index;
    [SerializeField] float _ratioAttack;
    public LayerMask _enemies;
    [SerializeField] protected Transform _target;
    [SerializeField] Animator[] _foots;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] ParticleSystem _damagedP;
    [SerializeField] Image _bar;
    [SerializeField] bool _lookTurret;
    [SerializeField] AI _ai;
    [SerializeField] protected AudioSource _source;
    [SerializeField] AudioClip _dieClip;
    [SerializeField] short _help;
    float _maxHP;
    bool _walking;
    [NonSerialized] public bool _attacking;
    List<RaycastHit> hit=new();

    //Target
    void OnEnable()
    {
        if(_mowing||_lookTurret)
            StartCoroutine(ChoosingTarget());
        if(_index!=-1)
        {
            if(gameObject.layer==6||gameObject.layer==7||gameObject.layer==10||gameObject.layer==13)
            {
                _hp=FindFirstObjectByType<Market>()._units[_index]._data._hp;
                _maxHP=_hp;
            }
        }
        else
        {
            _maxHP=_hp;
        }
    }

    IEnumerator ChoosingTarget()
    {
        while(true)
        {
            ChooseTarget();
            yield return new WaitForSeconds(1.3f);
        }
    }

    public void ChooseTarget()
    {
        hit=Physics.SphereCastAll(transform.position,1000,Vector3.forward,0,_enemies).ToList();
        hit.Sort((t, t1)=>Vector3.Distance(t.transform.position,transform.position).CompareTo(Vector3.Distance(t1.transform.position,transform.position)));
        _target = hit[_help].transform;

        if(_help > 0)
        {
            if(hit[_help].transform.GetComponent<Bases>()!=null)
                if(hit.Count>_help+1)
                {
                    _target = hit[_help+1].transform;
                    Mowe();
                }
            else
                Mowe();
        }
    }

    //Loops
    void FixedUpdate()
    {
        if(_help == 0)
            Mowe();
        if(_lookTurret)
        {
            transform.LookAt(_target);
        }
        if(_target!=null)
        {
            if(!_attacking&_mowing)
            {
                if(!_walking)
                {
                    _walking=true;
                    foreach(Animator a in _foots)
                    {
                        a.Play("Walk");
                    }
                }
            }
            else if(_walking)
            {
                _walking=false;
                foreach(Animator a in _foots)
                {
                    a.Play("Idle");
                }
            }
        }
        else if(_walking)
        {
            _walking=false;
            foreach(Animator a in _foots)
            {
                a.Play("Idle");
            }
        }
    }
    void OnDrawGizmos()
    {
        if(_hasRatioAttack&_GizmosVisability)
        {
            Gizmos.color = new Color(134,182,108,0.1f);
            Gizmos.DrawSphere(transform.position,_ratioAttack);
        }
    }

    //Defs
    public void TakeDamage(float i)
    {
        Debug.Log(i);
        _hp-=i;
        if(_hp<=0)
        {
            StartCoroutine(Die());
        }
        else if(_maxHP/2>_hp&_damagedP!=null)
        {
            _damagedP.gameObject.SetActive(true);
            _damagedP.Play();
        }
        if(_bar!=null)
        {
            if(_ai==null)
            {
                _bar.fillAmount=_hp/Market._maxHp;
            }
            else
            {
                _bar.fillAmount=_hp/_ai._maxHP;
            }
        }
    }

    public virtual IEnumerator Die()
    {
        Destroy(gameObject);
        // _source.PlayOneShot(_dieClip);
        yield return new WaitForSeconds(0);
    }
    public virtual void Mowe()
    {
        if(_agent!=null)
        {
            _agent.enabled=!_attacking;
        }
        if(_target!=null)
        {
            if(!_attacking&_mowing)
            {
                Debug.Log("Walk"+"  "+gameObject.name);
                _agent.SetDestination(_target.position);
            }
        }
    }
}