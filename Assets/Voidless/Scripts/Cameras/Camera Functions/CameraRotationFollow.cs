using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
	1.- Camera as an API:
		- 
*/

/// \TODO Fix the Smoothie
namespace Voidless
{
public class CameraRotationFollow : CameraFollow
{
	[Space(5f)]
	[Header("Rotation Following's Attributes:")]
	[SerializeField] private EulerRotation _rotationOffset; 	/// <summary>Rotation's Offset.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private float gizmosRadius; 				/// <summary>Gizmos' Radius.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float colorAlpha; 				/// <summary>Color's Alpha.</summary>
#endif
	protected float angularSpeed; 								/// <summary>Angular Speed's Reference.</summary>

	/// <summary>Gets and Sets rotationOffset property.</summary>
	public EulerRotation rotationOffset
	{
		get { return _rotationOffset; }
		set { _rotationOffset = value; }
	}

#if UNITY_EDITOR
	/// <summary>Draws Rotation's Gizmos.</summary>
	private void OnDrawGizmos()
	{
		Quaternion rotation = (transform.rotation * rotationOffset);

		Handles.color = Color.red.WithAlpha(colorAlpha);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, Vector3.Angle(transform.forward, rotation * Vector3.forward) * Mathf.Sign(rotationOffset.eulerAngles.x), gizmosRadius);

		Handles.color = Color.green.WithAlpha(colorAlpha);
		Handles.DrawSolidArc(transform.position, transform.up, transform.right, Vector3.Angle(transform.right, rotation * Vector3.right) * Mathf.Sign(rotationOffset.eulerAngles.y), gizmosRadius);

		Handles.color = Color.blue.WithAlpha(colorAlpha);
		Handles.DrawSolidArc(transform.position, transform.forward, transform.up, Vector3.Angle(transform.up, rotation * Vector3.up) * Mathf.Sign(rotationOffset.eulerAngles.z), gizmosRadius);
	}
#endif

	/// <summary>Resets Component.</summary>
	protected override void Reset()
	{
		base.Reset();
#if UNITY_EDITOR
		gizmosRadius = 0.5f;
		colorAlpha = 0.2f;
#endif
	}

	/// <summary>Follows Target [takes into account component's parameters, but target overrides the Target's Transform].</summary>
	/// <param name="_target">Target to follow.</param>
	/// <param name="_target">Target to follow.</param>
	public override void FollowTarget(Vector3 _from, Vector3 _target)
	{
		/// if instant, the rotation won't change.
		GetFollowingDirection(_from, _target);
		Quaternion rotation = Quaternion.LookRotation(followingDirection) * rotationOffset;

		switch(followMode)
		{
			case FollowMode.Smooth:
			float deltaAngle = Quaternion.Angle(voidlessCamera.rigidbody.rotation, rotation);

			if(deltaAngle > 0.0f)
			{
				float deltaTime = voidlessCamera.GetDeltaTime();
				float t = Mathf.SmoothDampAngle(
					deltaAngle,
					0.0f,
					ref angularSpeed,
					followDuration, //deltaTime
					limitFollowingSpeed ? maxFollowSpeed : Mathf.Infinity,
					deltaTime
				);
				t = (1.0f - (t / deltaAngle));

				rotation = Quaternion.Lerp(voidlessCamera.rigidbody.rotation, rotation, t);
			}
			break;
		}
		
		voidlessCamera.rigidbody.MoveRotation(rotation);
		EvaluateTargetProximity();
	}

	/// <summary>Gets Following's Direction.</summary>
	/// <param name="_from">Observer's Point.</param>
	/// <param name="_to">Target's Point.</param>
	/// <returns>Following direction from given origin towards target's point.</returns>
	protected override Vector3 GetFollowingDirection(Vector3 _from, Vector3 _to)
	{
		Vector3 direction = (_to - _from);

		desiredNonIgnoredDirection = direction;
	
		if(ignoreAxes.HasFlag(Axes3D.X)) direction.x = 0.0f;
		if(ignoreAxes.HasFlag(Axes3D.Y)) direction.y = 0.0f;
		if(ignoreAxes.HasFlag(Axes3D.Z)) direction.z = 0.0f;

		followingDirection = direction;

		return direction;
	}

	/// <summary>Evaluates Target's Proximity.</summary>
	protected override void EvaluateTargetProximity()
	{
		reachedTarget = Vector3.Dot(voidlessCamera.rigidbody.transform.forward, followingDirection) >= reachTolerance;
	}
}
}