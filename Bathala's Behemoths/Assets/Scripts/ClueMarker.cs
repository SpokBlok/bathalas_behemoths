using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueMarker : MonoBehaviour
{
    // Reference to the MeshFilter component
    private MeshFilter meshFilter;
    private BoxCollider boxCollider;
    private Renderer objectRenderer;
    private ParticleSystem particleSystem;

    // Meshes to switch between
    public Mesh model1;
    public Mesh model2;

    void Start()
    {
        // Get the MeshFilter component attached to this object
        meshFilter = GetComponent<MeshFilter>();
        boxCollider = GetComponent<BoxCollider>();
        objectRenderer = GetComponent<Renderer>();
        particleSystem = GetComponent<ParticleSystem>();
        ToggleObjectVisibility();
        TogglePause();

        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter component not found on the object.");
        }

        if (PlayerStats.Instance.introDone && PlayerStats.Instance.outdoorsScene)
        {
            SwitchToModel2();
            ToggleObjectVisibility();
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

    public void ToggleObjectVisibility()
    {
        if (objectRenderer != null)
        {
            // Toggle the renderer's enabled state (visible/invisible)
            objectRenderer.enabled = !objectRenderer.enabled;
        }
    }

    public void DisableBoxCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = false;  // Disable the BoxCollider
        }
    }

    public void EnableBoxCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = true;   // Enable the BoxCollider
        }
    }

    // Call this method to switch to model1
    public void SwitchToModel1()
    {
        if (meshFilter != null)
        {
            meshFilter.mesh = model1;  // Change the mesh to model1
        }
    }

    // Call this method to switch to model2
    public void SwitchToModel2()
    {
        if (meshFilter != null)
        {
            meshFilter.mesh = model2;  // Change the mesh to model2
        }
    }
}