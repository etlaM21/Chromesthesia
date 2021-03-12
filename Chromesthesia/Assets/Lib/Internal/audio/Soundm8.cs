using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

using System.Numerics;
using DSPLib;
public class Soundm8 : MonoBehaviour
{
    Main main;
	// TESTING ONLY
	//MainOnlyAudio main;
    AudioSource audioSource;
	SpectralFluxAnalyzer realTimeSpectralFluxAnalyzer;
	int numChannels;
	int numTotalSamples;
	public int sampleRate;
	float clipLength;
	float[] multiChannelSamples;
	public SpectralFluxAnalyzer preProcessedSpectralFluxAnalyzer;
	PlotController preProcessedPlotController;
	bool backgroundThreadCompleted;
	bool threeDimensionalSpectrumBuild;
	public bool preProcessSamples = true;
    public List<float[]> wholeSpectrum;

	int spectrumRows = 32;
	public List<Tuple<float, float[]>> simpleSpectrum;

	int[] bucketSizes;

	float[] scaleFactors;

    // Start is called before the first frame update
    void Start()
    {
        main = Main.Instance;
		// ONLY TESTIMG
		// main = GameObject.Find("Main").GetComponent<MainOnlyAudio>();;

        wholeSpectrum = new List<float[]>();
		simpleSpectrum = new List<Tuple<float, float[]>>();

		// Bucketsizes aree consistnet
		
		float spectrals = 1025;
		Debug.Log("spectrals: " + spectrals);
		bucketSizes = new int[spectrumRows];
		for(int i = spectrumRows-1; i >= 0; i--){
			if(i != 0){
				if(i == spectrumRows-1){
					bucketSizes[i] = (int)spectrals;
				}
				else {
					bucketSizes[i] = (int)Math.Floor(spectrals * (toLog((float)i, 0.1f, (float)spectrumRows) / (float)spectrumRows));
				}
			}
			else {
				bucketSizes[i] = 0;
			}
			Debug.Log("bucketSizes[" + i + "]: " + bucketSizes[i]);
		}
		
		scaleFactors = new float[spectrumRows];
		float scaleFactor = 100;
		for(int i = 0; i < scaleFactors.Length; i++){
			float relativeScaleFactor = 1 + scaleFactor * ((float)bucketSizes[i] / spectrals); //(inverseToLog((float)bucketSizes[i], 0.1f, spectrals) / spectrals); // / spectrals); */
			Debug.Log("relativeScaleFactor for " + i + ": " + relativeScaleFactor);
			scaleFactors[i] = relativeScaleFactor; // relativeScaleFactor;
		}
		

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void processSignal(AudioSource audio){
        audioSource = audio;
        // Preprocess entire audio file upfront
		if (preProcessSamples) {
			preProcessedSpectralFluxAnalyzer = new SpectralFluxAnalyzer ();
			// preProcessedPlotController = GameObject.Find ("PreprocessedPlot").GetComponent<PlotController> ();

			// Need all audio samples.  If in stereo, samples will return with left and right channels interweaved
			// [L,R,L,R,L,R]
			multiChannelSamples = new float[audioSource.clip.samples * audioSource.clip.channels];
			numChannels = audioSource.clip.channels;
			numTotalSamples = audioSource.clip.samples;
			clipLength = audioSource.clip.length;

			// We are not evaluating the audio as it is being played by Unity, so we need the clip's sampling rate
			this.sampleRate = audioSource.clip.frequency;

			audioSource.clip.GetData(multiChannelSamples, 0); // Fills mutliChannelSamples[] !
			Debug.Log ("GetData done");

			Thread bgThread = new Thread (this.getFullSpectrumThreaded);

			Debug.Log ("Starting Background Thread");
			bgThread.Start ();
		}
    }

    public int getIndexFromTime(float curTime) {
		float lengthPerSample = this.clipLength / (float)this.numTotalSamples;

		return Mathf.FloorToInt (curTime / lengthPerSample);
	}

	public float getTimeFromIndex(int index) {
		return ((1f / (float)this.sampleRate) * index);
	}

	public void getFullSpectrumThreaded() {
		try {
			// We only need to retain the samples for combined channels over the time domain
			float[] preProcessedSamples = new float[this.numTotalSamples];

			int numProcessed = 0;
			float combinedChannelAverage = 0f;
			for (int i = 0; i < multiChannelSamples.Length; i++) {
				combinedChannelAverage += multiChannelSamples [i];

				// Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
				if ((i + 1) % this.numChannels == 0) {
					preProcessedSamples[numProcessed] = combinedChannelAverage / this.numChannels;
					numProcessed++;
					combinedChannelAverage = 0f;
				}
			}

			Debug.Log ("Combine Channels done");
			Debug.Log (preProcessedSamples.Length);

			// Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
			int spectrumSampleSize = 2048;
			
			int iterations = preProcessedSamples.Length / spectrumSampleSize;

			FFT fft = new FFT ();
			fft.Initialize ((UInt32)spectrumSampleSize);

			Debug.Log (string.Format("Processing {0} time domain samples for FFT", iterations));
			double[] sampleChunk = new double[spectrumSampleSize];
			for (int i = 0; i < iterations; i++) {
				// Grab the current 2048 chunk of audio sample data
				Array.Copy (preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

				// Apply our chosen FFT Window
				double[] windowCoefs = DSP.Window.Coefficients (DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
				double[] scaledSpectrumChunk = DSP.Math.Multiply (sampleChunk, windowCoefs);
				double scaleFactor = DSP.Window.ScaleFactor.Signal (windowCoefs);

				// Perform the FFT and convert output (complex numbers) to Magnitude
				Complex[] fftSpectrum = fft.Execute (scaledSpectrumChunk);
				double[] scaledFFTSpectrum = DSPLib.DSP.ConvertComplex.ToMagnitude (fftSpectrum);
				scaledFFTSpectrum = DSP.Math.Multiply (scaledFFTSpectrum, scaleFactor);

				// These 2048 magnitude values correspond (roughly) to a single point in the audio timeline
				float curSongTime = getTimeFromIndex(i) * spectrumSampleSize;

				
				addToSpectrum(Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
				/* Start only for testing */
				//TestSpectrumBoy.instSpectrumRow(Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
				/* End only for testing */

				// Send our magnitude data off to our Spectral Flux Analyzer to be analyzed for peaks

				preProcessedSpectralFluxAnalyzer.analyzeSpectrum (Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
				
			}

			Debug.Log ("Spectrum Analysis done");
			Debug.Log ("Background Thread Completed");
            simpleSpectrum.Sort((a, b) => a.Item1.CompareTo(b.Item1));
			main.backgroundThreadFinished();
				
		} catch (Exception e) {
			// Catch exceptions here since the background thread won't always surface the exception to the main thread
			Debug.Log (e.ToString ());
		}
	}

    public void addToSpectrum(float[] spectrum, float time){
		wholeSpectrum.Add(spectrum);
		addToSimpleSpectrum(time, spectrum);
	}

	public void addToSimpleSpectrum(float time, float[] spectrum) {
		/*
		float stepFactor = 1.17f; // Results in 32 buckets
		float stepSize = 1f;
		List<int> bucketSizes = new List<int>();
		while(true){
			// increase stepSize each iteration by stepFactor, so we get bigger Buckets
			stepSize = stepSize * stepFactor;
			// check if the next bucket + the sum of all others will be bigger than our spectrum
			// if so: just calculate the buckets needed to represent the whole spectrum
			if(bucketSizes.Sum() + stepSize > spectrum.Length){
				bucketSizes.Add(spectrum.Length - bucketSizes.Sum());
				break;
			}
			else {
				bucketSizes.Add((int)Math.Floor(stepSize));
			}
		}
		float[] scaledSpectrumsPerRow = new float[spectrumRows];
		for (int i = 0; i < bucketSizes.Count; i++) {
			float max = 0f;
			int startBucket = 0;
			for(int o = 0; o < i; o++){
				startBucket = startBucket + bucketSizes[o];
			}
			int endBucket = startBucket + bucketSizes[i];
			for (int o = startBucket; o < endBucket; o++) {
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
		*/
		// v2 ??
		/*float spectrals = spectrum.Length;
		Debug.Log("spectrals: " + spectrals);
		int[] bucketSizes = new int[spectrumRows];
		for(int i = spectrumRows-1; i >= 0; i--){
			if(i != 0){
				if(i == spectrumRows-1){
					bucketSizes[i] = (int)spectrals;
				}
				else {
					bucketSizes[i] = (int)Math.Floor(spectrals * (toLog((float)i, 0.1f, (float)spectrumRows) / (float)spectrumRows));
				}
			}
			else {
				bucketSizes[i] = 0;
			}
			Debug.Log("bucketSizes[" + i + "]: " + bucketSizes[i]);
		}*/
		float[] scaledSpectrumsPerRow = new float[spectrumRows];
		for (int i = 0; i < bucketSizes.Length; i++) {
			float max = 0f;
			int startBucket = 0;
			if(i > 0){
				startBucket = bucketSizes[i-1];
			}
			/*for(int o = 0; o < i; o++){
				startBucket = startBucket + bucketSizes[o];
			}*/
			//int endBucket = startBucket + bucketSizes[i];
			int endBucket = bucketSizes[i];
			if(i == 0){
				endBucket = 1;
			}
			// Debug.Log("endBucket: " + endBucket + ", startBucket: " + startBucket);
			/* float scaleFactor = 1;
			float relativeScaleFactor = 1 + scaleFactor * (inverseToLog((float)startBucket, 0.1f, spectrals)); // / spectrals); */
			for (int o = startBucket; o < endBucket; o++) {
				if(spectrum[o]*scaleFactors[i] > max){
				//if(spectrum[o] * relativeScaleFactor > max){
					// The spectrum values go from 0.0 to 0.5
					// Some values go a little over 0.5 but we cap it at 0.5
					max = spectrum[o]*scaleFactors[i]; //* relativeScaleFactor;
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
		simpleSpectrum.Add(new Tuple<float, float[]>(time, scaledSpectrumsPerRow));
	}

	public float calcSecondsPerFFTChunk(float sampleRate){
			float secondsPerMusicSample = 1f / sampleRate;
			float secondsPerFFTChunk = secondsPerMusicSample * 1024f;
			return secondsPerFFTChunk;
	}

	public float toLog (float value, float min, float max){
		float exp = (value-min) / (max-min);
		return min * (float)Math.Pow(max/min, exp);
	}

	public float inverseToLog (float value, float max, float min){
		float exp = (value-min) / (max-min);
		return min * (float)Math.Pow(max/min, exp);
	}
}
