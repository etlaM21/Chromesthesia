using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beatTrigger : MonoBehaviour
{
    public GameObject spectrumBoy;
    public bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        spectrumBoy = GameObject.Find("SpectrumBoy");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z <= Main.Instance.Player.transform.position.z && activated == false){
            spectrumBoy.GetComponent<SpectrumBoy>().BeatHitChangeShaderMat();
            activated = true;
        }
    }
}
