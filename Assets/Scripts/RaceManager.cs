using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{

    public Checkpoint[] allCheckpoints;
    public static RaceManager instance;
    public GameObject lapTime;
    public GameObject bestLap;
    public int totalLaps = 5;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < allCheckpoints.Length; i++) {
            allCheckpoints[i].checkpointNo = i;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.time);
        /*lapTime.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)Time.time/60).ToString() + "m" + (Time.time%60).ToString("0,#.000") + "s";
        if (string.Compare(lapTime.GetComponent<TMPro.TextMeshProUGUI>().text, bestLap.GetComponent<TMPro.TextMeshProUGUI>().text) < 0)
        {
            bestLap.GetComponent<TMPro.TextMeshProUGUI>().text = lapTime.GetComponent<TMPro.TextMeshProUGUI>().text;
        }*/
    }
}
