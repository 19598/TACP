using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileController : MonoBehaviour
{
    public Vector3 target;
    public GameObject particles;
    public PlayerController Player;

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Missile"))
        {
            //if the missile hits an object that is marked as an Explodeable, it destroys the gameobject and calls its win method
            if (col.gameObject.CompareTag("Explodeable"))
            {
                Destroy(col.gameObject);
                Win();
            }

            //explodes if it hits anything
            Explode();
        }
    }

    void Update()
    {
        transform.LookAt(target);//points towards the strike location

        transform.position += transform.forward * 1500f * Time.deltaTime;//moves the missile forwards

        //explodes if it is low enough or within 5 units of its target
        if (transform.position.y <= -10 || Vector3.Distance(target, transform.position) <= 5)
        {
            Explode();
        }
    }

    void Explode()
    {
        //creates particles and sets them to the explosion location
        particles = Instantiate(particles);
        particles.transform.position = transform.position;

        Destroy(gameObject);//destroys this missile
    }

    void Win()
    {
        Player.Win();
    }
}
