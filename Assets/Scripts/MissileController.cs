using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public Vector3 target;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
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
        Destroy(gameObject);
    }
}
