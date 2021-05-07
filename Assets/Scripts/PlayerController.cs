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
    public GameObject xPos;
    public GameObject zPos;
    public GameObject submit;
    public GameObject jetPrefab;
    public GameObject jet;
    public Puzzle radioChest;
    public float lastFired;
    public float resetTime = 0.5f;
    private float[,] positions = new float[5,2] {{361f, 4714f}, {454f, 4589f}, {519f, 4486f}, {591f, 4589f}, {671f, 4719f}};
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            messages.text = "";
        }

        if (active)
        {
            float z = Input.GetAxis("Horizontal");
            float x = Input.GetAxis("Vertical");

            Vector3 move = transform.forward * x + transform.right * z;

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

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
                messages.text = "TACP (pronounced tack-pea) stands for Tactical Air Control Party. Their job is to attach to units of the armed forces, usually special forces of branches other than the Air Force, and call in air support. They are special forces themselves, and go through a pipeline that is over half a year long. Their training involves SERE and Airborne school. Press escape to close.";
            }
        }

        if ((Input.GetKeyDown("k") || Input.GetButtonDown("Fire3")) && !radioChest.isActive())
        {
            Cursor.lockState = CursorLockMode.Confined;
            setActive(false);
            xPos.SetActive(true);
            zPos.SetActive(true);
            submit.SetActive(true);
        }

        if (Input.GetKeyDown("f"))
        {
            Win();
        }
    }

    public void strike()
    {
        try
        {
            float x = float.Parse(xPos.GetComponent<TMPro.TMP_InputField>().text);
            float z = float.Parse(zPos.GetComponent<TMPro.TMP_InputField>().text);
            lastFired = Time.time + resetTime;
            jet = Instantiate(jetPrefab);
            jet.GetComponent<PlaneController>().Player = this;
            jet.GetComponent<PlaneController>().strikeLocation = new Vector3(x, -10f, z);
        }
        catch { }
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
        messages.text = "Congratulations, you destroyed the bunker and beat the game! Press escape to close this message.";
        for (int i = 0; i < 5; i++) {
            jet = Instantiate(jetPrefab);
            jet.transform.position = new Vector3(positions[i,0], 400f, positions[i,1]);
            Debug.Log(positions[i, 0]);
            Debug.Log(positions[i, 1]);
            jet.GetComponent<PlaneController>().SetFire(false);
        }
    }
}
