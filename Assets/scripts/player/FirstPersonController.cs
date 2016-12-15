using UnityEngine;
using System.Collections;
/// <summary>
/// this class handles all the player's movement controls and behavior
/// </summary>
public class FirstPersonController : MonoBehaviour
{
    /// <summary>
    /// the controller
    /// </summary>
    CharacterController player;
    /// <summary>
    /// the camera
    /// </summary>
    public Transform cam;
    /// <summary>
    /// how fast the player moves
    /// </summary>
    public float speed = 1;
    /// <summary>
    /// the mouse's x speed
    /// </summary>
    public float mouseSensitivityX = 1;
    /// <summary>
    /// the mouse's y speed
    /// </summary>
    public float mouseSensitivityY = 1;

    /// <summary>
    /// the joystick x speed
    /// </summary>
    public float joySensitivityX = 3;
    /// <summary>
    /// the joystick y speed
    /// </summary>
    public float joySensitivityY = 3;
    /// <summary>
    /// whether or not to invert the y axis
    /// </summary>
    public bool invertLookY = true;

    /// <summary>
    /// how fast the player moves while sliding
    /// </summary>
    public float slideBoost = 1.1f;
    /// <summary>
    /// how long to slide for
    /// </summary>
    public float slideTimer = .5f;
    /// <summary>
    /// tracking if the player is sliding
    /// </summary>
    public float slideTimerLife = 0;
    /// <summary>
    /// how much to squash the player by when sliding (crouching)
    /// </summary>
    public float slideScale = .5f;
    /// <summary>
    /// if the player is sliding
    /// </summary>
    public bool isSliding = false;

    /// <summary>
    /// how strong the player's wall jump is
    /// </summary>
    public float doubleJumpImpulse = 5;
    /// <summary>
    /// this timer gives the player some leeway when trying to walljump so it's not frame perfect
    /// </summary>
    public float doubleJumpTimer = .5f;
    /// <summary>
    /// tracking if the player is trying to double jump
    /// </summary>
    public float doubleJumpTimerLife = 0;
    /// <summary>
    /// slows down horizontal movement if you've walljumped so it doesn't fling you around
    /// </summary>
    public float doubleJumpPenalty = .5f;

    /////CLIMB FUNCTION CURRENTLY BUGGED
    /// <summary>
    /// if the player can climb
    /// </summary>
    public bool canClimb = false;
    /// <summary>
    /// if the player is climbing
    /// </summary>
    public bool isClimbing = false;
    /// <summary>
    /// how long the climbing animation takes
    /// </summary>
    public float climbTimer = 1;
    /// <summary>
    /// tracking if the player is currently climbing
    /// </summary>
    public float climbTimerLife = 0;
    /// <summary>
    /// the vertical height the player is trying to climb to
    /// </summary>
    public float climbTarget = 0;

    /// <summary>
    /// if the player is swimming
    /// </summary>
    public bool isSwimming = false;
    /// <summary>
    /// slow down movement underwater
    /// </summary>
    public float swimPenalty = .8f;
    /// <summary>
    /// this lets the player kind of float out of the water slightly and also
    /// prevents repeated splooshing at the surface
    /// </summary>
    public float wasSwimmingTimer = .5f;
    /// <summary>
    /// track if the player was recently swimming
    /// </summary>
    public float wasSwimmingTimerLife = 0;

    /// <summary>
    /// if the player was grounded last frame...
    /// </summary>
    public bool wasGrounded = false;
    /// <summary>
    /// how strong gravity is
    /// </summary>
    public float gravity = -20;
    /// <summary>
    /// how strong the player's jump is
    /// </summary>
    public float jumpImpulse = 10;
    /// <summary>
    /// if the player is sliding, how much to increase the jump impulse by
    /// </summary>
    public float jumpBoost = 3;
    /// <summary>
    /// whether or not the player can get a boosted jump
    /// needs refactoring
    /// </summary>
    public bool canBoost = false;

    /// <summary>
    /// the audio clips for the different animations
    /// run, landing, jump, splash slide
    /// </summary>
    public AudioClip run;
    public AudioClip landing;
    public AudioClip jump;
    public AudioClip splash;
    public AudioClip slide;

