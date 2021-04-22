using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public Vector3 strikeLocation;
    private CharacterController controller;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(strikeLocation);
        transform.Rotate(new Vector3(transform.eulerAngles.x, 180, 0));
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(transform.forward * speed * -1f);

        if (Vector3.Distance(transform.position, strikeLocation) <= 30)
        {
            fireMissiles();
        }
        if (Vector3.Distance(new Vector3 (0, 0, 0), transform.position) >= 10000)
        {
            Destroy(gameObject);
        }
    }

    private void fireMissiles()
    {

    }
}
