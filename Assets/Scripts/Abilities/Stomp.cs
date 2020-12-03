using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum StompState
{
	Unactive,
	Stomping,
	OnGround,
	Returning
}

[RequireComponent(typeof(Rigidbody))]
public class Stomp : MonoBehaviour, IFiniteStateMachine<StompState>
{
	public event OnStateChange<StompState> onStateChange; 	/// <summary>OnStateChange's event delegate.</summary>

	[SerializeField] private Renderer _renderer; 			/// <summary>Renderer's Component that contains the bounds.</summary>
	[SerializeField] private float _stompSpeed; 			/// <summary>Stomp's Speed.</summary>
	[SerializeField] private float _returnSpeed; 			/// <summary>Return's Speed.</summary>
	[SerializeField] private float _distanceThreshold; 		/// <summary>Distance's Threshold.</summary>
	[SerializeField] private float _returningHeight; 		/// <summary>Returning's Height [as Y coordinate].</summary>
	[SerializeField] private float _stompDuration; 			/// <summary>Stomp's Duration.</summary>
	private StompState _state; 								/// <summary>Current State.</summary>
	private StompState _previousState;	 					/// <summary>Previous State.</summary>
	private Rigidbody _rigidbody; 							/// <summary>Rigidbody's Component.</summary>
	private Coroutine coroutine; 							/// <summary>General Coroutine's reference.</summary>

#region Getters/Setters:
	/// <summary>Gets renderer property.</summary>
	public Renderer renderer { get { return _renderer; } }

	/// <summary>Gets stompSpeed property.</summary>
	public float stompSpeed { get { return _stompSpeed; } }

	/// <summary>Gets returnSpeed property.</summary>
	public float returnSpeed { get { return _returnSpeed; } }

	/// <summary>Gets distanceThreshold property.</summary>
	public float distanceThreshold { get { return _distanceThreshold; } }

	/// <summary>Gets and Sets returningHeight property.</summary>
	public float returningHeight
	{
		get { return _returningHeight; }
		set { _returningHeight = value; }
	}

	/// <summary>Gets stompDuration property.</summary>
	public float stompDuration { get { return _stompDuration; } }

	/// <summary>Gets and Sets state property.</summary>
	public StompState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public StompState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
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
#endregion

	/// <summary>Begins Stompíng.</summary>
	public void Begin()
	{
		if(state == StompState.Unactive)
		this.ChangeState(StompState.Stomping);
	}

	/// <summary>Ends the stomping, regardless of the state.</summary>
	public void End()
	{
		this.ChangeState(StompState.Unactive);
	}

	/// <summary>Enters StompState State.</summary>
	/// <param name="_state">StompState State that will be entered.</param>
	public void OnEnterState(StompState _state)
	{
		switch(_state)
		{
			case StompState.Unactive:
			this.DispatchCoroutine(ref coroutine);
			break;

			case StompState.Stomping:
			this.StartCoroutine(StompCoroutine(), ref coroutine);
			break;

			case StompState.OnGround:
			this.StartCoroutine(this.WaitSeconds(stompDuration, OnGroundWaitEnds), ref coroutine);
			break;

			case StompState.Returning:
			this.StartCoroutine(ReturnCoroutine(), ref coroutine);
			break;
		}

		if(onStateChange != null) onStateChange(_state);
	}
	
	/// <summary>Leaves StompState State.</summary>
	/// <param name="_state">StompState State that will be left.</param>
	public void OnExitState(StompState _state) {/**/}

	private void OnGroundWaitEnds()
	{
		this.ChangeState(StompState.Returning);
	}

	/// <summary>Begins Stomp's Coroutine.</summary>
	private IEnumerator StompCoroutine()
	{
		Bounds bounds = renderer.bounds;
		float floorPoint = bounds.extents.y;
		Vector3 target = rigidbody.position.WithY(floorPoint);
		Vector3 direction = target - rigidbody.position;
		Vector3 force = Vector3.zero;
		float squareDistance = distanceThreshold * distanceThreshold;
		float dt = 0.0f;

		while(direction.sqrMagnitude > squareDistance)
		{
			dt = Time.fixedDeltaTime;
			force += (direction.normalized * stompSpeed * dt * dt);
			rigidbody.MovePosition(rigidbody.position + force);
			target = rigidbody.position.WithY(floorPoint);
			direction = target - rigidbody.position;
			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}

		rigidbody.MovePosition(target);
		this.ChangeState(StompState.OnGround);
	}

	/// <summary>Performs Returning's Coroutine.</summary>
	private IEnumerator ReturnCoroutine()
	{
		Vector3 target = rigidbody.position.WithY(returningHeight);
		Vector3 direction = target - rigidbody.position;
		float squareDistance = distanceThreshold * distanceThreshold;

		while(direction.sqrMagnitude > squareDistance)
		{
			rigidbody.MovePosition(rigidbody.position + (direction.normalized * returnSpeed * Time.fixedDeltaTime));
			target = rigidbody.position.WithY(returningHeight);
			direction = target - rigidbody.position;
			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}

		rigidbody.MovePosition(target);
		this.ChangeState(StompState.Unactive);
	}
}
}