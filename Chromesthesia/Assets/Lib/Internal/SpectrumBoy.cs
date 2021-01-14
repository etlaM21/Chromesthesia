using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumBoy : MonoBehaviour {

	public Main main; // NOOOOT GOOD CHANGE THIS SHIT: CALLBACK WFROM MAIN ????

	public GameObject myPrefab;	

	public List<Tuple<float, float[]>> spectrum;
	SpectrumObject SpectrumObject;

	public List<SpectrumObject[]> spectrumList;
	public float currentTime;

	public int currentIndex;
	
	public int maxObjects = 500;
	public int spectrumRows = 32;

	public float secondsPerFFTChunk;

	public bool tunnelGrid = true;

	public float radius = 40f;
	
	public float outerRadius = 20f;

	public float innerRadius = 5f;

	// Use this for initialization
	void Start () {
		main = GameObject.Find("Main").GetComponent<Main> (); // NOOOOT GOOD CHANGE THIS SHIT: CALLBACK WFROM MAIN ????

		spectrumList = new List<SpectrumObject[]>();

		
		
		for(int i = 0; i < maxObjects; i++){
			SpectrumObject[] currentRowOfObjects = new SpectrumObject[spectrumRows];
			for(int o = 0; o < spectrumRows; o++){
				if(tunnelGrid == true) {
					/* Tunnel CALC */
					currentRowOfObjects[o] = new SpectrumObject { 
						TimeInSong = (float) i, 
						Amplitude = 1f,
						Object = Instantiate(myPrefab, new Vector3(0,0,0), Quaternion.identity),
						indexInRow = o,
						numberOfObjects = spectrumRows,
						radiusOuter = outerRadius,
						radiusInner = innerRadius						
					};
					currentRowOfObjects[o].updateMesh(currentRowOfObjects[o].tunnelMeshPoints());
					currentRowOfObjects[o].updateRotation();



					/*Vector3 gridPos = new Vector3(0, 0, i);
					float angle = o * Mathf.PI * 2  / spectrumRows;
					float x = Mathf.Cos(angle) * radius;
					float y = Mathf.Sin(angle) * radius;
					Vector3 tunnelPos = gridPos + new Vector3(x, y, 0);
					float angleDegrees = +angle*Mathf.Rad2Deg+90;
					Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
					currentRowOfObjects[o] = new SpectrumObject { TimeInSong = (float) i, Object = Instantiate(myPrefab, tunnelPos, rot) };*/
				}
				else {
					// Old grid calculation
					Vector3 gridPos = new Vector3(o, 0, i); // old: Don't need x -> new x by circle
					currentRowOfObjects[o] = new SpectrumObject { TimeInSong = (float) i, Object = Instantiate(myPrefab, gridPos, Quaternion.identity) };
				}				
			}
			spectrumList.Add(currentRowOfObjects);
		}
	}

	public void setSpectrum(List<Tuple<float, float[]>> newSpectrum){
		spectrum = newSpectrum;
	}

	public void setSecondsPerFFTChunk(float chunkSize){
		secondsPerFFTChunk = chunkSize;
	}
	

	public void buildSpectrumGraph(){
		// Testing only
		// instSimple2DSpectrum();
		for (int i = 0; i < spectrumList.Count; i++) {
			for(int o = 0; o < spectrumRows; o++){
				spectrumList[i][o].TimeInSong =  spectrum[i].Item1;
				spectrumList[i][o].Amplitude =  spectrum[i].Item2[o];
				spectrumList[i][o].SecondsPerFFTChunk =  secondsPerFFTChunk;
				//spectrumList[i][o].updateScale();
				//spectrumList[i][o].updateZPosition();
				spectrumList[i][o].updateObject();
			}
			currentIndex++;
		}	
	}

	public void instSimple2DSpectrum(){
		for (int i = 0; i < spectrum[0].Item2.Length; i++) { // spectrum[0].Item2.Length = spectrumRows (should be)
			//simpleSpectrumObjects.Add(Instantiate(myPrefab, new Vector3(i, spectrum[0].Item2[i], spectrum[0].Item1), Quaternion.identity));
			//spectrumObjects.Add(Instantiate(myPrefab, new Vector3(i, -10, spectrum[0].Item1), Quaternion.identity));
			currentTime = spectrum[0].Item1;
			currentIndex = 0;
		}
	}

	float maxAmplitude = 0f;
	public void updateSpectrumGraph(float playerPosZ){
		if(playerPosZ > spectrumList[spectrumList.Count/2][0].Object.transform.position.z){
				currentIndex++;
				SpectrumObject[] rowToReorder = spectrumList[0];
				spectrumList.RemoveAt(0);
				for(int i = 0; i < rowToReorder.Length; i++){
					rowToReorder[i].TimeInSong =  spectrum[currentIndex].Item1;
					rowToReorder[i].Amplitude =  spectrum[currentIndex].Item2[i];
					//rowToReorder[i].updateScale();
					//rowToReorder[i].updateZPosition();
					rowToReorder[i].updateObject();
					
				}
				spectrumList.Add(rowToReorder);
		}
		

		// FIFO QUEUE
	}
}
