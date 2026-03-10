using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] cameras;
    private int currentCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        /*cameras[0].SetActive(true);
        for (int i = 1; i < cameras.Length; i++) {
            cameras[i].SetActive(false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Camera")) {
            currentCamera++;
            if (currentCamera >= cameras.Length)
            {
                currentCamera = 0;
            }for (int i = 0; i < cameras.Length; i++) {
                if (i == currentCamera)
                {
                    cameras[i].SetActive(true);
                }
                else {
                    cameras[i].SetActive(false);
                }
            }
        }
    }
}
