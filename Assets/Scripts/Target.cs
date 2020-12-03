using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Health))]
public class Target : PoolGameObject
{
	[Space(5f)]
	[SerializeField] private float _score; 	/// <summary>Score's value when this target is hit.</summary>
	private Health _health; 				/// <summary>Health's Component.</summary>

	/// <summary>Gets and Sets score property.</summary>
	public float score
	{
		get { return _score; }
		set { _score = value; }
	}

	/// <summary>Gets health Component.</summary>
	public Health health
	{ 
		get
		{
			if(_health == null) _health = GetComponent<Health>();
			return _health;
		}
	}

	/// <summary>Target's instance initialization.</summary>
	private void Awake()
	{
		health.onHealthEvent += OnHealthEvent;
	}

	/// <summary>Callback invoked when Target's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		health.onHealthEvent -= OnHealthEvent;
	}

	/// <summary>Callback invoked when health is fully depleted.</summary>
	private void OnHealthEvent(HealthEvent _event, float _amount = 0.0f)
	{
		if(_event != HealthEvent.FullyDepleted) return; 
		
		OnObjectDeactivation();
		GameplayController.OnTargetHit(score);
	}
}
}