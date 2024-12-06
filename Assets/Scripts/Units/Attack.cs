using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack : Layer
{
    [SerializeField] protected float _attackDist, _cooldown;
    [SerializeField] protected bool _animated;
    [SerializeField] protected Animator[] _guns;
    protected bool _attacking;
    [SerializeField] Uint _unit;
    [SerializeField] protected List<RaycastHit> hit;
    [SerializeField] float _offsetY;
    Coroutine _attack;

    public virtual void FixedUpdate()
    {
        hit=Physics.RaycastAll(transform.position+new Vector3(0,_offsetY),transform.forward,_attackDist,_enemies).ToList();
        if(hit.Count>0)
        {
            if(!_attacking)
            {
                if(!_attacking)
                {
                    _attack=StartCoroutine(attack());
                }
                _attacking=true;
                _unit._attacking=true;
            }
        }
        else
        {
            if(_attacking)
            {
                foreach(Animator a in _guns)
                {
                    a.Play("Walk");
                }
                _attacking=false;
                _unit._attacking=false;
                StopCoroutine(_attack);
            }
        }
    }

    IEnumerator attack()
    {
        foreach(var g in _guns)
            g.Play("AIdle");
        while(true)
        {
            yield return new WaitForSeconds(_cooldown);
            if(hit.Count>0)
            {
                DoAttack();
            }
            else
            {
                break;
            }
        }
    }

    virtual public void OnDrawGizmos()
    {
        Gizmos.color=new Color(255,29,0,103);
        Gizmos.DrawRay(transform.position+new Vector3(0,_offsetY),transform.forward*_attackDist);
    }

    public virtual void DoAttack()
    {
    }
}
