# 1st Term Project Diary
## 20/11/04
**Main Concept**

 _Create a (visually pleasing) environment in VR which reacts to music._

* analyze audio and pass values of frequency regions (FFT)
* have elements react to values / value changes of assigned frequency regions
  * play assigned animation at threshold
  * map (transform) parameter to frequency regions' value 
* ability to view scene with VR headset

**Worst Case**

* ability to analyze audio and pass values of frequency regions for **one single song**
* have elements react to values / value changes of assigned frequency regions
  * play assigned animation at threshold
  * map (transform) parameter to frequency regions' value 
* ability to view scene with VR headset

**Best Case**

* **implement ability for user to play any song he wants**
  * **implement file explorer in UI / implement streaming API ( SoundCloud ? )**
* ability to analyze audio and pass values of frequency regions for **every given song**
* have elements react to values / value changes of assigned frequency regions
  * **give user ability to fine-tune pre-assigned frequency regions (on the fly)**
    * play assigned animation at threshold
    * map (transform) parameter to frequency regions' value 
      * **give user ability to fine-tune mapped parameters (on the fly)**
* ability to **view multiple scenes** with VR headset
  * **implement UI to transition between scenes**

---

## 20/11/08

_searched around for inspiration and audio-reactive works in Unity_

**Particle Audio Spectrum - Unity Package Asset** by [luxgile](https://github.com/luxgile)

[![Particle Audio Spectrum - Unity Package Asset](http://img.youtube.com/vi/f7iVopYjeGw/0.jpg)](https://www.youtube.com/watch?v=f7iVopYjeGw "Particle Audio Spectrum - Unity Package Asset")

[GitHub](https://github.com/luxgile/Particle-Audio-Spectrum)

**Unity VFX Graph / particles reacting to audio** by [Ji young Kim](https://www.youtube.com/channel/UCdyKY9xtRFDhcDihMKpxELg)

[![Unity VFX Graph / particles reacting to audio](http://img.youtube.com/vi/Py-hdvfq_4I/0.jpg)](https://www.youtube.com/watch?v=Py-hdvfq_4I "Unity VFX Graph / particles reacting to audio")

**String Theory (Music Visualization)** by [Chris Jones](https://www.chrisj.com.au/)

[![String Theory (Music Visualization)](http://img.youtube.com/vi/SZzehktUeko/0.jpg)](https://www.youtube.com/watch?v=SZzehktUeko "String Theory (Music Visualization)")

**Lapalux - '4EVA (feat. Talvi)'** by [Lapalux](https://www.youtube.com/channel/UC--yIemFNSgwQ0JxyYsABAQ)

[![Lapalux - '4EVA (feat. Talvi)'](http://img.youtube.com/vi/XreMtQz6HkY/0.jpg)](https://www.youtube.com/watch?v=XreMtQz6HkY "Lapalux - '4EVA (feat. Talvi)'")

Animation: Jakub Valtar

Art Direction: Marielle Tepper

---

* Unity's AudioSource GameObject has a [method called "GetSpectrumData"](https://docs.unity3d.com/ScriptReference/AudioSource.GetSpectrumData.html) which handles all the FFT and is able to return frequencies' values
* particle systems reacting seem to be the easiest to completely immerse the user in the music visualization
  * problem: it's hard to see exactly which particles are reacting to what, experience could be more that _something_ seems to be reacting _somehow_ to music
* the waveform itself can become an element
  * by constantly creating new waveforms in front of the user and letting the older ones fly by a tunnel effect can be achieved ( maybe speed affected by loudness ? )
* the rendering itself can become affected by audio and break / gitch according to song

---

## 20/11/13

_had a talk with my advisor, Lena, today and she made some good points_

**What is my project _actually_ about? Just visualizing music is pretty whack.**

**Why does it have to be in VR? What can VR do for my project, that can't be done else?**

One overarchieving theme could be **learning about and exploring music on a deeper level**. Visualizations of frequencies could have the chance that user's are able to see sounds they didn't hear at first and then concentrate on them to actually hear them. Like how you sometimes find interesting places in streets you thought you knew, just because you spotted them on Google Map's visualization of your surrondings.

Visualization of music has the chance to break up the complexity of some songs in seperate parts, without disrupting the sonic experience.
* A "tutorial" could make the user aware of what the element's animated represent: which frequency spectrum and what instrument's usually play there. In-Game it would be then easier to associate movement's of the elements to changes to in the frequency
* Looking at an element could also trigger a pop-up displaying technical information: What frequency spectrum is represented, show it's current value or it's value change over x time

Still, we **need a way to make use of the virtual environment and it's space**.
* Maybe use spatial audio to highlight certain frequencies at certain locations?
* Maybe give the user the ability to "walk" through parts of the song, visualized as waveforms?

**IDEA FOR THE THEME**

The VR headset is a "musical micoscope", giving you the ability to look into the sonic waves and their changes over time. You enter the world and you're in a scenery resembling something like the matrix. There are waveforms, frequency bands and all kind of stuff in your crazy labratory.

The style is a mixture of Ghetto-Cyberpunk or Steamwork and artificial digital screens.