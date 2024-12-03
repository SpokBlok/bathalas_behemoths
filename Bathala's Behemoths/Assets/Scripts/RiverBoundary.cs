using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverBoundary : MonoBehaviour
{
    // Reference to the MeshFilter component
    private MeshFilter meshFilter;
    private BoxCollider boxCollider;

    // Meshes to switch between
    public Mesh model1;
    public Mesh model2;

    void Start()
    {
        // Get the MeshFilter component attached to this object
        meshFilter = GetComponent<MeshFilter>();
        boxCollider = GetComponent<BoxCollider>();

        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter component not found on the object.");
        }
    }

    void Update()
    {
        if (PlayerStats.Instance.questComp)
        {
            DisableBoxCollider();
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