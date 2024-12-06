using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Vector3 _target;
    [SerializeField] float _speed,_verticalOffset,_forceExplosion,_ratioExplosion;
    [SerializeField] Vector3 _upperPoint;
    [SerializeField] ParticleSystem _explosion;
    [SerializeField] int _index;
    [SerializeField] AudioClip _shootClip;
    [SerializeField] GameObject _t;
    public LayerMask _targetL;
    [SerializeField] float _damage;
    bool _goUp=true;

    void Start()
    {
        // _damage=FindFirstObjectByType<Market>()._units[_index]._data._damage;
        _upperPoint=(_target+transform.position)/2;
        _upperPoint.y=transform.position.y+_verticalOffset;
    }

    void FixedUpdate()
    {
        if(_goUp && Vector3.Distance(transform.position,_upperPoint)<0.5f)
            _goUp=false;

        transform.LookAt(_goUp?_upperPoint:_target);
        transform.position=Vector3.MoveTowards(transform.position,_goUp?_upperPoint:_target,_speed);

        if(Vector3.Distance(transform.position,_target)<0.5f)
        {
            foreach(AudioSource s in FindObjectsOfType<AudioSource>())
            {
                if(s.gameObject.CompareTag("Fight"))
                {
                    s.PlayOneShot(_shootClip);
                }
            }
            foreach(RaycastHit h in Physics.BoxCastAll(transform.position,new Vector3(_ratioExplosion,_ratioExplosion,_ratioExplosion),Vector3.down,Quaternion.Euler(0,0,0),0,_targetL))
            {
                Debug.Log(_damage);
                h.transform.gameObject.GetComponent<Uint>().TakeDamage(_damage);
            }
            Instantiate(_explosion,transform.position,Quaternion.identity);
            Destroy(gameObject);
        } 
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(134,182,108,0.1f);
        Gizmos.DrawCube(transform.position,new Vector3(_ratioExplosion,_ratioExplosion,_ratioExplosion));
    }
}
