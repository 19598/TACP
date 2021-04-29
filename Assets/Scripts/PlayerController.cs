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
    Vector3 velocity;
    public GameObject chest1;
    public GameObject chest2;
    public GameObject chest3;
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    public bool isGrounded;
    public bool active = true;
    public GameObject xPos;
    public GameObject zPos;
    public GameObject submit;
    public GameObject jetPrefab;
    public GameObject jet;
    public Puzzle radioChest;
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

        if (Input.GetKeyDown("e") && active)
        {
            if (Vector3.Distance(transform.position, chest1.transform.position) <= 10)
            {
                chest1.GetComponent<Puzzle>().StartPuzzle();
            }
            if (Vector3.Distance(transform.position, chest2.transform.position) <= 10)
            {
                chest2.GetComponent<Puzzle>().StartPuzzle();
            }
            if (Vector3.Distance(transform.position, chest3.transform.position) <= 10)
            {
                chest3.GetComponent<Puzzle>().StartPuzzle();
            }
        }

        if (Input.GetKeyDown("k") && !radioChest.isActive())
        {
            Cursor.lockState = CursorLockMode.Confined;
            setActive(false);
            xPos.SetActive(true);
            zPos.SetActive(true);
            submit.SetActive(true);
        }
    }

    public void strike()
    {
        try
        {
            float x = float.Parse(xPos.GetComponent<TMPro.TMP_InputField>().text);
            float z = float.Parse(zPos.GetComponent<TMPro.TMP_InputField>().text);
            jet = Instantiate(jetPrefab);
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
}
