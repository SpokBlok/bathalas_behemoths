using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelPicker : MonoBehaviour
{
    public SteveAnimController steveModel = null;
    public MSAnimController mountedModel = null;
    public FollowTargetSwitcher camTargetSwitcher;

    // Start is called before the first frame update
    void Start()
    {
        steveModel = FindAnyObjectByType<SteveAnimController>(FindObjectsInactive.Include);
        mountedModel = FindAnyObjectByType<MSAnimController>(FindObjectsInactive.Include);
        camTargetSwitcher = FindAnyObjectByType<FollowTargetSwitcher>(FindObjectsInactive.Include);

        if (PlayerStats.Instance.tammyScene || PlayerStats.Instance.markyScene)
        {
            steveModel.gameObject.SetActive(false);
            mountedModel.gameObject.SetActive(true);
            camTargetSwitcher.FollowManny();
        }
        else if (PlayerStats.Instance.ruinsScene)
        {
            steveModel.gameObject.SetActive(true);
            mountedModel.gameObject.SetActive(false);
            camTargetSwitcher.FollowSteve();
        }
        else if (PlayerStats.Instance.outdoorsScene && PlayerStats.Instance.introDone == false)
        {
            steveModel.gameObject.SetActive(true);
            mountedModel.gameObject.SetActive(false);
            camTargetSwitcher.FollowSteve();

            // For testing Purposes on the Anim Test Scene
            // steveModel.gameObject.SetActive(false);
            // mountedModel.gameObject.SetActive(true);
        }
        else
        {
            steveModel.gameObject.SetActive(false);
            mountedModel.gameObject.SetActive(true);
            camTargetSwitcher.FollowManny();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
