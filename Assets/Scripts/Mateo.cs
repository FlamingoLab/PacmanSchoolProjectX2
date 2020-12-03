using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(ShootProjectile))]
[RequireComponent(typeof(SteeringVehicle))]
[RequireComponent(typeof(HitCollider))]
public class Mateo : MonoBehaviour
{
	[SerializeField] private float _shootDelay; 	/// <summary>Shoot Delay's Duration</summary>
	private SteeringVehicle _steeringVehicle; 		/// <summary>SteeringVehicle's Component.</summary>
	private ShootProjectile _shootProjectile; 		/// <summary>ShootProjectile's Component.</summary>
	private HitCollider _hitCollider; 				/// <summary>HitCollider's Component.</summary>
	private Cooldown _shootDelayCooldown; 			/// <summary>Attack Delay's Cooldown</summary>

#region Getters/Setters:
	/// <summary>Gets shootDelay property</summary>
	public float shootDelay { get { return _shootDelay; } }

	/// <summary>Gets steeringVehicle Component.</summary>
	public SteeringVehicle steeringVehicle
	{ 
		get
		{
			if(_steeringVehicle == null) _steeringVehicle = GetComponent<SteeringVehicle>();
			return _steeringVehicle;
		}
	}

	/// <summary>Gets shootProjectile Component.</summary>
	public ShootProjectile shootProjectile
	{ 
		get
		{
			if(_shootProjectile == null) _shootProjectile = GetComponent<ShootProjectile>();
			return _shootProjectile;
		}
	}

	/// <summary>Gets hitCollider Component.</summary>
	public HitCollider hitCollider
	{ 
		get
		{
			if(_hitCollider == null) _hitCollider = GetComponent<HitCollider>();
			return _hitCollider;
		}
	}

	/// <summary>Gets and Sets shootDelayCooldown property</summary>
	public Cooldown shootDelayCooldown
	{
		get { return _shootDelayCooldown; }
		private set { _shootDelayCooldown = value; }
	}
#endregion

	private void Awake()
	{
		shootDelayCooldown = new Cooldown(this, shootDelay, OnCooldownEnds);
	}

	public void Shoot()
	{
		if(!shootDelayCooldown.onCooldown)
		shootDelayCooldown.Begin();
	}

	private void OnCooldownEnds()
	{
		shootDelayCooldown.End();
		shootProjectile.Shoot();
	}
}
}