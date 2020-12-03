using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REDCollectCoin1 : MonoBehaviour
{
public AudioSource collectSound;
public int coinValue;

	void OnTriggerEnter(Collider other)
	{
		collectSound.Play();
		ScoringSystem.theScore += coinValue;
		ScoringSystem.redCoin += 1;
		Destroy(gameObject);
	}
}
