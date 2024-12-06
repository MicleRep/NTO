using System;
using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float _difficulty;
    public int _opened;
    public Obj[] _objects;
    [SerializeField] LayerMask _airLayer,_units, _enemies;
    [SerializeField] ParticleSystem _spawnParticle;
    GameObject _obj;
    public float _coins;
    [NonSerialized] public float _maxHP=10;
    [SerializeField] int _airplanes,c,i;
    public int _unlockNow=1,_unProgress;

    IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(3,15));
            _airplanes=Physics.BoxCastAll(transform.position,new Vector3(200,200,200),Vector3.right,Quaternion.Euler(0,0,0),0,_airLayer).Length;
            i=0;
            while(_coins>=1)
            {
                //PVO
                c=6-_airplanes;
                c=Mathf.Min(2,c);
                if(_airplanes>0&UnityEngine.Random.Range(0,c)==0&_opened>=4&_coins>_objects[5]._cost)
                {
                    _obj=Instantiate(_objects[5]._weapon,transform.position+new Vector3(UnityEngine.Random.Range(-20,-5),0,UnityEngine.Random.Range(-17,17)),Quaternion.identity);
                    _coins-=_objects[4]._cost;
                }
                else
                {
                    SpawnUnit();
                }
                //Layers
                if(_obj.GetComponent<Spawner>()==null && _obj.GetComponent<BeeHive>()==null)
                {
                    _obj.GetComponent<Layer>()._enemies=_units;
                    _obj.GetComponent<Uint>()._enemies=_units;
                    _obj.GetComponent<Uint>().ChooseTarget();
                }
                else if(_obj.GetComponent<Comander>() != null)
                {   
                    _obj.GetComponent<Comander>()._enemies = _enemies;
                }

                if(_obj.GetComponent<Airplane>()!=null)
                {
                    _obj.layer=9;
                    foreach(Collider col in _obj.GetComponentsInChildren<Collider>())
                    {
                        col.gameObject.layer=9;
                    }
                }
                else if(_obj.GetComponent<Drone>()!=null)
                {
                    _obj.layer=11;
                    foreach(Collider col in _obj.GetComponentsInChildren<Collider>())
                    {
                        col.gameObject.layer=11;
                    }
                }
                else
                {
                    _obj.layer=3;
                }
                i++;
                if(i>500)
                {
                    Debug.Log("while");
                    break;
                }
            }
        }
    }

    void SpawnUnit()
    {
        c=UnityEngine.Random.Range(0,_opened);
        if(c!=4&_coins>=_objects[c]._cost)
        {
            _obj=Instantiate(_objects[c]._weapon,transform.position+new Vector3(UnityEngine.Random.Range(-20,-5),0,UnityEngine.Random.Range(-17,17))+_objects[c]._offset,Quaternion.identity);
            _coins-=_objects[c]._cost;
            Instantiate(_spawnParticle,_obj.transform.position+new Vector3(0,3,0),Quaternion.identity);
        }
        else
        {
            SpawnUnit();
        }
    }

    void OnEnable()
    {
        StartCoroutine(Enable());
    }
    IEnumerator Enable()
    {
        StartCoroutine(Spawn());
        while(true)
        {
            yield return new WaitForSeconds(1);
            _coins+=_difficulty;
        }
    }

    [Serializable]
    public struct Obj
    {
        public GameObject _weapon;
        public int _cost, _toUnlock;
        public Vector3 _offset;
    }
}
