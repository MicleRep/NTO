using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float _cooldown;
    [SerializeField] GameObject _unit;
    [SerializeField] Transform[] _muzzles;
    [SerializeField] GameObject _un, _ene;
    [SerializeField] LayerMask _unitsL, _enemiesL;
    GameObject _mob;

    IEnumerator Start()
    {
        _un.SetActive(gameObject.layer==6);
        _ene.SetActive(gameObject.layer!=6);
        while (true)
        {
            yield return new WaitForSeconds(_cooldown);
            foreach (var _muzzle in _muzzles)
            {
                _mob = Instantiate(_unit, _muzzle.position, _unit.transform.rotation);
                _mob.GetComponent<Uint>()._enemies = gameObject.layer==6 ? _enemiesL: _unitsL;
                _mob.GetComponent<Layer>()._enemies = gameObject.layer==6 ? _enemiesL: _unitsL;
                _mob.layer = _mob.GetComponent<Airplane>()==null ? gameObject.layer : gameObject.layer==3 ? 9 : 7;
            }
        }
    }
}