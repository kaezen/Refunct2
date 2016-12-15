using UnityEngine;
using System.Collections;
/// <summary>
/// this class just causes the first wall to raise when the game starts
/// </summary>
public class FirstWallUp : MonoBehaviour {
    /// <summary>
    /// the wall
    /// </summary>
    public GameObject wall;
    /// <summary>
    /// make the wall go up
    /// </summary>
	void Start () {
        wall.GetComponent<WallUp>().wallUp();
    }
	

}
