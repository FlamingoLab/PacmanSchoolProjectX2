using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(SteeringVehicle))]
[RequireComponent(typeof(ShootProjectile))]
public class Ship : MonoBehaviour
{
	private SteeringVehicle _steeringVehicle; 	/// <summary>SteeringVehicle's Component.</summary>
	private ShootProjectile _shootProjectile; 	/// <summary>ShootProjectile's Component.</summary>

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
}
}