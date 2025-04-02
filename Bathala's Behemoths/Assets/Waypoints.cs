using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Waypoints : MonoBehaviour
{
    public Image clueWaypoint;
    public TextMeshProUGUI clueWPMeter;

    public Transform clue1Target;
    public Transform clue2Target;
    public Transform clue3Target;
    public Transform clue4Target;
    public Transform clue5Target;
    public Transform clue6Target;
    public Transform trackedClue;

    float minX;
    float maxX;

    float minY;
    float maxY;

    public Vector3 offset = new Vector3(0f, 20f, 0f);
    private Vector2 pos;
    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerStats.Instance.introDone && PlayerStats.Instance.ruinsVisitedOnce)
        {
            clueWaypoint.gameObject.SetActive(true);
        }
        else
        {
            clueWaypoint.gameObject.SetActive(false);
        }

        minX = clueWaypoint.GetPixelAdjustedRect().width / 2 + 5;
        maxX = Screen.width - minX;

        minY = clueWaypoint.GetPixelAdjustedRect().height / 2 + 5;
        maxY = Screen.height - minY;
        
        trackedClue = clue1Target;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.Instance.activeQuest1)
        {
            trackedClue = clue1Target;
        }
        else if(PlayerStats.Instance.activeQuest2)
        {
            trackedClue = clue2Target;
        }
        else if(PlayerStats.Instance.activeQuest3)
        {
            trackedClue = clue3Target;
        }
        else if(PlayerStats.Instance.activeQuest4)
        {
            trackedClue = clue4Target;
        }
        else if(PlayerStats.Instance.activeQuest5)
        {
            trackedClue = clue5Target;
        }
        else if(PlayerStats.Instance.activeQuest6)
        {
            trackedClue = clue6Target;
        }

        dist = Vector3.Distance(trackedClue.position, transform.position);
        pos = Camera.main.WorldToScreenPoint(trackedClue.position + offset);

        if(dist > 400)
        {
            pos.y = maxY;
        }
        
        if(Vector3.Dot((trackedClue.position - transform.position), transform.forward) < 0)
        {
            if(pos.y < Screen.height / 2)
            {
                pos.y = maxY;
            }
            else
            {
                pos.y = minY;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        clueWaypoint.transform.position = pos;
        clueWPMeter.text = (Vector3.Distance(trackedClue.position, transform.position)).ToString("0") + "m";
    }
}
