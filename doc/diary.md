# 1st Term Project Diary
- [1st Term Project Diary](#1st-term-project-diary)
  - [20/11/04](#201104)
  - [20/11/08](#201108)
  - [20/11/13](#201113)
  - [20/11/27](#201127)
  - [20/12/02](#201202)
  - [20/12/18](#201218)
  - [20/12/30](#201230)
  - [20/01/04](#200104)
  - [20/01/05](#200105)
  - [20/01/05](#200105-1)

---

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

---

## 20/11/27

_had a talk with my advisor again_

* 3D Spectrum as landscape
* Learning through itneracitivity
* Interaction changes visualization (changes music (lowpass etc.), so people learn as well?)

---


## 20/12/02

_presented my project and got nice input_

* Interacitivity can also mean adding sound objects ?
* Use Chroma feature or Chromagram instead of FFT because it "evens" out frequencies and displays frequencies' amplitudes more like they are actually heard by us

---

## 20/12/18

_talk with advisor Lena_

* [Made a schedule](schedule.md) for the upcoming weeks
  * Lena gave input on wether or not it's possible / does make sense

---

## 20/12/30

_one thing's for sure: we're not ahead of schedule_

* during the holidays I was able to implement background threaded audio analysis
  * [This implementation of it by Jesse from Giant Scam now is the base of my project](https://medium.com/giant-scam/algorithmic-beat-mapping-in-unity-preprocessed-audio-analysis-d41c339c135a)
  * new problems occured: we get **too much data** to display!

---

## 20/01/04

_first day of the project week: tried to build the world by reducing the data but still ran into problems_

* reduced the number of frequency bands to 24 by taking the median of a region
  * data seems fishy, maybe I implemented it wrong?
  * median also shouldn't be the way to go, so maybe I have to rewrite it
* even with reduced "detail" of frequency, it's still to much for the instantiating approach:
  * 24 * > 12 000 (for a 4-minute song) data points in time = 288 000
    * the solution might be to instantiate maybe 10 000 objects at the beginning and then just reposition them after they move out of sight ?


---

## 20/01/05

_reduced the amount of data and learned how fft actually works and what data i recieve_

---

## 20/01/05

_"fixed" my daat to soomehow resemble a proper spectrum like in a DAW_

![FFT comparison](img/FFT_comparisons.gif)