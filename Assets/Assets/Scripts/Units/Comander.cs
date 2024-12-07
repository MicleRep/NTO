using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Comander : Layer
{
    [SerializeField] int _teamLayer;
    [SerializeField] float _multiple;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _teamLayer)
        {
            other.GetComponent<Uint>()._hp *= _multiple;;
            other.GetComponent<Layer>()._damage *= _multiple;
            other.GetComponent<NavMeshAgent>().speed *= _multiple;
        }
        if(other.gameObject.layer == 3 && other.gameObject.GetComponent<AI>()!=null)
                GetComponent<Uint>()._attacking = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == _teamLayer)
        {
            other.GetComponent<Uint>()._hp /= _multiple;;
            other.GetComponent<Layer>()._damage /= _multiple;
            other.GetComponent<NavMeshAgent>().speed /= _multiple;
        }
    }
}