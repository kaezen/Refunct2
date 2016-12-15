using UnityEngine;
using System.Collections;
/// <summary>
/// this class has two functions, when the player stands on the bounce pad, it squashes it
/// and also plays the sound for it... when i get one
/// </summary>
public class SquashAndStretch : MonoBehaviour {
    /// <summary>
    /// how much to squash by
    /// </summary>
    public float squash = .7f;
    /// <summary>
    /// the sound and audiosource
    /// </summary>
    public AudioClip compress;
    AudioSource playSound;
	/// <summary>
    /// instantiate the source
    /// </summary>
    void Start()
    {
        playSound = GetComponent<AudioSource>();
    }
    /// <summary>
    /// when stepped on, squash and play the sound
    /// </summary>
    /// <param name="collision">the player</param>
void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            transform.localScale = new Vector3(1, squash, 1);
            ///play the squash sound here
        }
        else transform.localScale = Vector3.one;
    }
}
