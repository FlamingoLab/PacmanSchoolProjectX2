using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

/*============================================================
**
** Class:  RigidbodyRotation
**
** Purpose: The purpose of this Component is to separate the rotation ability (with Rigidbody), so all Rigidbodies'
** rotations on this game work using the same exact logic and using the same exact parameters.
**
** Use this API on the Physics' Thread only.
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Flamingo
{
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyRotation : Rotation
{
	private Rigidbody _rigidbody; 					/// <summary>Rigidbody's Component.</summary>

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}

	/// \TODO Make someting inside this Method...
	/// <summary>Rotates Object on given axes in Euler space.</summary>
	/// <param name="_eulerAxes">Axes represented in Euler space.</param>
	/// <param name="_space">Space relativeness [Space.Self as default].</param>
	public override void RotateInAxes(Vector3 _eulerAxes, Space _space = Space.Self){}

	/// <summary>Rotates towards given target.</summary>
	/// <param name="_target">Target to rotate towards.</param>
	/// <param name="_rotateAxes">Rotation Axes [All by default].</param>
	public override void RotateTowards(Vector3 _target, Axes3D _rotateAxes = Axes3D.All)
	{
		Vector3 direction = (_target - rigidbody.position).WithAxes(_rotateAxes);

		rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.fixedDeltaTime));
	}
}
}