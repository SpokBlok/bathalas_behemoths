using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueMarkerParticles : MonoBehaviour
{
    // Reference to the MeshFilter component
    private ParticleSystem particleSystem;


    void Start()
    {
        // Get the MeshFilter component attached to this object
        particleSystem = GetComponent<ParticleSystem>();
        TogglePause();

        if (PlayerStats.Instance.introDone && PlayerStats.Instance.outdoorsScene)
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        if (particleSystem.isPlaying) // If the particles are playing
        {
            particleSystem.Pause(); // Pause the particle system
        }
        else
        {
            particleSystem.Play(); // Resume the particle system
        }
    }

}