using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pvo : Layer
{
    [SerializeField] protected float _attackDist,_cooldown;
    protected bool _attacking;
    [SerializeField] ParticleSystem[] _bullets;
    List<RaycastHit> hit=new List<RaycastHit>();
    [SerializeField] Uint _unit;

    void DoAttack()
    {
        _source.PlayOneShot(_shootClip);
        foreach(ParticleSystem bullet in _bullets)
        {
            bullet.transform.LookAt(hit[0].transform.position);
            bullet.Play();
        }
        hit[0].transform.GetComponentInParent<Uint>().TakeDamage(_damage);
    }

    void FixedUpdate()
    {
        hit=Physics.BoxCastAll(transform.position,new Vector3(_attackDist,_attackDist,_attackDist),Vector3.down,Quaternion.Euler(0,0,0),_attackDist,_enemies).ToList();
        if(hit.Count>0)
        {
            if(!_attacking)
            {
                StartCoroutine(attack());
                _attacking=true;
                _unit._attacking=true;
            }
        }
        else
        {
            if(_attacking)
            {
                _attacking=false;
                _unit._attacking=false;
                StopCoroutine(attack());
            }
        }
    }

    IEnumerator attack()
    {
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
    
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(134,182,108,0.1f);
        Gizmos.DrawCube(transform.position,new Vector3(_attackDist,_attackDist,_attackDist));
    }
}
