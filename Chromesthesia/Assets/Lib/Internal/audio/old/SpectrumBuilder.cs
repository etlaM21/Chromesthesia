using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumBuilder : MonoBehaviour {

	public GameObject myPrefab;

	int numSamples = 1024;

	int spectrumRows = 32;

	public List<float[]> wholeSpectrum;

	public List<float[]> simpleSpectrumOld;
	public List<Tuple<float, float[]>> simpleSpectrum;

	public List<GameObject> simpleSpectrumObjects;


	public float currentTime;

	public int currentIndex;

	// Use this for initialization
	void Start () {
		wholeSpectrum = new List<float[]>();
		simpleSpectrumOld = new List<float[]>();
		simpleSpectrum = new List<Tuple<float, float[]>>();
		simpleSpectrumObjects = new List<GameObject>();
	}

	public void addToSpectrum(float[] spectrum, float time){
		wholeSpectrum.Add(spectrum);
		addToSimpleSpectrum(time, spectrum);
	}

	public void addToSimpleSpectrum(float time, float[] spectrum) {
		/*
		step = 1.3
		step_size = 1
		step_sizes = []
		while True:
			step_size *= step
			if sum(step_sizes) + step_size > len(source):
				step_sizes.append(len(source)-sum(step_sizes))
				break
			else:
				step_sizes.append(round(step_size)) */
		
		float stepFactor = 1.17f; // Results in 32 buckets
		float stepSize = 1f;
		List<int> bucketSizes = new List<int>();
		//bucketSizes.Add(0);
		while(true){
			stepSize = stepSize * stepFactor;
			if(bucketSizes.Sum() + stepSize > spectrum.Length){
				bucketSizes.Add(spectrum.Length -bucketSizes.Sum());
				break;
			}
			else {
				bucketSizes.Add((int)Math.Floor(stepSize));
			}
		}
		float[] scaledSpectrumsPerRow = new float[spectrumRows];
		//Debug.Log(bucketSizes.Count);
		for (int i = 0; i < bucketSizes.Count; i++) {
			//Debug.Log(i);
			//Debug.Log("bucketSizes[" + i + "]: " + bucketSizes[i]);
			float max = 0f;
			int startBucket = 0;
			for(int o = 0; o < i; o++){
				startBucket = startBucket + bucketSizes[o];
			}
			int endBucket = startBucket + bucketSizes[i];
			//Debug.Log("i: " + i);
			//for (int o = bucketSizes[i]; o < bucketSizes[i+1]; o++) {
			for (int o = startBucket; o < endBucket; o++) {
				
				//Debug.Log("o: " + o);
				if(spectrum[o] > max){
					// The spectrum values go from 0.0 to 0.5
					// Some values go a little over 0.5 but we cap it at 0.5
					max = spectrum[o];
					if(max > 0.5f){
						max = 0.5f;
					}
					
				}
			}
			// Before we add the values (0.0 to 0.5) to our spectrum we multiply it by 2
			// This way we end up with values ranging from 0 to 1
			// Helps us to work with the data
			max = max * 2;
			scaledSpectrumsPerRow[i] = Mathf.Abs(max); // FFT can return negative spikes as the signal is a wave
		}



		// WRONG CALCULATION NOT THE MEDIAN !!!
		/*float spectralsInRow = spectrum.Length / spectrumRows;
		int spectralsToLookAt = (int)Math.Floor(spectralsInRow);
		float[] floatArray = new float[spectrumRows];
		for(int i = 0; i < spectrumRows; i++){	
			float median = 0f;
			float sum = 0f;
			float max = 0f;

			for (int o = spectralsToLookAt*i; o < spectralsToLookAt*(i+1); o++) {
				sum = sum + spectrum[o];
				if(spectrum[o] > max){
					max = spectrum[o];
				}
			}
			median = sum / spectralsToLookAt;	
			
			floatArray[i] = max;
			*/
			/*using (StreamWriter w = File.AppendText("log3.txt"))
			{
				w.WriteLine(median + ";" + i);
			} 
		}	*/
		//simpleSpectrum.Add((time, floatArray));
		simpleSpectrum.Add(new Tuple<float, float[]>(time, scaledSpectrumsPerRow));
	}

	public void addToSimpleSpectrumOld(float time, float[] spectrum) {
		// WRONG CALCULATION NOT THE MEDIAN !!!
		float spectralsInRow = spectrum.Length / spectrumRows;
		int spectralsToLookAt = (int)Math.Floor(spectralsInRow);
		float[] floatArray = new float[spectrumRows];
		for(int i = 0; i < spectrumRows; i++){	
			float median = 0f;
			float sum = 0f;
			for (int o = spectralsToLookAt*i; o < spectralsToLookAt*(i+1); o++) {
				sum = sum + spectrum[o];
				//Debug.Log(spectrum[o]);
			}
			median = sum / spectralsToLookAt;	
			
			floatArray[i] = median;

			/*using (StreamWriter w = File.AppendText("log3.txt"))
			{
				w.WriteLine(median + ";" + i);
			} */
		}	
		simpleSpectrumOld.Add(floatArray);
	}

	public void buildSpectrumGraph(){
		simpleSpectrum.Sort((a, b) => a.Item1.CompareTo(b.Item1));
		/*for (int i = 5000; i < 5500; i++) {
			instSpectrumRow(simpleSpectrum[i], i);
		}*/
		instSimple2DSpectrum();
	}

	public void buildSpectrumGraphOld(){
		/*for (int i = 0; i < wholeSpectrum.Count; i++) {
			instSpectrumRow(wholeSpectrum[i], i);
		}*/
		Debug.Log("simpleSpectrum.Count: ");
		Debug.Log(simpleSpectrumOld.Count);
		for (int i = 0; i < simpleSpectrumOld.Count; i++) {
			Debug.Log("simpleSpectrum[" + i + "].Length: ");
			Debug.Log(simpleSpectrumOld[i].Length);
			//instSpectrumRow(simpleSpectrum[i], i);
		}
		for (int i = 5000; i < 5500; i++) {
			instSpectrumRow(simpleSpectrumOld[i], i);
		}
	}

	public void instSimple2DSpectrum(){
		for (int i = 0; i < spectrumRows; i++) {
			//simpleSpectrumObjects.Add(Instantiate(myPrefab, new Vector3(i, simpleSpectrum[0].Item2[i], simpleSpectrum[0].Item1), Quaternion.identity));
			simpleSpectrumObjects.Add(Instantiate(myPrefab, new Vector3(i, -10, simpleSpectrum[0].Item1), Quaternion.identity));
			currentTime = simpleSpectrum[0].Item1;
			currentIndex = 0;
		}
	}

	float maxAmplitude = 0f;
	public void updateSpectrumGraph(float time){
		if(time > currentTime && currentIndex + 1 != simpleSpectrum.Count){
			Debug.Log("time: " + time);
			currentIndex = currentIndex + 1;
			currentTime = simpleSpectrum[currentIndex].Item1;
			for (int i = 0; i < spectrumRows; i++) {
				// POSITION
				//simpleSpectrumObjects[i].transform.position = new Vector3(simpleSpectrumObjects[i].transform.position.x, simpleSpectrum[currentIndex].Item2[i]*10, simpleSpectrumObjects[i].transform.position.z);
				// SCALE
				Debug.Log("simpleSpectrum[currentIndex].Item2[" + i + "]: " + simpleSpectrum[currentIndex].Item2[i]);
				
				simpleSpectrumObjects[i].transform.localScale = new Vector3(simpleSpectrumObjects[i].transform.localScale.x, simpleSpectrum[currentIndex].Item2[i]*100+1, simpleSpectrumObjects[i].transform.localScale.z);
			}
		}
		//Debug.Log(time);
	}
	public void instSpectrumRow(float[] spectrum, float time){
		//for (int i = 0; i < numSamples; i++) {
		for (int i = 0; i < spectrumRows; i++) { /** WAAYYY TOO MANY !!! FIX !!!! ***/
			instBox(i, spectrum[i], time);
		}
	}

	public void instBox(int xCoord, float yCoord, float zCoord) {
		Instantiate(myPrefab, new Vector3(xCoord, yCoord*10, zCoord), Quaternion.identity);
	}

}
