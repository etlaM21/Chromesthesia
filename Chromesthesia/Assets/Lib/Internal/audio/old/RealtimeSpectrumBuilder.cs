using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeSpectrumBuilder : MonoBehaviour {

	public GameObject myPrefab;
	int spectrumRows = 64;
	public float[] realTimeSpectrum;
	public List<GameObject> realTimeSpectrumObjects;

	public GameObject audioPlayer;

	public AudioSource audioPlayerSource;

	// Use this for initialization
	void Start () {
		realTimeSpectrum = new float[64];
		realTimeSpectrumObjects = new List<GameObject>();
		instSimple2DRealTimeSpectrum();
		audioPlayerSource = audioPlayer.GetComponent<AudioSource> ();
	}

	public void instSimple2DRealTimeSpectrum(){
		for (int i = 0; i < spectrumRows; i++) {
			realTimeSpectrumObjects.Add(Instantiate(myPrefab, new Vector3(i, 10, 0), Quaternion.identity));
		}
	}

	public void updateRealTimeSpectrumGraph(){
		AudioListener.GetSpectrumData(realTimeSpectrum, 0, FFTWindow.Hanning);
		for (int i = 0; i < spectrumRows; i++) {
			realTimeSpectrumObjects[i].transform.localScale = new Vector3(realTimeSpectrumObjects[i].transform.localScale.x,  realTimeSpectrum[i]*10+1, realTimeSpectrumObjects[i].transform.localScale.z);
		}
	}

}
