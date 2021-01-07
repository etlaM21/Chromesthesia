using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioSourceGetSpectrumDataExample : MonoBehaviour
{
    public GameObject AudioSourceToAnalyze;

    private AudioSource ActualAudioSource;

    void Start(){
        ActualAudioSource = AudioSourceToAnalyze.GetComponent<AudioSource>();
        Debug.Log(ActualAudioSource);
        PreProcessAudio();
    }

    void Update()
    {
        // RealTimeFFT();
    }

    void RealTimeFFT() {
        /*
        * This is a function implemeninting FFT in realtime by making use of Unity's GetSpectrumData() function:
        * https://docs.unity3d.com/ScriptReference/AudioSource.GetSpectrumData.html
        */
        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        }
    }

    void PreProcessAudio() {
        /*
        * This is an implementation of "Algorithmic Beat Mapping in Unity: Preprocessed Audio Analysis" by Jesse from GiantScam:
        * https://medium.com/giant-scam/algorithmic-beat-mapping-in-unity-preprocessed-audio-analysis-d41c339c135a
        */
        AudioSource aud = ActualAudioSource;
        /*  float[] samples = new float[aud.clip.samples * aud.clip.channels];
        aud.clip.GetData(samples, 0);   */

        // Need all audio samples.  If in stereo, samples will return with left and right channels interweaved
        // [L,R,L,R,L,R]
        float[] multiChannelSamples = new float[aud.clip.samples * aud.clip.channels];
        float numChannels = aud.clip.channels;
        float numTotalSamples = aud.clip.samples;
        float clipLength = aud.clip.length;

        // We are not evaluating the audio as it is being played by Unity, so we need the clip's sampling rate
        float sampleRate = aud.clip.frequency;

        aud.clip.GetData(multiChannelSamples, 0);
        for (int i = 0; i < multiChannelSamples.Length; ++i)
        {
            Debug.Log(multiChannelSamples[i]);
        }
    }
}