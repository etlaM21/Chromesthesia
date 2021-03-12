using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freqModToggler : MonoBehaviour
{
    private string leftHand = "LeftHandAnchor";
    private string rightHand = "RightHandAnchor";

    public bool active = false;
    
    public GameObject soundm8;

    public GameObject otherButton;

    public Material  defaultMaterial;
    public Material  activeMaterial;

    public string mode;


    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = defaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider colObj)
    {
        if(colObj.name == leftHand || colObj.name == rightHand){
            if(active){
                deactivate();
            }
            else {
                activate();
            }
        }
    }

    public void activate(){
        active = true;
        if(mode == "solo"){
            soundm8.GetComponent<AudioManipul8r>().setSoloToogle(true);
        }
        if(mode == "mute"){
            soundm8.GetComponent<AudioManipul8r>().setMuteToogle(true);
        }
        transform.GetChild(0).GetComponent<MeshRenderer>().material = activeMaterial;
        otherButton.GetComponent<freqModToggler>().deactivate();
    }

    public void deactivate(){
        active = false;
        if(mode == "solo"){
            soundm8.GetComponent<AudioManipul8r>().setSoloToogle(false);

        }
        if(mode == "mute"){
            soundm8.GetComponent<AudioManipul8r>().setMuteToogle(false);
        }
        transform.GetChild(0).GetComponent<MeshRenderer>().material = defaultMaterial;
    }

}
