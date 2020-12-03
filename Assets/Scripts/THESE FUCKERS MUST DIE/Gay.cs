using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Gay : MonoBehaviour
{
    public float speed;
    Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Respawn") transform.position = initialPosition;
    }
}
