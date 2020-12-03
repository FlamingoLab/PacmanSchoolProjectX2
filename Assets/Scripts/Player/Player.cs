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
	public event OnPlayerEvent onPlayerEvent; 				/// <summary>OnPlayerEvent's Delegate.</summary>

	[SerializeField] private HitCollider _shipCollider; 	/// <summary>Ship's Collider.</summary>
	[SerializeField] private Ship _ship; 					/// <summary>Player's Ship.</summary>
	[SerializeField] private BarrelRoll _barrelRoll; 		/// <summary>BarrelRoll's Component.</summary>
	[SerializeField] private Hand _hand; 					/// <summary>Hand where the Ship will be parented.</summary>
	private OVRHead _head; 									/// <summary>Player's VR Head.</summary>
	private Rigidbody _rigidbody; 							/// <summary>Rigidbodyu's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets shipCollider property.</summary>
	public HitCollider shipCollider
	{
		get { return _shipCollider; }
		set { _shipCollider = value; }
	}

	/// <summary>Gets and Sets hand property.</summary>
	public Hand hand
	{
		get { return _hand; }
		set { _hand = value; }
	}

	/// <summary>Gets and Sets ship property.</summary>
	public Ship ship
	{
		get { return _ship; }
		set { _ship = value; }
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
		if(ship == null) return;

		rigidbody.MovePosition(rigidbody.position + (Vector3.forward * ship.steeringVehicle.maxSpeed * Time.fixedDeltaTime));
		if(rigidbody.position.z >= Game.data.zLimits /*&& onPlayerEvent != null*/) //onPlayerEvent(Game.ID_EVENT_PLAYER_OFFLIMITS);
		Game.RepositionPlayer(this, transform); // Temporal shit...
	}

	/// <summary>Projects the forward position of the player's ship on given seconds.</summary>
	/// <param name="_time">Time projection [in seconds].</param>
	/// <returns>Projected Player's position considering its forward displacement.</returns>
	public Vector3 ProjectForwardPosition(float _time)
	{
		return ship.steeringVehicle.rigidbody.position + (Vector3.forward * ship.steeringVehicle.maxSpeed * _time);
	}

	/// <summary>Parents Ship into desired Hand.</summary>
	public void ParentShip()
	{
		if(ship == null) return;
		ship.transform.parent = hand == Hand.Left ? head.leftHandAnchor : head.rightHandAnchor;
	}

	/// <summary>Shoots Projectile.</summary>
	public void ShootProjectile()
	{
		if(ship == null) return;
		ship.shootProjectile.Shoot();
	}

	/// <summary>Performs Barrel Roll.</summary>
	public void DoBarrelRoll()
	{
		if(barrelRoll == null) return;
		barrelRoll.BeginBarrelRoll();
	}
}
}