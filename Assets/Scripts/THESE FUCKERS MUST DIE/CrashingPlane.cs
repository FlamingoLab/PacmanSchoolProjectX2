using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashingPlane : MonoBehaviour
{

public AudioSource crashSound;
public GameObject plane;


	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("obstacle"))
		{
			crashSound.Play();
			FindObjectOfType<GameManager>().GameOver();
			Destroy(plane);
		}

	}
		
}
