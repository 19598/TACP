using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileController : MonoBehaviour
{
    public Vector3 target;
    public GameObject particles;
    public PlayerController Player;
    private RaycastHit hitInfo;
    public bool willHitBunker = false;

    void Start()
    {
        //makes raycast to determine if it will hit the bunker
        Physics.Raycast(transform.position, transform.forward, out hitInfo);
        if (hitInfo.collider.gameObject.CompareTag("Explodeable"))
        {
            willHitBunker = true;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Missile"))
        {
            try
            {
                //if the missile hits an object that is marked as an Explodeable a raycast determines it will collide with the bunker, it destroys the gameobject and calls its win method
                if (col.gameObject.CompareTag("Explodeable") || willHitBunker)
                {
                    Destroy(Player.bunker);
                    Win();
                }
            }
            catch { }

            //explodes if it hits anything
            Explode();
        }
    }

    void Update()
    {
        transform.position += transform.forward * 1500f * Time.deltaTime;//moves the missile forwards

        //explodes if it is low enough
        if (transform.position.y <= -10)
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
