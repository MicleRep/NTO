using System.Collections;
using UnityEngine;

public class Mex : Attack
{
    [SerializeField] GameObject _rocket;
    [SerializeField] float _radius;
    [SerializeField] LayerMask _mask;
    [SerializeField] Transform[] _muzzles;
    [SerializeField] ParticleSystem[] _bullets;
    [SerializeField] Color _gizmosColor;
    RaycastHit _hit;

    public override void DoAttack()
    {
        _source.PlayOneShot(_shootClip);
        foreach(ParticleSystem bullet in _bullets)
        {
            bullet.Play();
        }
        foreach(Transform M in _muzzles)
        {
            M.transform.LookAt(hit[0].transform);
        }
        hit[0].transform.GetComponent<Uint>().TakeDamage(_damage);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Physics.SphereCast(transform.position, _radius, transform.forward, out _hit,0, _mask);
        // Debug.Log(_hit.collider);
        // if(_hit.collider!=null)
        // {
        //     Debug.Log("DronwKill");
        //     _source.PlayOneShot(_shootClip);
        //     foreach(ParticleSystem bullet in _bullets)
        //     {
        //         bullet.Play();
        //     }
        //     foreach(Transform M in _muzzles)
        //     {
        //         M.transform.LookAt(_hit.transform);
        //     }
        //     Destroy(_hit.transform.gameObject);
        // }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color=_gizmosColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
  