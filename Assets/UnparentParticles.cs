using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
public class UnparentParticles : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Particle[] _particles;
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particles = new Particle[_particleSystem.particleCount];
        
        _particleSystem.GetParticles(_particles);

        _particleSystem.transform.DetachChildren();

        //foreach (var particle in _particles)
        //{
        //    particle.
        //}
    }

}
