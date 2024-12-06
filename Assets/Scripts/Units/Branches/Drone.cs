using TMPro;
using UnityEngine;

public class Drone : Layer
{
    [SerializeField] float _speed;
    [SerializeField] ParticleSystem _explosion;
    Vector3 _target;
    float _dist=1000;

    void Start()
    {
        foreach(RaycastHit r in Physics.BoxCastAll(transform.position,new Vector3(300,300,300),transform.forward,Quaternion.Euler(0,0,0),0,_enemies))
        {
            if(Vector3.Distance(r.transform.position,transform.position)<_dist)
            {
                _dist=Vector3.Distance(r.transform.position,transform.position);
                _target=r.transform.position;
            }
        }
    }

    void FixedUpdate()
    {
        transform.position=Vector3.MoveTowards(transform.position,_target,_speed);
        transform.LookAt(_target);
        if(Vector3.Distance(_target,transform.position)<0.5f)
        {   
            foreach(RaycastHit r in Physics.BoxCastAll(transform.position,new Vector3(5,5,5),transform.forward,Quaternion.Euler(0,0,0),0,_enemies))
            {
                foreach(AudioSource s in FindObjectsOfType<AudioSource>())
                {
                    if(s.gameObject.CompareTag("Fight"))
                    {
                        s.PlayOneShot(_shootClip);
                    }
                }
                r.transform.GetComponent<Uint>().TakeDamage(_damage);
            }
            Instantiate(_explosion,transform.position,Quaternion.identity).Play();
            Destroy(gameObject);
        }
    }
}
