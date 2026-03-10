using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public CarControl target;
    private Vector3 offSetDir;

    public float minDistance, maxDistance;
    private float activeDistance;

    public Transform startTargetOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        offSetDir = transform.position - startTargetOffset.position;
        activeDistance = minDistance;
        offSetDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        activeDistance = minDistance + ((maxDistance - minDistance)*(target.body.velocity.magnitude/target.maxSpeed)); 
        transform.position = target.transform.position + (offSetDir*activeDistance);
    }
}
