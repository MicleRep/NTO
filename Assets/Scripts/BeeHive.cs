using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHive : MonoBehaviour
{
    [SerializeField] float _plusPerSec;
    [SerializeField] GameObject _un, _ene;
    AI _ai;
    Main _main=new();

    IEnumerator Start()
    {
        _plusPerSec = FindFirstObjectByType<Market>()._units[6]._data._damage;
        _main = FindFirstObjectByType<Main>();
        _un.SetActive(gameObject.layer==6);
        _ene.SetActive(gameObject.layer!=6);
        if(gameObject.layer==6)
        {
            while(true)
            {
                _main._coins+=_plusPerSec;
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            _ai=FindObjectOfType<AI>();
            while(true)
            {
                _ai._coins+=_plusPerSec;
                yield return new WaitForSeconds(1);
            }
        }
    }
}