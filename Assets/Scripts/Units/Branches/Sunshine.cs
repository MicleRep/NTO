using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sunshine : Attack
{
    [SerializeField] GameObject _bullet;
    [SerializeField] Transform[] _muzzles;
    Rocket _b;
    float _dist;
    Transform _target;

    public override void DoAttack()
    {
        _source.PlayOneShot(_shootClip);
        for(int i = 0;i<_muzzles.Length;i++)
        {
            _b=Instantiate(_bullet,_muzzles[i].position,Quaternion.identity).GetComponent<Rocket>();
            _dist=1000;
            foreach(RaycastHit r in hit)
            {
                if(Vector3.Distance(transform.position,r.transform.position)<_dist)
                {
                    _dist=Vector3.Distance(transform.position,r.transform.position);
                    _target=r.transform;
                }
            }
            _b._target=_target.position;
            _b._targetL=_enemies;
        }
    }
}
