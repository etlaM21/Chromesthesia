using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flymodeToggle : MonoBehaviour
{
    private string leftHand = "LeftHandAnchor";
    private string rightHand = "RightHandAnchor";
    
    public GameObject soundm8;

    public Material  defaultMaterial;
    public Material  soloMaterial;

    public Material muteMaterial;

    public int currentIndex = 0;

    public string [] modes = new string[] {"default", "solo", "mute"};

    void Start(){
        activateState(currentIndex);
    }

    private void OnTriggerEnter(Collider colObj)
    {
        if(colObj.name == leftHand || colObj.name == rightHand){
            if(currentIndex < 3){
                currentIndex ++;
            }
            else {
                currentIndex = 0;
            }
            activateState(currentIndex);
        }
    }

    public void activateState(int index){
        string mode = modes[index];
        if(mode == "default"){
            soundm8.GetComponent<AudioManipul8r>().setMuteToogle(false);
            soundm8.GetComponent<AudioManipul8r>().setSoloToogle(false);
            transform.GetChild(0).GetComponent<MeshRenderer>().material = defaultMaterial;

        }
        if(mode == "solo"){
            soundm8.GetComponent<AudioManipul8r>().setMuteToogle(false);
            soundm8.GetComponent<AudioManipul8r>().setSoloToogle(true);
            transform.GetChild(0).GetComponent<MeshRenderer>().material = soloMaterial;

        }
        if(mode == "mute"){
            soundm8.GetComponent<AudioManipul8r>().setMuteToogle(true);
            soundm8.GetComponent<AudioManipul8r>().setSoloToogle(false);
            transform.GetChild(0).GetComponent<MeshRenderer>().material = muteMaterial;

        }
    }
}
