![CHROMESTHESIA](doc/img/logocyber_shadow.png)

**_Chromesthesia is a interactive audio visualizer. It's an audiovisual experience for VR meant to function both as new way to listen to songs as well as to learn about audio frequencies and audio manipulation. But most importantly, it's fun._**

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

With a spectral flux analysis extreme changes in amplitudes are detected and these "beatpoints" are affecting the shading of the material to have it "glow" on beat by brighthening the material. **How the tunnel behaves is solely is exactly how the sound behaves**.

### **Playfully learning how audio works**

For the "learning" part of the experience I figured playful interactions would be great way to show the user certain characteristics of sound. This way there are no boring panels to be read, but rather **different aspects to be figured out with a fun user experience**. I divided the main aspects into two parts:
- _how audio effects shape the sound_
- _what frequencies make up the sound_

To learn about audio effects, the user has two sliders in his "command center" with which he or she is able to interact with the sound: a **chorus** and **reverb effect**. I chose these two effects because most other sound effects like a delay, flanger, distort etc. have to have a whole lot of parameters to be fiddled around with in order to make them sound "good".

The chorus and reverb effects on the other hand work quite well already simply by choosing their wetness in the mix. I figured that **overwhelming the user with too many parameters would rather scare them away** from playing around after not hearing something pleasant at first, so a "dumbed down" mini-version of sound effects would keep the fun in the learning experience.

In order to show the user what the certain frequencies are that are surronding them (on a sonic level), I gave him / her two modes to activate: **Soloing and muting**.  To specify which frequencies to solo or mute the user can simply fly into them with the controller. How strongly the frequencies are solo'd / muted depends on how deep the user chooses to "dive" into the frequency. **The deeper he/she is within a frequency, the stronger the chosen mode is**.

To further clarify to the user what he/she is actually doing, a **realtime spectrum** is displayed in the command center. It shows **the user in which frequency he/she currently is** and also colors the bar on the spectrum according to the extent to which he/she is applying the solo/mute on that frequency.

## **FFT and Spectral Flux Analysis**

### **Why are they needed?**