using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowTargetSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private Transform SteveTarget;
    [SerializeField] private Transform MountedModelTarget;
    private Transform followHolder;
    public Transform newFollowTarget;
    public Transform currentFollowTarget;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerStats.Instance.outdoorsScene && PlayerStats.Instance.introDone == true)
            virtualCam.Follow = GameObject.Find("MannyFollowTarget").transform;
        else
            virtualCam.Follow = GameObject.Find("SteveFollowTarget").transform;

        SteveTarget = FindAnyObjectByType<SteveFollowTarget>(FindObjectsInactive.Include).transform;
        MountedModelTarget = FindAnyObjectByType<MannyFollowTarget>(FindObjectsInactive.Include).transform;
        currentFollowTarget = SteveTarget;
        newFollowTarget = MountedModelTarget;

        Vector3 alignedRotation = new Vector3(0f, SteveTarget.eulerAngles.y, 0f);
        virtualCam.transform.rotation = Quaternion.Euler(alignedRotation);
    }

    public void SwitchFollowTarget()
    {
        if (virtualCam != null && newFollowTarget != null)
        {
            virtualCam.Follow = newFollowTarget;

            // Reassigns current and new followTargets so that next switch is correct
            followHolder = currentFollowTarget;
            currentFollowTarget = newFollowTarget;
            newFollowTarget = followHolder;
        }
    }

    public void FollowSteve()
    {
        if (virtualCam != null && newFollowTarget != null)
        {
            virtualCam.Follow = SteveTarget;
        }
    }

    public void FollowManny()
    {
        if (virtualCam != null && newFollowTarget != null)
        {
            virtualCam.Follow = MountedModelTarget;
        }
    }

    public void SetFollowTarget(Transform target)
    {
        newFollowTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
