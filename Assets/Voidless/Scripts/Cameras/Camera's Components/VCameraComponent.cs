using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(VCamera))]
public abstract class VCameraComponent : MonoBehaviour
{
	private VCamera _voidlessCamera; 	/// <summary>voidlessCamera's Component.</summary>
	
	/// <summary>Gets and Sets voidlessCamera Component.</summary>
	public VCamera voidlessCamera
	{ 
		get
		{
			if(_voidlessCamera == null) _voidlessCamera = GetComponent<VCamera>();
			return _voidlessCamera;
		}
	}
}
}