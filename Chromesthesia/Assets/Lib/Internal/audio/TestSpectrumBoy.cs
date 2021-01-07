using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpectrumBoy : MonoBehaviour {

	public GameObject myPrefab;	

	public List<Tuple<float, float[]>> spectrum;

	public List<GameObject> spectrumObjects;
	public float currentTime;

	public int currentIndex;
	

	// Use this for initialization
	void Start () {
		spectrumObjects = new List<GameObject>();
	}

	public void setSpectrum(List<Tuple<float, float[]>> newSpectrum){
		spectrum = newSpectrum;
	}
	

	public void buildSpectrumGraph(){
		/*for (int i = 5000; i < 5500; i++) {
			instSpectrumRow(spectrum[i], i);
		}*/
		instSimple2DSpectrum();
	}

	public void instSimple2DSpectrum(){
		for (int i = 0; i < spectrum[0].Item2.Length; i++) { // spectrum[0].Item2.Length = spectrumRows (should be)
			//simpleSpectrumObjects.Add(Instantiate(myPrefab, new Vector3(i, spectrum[0].Item2[i], spectrum[0].Item1), Quaternion.identity));
			spectrumObjects.Add(Instantiate(myPrefab, new Vector3(i, -10, spectrum[0].Item1), Quaternion.identity));
			currentTime = spectrum[0].Item1;
			currentIndex = 0;
		}
	}

	float maxAmplitude = 0f;
	public void updateSpectrumGraph(float time){
		if(time > currentTime && currentIndex + 1 != spectrum.Count){
			Debug.Log("time: " + time);
			currentIndex = currentIndex + 1;
			currentTime = spectrum[currentIndex].Item1;
			for (int i = 0; i < spectrum[0].Item2.Length; i++) {
				// POSITION
				//simpleSpectrumObjects[i].transform.position = new Vector3(simpleSpectrumObjects[i].transform.position.x, spectrum[currentIndex].Item2[i]*10, simpleSpectrumObjects[i].transform.position.z);
				// SCALE
				Debug.Log("spectrum[currentIndex].Item2[" + i + "]: " + spectrum[currentIndex].Item2[i]);
				
				spectrumObjects[i].transform.localScale = new Vector3(spectrumObjects[i].transform.localScale.x, spectrum[currentIndex].Item2[i]*100+1, spectrumObjects[i].transform.localScale.z);
			}
		}
		//Debug.Log(time);
	}

}
