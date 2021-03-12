![CHROMESTHESIA](doc/img/logocyber_shadow.png)

**_Chromesthesia is an interactive audio visualizer._**

**_It's an audiovisual experience for VR meant to function both as new way to listen to songs as well as to learn about audio frequencies and audio manipulation. But most importantly, it's fun._**

You can **download the final release** here: https://github.com/etlaM21/Chromesthesia/releases/tag/v1.0 _(or just navigate to the "Releases" section of this repository)_. For installing the .apk on the Oculus Quest, I recommend using [SideQuest](https://sidequestvr.com/setup-howto), it's a third-party program which makes installing .apks possible by simply drag and dropping them.

The Unity project is in the directory _Chromesthesia_ the If you'd like to **clone this repository and look at the full prject in Unity** you need to **download additional texture files** from [here](https://maltehillebrand.de/Chromesthesia/tex.zip) and extract them into _Assets/Ressources/models/introRoom/tex_. This is because some textures are too large for GitHub.



# **Documentation**
If you'd like to have a day by day look into my process, you will find such in my [diary](doc/diary.md).

## **Building a world based on frequencies**

### **What do I want to show?**

While concepting this project two things became my main focus:
- _a visually pleasing experience_
- _a fun way to explore and learn about sound and frequencies_

### **The construction of the audiovisual experience**

I wanted to create a world where the user can quite literally "dive" into sounds and frequencies and learn playfully about their relation to a song while having a mesmerzing experience in virtual reality. One of the first steps though was to figure out a way to show a song as what it is: **an immersive, all surronding experience**. That's why I chose to build a tunnel around the user, so everything surronding him visually is the same as it's sonically: the sound.

The frequency amplitudes shape the structure of the tunnel: **the higher the amplitude, the closer the mesh is to the user**, just like it is louder in the ears, it's "louder" in the eyes.

With a spectral flux analysis extreme changes in amplitudes are detected and these "beatpoints" are affecting the shading of the material to have it "glow" on beat by brighthening the material. **How the tunnel behaves is exactly how the sound behaves**.

### **Playfully learning how audio works**

For the "learning" part of the experience I figured playful interactions would be great way to show the user certain characteristics of sound. This way there are no boring panels to be read, but rather **different aspects to be figured out with a fun user experience**. I divided the main aspects into two parts:
- _how audio effects shape the sound_
- _what frequencies make up the sound_

To learn about audio effects, the user has two sliders in his "command center" with which he or she is able to interact with the sound: a **chorus** and **reverb effect**. I chose these two effects because most other sound effects like a delay, flanger, distort etc. have to have a whole lot of parameters to be fiddled around with in order to make them sound "good".

The chorus and reverb effects on the other hand work quite well already simply by choosing their wetness in the mix. I figured that **overwhelming the user with too many parameters would rather scare them away** from playing around after not hearing something pleasant at first, so a "dumbed down" mini-version of sound effects would keep the fun in the learning experience.

In order to show the user what the certain frequencies are that are surronding them (on a sonic level), I gave him / her two modes to activate: **Soloing and muting**.  To specify which frequencies to solo or mute the user can simply fly into them with the controller. How strongly the frequencies are solo'd / muted depends on how deep the user chooses to "dive" into the frequency. **The deeper he/she is within a frequency, the stronger the chosen mode is**.

To further clarify to the user what he/she is actually doing, a **realtime spectrum** is displayed in the command center. It shows **the user in which frequency he/she currently is** and also colors the bar on the spectrum according to the extent to which he/she is applying the solo/mute on that frequency.

## **FFT and Spectral Flux Analysis**

### **What are they and why are they needed?**

The **_Fast Fourier Transform_** converts a signal from it's original domain to a representation in the frequency domain. By calculating the Discrete Fourier Transform of a sound it transform it's **structure of a waveform into it's sine components**, giving us the amplitudes for the frequencies of a sound at specific time intervals. This is the basis of the Chromesthesia world building.

Analyzing the **_Spectral Flux_** means that measuring the spectral change between two time intervals and computing their amplitude's difference. If there is a Spectral Flux, that means we have sudden, large change in a frequency's amplitude. For example when a kick drum is suddenly kicked and there is a spike in the lows. This is used to detect beatpoints in the case of Chromesthesia, but can also be used to calculate for example the BPM of a song.

### **Implementation**

To be honest, I'm way too dumb to implement these sort of things, at least with the time given for this project. Both implementations could probably be a first term project by themselves. But lucky for me, I stumbled upon [this implemenation by Jesse from GiantScam](https://medium.com/giant-scam/algorithmic-beat-mapping-in-unity-preprocessed-audio-analysis-d41c339c135a). It gave me a great **starting point for my project**, by both implementing a DFT and a Spectral Flux analysis in a pre-processing nature.

Pre-processing both is very important for this project. As we're building the world based on the frequencies, we can't relay on realtime audio analysis. That way we could only have frequencies being displayed in hindsight, literally. **By pre-processing both we can store the spectrum and Spectral Fluxes of a whole song** and then use this data to construct our world.

### **Working with the data**

The data of a DFT is a linear representation of a sample region at a given point in time. This is nice, because the data is _(for the most part, there are windowing functions etc.)_ is an uneffected display of the frequencies' amplitudes. **But music doesn't behave in a linear fashion**:

**Notes are logarithmically spaced**. The difference between consecutive low notes, say A2(110 Hz) and A2#(116.5 Hz) is 6.5 Hz while the difference between the same 2 notes on a higher octave A3(220 Hz) and A3#(233.1 Hz) is 13.1 Hz. That's why we need to logarithmically stagger our DFT spectrum to have our "music spectrum" when we apply to a given amount of "targets" in our tunnel radius (32 blocks in this implementation). To logarithmically stagger in a given range (the range of the spectrum of our DFT), we use this function:

    float toLog (float value, float min, float max){
        float exp = (value-min) / (max-min);
        return min * (float)Math.Pow(max/min, exp);
    }

This was the first step to a visual representation of the sound which is closer to how we percive music. Another problem was that **amplitudes of lower frequencies are much higher than the ones of the higher frequencies**. Because a higher frequency means a shorter signal, a lower frequency will always have a much greater amplitude when it's coming out of the DFT. This is accurate, but simply not how we hear these frequencies. Both could be percieved as having the same volume, while having drastically different amplitudes when calculated by a DFT.

To fix this problem, I simply inversed the logarithmic function and applied it to a scaling function. Resulting in **exponentially scaling the frequencies**. This way, lower frequencies where mostly unaffected by our scaling, while higher ones were immensly pushed. Resulting in a representation like one might have seen in a DAW's EQ.

After having quite the headache with the DFT, I turned to the data of our Spectral Flux analysis and was quite happy to have a much easier experience. A **Spectral Flux is defined by it's time and it's relative change** to the amplitudes of the freqency before it. Because the implementation I used had already set up the parameters for defining when such a change counts as a Spectral Flux, I could simply use the time data I recieved for my implementation.


## **Devevloping for VR / Oculus Quest**
### **Optimizing perfomance**


