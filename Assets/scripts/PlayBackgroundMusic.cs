using UnityEngine;
using System.Collections;
/// <summary>
/// this class controls the background music
/// </summary>
public class PlayBackgroundMusic : MonoBehaviour {
    /// <summary>
    /// load the music file and audiosource
    /// </summary>
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
