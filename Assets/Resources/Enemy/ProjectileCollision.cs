using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject target; // Add this line

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target) // Change this line
        {
            // Instantiate the VFX at the projectile's position and rotation
            GameObject vfxInstance = Instantiate(explosionVFX, transform.position, transform.rotation);

            // Automatically destroy the VFX instance after the duration of the particle system
            ParticleSystem vfxParticleSystem = vfxInstance.GetComponent<ParticleSystem>();
            Destroy(vfxInstance, vfxParticleSystem.main.duration);

            // Handle damage to the target here if needed
            // ...

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
