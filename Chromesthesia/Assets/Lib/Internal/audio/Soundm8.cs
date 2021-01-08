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
    Main main; // NOOOOT GOOD CHANGE THIS SHIT: CALLBACK WFROM MAIN ????
    AudioSource audioSource;
	SpectralFluxAnalyzer realTimeSpectralFluxAnalyzer;
	int numChannels;
	int numTotalSamples;
	public int sampleRate;
	float clipLength;
	float[] multiChannelSamples;
	SpectralFluxAnalyzer preProcessedSpectralFluxAnalyzer;
	PlotController preProcessedPlotController;
	bool backgroundThreadCompleted;
	bool threeDimensionalSpectrumBuild;
	public bool preProcessSamples = true;
    public List<float[]> wholeSpectrum;

	int spectrumRows = 32;
	public List<Tuple<float, float[]>> simpleSpectrum;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("Main").GetComponent<Main> (); // NOOOOT GOOD CHANGE THIS SHIT: CALLBACK WFROM MAIN ????

        wholeSpectrum = new List<float[]>();
		simpleSpectrum = new List<Tuple<float, float[]>>();
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
			preProcessedPlotController = GameObject.Find ("PreprocessedPlot").GetComponent<PlotController> ();

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
			//int spectrumSampleSize = 1024;
			int spectrumSampleSize = 2048; // Fucks the spectralFluxShit
			
			int iterations = preProcessedSamples.Length / spectrumSampleSize;

			FFT fft = new FFT ();
			fft.Initialize ((UInt32)spectrumSampleSize);

			Debug.Log (string.Format("Processing {0} time domain samples for FFT", iterations));
			double[] sampleChunk = new double[spectrumSampleSize];
			for (int i = 0; i < iterations; i++) {
				// Grab the current 1024 chunk of audio sample data
				Array.Copy (preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

				// Apply our chosen FFT Window
				double[] windowCoefs = DSP.Window.Coefficients (DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
				double[] scaledSpectrumChunk = DSP.Math.Multiply (sampleChunk, windowCoefs);
				double scaleFactor = DSP.Window.ScaleFactor.Signal (windowCoefs);

				// Perform the FFT and convert output (complex numbers) to Magnitude
				Complex[] fftSpectrum = fft.Execute (scaledSpectrumChunk);
				double[] scaledFFTSpectrum = DSPLib.DSP.ConvertComplex.ToMagnitude (fftSpectrum);
				scaledFFTSpectrum = DSP.Math.Multiply (scaledFFTSpectrum, scaleFactor);

				// These 1024 magnitude values correspond (roughly) to a single point in the audio timeline
				float curSongTime = getTimeFromIndex(i) * spectrumSampleSize;

				/* START MY SHITTY ADDON */
				// Test Building Shit
				addToSpectrum(Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
				//TestSpectrumBoy.instSpectrumRow(Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
				/* END MY SHITTY ADDON */

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
		float stepFactor = 1.17f; // Results in 32 buckets
		float stepSize = 1f;
		List<int> bucketSizes = new List<int>();
		while(true){
			stepSize = stepSize * stepFactor;
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
		simpleSpectrum.Add(new Tuple<float, float[]>(time, scaledSpectrumsPerRow));
	}

	public float calcSecondsPerFFTChunk(float sampleRate){
			float secondsPerMusicSample = 1f / sampleRate;
			float secondsPerFFTChunk = secondsPerMusicSample * 1024f;
			return secondsPerFFTChunk;
	}
}
