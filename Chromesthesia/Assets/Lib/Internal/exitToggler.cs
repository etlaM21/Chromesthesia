using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exitToggler : MonoBehaviour
{

    private string leftHand = "LeftHandAnchor";
    private string rightHand = "RightHandAnchor";

    public float timePassed = 0;
    
    public GameObject timeDisplay;

    public Material  defaultMaterial;
    public Material  activeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        timeDisplay.GetComponent<Text>().text = "";
        transform.GetChild(0).GetComponent<MeshRenderer>().material = defaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider colObj)
    {
        if(colObj.name == leftHand || colObj.name == rightHand){
            transform.GetChild(0).GetComponent<MeshRenderer>().material = activeMaterial;
            timePassed += Time.deltaTime;
            timeDisplay.GetComponent<Text>().text = Mathf.Round(3-timePassed).ToString(); // Changing text
            if(timePassed >= 3f){
                // Go back to SelectScene by stopping Audio Playback, causing main to trigger end function
                Main.Instance.AudioSource.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider colObj)
    {
        if(colObj.name == leftHand || colObj.name == rightHand){
            transform.GetChild(0).GetComponent<MeshRenderer>().material = defaultMaterial;
            timePassed = 0;
            // disable display
            timeDisplay.GetComponent<Text>().text = "";//Changing text
        }
    }
}