    /// <summary>
    /// audio source for basic sound effects
    /// </summary>
    AudioSource effects;
    /// <summary>
    /// audio source for the music
    /// </summary>
    AudioSource music;
    /// <summary>
    /// audio source for landing effect
    /// </summary>
    AudioSource landingSource;
    /// <summary>
    /// audio source for sliding sound effect
    /// </summary>
    AudioSource slideSource;
    /// <summary>
    /// the current direction the player is jumping
    /// used when wall jumping
    /// </summary>
    Vector3 jumpDirection = Vector3.zero;
    /// <summary>
    /// if the player is currently contacting a wall
    /// </summary>
    public bool touchingWall = false;

    /// <summary>
    /// the camera's pitch
    /// </summary>
    float camPitch = 0;
    /// <summary>
    /// the player's velocity
    /// </summary>
    Vector3 velocity = Vector3.zero;
    /// <summary>
    /// where the mouse was last frame
    /// </summary>
    Vector3 prevMousePosition = Vector3.zero;

    void Start()
    {// this gets all the audiosources attached to the player as a pseudo array
        var aSources = GetComponents<AudioSource>();
        player = GetComponent<CharacterController>();

        //assigns each of the audio sources to one of the attached sources
        effects = aSources[0];
        landingSource = aSources[1];
        landingSource.clip = landing;

        //sets the parameters of the audio sources
        slideSource = aSources[2];
        slideSource.clip = slide;
        slideSource.pitch = .6f;
        slideSource.volume = .4f;

        music = cam.GetComponent<AudioSource>();
    }

    void Update()
    {

        // rotate:
        cameraRotation();

        // move:
        PlayerMovement();


        //if (CollisionFlags.Below != 0) print("STANDING ON SOMETHING");
        //   if ((player.collisionFlags & CollisionFlags.Below) != 0) print("NEXT TO ME");
        //   print(velocity);
        //  print(player.collisionFlags.ToString());

        //   print(touchingWall);
    }
    /// <summary>
    /// control's the camera's rotation via mouse or joystick
    /// </summary>
    void cameraRotation()
    {
        Vector3 mouseDiff = Input.mousePosition - prevMousePosition;
        prevMousePosition = Input.mousePosition;

        //if the mouse has moved since the last frame
        if (mouseDiff != new Vector3(0, 0, 0))
        {
            transform.Rotate(0, mouseDiff.x * mouseSensitivityX, 0);
            camPitch += mouseDiff.y * mouseSensitivityY * (invertLookY ? -1 : 1);
            camPitch = Mathf.Clamp(camPitch, -80, 80);
            cam.localEulerAngles = new Vector3(camPitch, 0, 0);
        }
        else
        {
            //otherwise use joystick controls

            //camera rotates vertically
            camPitch += (Input.GetAxis("Joy Y")) * joySensitivityY;
            camPitch = Mathf.Clamp(camPitch, -80, 80);

            cam.localEulerAngles = new Vector3(camPitch, 0, 0);

            //PLAYER rotates horizontally
            transform.Rotate(0, ((Input.GetAxis("Joy X")) * joySensitivityX), 0);



            //cam.transform.Rotate(cam.eulerAngles.x, cam.eulerAngles.y +(Input.GetAxis("Joy X") * joySensitivityX), 0);
            // cam.transform.Rotate(cam.eulerAngles.x + (Input.GetAxis("Joy X") * joySensitivityX) ,0 ,0);
        }
    }




