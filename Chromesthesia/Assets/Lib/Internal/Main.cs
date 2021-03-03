using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	// Singleton stuff !
	/*
	private static Main _instance;
    
    void Awake(){

        if (_instance == null){

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
    
            //Rest of your Awake code
    
        } else {
            Destroy(this);
        }
    } */

	// Singleton Stuff v2
	// Code by: CaptainRedmuff
	// URL: https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
	private static Main _instance;

    public static Main Instance
    {
        get
        {
			// If I would be good programmer, I would check every scene if it already has a "Main" gameObject and then delete this new instance to keep the old
			// Instead, I'm an even better programmer and just use 2 scenes and pray to god that I can't fuck it up with a setup this simple!
            /*if(_instance == null)
            {
                _instance = new GameObject("Main");
                _instance.AddComponent<Main>();
            }*/

            return _instance;
        }
    }

    void Awake()
    {
		if (_instance == null){

			_instance = this;
			DontDestroyOnLoad(this.gameObject);

			//Rest of your Awake code

		} else {
			Destroy(this);
		}
    }

	// Scene Management
	public bool inChromesthestia = false;

	void OnEnable(){
		Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
		if(scene.name == "Chromesthesia"){
			inChromesthestia = true;
			InitializeChromesthesia();
		}
		else {
			inChromesthestia = false;
		}
        Debug.Log(mode);
    }


	// Chromestia Scene Variables
	bool backgroundThreadCompleted;
	bool threeDimensionalSpectrumBuild;
	Soundm8 Soundm8;
	AudioManipul8r AudioManipul8r;
	public AudioSource AudioSource;

	SpectrumBoy SpectrumBoy;
	GameObject Player;
	public float secondsPerFFTChunk;
	void Start() {
		AudioSource = GetComponent<AudioSource>();
	}

	public void InitializeChromesthesia(){
		Player = GameObject.Find("Player");
		Soundm8 = GameObject.Find("Soundm8").GetComponent<Soundm8>();
		AudioManipul8r = GameObject.Find("Soundm8").GetComponent<AudioManipul8r>();
		backgroundThreadCompleted = false;
		threeDimensionalSpectrumBuild = false;
		SpectrumBoy = GameObject.Find("SpectrumBoy").GetComponent<SpectrumBoy> ();
		Soundm8.processSignal(AudioSource);
	}

	void Update() {
		// Confirm Chromesthesia Scene
		if(inChromesthestia){
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
				Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, songPositionToWorldPosition(AudioSource.time));

				/*
				* AUDIO FX STUFF
				*/

				// Manipulate Audio according to rotation (freq solo or mute)
				Debug.Log(Player.transform.eulerAngles.z);
				// We calculate the players current height against the outer Radius of our tunnel: When he is exactly at the lowest, the gain is 0, else it is 1 ("normal")
				float gain = (Player.GetComponent<PlayerMovement>().PlayerPosition.transform.localPosition.y / SpectrumBoy.outerRadius) * 1 + 1;
				// We set the frequency by calculating his rotation in euelerAngles (0 to 360) logarithmically against the 22.000 hz spectrum. This is kind of accurate.
				float freq = 22000f * (toLog(Player.transform.eulerAngles.z, 0.1f, 360) / 360);
				AudioManipul8r.setMuteFreq(freq, gain);
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

	public void testSingleton(){
		Debug.Log("Hello, I'm the Singleton!");
		Debug.Log(this.gameObject);
	}

}