using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

/*============================================================
**
** Class:  Rotation
**
** Purpose: The purpose of this Component is to separate the rotation ability, so all GameObjects'
** rotations on this game work using the same exact logic and using the same exact parameters.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Flamingo
{
public abstract class Rotation : MonoBehaviour
{
	[SerializeField] private float _rotationSpeed; 	/// <summary>Rotation's Speed.</summary>

	/// <summary>Gets and Sets rotationSpeed property.</summary>
	public float rotationSpeed
	{
		get { return _rotationSpeed; }
		set { _rotationSpeed = value; }
	}

	/// <summary>Rotates Object on given axes in Euler space.</summary>
	/// <param name="_eulerAxes">Axes represented in Euler space.</param>
	/// <param name="_space">Space relativeness [Space.Self as default].</param>
	public abstract void RotateInAxes(Vector3 _eulerAxes, Space _space = Space.Self);

	/// <summary>Rotates towards given target.</summary>
	/// <param name="_target">Target to rotate towards.</param>
	/// <param name="_rotateAxes">Rotation Axes [All by default].</param>
	public abstract void RotateTowards(Vector3 _target, Axes3D _rotateAxes = Axes3D.All);
}
}