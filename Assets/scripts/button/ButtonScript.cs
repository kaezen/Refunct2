using UnityEngine;
using System.Collections;
/// <summary>
/// this class says what happens when you step on a button
/// </summary>
public class ButtonScript : MonoBehaviour
{
    /// <summary>
    /// the linked wall
    /// </summary>
    public GameObject wall;
    /// <summary>
    /// the attached lightbeam
    /// </summary>
    public GameObject lightBeam;

    /// <summary>
    /// set the beam as inactive initially
    /// </summary>
    void Start()
    {//the light beams are invisible while underwater
        lightBeam.SetActive(false);
    }

    /// <summary>
    /// check if we need to activate the lightbeam
    /// </summary>
    void Update()
    {//as long as the lightbeam exists
        if (!lightBeam == false)
        {  //if the lightbeam is above the water, and inactive, make it active
            if (transform.position.y >= 0 & !lightBeam.activeInHierarchy) lightBeam.SetActive(true);
        }
    }
    /// <summary>
    /// when the button is stepped on, make the linked terrain (wall) rise from underwater
    /// </summary>
    /// <param name="collider">the player</param>
    void OnTriggerEnter(Collider collider)
    {
       // print("IT WORKS");

        //if the player steps on the button
        if (collider.tag == "Player" && transform.position.y > 0)
        {
              //turn off the beam
            Destroy(lightBeam);
            //move the connected wall up
            wall.GetComponent<WallUp>().wallUp();
        }

    }
}