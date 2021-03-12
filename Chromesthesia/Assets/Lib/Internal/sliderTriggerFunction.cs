using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderTriggerFunction : MonoBehaviour
{
    // we always call execute from the slider to make it easier, so we define the mode here
    // there are three modes:
    // 01: "reverb"
    // 02: "chorus"
    // 03: "flanger"
    public string job;

    public GameObject soundm8;

    public bool reverbOn = false;

    public bool chorusOn = false;

    public bool flangerOn = false;

    public int currentValue = 0;

    void Start(){

        soundm8  = GameObject.Find("Soundm8");

    }
    public void execute(int value){
        //Debug.Log("Parent executing!");

        if(job == "reverb"){
            soundm8.GetComponent<AudioManipul8r>().setReverb((float) value - 20);
        }

        if(job == "chorus"){
            soundm8.GetComponent<AudioManipul8r>().setChorus((float) value - 20);
        }

        if(job == "flanger"){
            soundm8.GetComponent<AudioManipul8r>().setFlanger((float) value - 20);
        }
    }

    public void activateEffect(){
        if(job == "reverb"){
            reverbOn = true;
        }

        if(job == "chorus"){
            chorusOn = true;
        }

        if(job == "flanger"){
            chorusOn = true;
        }

        execute(currentValue);
    }
     public void deActivateEffect(){
        if(job == "reverb"){
            reverbOn = false;
        }

        if(job == "chorus"){
            chorusOn = false;
        }

        if(job == "flanger"){
            chorusOn = false;
        }
        execute(0);
    }
}
