using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using System.Numerics;
using DSPLib;


public class MainOnlyAudio : MonoBehaviour {

	bool backgroundThreadCompleted;
	bool threeDimensionalSpectrumBuild;
	Soundm8 Soundm8;
	AudioSource AudioSource;

	/* THIS IS ONLY IN TESTING */
	TestSpectrumBoy TestSpectrumBoy;

	RealtimeSpectrumBuilder spectrumRealTime;

	/* END ONLY IN TESTING */

	void Start() {

		AudioSource = GetComponent<AudioSource> ();
		Soundm8 = GameObject.Find("Soundm8").GetComponent<Soundm8> ();
		backgroundThreadCompleted = false;
		threeDimensionalSpectrumBuild = false;

		/* THIS IS ONLY IN TESTING */

		TestSpectrumBoy = GameObject.Find("TestSpectrumBoy").GetComponent<TestSpectrumBoy> ();

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
			TestSpectrumBoy.buildSpectrumGraph();
		}
		if(backgroundThreadCompleted == true && threeDimensionalSpectrumBuild == true) {
			TestSpectrumBoy.updateSpectrumGraph(AudioSource.time);
			// spectrumRealTime.updateRealTimeSpectrumGraph();
		}
	}

	public void backgroundThreadFinished(){
		backgroundThreadCompleted = true;
		TestSpectrumBoy.setSpectrum(Soundm8.simpleSpectrum);
	}
}