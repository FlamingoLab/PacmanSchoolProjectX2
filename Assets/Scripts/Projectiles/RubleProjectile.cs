using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class RubleProjectile : ParabolaProjectile
{
	[Space(5f)]
	[Header("Rubble Projectile's Attributes:")]
	[SerializeField] private TransformRotation _rubleRotation; 	/// <summary>TransformRotation's Component.</summary>

	/// <summary>Gets rubleRotation property.</summary>
	public TransformRotation rubleRotation { get { return _rubleRotation; } }

	/// <summary>Callback internally invoked insided Update.</summary>
	protected override void OnUpdate()
	{
		rubleRotation.RotateInAxes(Vector3.left);
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		rubleRotation.transform.rotation = VQuaternion.Random();
	}
}
}