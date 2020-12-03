using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
	private Rigidbody _rigidbody; /// <summary>Rigidbody's Component.</summary>

	/// <summary>Gets and Sets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}
}
}