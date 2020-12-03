using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectlieDeath : MonoBehaviour
{
	
public float bulletLifespan = 3f;

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("obstacle"))
		{
			Destroy(gameObject);
		}

	}

	void Start()
	{
 		Invoke("BulletDeath", bulletLifespan);
	} 

    
    void BulletDeath()
	{
		Destroy(gameObject);
	}


}
