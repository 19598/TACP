using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public Vector3 strikeLocation;
    private CharacterController controller;
    public float speed;
    public GameObject missile;
    private bool canFire = true;
    public PlayerController Player;
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

        if (Vector3.Distance(transform.position, strikeLocation) <= 2000f && canFire)
        {
            canFire = false;
            fireMissiles();
            speed *= 1.4f;
        }
        if (Vector3.Distance(new Vector3 (0, 0, 0), transform.position) >= 10000)
        {
            Destroy(gameObject);
        }
    }

    private void fireMissiles()
    {
        Debug.Log("fire");
        missile = Instantiate(missile);
        missile.GetComponent<MissileController>().Player = Player;
        missile.GetComponent<MissileController>().target = strikeLocation;
        missile.transform.position = transform.position + transform.up * -5f;
        missile.transform.LookAt(strikeLocation);
    }
}
