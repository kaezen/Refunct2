using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    //get the linked wall and lightbeam
    public GameObject wall;
    public GameObject lightBeam;

    void Start()
    {//the light beams are invisible while underwater
        lightBeam.SetActive(false);
    }

    void Update()
    {//as long as the lightbeam exists
        if (!lightBeam == false)
        {  //if the lightbeam is above the water, and inactive, make it active
            if (transform.position.y >= 0 & !lightBeam.activeInHierarchy) lightBeam.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
       // print("IT WORKS");

        //if the player steps on the button
        if (collider.tag == "Player" && transform.position.y > 0)
        {
            //wall.WallUp;
            /*  while(wall.transform.position.y < 0)
              {
                  wall.transform.position = new Vector3(wall.transform.position.x, wall.transform.position.y + raiseSpeed, wall.transform.position.z);

              }*/
              //turn off the beam
            Destroy(lightBeam);
            //move the connected wall up
            wall.GetComponent<WallUp>().wallUp();
        }

    }
}