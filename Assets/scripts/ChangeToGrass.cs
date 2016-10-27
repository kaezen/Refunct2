using UnityEngine;
using System.Collections;

public class ChangeToGrass : MonoBehaviour
{

    // Use this for initialization
    public Material grass;
    Transform player;

    void OnControllerColliderHit(ControllerColliderHit collision)
    {

        player = GetComponentInParent<Transform>();
        //  print("Collision");
        //if the normal of the collision is pointing up (if the player is standing ontop of it)
        //and if the player if above 0 (above the water)
        if (collision.normal == Vector3.up & player.position.y > 0)
        {//and if we collided with a wall/platform terrain, and the texture is not already grass
            if (collision.collider.tag == "Wall" & collision.gameObject.GetComponent<MeshRenderer>().material != grass)
                //then turn the texture to grass
                collision.gameObject.GetComponent<MeshRenderer>().material = grass;
        }

    }



}
