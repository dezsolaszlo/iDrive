using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CpChecker : MonoBehaviour
{

    public CarControl theCar;
    public GameObject lapDisplay;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint") {
            theCar.CheckpointHit(other.GetComponent<Checkpoint>().checkpointNo);
        }
    }
}
