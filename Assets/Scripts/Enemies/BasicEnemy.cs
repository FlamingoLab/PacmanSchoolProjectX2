using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(RigidbodyRotation))]
[RequireComponent(typeof(SteeringMovementAlongRadius))]
public class BasicEnemy : Enemy
{
	[SerializeField] private SteeringVehicle test; 				/// <summary>Poop.</summary>
	private SteeringMovementAlongRadius _movementAlongRadius; 	/// <summary>SteeringMovementAlongRadius' Component.</summary>
	private RigidbodyRotation _rotation; 						/// <summary>Rotation's Component.</summary>
	private ShootProjectile _shootProjectile; 					/// <summary>ShootProjectile's Component.</summary>

	/// <summary>Gets movementAlongRadius Component.</summary>
	public SteeringMovementAlongRadius movementAlongRadius
	{ 
		get
		{
			if(_movementAlongRadius == null) _movementAlongRadius = GetComponent<SteeringMovementAlongRadius>();
			return _movementAlongRadius;
		}
	}

	/// <summary>Gets rotation Component.</summary>
	public RigidbodyRotation rotation
	{ 
		get
		{
			if(_rotation == null) _rotation = GetComponent<RigidbodyRotation>();
			return _rotation;
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

	/// <summary>BasicEnemy's tick at each frame.</summary>
	private void Update ()
	{
		if(test == null) return;
		if(!this.HasState(ID_STATE_ALIVE)) return;
		//if(this.HasState(ID_STATE_IDLE)) return;

		movementAlongRadius.Seek(test);
		rotation.RotateTowards(test.rigidbody.position);

		if(this.HasState(ID_STATE_ATTACK) && !shootProjectile.onCooldown)
		shootProjectile.Shoot();
	}
}
}