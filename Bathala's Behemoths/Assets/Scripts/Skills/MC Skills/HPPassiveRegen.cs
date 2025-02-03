using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class HPPassiveRegen : MonoBehaviour
{
    private Coroutine activeCoroutine;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        EventManager.OnTakingDamage += Damaged;
        EventManager.OnFullHealth += MaxHealth;
    }

    private IEnumerator PassiveRegen()
    {
        player = GameObject.FindWithTag("Player");
        Debug.Log("Passive Regen Start");
        yield return new WaitForSeconds(10);
        player.GetComponent<PlayerMovement>().Heal(5);
        if (!PlayerStats.Instance.IsMaxHealth())
        {
            activeCoroutine = StartCoroutine(PassiveRegen());
        }
        yield return null;
    }

    private void Damaged()
    {
        if (activeCoroutine == null)
        {
            Debug.Log("Damaged");
            activeCoroutine = StartCoroutine(PassiveRegen());
        }
    }

    private void MaxHealth()
    {
        if (activeCoroutine != null)
        {
            Debug.Log("Max Health");
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }
}
