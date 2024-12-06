using UnityEngine;

public class Solder : Attack
{
    [SerializeField] protected ParticleSystem[] _bullets;

    public override void DoAttack()
    {
        foreach(Animator a in _guns)
        {
            a.Play("Attack");
        }
        _source.PlayOneShot(_shootClip);
        foreach(ParticleSystem bullet in _bullets)
        {
            bullet.Play();
        }
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
