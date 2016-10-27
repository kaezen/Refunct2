using UnityEngine;
using System.Collections;

public class SetStartingHeight : MonoBehaviour {

	/// <summary>
    /// This script sets the starting height of all the platforms to the same number, but lets
    /// me do basic level design with all of them over the water, then upon game loading they all 
    /// snap below the water to the appropriate height
    /// </summary>
	void Start () {
        transform.position = new Vector3(transform.position.x, GlobalVariables.startHeight, transform.position.z);
	}
}
