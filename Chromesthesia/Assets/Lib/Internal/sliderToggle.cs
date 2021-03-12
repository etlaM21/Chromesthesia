using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderToggle : MonoBehaviour
{
    private string leftHand = "LeftHandAnchor";
    private string rightHand = "RightHandAnchor";

    public bool activated = false;

    public Material  inActiveMaterial;

    public Material  activeMaterial;

    private void OnTriggerEnter(Collider colObj)
    {
        if(colObj.name == leftHand || colObj.name == rightHand){
            GameObject sliderParent = transform.parent.transform.parent.gameObject;
            // Get current state from parent
            if(sliderParent.GetComponent<sliderTriggerFunction>().job == "reverb"){
                if(sliderParent.GetComponent<sliderTriggerFunction>().reverbOn){
                    sliderParent.GetComponent<sliderTriggerFunction>().deActivateEffect();
                    deactivate();
                }
                else {
                    sliderParent.GetComponent<sliderTriggerFunction>().activateEffect();
                    activate();
                }
            }

            if(sliderParent.GetComponent<sliderTriggerFunction>().job == "chorus"){
                if(sliderParent.GetComponent<sliderTriggerFunction>().chorusOn){
                    sliderParent.GetComponent<sliderTriggerFunction>().deActivateEffect();
                    deactivate();
                }
                else {
                    sliderParent.GetComponent<sliderTriggerFunction>().activateEffect();
                    activate();
                }
            }

            if(sliderParent.GetComponent<sliderTriggerFunction>().job == "flanger"){
                if(sliderParent.GetComponent<sliderTriggerFunction>().flangerOn){
                    sliderParent.GetComponent<sliderTriggerFunction>().deActivateEffect();
                    deactivate();
                }
                else {
                    sliderParent.GetComponent<sliderTriggerFunction>().activateEffect();
                    activate();
                }
            }
        }
    }

    public void activate(){
        activated = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = activeMaterial;
    }

    public void deactivate(){
        activated = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = inActiveMaterial;
    }
}
