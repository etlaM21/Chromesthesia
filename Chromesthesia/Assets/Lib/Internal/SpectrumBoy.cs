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
	
	public int rows = 500;
	public int spectrals = 32;

	public float secondsPerFFTChunk;

	public bool tunnelGrid = true;

	public float radius = 40f;
	
	public float outerRadius = 20f;

	public float innerRadius = 5f;

	// NEW NEW NEW

	Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
	Color[] colors;

	// public Gradient gradient;
	int spectralRow = 0;
	int zOff = 0;

	// Use this for initialization
	void Start () {
		main = GameObject.Find("Main").GetComponent<Main> (); // NOOOOT GOOD CHANGE THIS SHIT: CALLBACK WFROM MAIN ????

		spectrumList = new List<SpectrumObject[]>();

		mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateTunnel();
        UpdateMesh();
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
		for (int i = 0, z = 0; z < rows*2; z++){
			zOff = zOff + z % 2;
			float relativeDistFromMid = Mathf.Abs((z-rows)/rows); // DOESNT WORK ?!?!?!
			for (int x = 0; x < spectrals; x++){
				// RADIUS: outerRadius - (outerRadius - innerRadius) * Amplitude
				Vector2 circlePosition = calculateCirclePosition(x, spectrals, outerRadius - (outerRadius - innerRadius) * spectrum[spectralRow].Item2[x]);
				vertices[i] = new Vector3(circlePosition.x, circlePosition.y, zOff);
				//colors[i] = gradient.Evaluate(spectrum[spectralRow].Item2[x]);
				colors[i] = new Color(spectrum[spectralRow].Item2[x], relativeDistFromMid ,0,0);
				i++;
				circlePosition = calculateCirclePosition(x+1, spectrals, outerRadius - (outerRadius - innerRadius) * spectrum[spectralRow].Item2[x]);
				vertices[i] = new Vector3(circlePosition.x, circlePosition.y, zOff);
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

	public void UpdateSpectrumForward(float playerPosZ){
		// Debug.Log(vertices[vertices.Length/4].z);
		if(playerPosZ > vertices[vertices.Length/4].z){
			BuildVerticesSpectrum((int)vertices[0].z + 2);
		}
	}
}
