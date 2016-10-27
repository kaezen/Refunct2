using UnityEngine;
using System.Collections;

public class SquashAndStretch : MonoBehaviour {

    public float squash = .7f;
    public AudioClip compress;
    AudioSource playSound;
	
    void Start()
    {
        playSound = GetComponent<AudioSource>();
    }
void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            transform.localScale = new Vector3(1, squash, 1);
        }
        else transform.localScale = Vector3.one;
    }
}
