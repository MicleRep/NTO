using System.Collections;
using UnityEngine;

public class VFX : MonoBehaviour
{
    [SerializeField] ParticleSystem[] _particles;

    IEnumerator Start()
    {
        while(true)
        {
            foreach(ParticleSystem particle in _particles)
                particle.Play();
            yield return new WaitForSeconds(2);
        }
    }
}
