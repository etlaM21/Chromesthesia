using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManipulator : MonoBehaviour
{
    public static AudioManipulator instance;

    public AudioMixer mixer;

    bool muteFreq = false;

    bool soloFreq = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setReverb(float value){
        if(value != -20.0f) { // Slider maximum is -20f
            mixer.SetFloat("ReverbWetness", value);
        } 
        else {
            mixer.SetFloat("ReverbWetness", -80f);
        }
    }

    public void setChorus(float value){
        if(value != -20.0f) { // Slider maximum is -20f
            mixer.SetFloat("ChorusWetness", value);
        } 
        else {
            mixer.SetFloat("ChorusWetness", -80f);
        }
    }

    public void setFlanger(float value){
        if(value != -20.0f) { // Slider maximum is -20f
            mixer.SetFloat("FlangerWetness", value);
        } 
        else {
            mixer.SetFloat("FlangerWetness", -80f);
        }
    }

    public void setMuteFreq(float freqRange, float gain){
        mixer.SetFloat("MuteFreq", freqRange);
        mixer.SetFloat("MuteFreqGain", gain);
    }

    public void setMuteToogle(bool state){
        muteFreq = state;
        Debug.Log(muteFreq);
    }
    public void setSoloFreq(float freqRange, float soloRange) {
        // soloRange = soloRange / 2;
        float lowcutoff = freqRange - soloRange;
        if(lowcutoff < 10) {
            lowcutoff = 10;
        }
        if(lowcutoff > 22000) {
            lowcutoff = 22000;
        }
        float highcutoff = freqRange + soloRange;
        if(highcutoff < 10) {
            highcutoff = 10;
        }
        if(highcutoff > 22000) {
            highcutoff = 22000;
        }
        mixer.SetFloat("SoloLowPass", highcutoff);
        mixer.SetFloat("SoloHighPass", lowcutoff);
    }
    public void setSoloToogle(bool state){
        soloFreq = state;
        Debug.Log(soloFreq);
    }
}
