using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum TackleState
{
	Deactivated,
	Activated,
	OnCooldown,
	ReturningToPoint
}

[RequireComponent(typeof(Rigidbody))]
public class Tackle : MonoBehaviour, IFiniteStateMachine<TackleState>
{
	public event OnStateChange<TackleState> onStateChange; 	/// <summary>OnStateChange's event delegate.</summary>

	[SerializeField] private float _force; 					/// <summary>Tackle's Force.</summary>
	[SerializeField] private float _duration; 				/// <summary>Tackle's Duration.</summary>
	[SerializeField] private float _cooldownDuration; 		/// <summary>Cooldown's Duration.</summary>
	private Rigidbody _rigidbody; 							/// <summary>Rigidbody's Component.</summary>
	private Cooldown _cooldown; 							/// <summary>Cooldown's Reference.</summary>
	private TackleState _state; 							/// <summary>Current State.</summary>
	private TackleState _previousState; 					/// <summary>Previous State.</summary>
	private Coroutine coroutine; 							/// <summary>General Coroutine's Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets force property.</summary>
	public float force
	{
		get { return _force; }
		set { _force = value; }
	}

	/// <summary>Gets and Sets duration property.</summary>
	public float duration
	{
		get { return _duration; }
		set { _duration = value; }
	}

	/// <summary>Gets and Sets cooldownDuration property.</summary>
	public float cooldownDuration
	{
		get { return _cooldownDuration; }
		set { _cooldownDuration = value; }
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}

	/// <summary>Gets and Sets cooldown property.</summary>
	public Cooldown cooldown
	{
		get { return _cooldown; }
		private set { _cooldown = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public TackleState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public TackleState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}
#endregion

	/// <summary>Tackle's instance initialization.</summary>
	private void Awake()
	{
		cooldown = new Cooldown(this, cooldownDuration, OnCooldownEnds);
		this.ChangeState(TackleState.Deactivated);
	}

	/// <summary>Performs Tackle [activated the Tackle Coroutine].</summary>
	public void PerformTackle()
	{
		if(state != TackleState.OnCooldown) this.ChangeState(TackleState.Activated);
	}

	/// <summary>Performs a Coroutine that returns GameObject to a central point.</summary>
	/// <param name="vectorFunction">Function that returns a Vector3.</param>
	/// <param name="_speed">Displacement Speed.</param>
	/// <param name="_distance">Minimum distance to reach the objective.</param>
	public void ReturnToPoint(Func<Vector3> vectorFunction, float _speed, float _distance)
	{
		this.StartCoroutine(ReturnToPointCoroutine(vectorFunction, _speed, _distance), ref coroutine);
		//this.ChangeState(TackleState.ReturningToPoint);
	}

#region Callbacks:
	/// <summary>Enters TackleState State.</summary>
	/// <param name="_state">TackleState State that will be entered.</param>
	public void OnEnterState(TackleState _state)
	{
		switch(_state)
		{
			case TackleState.Deactivated:
			//this.DispatchCoroutine(ref coroutine);
			break;

			case TackleState.Activated:
			this.StartCoroutine(PerformTackleCoroutine(), ref coroutine);
			break;

			case TackleState.OnCooldown:
			cooldown.Begin();
			break;

			case TackleState.ReturningToPoint:
			
			break;
		}

		if(onStateChange != null) onStateChange(_state);
	}
	
	/// <summary>Leaves TackleState State.</summary>
	/// <param name="_state">TackleState State that will be left.</param>
	public void OnExitState(TackleState _state)
	{
		switch(_state)
		{
			case TackleState.Activated:
			this.DispatchCoroutine(ref coroutine);
			break;

			case TackleState.OnCooldown:
			//this.DispatchCoroutine(ref coroutine);
			break;
		}
	}

	/// <summary>Callback internally called when the cooldown ends.</summary>
	private void OnCooldownEnds()
	{
		this.ChangeState(TackleState.Deactivated);
	}
#endregion

#region Coroutines:
	/// <summary>Performs Tackle Coroutine [taking into account the Physics' Thread].</summary>
	private IEnumerator PerformTackleCoroutine()
	{
		float t = 0.0f;
		float dt = Time.fixedDeltaTime;
		Vector3 displacement = Vector3.zero;

		while(t < duration)
		{
			dt = Time.fixedDeltaTime;
			displacement += transform.forward * force * dt * dt;
			rigidbody.MovePosition(rigidbody.position + displacement);
			t += dt;

			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}

		this.ChangeState(TackleState.OnCooldown);
	}

	/// <summary>Coroutine that performs a return to a central point.</summary>
	/// <param name="vectorFunction">Function that returns a Vector3.</param>
	/// <param name="_speed">Displacement Speed.</param>
	/// <param name="_distance">Minimum distance to reach the objective.</param>
	private IEnumerator ReturnToPointCoroutine(Func<Vector3> vectorFunction, float _speed, float _distance)
	{
		Vector3 direction = vectorFunction() - rigidbody.position;
		float sqrDistance = _distance * _distance;

		while(direction.sqrMagnitude > sqrDistance)
		{
			rigidbody.MovePosition(rigidbody.position + (direction.normalized * _speed * Time.fixedDeltaTime));
			direction = vectorFunction() - rigidbody.position;
			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}

		rigidbody.MovePosition(vectorFunction());
	}
#endregion

}
}