using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderTrigger : MonoBehaviour
{
    private string leftHand = "LeftHandAnchor";
    private string rightHand = "RightHandAnchor";

    public bool activated = false;

    public Material  inActiveMaterial;

    public Material  activeMaterial;
    private void OnTriggerEnter(Collider colObj)
    {
        if(colObj.name == leftHand || colObj.name == rightHand){
            activate();
            activateAllLower();
            deActivateAllHigher();
            activateToggle();
            // Call parent script
            GameObject sliderParent = transform.parent.transform.parent.gameObject;
            // Activate parent
            sliderParent.GetComponent<sliderTriggerFunction>().activateEffect();
            // Set value
            int value = int.Parse(this.name);
            sliderParent.GetComponent<sliderTriggerFunction>().currentValue = value;
            sliderParent.GetComponent<sliderTriggerFunction>().execute(value);
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

    public void activateAllLower(){
        Transform triggerHolder = transform.parent;
        foreach(Transform trigger in triggerHolder) {
            GameObject triggerObj = trigger.gameObject;
            if(triggerObj.name != "Toggle"){
                if(int.Parse(triggerObj.name) < int.Parse(this.name)){
                    triggerObj.GetComponent<sliderTrigger>().activate();
                } 
            }
        }
    }

    public void deActivateAllHigher(){
        Transform triggerHolder = transform.parent;
        foreach(Transform trigger in triggerHolder) {
            GameObject triggerObj = trigger.gameObject;
            if(triggerObj.name != "Toggle"){
                if(int.Parse(triggerObj.name) > int.Parse(this.name)){
                    triggerObj.GetComponent<sliderTrigger>().deactivate();
                } 
            }
        }
    }

    public void activateToggle(){
        Transform triggerHolder = transform.parent;
        foreach(Transform trigger in triggerHolder) {
            GameObject triggerObj = trigger.gameObject;
            if(triggerObj.name == "Toggle"){
                triggerObj.GetComponent<sliderToggle>().activate();
            }
        }
    }
}
