using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Airplane : Layer
{
    [SerializeField] ParticleSystem[] _particles;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _attackDist,_cooldown;
    bool _attacking;
    Transform _target;
    [SerializeField] List<RaycastHit> hit;
    Coroutine _attack;

    void FixedUpdate()
    {
        transform.Rotate(0,_rotateSpeed,0);
        hit=Physics.BoxCastAll(transform.position,new Vector3(_attackDist,_attackDist,_attackDist),Vector3.down,Quaternion.Euler(0,0,0),_attackDist,_enemies).ToList();
        if(hit.Count>0)
        {
            _target=hit[0].transform;
            if(!_attacking)
            {
                _attack=StartCoroutine(attack());
                _attacking=true;
            }
        }
        else
        {
            if(_attacking)
            {
                _attacking=false;
                StopCoroutine(_attack);
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
                _source.PlayOneShot(_shootClip);
                _target.GetComponent<Uint>().TakeDamage(_damage);
                foreach(ParticleSystem p in _particles)
                {
                    p.transform.LookAt(_target);
                    p.Play();
                }
            }         
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(134,182,108,0.1f);
        Gizmos.DrawCube(transform.position,new Vector3(_attackDist,_attackDist,_attackDist));
    }
}
