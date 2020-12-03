using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Tackle))]
public class TESTTackle : MonoBehaviour
{
	[SerializeField] private float speed; 			/// <summary>Speed.</summary>
	[SerializeField]
	[Range(0.0f, 0.5f)] private float distance; 	/// <summary>Distance.</summary>
	private Tackle tackle;
	private Vector3 initialPosition;

	/// <summary>TESTTackle's instance initialization.</summary>
	private void Awake()
	{
		initialPosition = transform.position;
		tackle = GetComponent<Tackle>();
		tackle.PerformTackle();
		tackle.onStateChange += OnStateChange;
	}

	private void OnStateChange(TackleState _state)
	{
		Debug.Log("[TESTTackle] Tackle Ended...");
		if(_state == TackleState.OnCooldown)
		tackle.ReturnToPoint(()=> { return initialPosition; }, speed, distance);
	}
}
}