using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightningStrike : BaseSkill
{
    private PlayerInput input;
    public LightningStrikeFillEffect lightningPrefab;
    private Vector3 lightningPosition;
    private bool charged;
    private float timer;
    public float attackGapDuration;
    public float attacksLeft;

    // Start is called before the first frame update
    void Start()
    {
        maxCharges = 1;
        cooldown = 10;
        charged = false;
        timer = 0;
        attackGapDuration = 3;
        attacksLeft = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (charged)
        {
            if (timer < attackGapDuration)
                {
                timer += Time.deltaTime;
            }
            else if (attacksLeft > 0)
            {
                LightningStrikeFillEffect lightning = Instantiate(lightningPrefab, lightningPosition, Quaternion.identity);
                lightning.gameObject.transform.SetParent(gameObject.transform, false);
                timer = 0f;
                attacksLeft--;
                Debug.Log("Attack!");
            } 
            else
            {
                Debug.Log("End achieved");
                attacksLeft = 6;
                charged = false;
            }
        }
        
    }

    public override IEnumerator RunSkill()
    {
        player = GameObject.FindWithTag("Player");
        input = GameObject.FindWithTag("Player Input").GetComponent<PlayerInput>();
        
        input.actions["Move"].Disable();
        lightningPosition = player.transform.position;
        yield return new WaitForSeconds(1.0f); //Charge up time, animation of lightning strike
        
        input.actions["Move"].Enable();
        charged = true;
        Debug.Log("Run Skill finished");
    }
}
