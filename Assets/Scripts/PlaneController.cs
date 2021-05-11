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
        if (canFire)
        {
            //if the jet can fire, it looks towards the strike position, and then points itself so that it doesn't change its height
            transform.LookAt(strikeLocation);
            transform.Rotate(new Vector3(transform.eulerAngles.x, 180, 0));
        }
        else
        {
            speed *= 0.5f;//sets lower speed for flyover jets
        }
        controller = this.GetComponent<CharacterController>();//gets character controller for movement
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(transform.forward * speed * -1f);//moves the jet forward

        //if the jet can fire and it is within 20000 distance units, it fires the missile and speeds up
        if (Vector3.Distance(transform.position, strikeLocation) <= 2000f && canFire)
        {
            canFire = false;
            fireMissiles();
            speed *= 1.4f;
        }

        //if the jet is far enough away, it gets destroyed
        if (Vector3.Distance(new Vector3 (0, 0, 0), transform.position) >= 10000)
        {
            Destroy(gameObject);
        }
    }

    private void fireMissiles()
    {
        missile = Instantiate(missile);//create missile
        missile.GetComponent<MissileController>().Player = Player;//sets the player as its player
        missile.GetComponent<MissileController>().target = strikeLocation;//gets the location it needs to go to
        missile.transform.position = transform.position + transform.up * -5f;// spawns below the jet
        missile.transform.LookAt(strikeLocation);//points missile towards the strike location
    }

    public void SetFire(bool value)
    {
        canFire = value;
    }
}
