using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileController : MonoBehaviour
{
    public Vector3 target;
    public float sphere = 1f;
    public GameObject particles;
    public AudioSource explosionSound;
    public PlayerController Player;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Explodeable"))
        {
            Destroy(col.gameObject);
            Win();
        }
        Explode();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * 5f;
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        AudioSource.PlayClipAtPoint(explosionSound.clip, transform.position);
        Debug.Log("Explode");
        particles = Instantiate(particles);
        particles.transform.position = transform.position;
        //Destroy(gameObject);
    }

    void Win()
    {
        Player.Win();
    }
}
