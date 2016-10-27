using UnityEngine;
using System.Collections;

public class FirstWallUp : MonoBehaviour {

    //simply tells the first set of platforms to go up on start of the game
    public GameObject wall;
	void Start () {
        wall.GetComponent<WallUp>().wallUp();
    }
	

}
