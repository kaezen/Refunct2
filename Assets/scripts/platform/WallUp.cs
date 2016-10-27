using UnityEngine;
using System.Collections;

public class WallUp : MonoBehaviour {

    //we use this to know if the plaform needs to go up
    bool goingUp = false;
    //the speed at which the platform rises
    public float upSpeed = 17;


    /// <summary>
    /// This function checks if the platform needs to rise, if so
    /// it will keep rising until it's y position is >= 0;
    /// </summary>
    void Update()
    {
        if (transform.position.y >= 0) goingUp = false;
        if (goingUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y +Time.deltaTime* upSpeed, transform.position.z);
        }
       // print(goingUp);
    }

    /// <summary>
    /// This function is called externally by the corresponding button to make the platform rise
    /// goingUp is set to true so the platform knows to rise
    /// </summary>
public void wallUp()
    {
      //  print("trigger");
        goingUp = true;
    }
}
