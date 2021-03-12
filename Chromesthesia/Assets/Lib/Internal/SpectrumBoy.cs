using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectrumBoy : MonoBehaviour {

	public Main main;

	public GameObject myPrefab;	

	public List<Tuple<float, float[]>> spectrum;
	SpectrumObject SpectrumObject;

	public List<SpectrumObject[]> spectrumList;
	public float currentTime = 0;

	public int currentIndex = 0;
	
	public int rows = 500;
	public int spectrals = 32;

	public float secondsPerFFTChunk;

	public bool tunnelGrid = true;

	public float radius = 40f;
	
	public float outerRadius = 20f;

	public float innerRadius = 5f;

	public GameObject hitObject;

	public GameObject RealTimeFrequencyCanvas;

	public GameObject CanvasFrequencyPrefab;

	Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
	Color[] colors;

	public Material shaderMat;
	int spectralRow = 0;
	int currentSpectral = 0;
	int zOff = 0;


	// Use this for initialization
	void Start () {
		main = Main.Instance;

		spectrumList = new List<SpectrumObject[]>();

		mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateTunnel();
        UpdateMesh();

		InitiateRealTimeCanvas();
		/*
		for(int i = 0; i < maxObjects; i++){
			SpectrumObject[] currentRowOfObjects = new SpectrumObject[spectrumRows];
			for(int o = 0; o < spectrumRows; o++){
				if(tunnelGrid == true) {
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
				}		
			}
			spectrumList.Add(currentRowOfObjects);
		} */
	}

	public void setSpectrum(List<Tuple<float, float[]>> newSpectrum){
		spectrum = newSpectrum;
	}

	public void setSecondsPerFFTChunk(float chunkSize){
		secondsPerFFTChunk = chunkSize;
	}

	public Vector2 calculateCirclePosition(int circlePosition, int numberofObjectsInCircle, float radius){
        float angle = circlePosition * Mathf.PI * 2  / (numberofObjectsInCircle);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        Vector2 position = new Vector2(x, y);
        return position;
    }

	public float calculateYAgainstPlayerDistance(Vector3 currentVector, float minY, float minDist, float initialY){
		float dist = Vector3.Distance(currentVector, Main.Instance.Player.GetComponent<PlayerMovement>().PlayerPosition.transform.position);
		// Debug.Log(dist);
		if(dist < minDist){
			float newY = initialY + (minDist - dist);
			if(newY < minY){
				return newY;
			}
			else {
				return minY;
			}
		}
		else {
			return initialY;
		}
	}

	public void UpdateMesh(){
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
		mesh.colors = colors;

        mesh.RecalculateNormals();
    }

	public void CreateTunnel(){
		vertices = new Vector3[(spectrals*2) * (rows*2)];
		zOff = 0;
		for (int i = 0, z = 0; z < rows*2; z++){
			zOff = zOff + z % 2;
			for (int x = 0; x < spectrals; x++){
				Vector2 circlePosition = calculateCirclePosition(x, spectrals, outerRadius);
				vertices[i] = new Vector3(circlePosition.x, circlePosition.y, zOff);
				i++;
				circlePosition = calculateCirclePosition(x+1, spectrals, outerRadius);
				vertices[i] = new Vector3(circlePosition.x, circlePosition.y, zOff);
				i++;
			}
		}

		triangles = new int[6 * spectrals*4 * rows*2];

		int vert = 0;
		int tris = 0;
		for (int z = 0; z < rows+1; z++){
			for (int x = 1; x < spectrals*2; x++){
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + spectrals*2 + 0;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + spectrals*2 + 0;
				triangles[tris + 5] = vert + spectrals*2 + 1;

				vert++;
				tris += 6;
			}
			vert++;
		}

		colors = new Color[vertices.Length];
		for(int i = 0; i < vertices.Length; i++){
			colors[i] = new Color(0,0,0,0);
		}
	}

	public void BuildVerticesSpectrum(int startRow){
		spectralRow = startRow;
		zOff = startRow;
		vertices = new Vector3[(spectrals*2) * (rows*2)];
		colors = new Color[vertices.Length];
		float radiusFromPlayer = innerRadius * 2;
		for (int i = 0, z = 0; z < rows*2; z++){
			zOff = zOff + z % 2;
			float relativeDistFromMid = Mathf.Abs((z-rows)/rows); // DOESNT WORK ?!?!?!
			for (int x = 0; x < spectrals; x++){
				// RADIUS: outerRadius - (outerRadius - innerRadius) * Amplitude
				float initialRadius = outerRadius - (outerRadius - innerRadius) * spectrum[spectralRow].Item2[x];
				Vector2 circlePosition = calculateCirclePosition(x, spectrals, initialRadius);
				Vector3 circlePositionWithPlayerDist = calculateCirclePosition(x, spectrals, calculateYAgainstPlayerDistance(new Vector3(circlePosition.x, circlePosition.y, zOff), outerRadius, radiusFromPlayer, initialRadius));
				vertices[i] = new Vector3(circlePositionWithPlayerDist.x, circlePositionWithPlayerDist.y, zOff);
				//colors[i] = gradient.Evaluate(spectrum[spectralRow].Item2[x]);
				colors[i] = new Color(spectrum[spectralRow].Item2[x], relativeDistFromMid ,0,0);
				i++;
				initialRadius = outerRadius - (outerRadius - innerRadius) * spectrum[spectralRow].Item2[x];
				circlePosition = calculateCirclePosition(x+1, spectrals, initialRadius);
				circlePositionWithPlayerDist = calculateCirclePosition(x+1, spectrals, calculateYAgainstPlayerDistance(new Vector3(circlePosition.x, circlePosition.y, zOff), outerRadius, radiusFromPlayer, initialRadius));
				vertices[i] = new Vector3(circlePositionWithPlayerDist.x, circlePositionWithPlayerDist.y, zOff);
				//colors[i] = gradient.Evaluate(spectrum[spectralRow].Item2[x]);
				colors[i] = new Color(spectrum[spectralRow].Item2[x], relativeDistFromMid, 0,0);
				i++;
			}
			if (z % 2 == 1){
				spectralRow++;
			}
		}

		triangles = new int[6 * spectrals*4 * rows*2];

		int vert = 0;
		int tris = 0;
		for (int z = 0; z < rows+1; z++){
			for (int x = 1; x < spectrals*2; x++){
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + spectrals*2 + 0;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + spectrals*2 + 0;
				triangles[tris + 5] = vert + spectrals*2 + 1;

				vert++;
				tris += 6;
			}
			vert++;
		}
	}

	public void UpdateSpectrum(float playerPosZ){
		// Debug.Log(vertices[vertices.Length/4].z);
		if(playerPosZ > vertices[vertices.Length/4].z){
			BuildVerticesSpectrum((int)vertices[0].z + 2);
		}
		if(playerPosZ < vertices[vertices.Length/4].z){
			BuildVerticesSpectrum((int)vertices[0].z - 2);
		}

		// Update Colors
		float currentShaderBeatAmount = shaderMat.GetFloat("Vector1_HighAmount");
		if(currentShaderBeatAmount > 0){
			float nextValue = currentShaderBeatAmount - 3f * Time.deltaTime;
			Debug.Log("Lowering BeatHitValue to: " + nextValue);
			shaderMat.SetFloat("Vector1_HighAmount", nextValue);
		}
	}

	public void BeatHitChangeShaderMat(){
		shaderMat.SetFloat("Vector1_HighAmount", 1);
	}

	public void GenerateHitPoints(List<SpectralFluxInfo> spectralFluxInfo){
		
				for(int i = 0; i < spectralFluxInfo.Count; i++){
					if( spectralFluxInfo[i].isPeak){
						// Debug.Log("SpectralFlux at: " + Soundm8.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[i].time + "; SpectralFlux: " + Soundm8.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[i].spectralFlux + "; prunedSpectralFlux: " + Soundm8.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[i].prunedSpectralFlux + "; threshold: " + Soundm8.preProcessedSpectralFluxAnalyzer.spectralFluxSamples[i].threshold);
						Instantiate(hitObject, new Vector3(0, 0, Main.Instance.songPositionToWorldPosition(spectralFluxInfo[i].time)), Quaternion.identity);
					}
				}
	}

	public void InitiateRealTimeCanvas(){
		Vector2 canvasSize = RealTimeFrequencyCanvas.GetComponent<RectTransform>().sizeDelta;
        // Create Buttons from Song Array
        for(int i = 0; i < spectrals; i++){
			GameObject button = (GameObject)Instantiate(CanvasFrequencyPrefab);
            button.transform.SetParent(RealTimeFrequencyCanvas.transform);//Setting button parent
            float xPos = -canvasSize.x/2 +  i * canvasSize.x/spectrals + (canvasSize.x/spectrals/2);
            button.GetComponent<RectTransform>().localPosition = new Vector3(xPos, 0, 0);
            button.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
			float startSize = 1f * i * canvasSize.x/spectrals;
            button.GetComponent<RectTransform>().localScale = new Vector3(canvasSize.x/spectrals, startSize, 1);
			button.GetComponent<Image>().color = new Color(255, 255, 255, 255);
		}
	}

	public void updateRealTimeCanvas(float time, float playerHoverIndex, float gain){
		float closestDistance = 1000;
		int closestIndex = 0;
		for(int i = 0; i < spectrum.Count; i++){
			float currentDistance = Math.Abs(spectrum[i].Item1 - time);
			if(closestDistance > currentDistance){
				closestDistance = currentDistance;
				closestIndex = i;
			}
		}
		for(int i = 0; i < spectrals; i++){
			GameObject button = RealTimeFrequencyCanvas.transform.GetChild(i).gameObject;
			button.GetComponent<RectTransform>().localScale = new Vector3(button.GetComponent<RectTransform>().localScale.x, spectrum[closestIndex].Item2[i], 1);
			if(i == (int)playerHoverIndex){
				float redColor = Mathf.Round(gain * 255);
				Debug.Log(redColor);
				button.GetComponent<Image>().color = new Color(1, gain, 1, 1);
			}
			else {
				button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
			}
		}
	}
	
}
