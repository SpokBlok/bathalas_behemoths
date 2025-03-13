using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingLightningStrike : MonoBehaviour
{
    public FillEffect lightningPrefab;
    private GameObject player;
    private float timer = 0;
    public float attackGapDuration;
    public float attacksLeft;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (timer < attackGapDuration)
            {
            timer += Time.deltaTime;
        }
        else if (attacksLeft > 0)
        {
            FillEffect lightning = Instantiate(lightningPrefab, player.transform.position, Quaternion.identity);
            lightning.gameObject.transform.SetParent(gameObject.transform, false);
            timer = 0f;
            attacksLeft--;

            Tambanokano tammy = GameObject.FindWithTag("Tambanokano").GetComponent<Tambanokano>();
            tammy.BlinkOnce();
        } 
        else
        {
            Destroy(gameObject);
        }
    }
}
