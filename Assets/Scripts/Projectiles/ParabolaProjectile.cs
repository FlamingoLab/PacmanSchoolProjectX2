using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class ParabolaProjectile : Projectile
{
	private Vector3 _velocity; 							/// <summary>Velocity vector in case this projectile is of parabola type.</summary>

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector3 velocity
	{
		get { return _velocity; }
		set
		{
			_velocity = value;
			rigidbody.velocity += velocity;
		}
	}

	/// <returns>Displacement acoording to the Projectile's Type.</returns>
	protected override Vector3 CalculateDisplacement()
	{
		float dt = Time.fixedDeltaTime;

		/// Gravity needs to augment exponentially each frame, but the velocity is kept.
		rigidbody.velocity += (Game.data.gravity * dt);
		return (rigidbody.velocity * dt);
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		rigidbody.Sleep();
	}
}
}