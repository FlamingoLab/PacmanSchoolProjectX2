using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
	public GameObject scoreText;
	public GameObject coinsText;
	public GameObject mainSong;
	public GameObject star64;
	public static int theScore;
	public static int redCoin;

	void Start()
	{
		theScore = 0;
		star64.GetComponent<MeshRenderer>().enabled = false;
		star64.GetComponent<Animator>().enabled = false;
		star64.GetComponent<AudioSource>().enabled = false;
		redCoin = 0;

	}

	void Update()
	{
		scoreText.GetComponent<Text>().text = "SCORE: " + theScore;
		coinsText.GetComponent<Text>().text = "  " + redCoin;	

		if(redCoin == 8)
		{
			star64.GetComponent<MeshRenderer>().enabled = true;
			star64.GetComponent<Animator>().enabled = true;
			star64.GetComponent<AudioSource>().enabled = true;
			mainSong.GetComponent<AudioSource>().enabled = false;
		}

	}


}
