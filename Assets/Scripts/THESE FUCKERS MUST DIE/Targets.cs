using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{

public AudioSource targetSound;
public int targetValue;

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("bullet"))
		{
			targetSound.Play();
			ScoringSystem.theScore += targetValue;
			Destroy(gameObject);
		}

	}
}

