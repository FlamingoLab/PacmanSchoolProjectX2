using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum InterpolateOrigin
{
	PathWaypointGenerator,
	FirstWaypoint
}

[RequireComponent(typeof(CameraOrbitedDisplacementFollow))]
[RequireComponent(typeof(CameraRotationFollow))]
public class ThirdPersonCamera : VCamera
{
	private static ThirdPersonCamera _Instance; 			/// <summary>Third person camera's instance.</summary>

	private CameraOrbitedDisplacementFollow _orbitFollow; 	/// <summary>CameraOrbitedDisplacementFollow's Component.</summary>
	private CameraRotationFollow _rotationFollow; 			/// <summary>CameraRotationFollow's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets Instance property.</summary>
	public new static ThirdPersonCamera Instance
	{
		get
		{
			if (_Instance == null) {
				_Instance = (ThirdPersonCamera)FindObjectOfType(typeof(ThirdPersonCamera));

				if (_Instance == null) {
					Debug.LogError("An Instance of " + typeof(ThirdPersonCamera) + " is needed in the scene, but there is none.");
				}
			}
			return _Instance;
		}
		private set { _Instance = value; }
	}

	/// <summary>Gets and Sets orbitFollow Component.</summary>
	public CameraOrbitedDisplacementFollow orbitFollow
	{ 
		get
		{
			if(_orbitFollow == null) _orbitFollow = GetComponent<CameraOrbitedDisplacementFollow>();
			return _orbitFollow;
		}
	}

	/// <summary>Gets and Sets rotationFollow Component.</summary>
	public CameraRotationFollow rotationFollow
	{ 
		get
		{
			if(_rotationFollow == null) _rotationFollow = GetComponent<CameraRotationFollow>();
			return _rotationFollow;
		}
	}
#endregion

	/// <summary>Updates Camera.</summary>
	protected override void UpdateCamera()
	{
		///...
	}

	/// <summary>Updates Camera on Physics' Thread.</summary>
	protected override void FixedUpdateCamera()
	{
		///...
	}
}
}