using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Main_backup : MonoBehaviour {

	bool backgroundThreadCompleted;
	bool threeDimensionalSpectrumBuild;
	Soundm8 Soundm8;
	AudioSource AudioSource;

	AudioManipul8r AudioManipul8r;
	SpectrumBoy SpectrumBoy;
	GameObject Player;
	public float secondsPerFFTChunk;
	void Start() {
		Player = GameObject.Find("Player");
		AudioSource = GetComponent<AudioSource> ();
		AudioManipul8r = GameObject.Find("Soundm8").GetComponent<AudioManipul8r> ();
		Soundm8 = GameObject.Find("Soundm8").GetComponent<Soundm8> ();
		backgroundThreadCompleted = false;
		threeDimensionalSpectrumBuild = false;
		SpectrumBoy = GameObject.Find("SpectrumBoy").GetComponent<SpectrumBoy> ();

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
			
			// MAKE NEW
			SpectrumBoy.BuildVerticesSpectrum(0);
			SpectrumBoy.UpdateMesh();
			//SpectrumBoy.buildSpectrumGraph();
		}
		if(backgroundThreadCompleted == true && threeDimensionalSpectrumBuild == true) {
			// PLayer Movement -> Put thjis in right scritp !!
			// Forward / Backward
			float fwdSpeed = 1f;
			float bckwdSpeed = 2f; // Backward should be a little higher, because Audio is playing back. So if we go back 1 sec every sec, we actually stay in place
			if (Input.GetKey(KeyCode.Q)) {
				AudioSource.time = AudioSource.time + fwdSpeed * Time.deltaTime;
			}

			if (Input.GetKey(KeyCode.E)) {
				AudioSource.time = AudioSource.time - bckwdSpeed * Time.deltaTime;
			}

			Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, songPositionToWorldPosition(AudioSource.time));

			// Manipulate Audio according to rotation (freq solo or mute)
			Debug.Log(Player.transform.eulerAngles.z);
			// We calculate the players current height against the outer Radius of our tunnel: When he is exactly at the lowest, the gain is 0, else it is 1 ("normal")
			float gain = (Player.GetComponent<PlayerMovement>().PlayerPosition.transform.localPosition.y / SpectrumBoy.outerRadius) * 1 + 1;
			// We set the frequency by calculating his rotation in euelerAngles (0 to 360) logarithmically against the 22.000 hz spectrum. This is kind of accurate.
			float freq = 22000f * (toLog(Player.transform.eulerAngles.z, 0.1f, 360) / 360);
			//AudioManipul8r.setMuteFreq(freq, gain);
			// We calculate the players current height against the outer Radius of our tunnel: When he is exactly at the lowest, the range is 0, else it is 22000f ("normal")
			
			Debug.Log(gain);
			float soloRange = toLog(gain, 0.01f, 1) * 22000f + 10;
			Debug.Log(soloRange);


			AudioManipul8r.setSoloFreq(freq, soloRange);

			// MAKE NEW
			SpectrumBoy.UpdateSpectrumForward(Player.transform.position.z);
			SpectrumBoy.UpdateMesh();
			// SpectrumBoy.updateSpectrumGraph(Player.transform.position.z);


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

	public float toLog (float value, float min, float max){
		float exp = (value-min) / (max-min);
		return min * (float)Math.Pow(max/min, exp);
	}
}