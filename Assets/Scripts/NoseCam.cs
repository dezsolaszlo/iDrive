using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseCam : MonoBehaviour
{
    public CarControl target;
    private Vector3 offSetDir;

    // Start is called before the first frame update
    void Start()
    {
        offSetDir = transform.position - target.transform.position;
        offSetDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = target.transform.position + offSetDir*0.5f;
    }
}
