using System.Collections;
using UnityEngine;

public class Thrower : Attack
{
    [SerializeField] protected GameObject _bullet;
    [SerializeField] protected Transform _muzzle;
    [SerializeField] float _cooldownAfterAnim;

    public override void DoAttack()
    {
        StartCoroutine(DAttack());
    }
    IEnumerator DAttack()
    {
        foreach(Animator a in _guns)
        {
            a.Play("Attack");
        }
        yield return new WaitForSeconds(_cooldownAfterAnim);
        _source.PlayOneShot(_shootClip);
        Instantiate(_bullet, _muzzle.position, Quaternion.identity).GetComponent<Rocket>()._target=hit[0].transform.position;
        hit[0].transform.GetComponent<Uint>().TakeDamage(_damage);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