    /// <summary>
    /// handles all the player's movement commands
////////NEEDS HEAVY REFACTORING
    /// </summary>
    void PlayerMovement()
    {

        //x and z input
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3();

        //only want input available if the player isn't climbing
        if (!isClimbing)
        {
            //if the player is on the ground and slide is pressed
            if (player.isGrounded & Input.GetButton("Slide"))
            {
                /*  slideTimerLife = slideTimer;
                  isSliding = true;

                  //on the first frame slide is pressed, start the timer and shrink the player a little
                  //to simulate crouching
                  if (Input.GetButtonDown("Slide"))
                  {
                      transform.localScale = new Vector3(1, slideScale, 1);

                      doubleJumpTimerLife = slideTimer;


                     // if (!slideSource.isPlaying)
                          slideSource.Play();
                  }
                  move = velocity; //figure out how fast the player is currently going
                                   //move = transform.forward + transform.right;
                                   //print("isSliding");
                                   */
            }
            else
            {
                move = transform.forward * v + transform.right * h;
                //would like to not have the player 'snap' to specific directions, but that's low on priority list
                //if the player isn't grounded
                if (!player.isGrounded)
                {
                    //  if (Input.GetButton("Horizontal") | Input.GetButton("Vertical"))
                    /////
                    //this prevents the player from being able to scale the wall with walljumps
                    //////
                    if (doubleJumpTimerLife > 0) move += velocity * .9f;
                }
            }

            //   print(player.isGrounded);


            if (player.isGrounded)
            {

                if (move.magnitude > 1) move.Normalize();
                if (slideTimerLife > 0)
                {
                    velocity = move * speed * slideBoost;
                }
                else velocity = move * speed;


                if (Input.GetButtonDown("Jump"))
                {
                    effects.clip = jump;
                    effects.Play();
                    if (canBoost)
                    {
                        // print("boosting");
                        velocity.y = jumpImpulse * jumpBoost;
                    }
                    else
                    {
                        velocity.y = jumpImpulse;
                    }
                    wasGrounded = false;
                }
            }
            else //if not grounded
            {

                float upSpeed = velocity.y;
                //print("flying");
                if (move.magnitude > 1)
                {

                    move.Normalize();
                    velocity = move * speed;
                }
                else
                {
                    velocity = move * speed;
                }
                velocity.y = upSpeed;

                //walljump
                if (Input.GetButtonDown("Jump") & touchingWall & !isSwimming)
                {//if jump is pressed and recently impacted a wall
                    if (slideTimerLife > 0)
                    {
                        velocity.y = jumpImpulse;
                        effects.clip = jump;
                        effects.Play();
                    }
                    else if (!canClimb)
                    {
                        if (jumpDirection == Vector3.left | jumpDirection == Vector3.right)
                        {
                            velocity.x = jumpDirection.x * jumpImpulse * doubleJumpImpulse;
                        }
                        if (jumpDirection == Vector3.forward | jumpDirection == Vector3.back)
                        {
                            velocity.z = jumpDirection.z * jumpImpulse * doubleJumpImpulse;
                        }
                        velocity.y = jumpImpulse * doubleJumpPenalty;

                        effects.clip = jump;
                        effects.Play();


                        touchingWall = false;
                    }//if the player presses jump while sliding
                    else if (Input.GetButtonDown("Jump") & isSliding)
                    {//can perform normal jump
                        effects.clip = jump;
                        effects.Play();
                        velocity.y = jumpImpulse;
                        wasGrounded = false;
                    }
                    else
                    {
                        /*
                        if (canClimb)
                        {

                              //  print("should climb");
                                isClimbing = true;
                            climbTimerLife = climbTimer;

                            canClimb = false;

                        }
                        */
                    }
                }

                //if jump is pressed while swimming
                if (isSwimming & Input.GetButton("Jump"))
                {//go up, but slowly, this also permits a short jump out of the water
                    velocity.y = jumpImpulse * .5f;

                }
                //if slide is pressed while simming, go down
                if (isSwimming & Input.GetButton("Slide"))
                {
                    // print("water fall");
                    velocity.y = -jumpImpulse;
                }

            }


            //if swimming, apply weaker gravity
            if (isSwimming) velocity.y *= .7f;
            else velocity.y += gravity * Time.deltaTime;
            //   print(velocity);

            if (isSwimming) player.Move(velocity * swimPenalty * Time.deltaTime);
            else player.Move(velocity * Time.deltaTime);

        }
        /////climbing function currently bugged
        /*
        if (climbTimerLife > 0)
        {
            climbTimerLife -= Time.deltaTime;
            float distance = climbTarget - player.transform.position.y;
            float speed = distance;
                 if (climbTimerLife  >= .5)
                 {
                player.transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y +1*Time.deltaTime, player.transform.position.z);
                print("up");
                print(distance);
                 }
                 else
                 {
                   //  player.transform.localPosition += -jumpDirection * speed * Time.deltaTime;
              //  print("forward");
                 }


    /*
            for (float y = climbTimer; y >= 0; y -= Time.deltaTime)
            {
                player.Move(Vector3.up * speed * Time.deltaTime);
            }         
    }



    if(climbTimerLife < 0)
    {
        climbTimerLife = 0;
        isClimbing = false;
    }
       */
        //  print(isClimbing);
        //   print("target" + climbTarget);
        //  print("player" + player.transform.position.y);



        //checking all timers
        if (isSliding & slideTimerLife > .1)
        {
            slideTimerLife -= Time.deltaTime;
        }
        else if (isSliding & slideTimerLife <= .1 & slideTimerLife > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isSliding = false;
        }
        else
        {
            slideTimerLife = 0;
        }

        if (doubleJumpTimerLife > 0)
        {
            doubleJumpTimerLife -= Time.deltaTime;
        }
        else
        {
            doubleJumpTimerLife = 0;
            touchingWall = false;
            jumpDirection = Vector3.zero;
            canClimb = false;
        }
        if (wasSwimmingTimerLife > 0)
        {
            if (player.isGrounded) wasSwimmingTimerLife = 0;
            else wasSwimmingTimerLife -= Time.deltaTime;
        }
        else if (wasSwimmingTimerLife != 0)
        {
            //   print("Swimming off");
            wasSwimmingTimerLife = 0;
            isSwimming = false;
        }
        else if (wasSwimmingTimerLife == 0 && player.transform.position.y > 1)
        {
            isSwimming = false;
        }

        //  print(player.isGrounded);

        //after everything else, set was grounded, to is grounded




    }
    /// <summary>
    /// what happens when the player collides with triggers (buttons or the water)
    /// </summary>
    /// <param name="collider">the player</param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Water" & !isSliding)
        {
            //print("water");
            if (!isSwimming & velocity.y < 0) player.Move(Vector3.down * 1);


            // AudioSource.PlayClipAtPoint(splash, player.transform.position);
            effects.clip = splash;
            if (!effects.isPlaying & !isSwimming) effects.Play();
            isSwimming = true;
            wasSwimmingTimerLife = 0;
        }
    }
    /// <summary>
    /// when the player leaves the water
    /// </summary>
    /// <param name="collider">the player</param>
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Water")
        {
            // isSwimming = false;
            wasSwimmingTimerLife = wasSwimmingTimer;
            //  print("leaving water");
        }
    }




    /*
    void OnCollisionEnter(Collision collision)
    { 
     //   print("IM HITTINGSOMETHING");

       // print(collision);
    }

    void OnCollisionLeave(Collision collision)
    {
        if (collision.collider.tag == "wall") touchingWall = false;
    }

    */


    /// <summary>
    /// when the player collides with solid terrain, walls, floor, etc
    /// handles all of that stuff
    /// </summary>
    /// <param name="collision">the player</param>
    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.normal != Vector3.up & collision.normal != Vector3.down) touchingWall = true;
        jumpDirection = collision.normal;
        if (!player.isGrounded | slideTimerLife > 0) doubleJumpTimerLife = doubleJumpTimer;

        // print("hitting something");

        if (collision.collider.tag == "Wall" & player.isGrounded != wasGrounded & !isSliding)
        {
            //landingSource.clip = landing;

            if (velocity.y > -1) landingSource.volume = .1f;
            else if (velocity.y >= -10) landingSource.volume = .5f;
            else if (velocity.y < -10) landingSource.volume = 1;
            // print("volume" + landingSource.volume);
            // print(velocity.y);
            if (!landingSource.isPlaying) landingSource.Play();


            //   effects.volume = 1;

            // print("landing");
            wasGrounded = player.isGrounded;
        }

        if ((collision.collider.tag == "Boost") & (collision.normal == Vector3.up))
        {
            canBoost = true;
        }
        else canBoost = false;


        //  print(jumpDirection);
        //print("HITTING SOMETHING");

        //  print("position" + collision.gameObject.transform.position);
        // print("   scale" + collision.gameObject.transform.localScale);

        if (!player.isGrounded)
        {
            climbTarget = collision.gameObject.transform.localScale.y / 2;
        }
        /*  
          if (player.transform.position.y >= climbTarget - .5 & !isClimbing)
          {
              // print("testing");
              canClimb = true;
              climbTarget += 2f; //playerheight
          }          
         */
    }
}

