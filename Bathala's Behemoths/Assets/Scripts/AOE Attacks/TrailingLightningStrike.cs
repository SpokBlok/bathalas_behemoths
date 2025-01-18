using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingLightningStrike : MonoBehaviour
{
    public TambanokanoLightningStrike lightningPrefab;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
