using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Main : MonoBehaviour {

	bool backgroundThreadCompleted;
	bool threeDimensionalSpectrumBuild;
	Soundm8 Soundm8;
	AudioSource AudioSource;
	SpectrumBoy SpectrumBoy;
	GameObject Player;
	public float secondsPerFFTChunk;
	void Start() {
		Player = GameObject.Find("Player");
		AudioSource = GetComponent<AudioSource> ();
		Soundm8 = GameObject.Find("Soundm8").GetComponent<Soundm8> ();
		backgroundThreadCompleted = false;
		threeDimensionalSpectrumBuild = false;

		/* THIS IS ONLY IN TESTING */

		SpectrumBoy = GameObject.Find("SpectrumBoy").GetComponent<SpectrumBoy> ();

		/* END ONLY IN TESTING */

		InitializeChromesthesia();
		
	}

	void InitializeChromesthesia(){
		Soundm8.processSignal(AudioSource);
	}

	void Update() {
		if(backgroundThreadCompleted == true && threeDimensionalSpectrumBuild == false) {
			AudioSource.Play(); // CAN ONLY BE CALLED FROM MAIN THREAD
			Debug.Log ("calling TestSpectrumBoy to build the thing!");
			threeDimensionalSpectrumBuild = true;
			Debug.Log (string.Format("threeDimensionalSpectrumBuild = ", threeDimensionalSpectrumBuild));
			SpectrumBoy.buildSpectrumGraph();
		}
		if(backgroundThreadCompleted == true && threeDimensionalSpectrumBuild == true) {
			Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, songPositionToWorldPosition(AudioSource.time));
			SpectrumBoy.updateSpectrumGraph(Player.transform.position.z);
			// SpectrumBoy.updateSpectrumGraph(AudioSource.time);
			// spectrumRealTime.updateRealTimeSpectrumGraph();
		}
	}

	public void backgroundThreadFinished(){
		backgroundThreadCompleted = true;
		secondsPerFFTChunk = Soundm8.calcSecondsPerFFTChunk(Soundm8.sampleRate);
		SpectrumBoy.setSecondsPerFFTChunk(Soundm8.calcSecondsPerFFTChunk(Soundm8.sampleRate));
		SpectrumBoy.setSpectrum(Soundm8.simpleSpectrum);
	}

	public float songPositionToWorldPosition(float songTime){
		return (songTime/secondsPerFFTChunk)/2;
	}

}