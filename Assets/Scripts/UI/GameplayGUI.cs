using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Flamingo
{
public class GameplayGUI : MonoBehaviour
{
	[Header("Clock's UI:")]
	[SerializeField] private Text _minutes; 				/// <summary>Minutes' Text.</summary>
	[SerializeField] private Text _seconds; 				/// <summary>Seconds' Text.</summary>
	[SerializeField] private Text _miliseconds; 			/// <summary>Miliseconds' Text.</summary>
	[Space(5f)]
	[SerializeField] private Text _score; 					/// <summary>Score's Text.</summary>
	[Space(5f)]
	[SerializeField] private Text _normalCollectibles; 		/// <summary>Normal Collectible's Text.</summary>
	[Space(5f)]
	[SerializeField] private Text _rareCollectibles; 		/// <summary>Rare Collectibles' Text.</summary>
	[Space(5f)]
	[SerializeField] private Text _targets; 				/// <summary>Targets' UI.</summary>
	[Space(5f)]
	[Header("Bars' UI:")]
	[SerializeField] private RectTransform _HPBar; 			/// <summary>HP Bar's Filling Image.</summary>
	[SerializeField] private RectTransform _barrelRollBar; 	/// <summary>Barrel Roll Bar's Filling Image.</summary>

	/// <summary>Gets minutes property.</summary>
	public Text minutes { get { return _minutes; } }

	/// <summary>Gets seconds property.</summary>
	public Text seconds { get { return _seconds; } }

	/// <summary>Gets miliseconds property.</summary>
	public Text miliseconds { get { return _miliseconds; } }

	/// <summary>Gets score property.</summary>
	public Text score { get { return _score; } }

	/// <summary>Gets normalCollectibles property.</summary>
	public Text normalCollectibles { get { return _normalCollectibles; } }

	/// <summary>Gets rareCollectibles property.</summary>
	public Text rareCollectibles { get { return _rareCollectibles; } }

	/// <summary>Gets targets property.</summary>
	public Text targets { get { return _targets; } }

	/// <summary>Gets HPBar property.</summary>
	public RectTransform HPBar { get { return _HPBar; } }

	/// <summary>Gets barrelRollBar property.</summary>
	public RectTransform barrelRollBar { get { return _barrelRollBar; } }

	/// <summary>Resets GameplayGUI's instance to its default values.</summary>
	public void Reset()
	{
		UpdateTime(0.0f, 0.0f, 0.0f);
		UpdateScore(0.0f);
		UpdateNormalCollectibles(0.0f);
		UpdateRareCollectibles(0.0f);
		UpdateTargets(0.0f);
		UpdateHPBar(1.0f);
		UpdateBarrelRollBar(1.0f);
	}

	/// <summary>GameplayGUI's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		Reset();
	}

	/// <summary>Updates Time's Text.</summary>
	/// <param name="_minutes">Minutes.</param>
	/// <param name="_seconds">Seconds.</param>
	/// <param name="_miliseconds">Miliseconds.</param>
	public void UpdateTime(float _minutes, float _seconds, float _miliseconds)
	{
		minutes.text = _minutes.ToString("00");
		seconds.text = _seconds.ToString("00");
		miliseconds.text = _miliseconds.ToString("000");
	}

	/// <summary>Updates Score's Text.</summary>
	/// <param name="_score">Score to update.</param>
	public void UpdateScore(float _score)
	{
		score.text = _score.ToString();
	}

	/// <summary>Updates NormalCollectibles' Text.</summary>
	/// <param name="_score">Score to update.</param>
	public void UpdateNormalCollectibles(float _score)
	{
		normalCollectibles.text = _score.ToString();
	}

	/// <summary>Updates Rare Collectibles' Text.</summary>
	/// <param name="_score">Score to update.</param>
	public void UpdateRareCollectibles(float _score)
	{
		rareCollectibles.text = _score.ToString();
	}

	/// <summary>Updates Targets' Text.</summary>
	/// <param name="_score">Score to update.</param>
	public void UpdateTargets(float _score)
	{
		targets.text = _score.ToString();
	}

	/// <summary>Updates HP's Bar.</summary>
	/// <param name="_ratio">HP's normalized ratio.</param>
	public void UpdateHPBar(float _ratio)
	{
		Vector3 scale = Vector3.one;

		scale.x = _ratio;
		HPBar.localScale = scale;
	}

	/// <summary>Updates Barrel Roll's Bar.</summary>
	/// <param name="_ratio">Barrel Roll's Cooldown normalized ratio.</param>
	public void UpdateBarrelRollBar(float _ratio)
	{
		Vector3 scale = Vector3.one;

		scale.x = _ratio;
		barrelRollBar.localScale = scale;
	}	
}
}