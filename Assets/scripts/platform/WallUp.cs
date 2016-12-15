using UnityEngine;
using System.Collections;
/// <summary>
/// this class contains the function that makes the wall rise
/// triggered by the player stepping on a button
/// </summary>
public class WallUp : MonoBehaviour {
    /// <summary>
    /// if the platform needs to or is going up
    /// </summary>
    bool goingUp = false;
    /// <summary>
    /// the speed at which the platform rises
    /// </summary>
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
