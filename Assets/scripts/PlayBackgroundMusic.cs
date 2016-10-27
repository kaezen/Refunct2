using UnityEngine;
using System.Collections;

public class PlayBackgroundMusic : MonoBehaviour {

    //load the music file and audiosource
    public AudioClip bgMusic;
    AudioSource musicPlayer;
	// Use this for initialization
	void Start () {

        //get the audio source, load the clip, make sure the music loops and play. nothing to it; 
        musicPlayer = GetComponent<AudioSource>();
        musicPlayer.clip = bgMusic;
        musicPlayer.loop = true;
        musicPlayer.Play();
	}
}
