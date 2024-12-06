using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField] float _speed, _bombCooldown;
    [SerializeField] GameObject _bomb;
    Transform _target;

    IEnumerator Start()
    {
        _target=FindFirstObjectByType<Main>()._bomberTarget;
        while(true)
        {
            yield return new WaitForSeconds(_bombCooldown);
            Instantiate(_bomb, transform.position, _bomb.transform.rotation).GetComponent<Rocket>()._target=transform.position-new Vector3(0,14);
        }
    }

    void FixedUpdate()
    {
        transform.position=Vector3.MoveTowards(transform.position, _target.position, _speed);
        if(Vector3.Distance(transform.position, _target.position)<1)
        {
            Destroy(gameObject);
        }
    }
}