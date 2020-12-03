using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class GameplayController : Singleton<GameplayController>
{
	[Space(5f)]
	[Header("Gameplay Dependencies:")]
	[SerializeField] private GameplayGUI _gameplayGUI; 	/// <summary>Gameplay's GUI.</summary>
	private float _generalScore; 						/// <summary>Gameplay's General Score.</summary>
	private float _normalCollectiblesScore; 			/// <summary>Score for normal collectibles.</summary>
	private float _rareCollectiblesScore; 				/// <summary>Score for rare collectibles.</summary>
	private float _targetsScore; 						/// <summary>Score for targets that were hit.</summary>
	private Clock _clock; 								/// <summary>Gameplay's Clock.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets gameplayGUI property.</summary>
	public GameplayGUI gameplayGUI
	{
		get { return _gameplayGUI; }
		set { _gameplayGUI = value; }
	}

	/// <summary>Gets and Sets generalScore property.</summary>
	public float generalScore
	{
		get { return _generalScore; }
		set { _generalScore = value; }
	}

	/// <summary>Gets and Sets normalCollectiblesScore property.</summary>
	public float normalCollectiblesScore
	{
		get { return _normalCollectiblesScore; }
		set { _normalCollectiblesScore = value; }
	}

	/// <summary>Gets and Sets rareCollectiblesScore property.</summary>
	public float rareCollectiblesScore
	{
		get { return _rareCollectiblesScore; }
		set { _rareCollectiblesScore = value; }
	}

	/// <summary>Gets and Sets targetsScore property.</summary>
	public float targetsScore
	{
		get { return _targetsScore; }
		set { _targetsScore = value; }
	}

	/// <summary>Gets and Sets clock property.</summary>
	public Clock clock
	{
		get { return _clock; }
		set { _clock = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>Resets GameplayController's instance to its default values.</summary>
	public void Reset()
	{
		generalScore = 0.0f;
		normalCollectiblesScore = 0.0f;
		rareCollectiblesScore = 0.0f;
		targetsScore = 0.0f;
		clock.ellapsedTime = 0.0f;
	}

	/// <summary>GameplayController's instance initialization.</summary>
	protected override void OnAwake()
	{
		if(clock == null) clock = new Clock();
		Reset();
	}

	/// <summary>GameplayController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		
	}
	
	/// <summary>GameplayController's tick at each frame.</summary>
	private void Update ()
	{
		clock.Update(Time.deltaTime);
		gameplayGUI.UpdateTime(clock.minutes, clock.seconds, clock.miliseconds);
	}
#endregion

#region Callbacks:
	/// <summary>Callback invoked when a normal collectible is picked.</summary>
	/// <param name="_score">Collectible's score.</param>
	public static void OnNormalCollectiblePicked(float _score)
	{
		if(Instance == null) return;

		Instance.generalScore += _score;
		Instance.normalCollectiblesScore++;

		Instance.gameplayGUI.UpdateScore(Instance.generalScore);
		Instance.gameplayGUI.UpdateNormalCollectibles(Instance.normalCollectiblesScore);
	}

	/// <summary>Callback invoked when a rare collectible is picked.</summary>
	/// <param name="_score">Collectible's score.</param>
	public static void OnRareCollectiblePicked(float _score)
	{
		if(Instance == null) return;

		Instance.generalScore += _score;
		Instance.rareCollectiblesScore++;

		Instance.gameplayGUI.UpdateScore(Instance.generalScore);
		Instance.gameplayGUI.UpdateRareCollectibles(Instance.rareCollectiblesScore);	
	}

	/// <summary>Callback invoked when an enemy is destroyed.</summary>
	/// <param name="_enemy">Enemy that was destroyed.</param>
	public static void OnEnemyDestroyed(Enemy _enemy)
	{
		if(Instance == null) return;
	}

	/// <summary>Callback invoked when a Target is being hit.</summary>
	/// <param name="_score">Target's score.</param>
	public static void OnTargetHit(float _score)
	{
		if(Instance == null) return;

		Instance.generalScore += _score;
		Instance.targetsScore++;

		Instance.gameplayGUI.UpdateScore(Instance.generalScore);
		Instance.gameplayGUI.UpdateTargets(Instance.targetsScore);
	}
#endregion

}
}