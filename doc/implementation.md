Here I'm going to describe how exactly my implementation works and hopefully explain why I chose to do things a certain way. 

- [(Pre-) Processing Audio](#pre--processing-audio)

# (Pre-) Processing Audio

* use _AudioClip.GetData_ to fill an array with audio samples
* get number of samples needed with _AudioClip.samples_
  * multiply with _AudioClip.Channels_ because _AudioClip.GetData_ returns samples in interleaved format:
    * If clip is in stereo, data will come back as [Left Channel, Right Channel, Left Channel, Right Channel, Left Channel, ...]
* Need to average every two samples together to get mono samples
* A **_LOT_** of work for the CPU:
  * If we have 5 minutes of stereo audio with a sampling rate of 48 000 samples per second
    * 300 seconds * 48 000 samples per second = 14 400 000 samples (_AudioClip.samples_)
      * **_BUT_** we are in stereo, so we double our sample count:
        * **14,400,000 * 2 = 28,800,000 samples**