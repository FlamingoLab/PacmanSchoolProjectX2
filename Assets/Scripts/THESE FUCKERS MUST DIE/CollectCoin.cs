using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
public AudioSource collectSound;
public int coinValue;

	void OnTriggerEnter(Collider other)
	{
		collectSound.Play();
		ScoringSystem.theScore += coinValue;
		Destroy(gameObject);
	}
}
