using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Voidless.VR;

namespace Flamingo
{
/// <summary>Event invoked when a Player event happens.</summary>
/// <param name="_ID">Event's ID.</param>
public delegate void OnPlayerEvent(int _ID);

/// \TODO Use an OVR's enumerator instead
/// <summary>Type of Hand Controller [Left or Right].</summary>
public enum Hand
{
	Left = OVRInput.Controller.LHand,
	Right = OVRInput.Controller.RHand
}

[RequireComponent(typeof(OVRHead))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	public const int ID_EVENT_PASSED_Z_LIMITS = 0; 			/// <summary>Passed Z Limits' Event ID.</summary>

	public event OnPlayerEvent onPlayerEvent; 				/// <summary>OnPlayerEvent's Delegate.</summary>

	[SerializeField] private Mateo _mateo; 					/// <summary>Player's Mateo.</summary>
	[SerializeField] private BarrelRoll _barrelRoll; 		/// <summary>BarrelRoll's Component.</summary>
	[SerializeField] private Hand _hand; 					/// <summary>Hand where the Mateo will be parented.</summary>
	private OVRHead _head; 									/// <summary>Player's VR Head.</summary>
	private Rigidbody _rigidbody; 							/// <summary>Rigidbodyu's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets hand property.</summary>
	public Hand hand
	{
		get { return _hand; }
		set { _hand = value; }
	}

	/// <summary>Gets and Sets mateo property.</summary>
	public Mateo mateo
	{
		get { return _mateo; }
		set { _mateo = value; }
	}

	/// <summary>Gets and Sets barrelRoll property.</summary>
	public BarrelRoll barrelRoll
	{
		get { return _barrelRoll; }
		set { _barrelRoll = value; }
	}

	/// <summary>Gets head Component.</summary>
	public OVRHead head
	{ 
		get
		{
			if(_head == null) _head = GetComponent<OVRHead>();
			return _head;
		}
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

	/// <summary>Updates Player's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		if(mateo == null) return;

		rigidbody.MovePosition(rigidbody.position + (Vector3.forward * mateo.steeringVehicle.maxSpeed * Time.fixedDeltaTime));
		if(rigidbody.position.z >= Game.data.zLimits && onPlayerEvent != null) onPlayerEvent(Game.ID_EVENT_PLAYER_OFFLIMITS);
	}

	/// <summary>Projects the forward position of the player's mateo on given seconds.</summary>
	/// <param name="_time">Time projection [in seconds].</param>
	/// <returns>Projected Player's position considering its forward displacement.</returns>
	public Vector3 ProjectForwardPosition(float _time)
	{
		return mateo.steeringVehicle.rigidbody.position + (Vector3.forward * mateo.steeringVehicle.maxSpeed * _time);
	}

	/// <summary>Parents Mateo into desired Hand.</summary>
	public void ParentMateo()
	{
		if(mateo == null) return;
		mateo.transform.parent = hand == Hand.Left ? head.leftHandAnchor : head.rightHandAnchor;
	}

	/// <summary>Shoots Projectile.</summary>
	public void ShootProjectile()
	{
		if(mateo == null) return;
		mateo.Shoot();
	}

	/// <summary>Performs Barrel Roll.</summary>
	public void DoBarrelRoll()
	{
		if(barrelRoll == null) return;
		barrelRoll.BeginBarrelRoll();
	}
}
}