using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//locks the cursor so the user doesn't see it
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().getActive())
        {
            //gets mouse input
            float mouseX = (Input.GetAxis("Mouse X") + Input.GetAxis("HorizontalTurn")) * mouseSensitivity * Time.deltaTime;
            float mouseY = (Input.GetAxis("Mouse Y") + Input.GetAxis("VerticalTurn")) * mouseSensitivity * Time.deltaTime;

            //computes the x rotation for the player based off of mouse movement
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //rotates the player
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate( Vector3.up * mouseX);
        }
    }
}
