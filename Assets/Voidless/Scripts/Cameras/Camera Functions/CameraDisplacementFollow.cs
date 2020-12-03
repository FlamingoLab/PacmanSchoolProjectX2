using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// \TODO Fix the Smoothie
/// \TODO Update the position as transform.position + GetFocusDirection()
namespace Voidless
{
public class CameraDisplacementFollow : CameraFollow
{
	[Space(5f)]
	[Header("Displacement Following's Attributes:")]
	[SerializeField] private NormalizedVector3 _displacementOffset; 	/// <summary>Displacement's Offset.</summary>
	protected Vector3 followVelocity;  									/// <summary>Following's Velocity.</summary>
	protected Vector3 _destinyPosition; 								/// <summary>Destinty's Position at tf.</summary>

	/// <summary>Gets and Sets displacementOffset property.</summary>
	public NormalizedVector3 displacementOffset
	{
		get { return _displacementOffset; }
		set { _displacementOffset = value; }
	}

	/// <summary>Gets and Sets destinyPosition property.</summary>
	public Vector3 destinyPosition
	{
		get { return _destinyPosition; }
		protected set { _destinyPosition = value; }
	}

	/// <summary>Follows target Smoothly towards given target [overrides Target's transform].</summary>
	/// <param name="_from">Origin's Point.</param>
	/// <param name="_target">Target to follow.</param>
	public override void FollowTarget(Vector3 _from, Vector3 _target)
	{
		float deltaTime = voidlessCamera.GetDeltaTime();
		Vector3 position = Vector3.zero;
		viewportOffset = GetCenterFocusDirection();
		
		viewportOffset = !viewportOffset.IsNaN() ? viewportOffset : Vector3.zero;
		GetFollowingDirection(_from, _target);

		switch(followMode)
		{
			case FollowMode.Instant:
			position = followingDirection;
			break;

			case FollowMode.Smooth:
			position = Vector3.SmoothDamp(
				transform.position,
				followingDirection,
				ref followVelocity,
				followMode != FollowMode.Instant ? followDuration : SMOOTH_TIME_INSTANT, //deltaTime
				(followMode != FollowMode.Instant && limitFollowingSpeed) ? maxFollowSpeed : Mathf.Infinity,
				deltaTime
			);
			break;
		}

		
		voidlessCamera.rigidbody.MovePosition(position/* + (!viewportOffset.IsNaN() ? viewportOffset : Vector3.zero)*/);
		
		Debug.DrawRay(position, viewportOffset);

		EvaluateTargetProximity();
	}

	/// <returns>Following's Direction.</returns>
	/// <param name="_from">Observer's Point.</param>
	/// <param name="_to">Target's Point.</param>
	/// <returns>Following direction from given origin towards target's point.</returns>
	protected override Vector3 GetFollowingDirection(Vector3 _from, Vector3 _to)
	{
		Axes3D axesInside = voidlessCamera.GetAxesWhereTargetIsWithin(_to);
		Vector3 offsetPosition = GetOffsetPositionRelativeToTarget(_to);

		desiredNonIgnoredDirection = offsetPosition;

		offsetPosition += viewportOffset;

		if(ignoreAxes.HasFlag(Axes3D.X) && axesInside.HasFlag(Axes3D.X)) offsetPosition.x = _from.x;
		if(ignoreAxes.HasFlag(Axes3D.Y) && axesInside.HasFlag(Axes3D.Y)) offsetPosition.y = _from.y;
		if(ignoreAxes.HasFlag(Axes3D.Z) && axesInside.HasFlag(Axes3D.Z)) offsetPosition.z = _from.z;

		followingDirection = offsetPosition;

		return offsetPosition;
	}

	/// <summary>Evaluates Target's Proximity.</summary>
	protected override void EvaluateTargetProximity()
	{
		reachedTarget = (followingDirection - transform.position).sqrMagnitude <= (reachTolerance * reachTolerance);
	}

	/// <param name="_target">Target's Vector.</param>
	/// <returns>Offseted position from target's position.</returns>
	protected virtual Vector3 GetOffsetPositionRelativeToTarget(Vector3 _target)
	{
		Vector3 scaledOffset = (displacementOffset.normalized * voidlessCamera.distance);
		Vector3 point = _target + (relativeToTarget ? (voidlessCamera.GetTargetRotation() * scaledOffset) : scaledOffset);

		return point;
	}
}
}