using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

/*============================================================
**
** Class:  TransformRotation
**
** Purpose: Separate the single responsibility of actions related to rotation of a
** Transform to this class. Use this API on the Main Thread only.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Flamingo
{
public class TransformRotation : Rotation
{
	/// <summary>Rotates Object on given axes in Euler space.</summary>
	/// <param name="_eulerAxes">Axes represented in Euler space.</param>
	/// <param name="_space">Space relativeness [Space.Self as default].</param>
	public override void RotateInAxes(Vector3 _eulerAxes, Space _space = Space.Self)
	{
		transform.Rotate(_eulerAxes * rotationSpeed * Time.deltaTime, _space);
	}

	/// <summary>Rotates towards given target.</summary>
	/// <param name="_target">Target to rotate towards.</param>
	/// <param name="_rotateAxes">Rotation Axes [All by default].</param>
	public override void RotateTowards(Vector3 _target, Axes3D _rotateAxes = Axes3D.All)
	{
		Vector3 direction = (_target - transform.position).WithAxes(_rotateAxes);

		transform.rotation = (Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime));
	}
}
}