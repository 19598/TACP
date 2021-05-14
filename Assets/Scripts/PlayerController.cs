using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public AudioSource walkingSound;
    public Vector3 velocity;
    public GameObject chest1;
    public GameObject chest2;
    public GameObject chest3;
    public GameObject book;
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    public bool isGrounded;
    public bool active = true;
    public TMPro.TMP_Text messages;
    public TMPro.TMP_Text GPS;
    public GameObject xPos;
    public GameObject zPos;
    public GameObject submit;
    public GameObject jetPrefab;
    public GameObject jet;
    public Puzzle radioChest;
    public GameObject reticle;
    public bool superTACP = false;
    private bool hasGPS = false;
    private float[,] positions = new float[5,2] {{361f, 4714f}, {454f, 4589f}, {519f, 4486f}, {591f, 4589f}, {671f, 4714f}};//positions to spawn the jets at

    RaycastHit hitInfo;
    public Camera myCamera;

    // Start is called before the first frame update
    void Start()
    {
        walkingSound.Play();
        walkingSound.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetButtonDown("Fire2")) && superTACP)
        {
            if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hitInfo))
            {
                jet = Instantiate(jetPrefab);
                jet.GetComponent<PlaneController>().Player = this;
                jet.GetComponent<PlaneController>().strikeLocation = hitInfo.point;
            }
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);//checks if player is in the ground

        if (isGrounded && velocity.y < 0)//clamps y velocity
        {
            velocity.y = -5f;
        }

        if (Input.GetButtonDown("Cancel"))//clears the alert text
        {
            messages.text = "";
        }

        if (active)
        {
            //gets inputs
            float z = Input.GetAxis("Horizontal");
            float x = Input.GetAxis("Vertical");

            Vector3 move = transform.forward * x + transform.right * z;//calculates the amount to move the player

            controller.Move(move * speed * Time.deltaTime);//moves the player

            //this block plays the walking sound if the player is on the ground and moving
            if (!(z == 0 && x == 0) && isGrounded)
            {
                walkingSound.UnPause();
            }
            else
            {
                walkingSound.Pause();
            }

            //makes the player jump if it is on the ground
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }
        else
        {
            walkingSound.Pause();
        }

        //displays the player's coordinates if they unlocked the GPS
        if (hasGPS)
        {
            GPS.text = "Current X: " + transform.position.x.ToString() + "\nCurrent Z: " + transform.position.z.ToString();
        }

        
        velocity.y += gravity * Time.deltaTime;//increases the veloctiy of the player

        controller.Move(velocity * Time.deltaTime);//moves the player down

        //opens the chest or grabs the book if the player is close enough to one of the chests or the book
        if ((Input.GetKeyDown("e") || Input.GetButtonDown("Fire2")) && active)
        {
            if (Vector3.Distance(transform.position, chest1.transform.position) <= 50)
            {
                chest1.GetComponent<Puzzle>().StartPuzzle();
            }
            else if (Vector3.Distance(transform.position, chest2.transform.position) <= 50)
            {
                chest2.GetComponent<Puzzle>().StartPuzzle();
            }
            else if (Vector3.Distance(transform.position, chest3.transform.position) <= 50)
            {
                chest3.GetComponent<Puzzle>().StartPuzzle();
            }
            else if (Vector3.Distance(transform.position, book.transform.position) <= 50)
            {
                hasGPS = true;
                messages.text = "You found a GPS and a book about TACP! It reads: TACP (pronounced tack-pea) stands for Tactical Air Control Party. Their job is to attach to units of the armed forces, usually special forces of branches other than the Air Force, and call in air support. They are special forces themselves, and go through a pipeline that is over half a year long. Their training involves SERE and Airborne school. Press escape to close.";
            }
        }

        //brings up the menu to call in the air strike if the user has the radio and presses k
        if ((Input.GetKeyDown("k") || Input.GetButtonDown("Fire3")) && !radioChest.isActive())
        {
            Cursor.lockState = CursorLockMode.Confined;
            setActive(false);
            xPos.SetActive(true);
            zPos.SetActive(true);
            submit.SetActive(true);
        }
    }

    public void strike()//this calls in an air strike
    {
        try
        {
            //gets the values of the input fields
            float x = float.Parse(xPos.GetComponent<TMPro.TMP_InputField>().text);
            float z = float.Parse(zPos.GetComponent<TMPro.TMP_InputField>().text);

            if (Physics.Raycast(new Vector3(x, 1000, z), Vector3.down, out hitInfo))//creates raycast to find highest point above the x and z coordinates, and only fires missile if it will hit something
            {
                jet = Instantiate(jetPrefab);//creates jet in editor
                jet.GetComponent<PlaneController>().Player = this;//gives jet a reference to this player
                jet.GetComponent<PlaneController>().strikeLocation = hitInfo.point;//sets the strike location to the point found with the raycast
            }
        }
        catch { }

        //makes all the UI neccessary for striking go away
        xPos.SetActive(false);
        zPos.SetActive(false);
        submit.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        setActive(true);
    }

    public bool getActive()
    {
        return active;
    }

    public void setActive(bool state)
    {
        active = state;
    }
    public void Win()
    {
        messages.text = "Congratulations, you destroyed the bunker and beat the game! You've unlocked Super TACP, which lets you call in air strikes by clicking the left and right mouse buttons. Press escape to close this message.";//displays text telling the user that they beat the game
        superTACP = true;//makes the player super
        reticle.active = true;//sets up the reticle so the player can see where they are pointing

        //creates 5 jets to do a flyover, specifiying that they will not fire a missile and putting them in a v formation
        for (int i = 0; i < 5; i++) {
            jet = Instantiate(jetPrefab);
            jet.transform.position = new Vector3(positions[i,0], 400f, positions[i,1]);
            jet.GetComponent<PlaneController>().SetFire(false);
        }
    }
}
