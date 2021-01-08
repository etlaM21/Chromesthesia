using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public GameObject cameraToControl;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraUpdate();
    }
    
    /* Camera Control
     * Code by Fuzzy Logic on stackexachange
     * https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera
     */
    public float camSpeedH = 4.0f;
    public float camSpeedV = 4.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void cameraUpdate () {

        // Camera Change
        yaw += camSpeedH * Input.GetAxis("Mouse X");
        pitch -= camSpeedV * Input.GetAxis("Mouse Y");

        cameraToControl.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
