using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public Vector3 target;
    public float sphere = 1f;
    public GameObject particles;
    // Start is called before the first frame update
    void OnCollisionEnter()
    {
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
        Debug.Log("Explode");
        particles = Instantiate(particles);
        particles.transform.position = transform.position;
        Destroy(gameObject);
    }
}
