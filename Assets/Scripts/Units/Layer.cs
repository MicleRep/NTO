using UnityEngine;

public class Layer : MonoBehaviour
{
    public LayerMask _enemies;
    [SerializeField] protected int _index;
    public float _damage;
    [SerializeField] protected AudioClip _shootClip;
    [SerializeField] protected AudioSource _source;
    [SerializeField] GameObject _ene,_un;

    void Start()
    {
        if(gameObject.layer==6||gameObject.layer==7||gameObject.layer==10||gameObject.layer==13)
        {
            _ene.SetActive(false);
            _un.SetActive(true);
            _damage=FindFirstObjectByType<Market>()._units[_index]._data._damage;
        }
        else
        {
            _ene.SetActive(true);
            _un.SetActive(false);
        }
    }
}
