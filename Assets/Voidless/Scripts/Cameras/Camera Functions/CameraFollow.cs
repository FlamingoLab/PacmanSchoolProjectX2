using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum FollowMode 													/// <summary>Follow Modes.</summary>
{
	Instant, 															/// <summary>Instant's Follow Mode.</summary>
	Smooth, 															/// <summary>Smooth's Follow Mode.</summary>
	//PhysicsSmooth 														/// <summary>Physics-Smooth's Follow Mode.</summary>
}

public abstract class CameraFollow : VCameraComponent
{
	protected const float SMOOTH_TIME_INSTANT = 0.1f; 					/// <summary>Instant's Smooth Time.</summary>

	[Header("Following's Attributes:")]
	[SerializeField] private FollowMode _followMode; 					/// <summary>Follow's Mode.</summary>
	[SerializeField] private bool _relativeToTarget; 					/// <summary>Make the following relative to the target?.</summary>
	[SerializeField] private bool _limitFollowingSpeed; 				/// <summary>Limit Maximum Following's Speed.</summary>
	[SerializeField] protected Axes3D _ignoreAxes; 						/// <summary>Axes to ignore when following.</summary>
	[SerializeField] protected Axes3D _ignoreFocusAxes; 				/// <summary>Center's Focus Axes to ignore when following.</summary>
	[SerializeField]
	[Range(0.1f, 1.0f)] private float _followDuration; 					/// <summary>Follow's Duration.</summary>
	[SerializeField] private float _maxFollowSpeed; 					/// <summary>Maximum's Following Speed.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _reachTolerance; 					/// <summary>Reach's Tolerance towards target.</summary>
	private Vector3 _followingDirection; 								/// <summary>Current Frame's Following Direction.</summary>
	private Vector3 _desiredNonIgnoredDirection; 						/// <summary>Current Frame's Following Direction without ignoring axes.</summary>
	private Vector3 _viewportOffset; 									/// <summary>Viewport's Offset.</summary>
	private bool _reachedTarget; 										/// <summary>Has this follow component reached its target.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets followMode property.</summary>
	public FollowMode followMode
	{
		get { return _followMode; }
		set { _followMode = value; }
	}

	/// <summary>Gets and Sets relativeToTarget property.</summary>
	public bool relativeToTarget
	{
		get { return _relativeToTarget; }
		set { _relativeToTarget = value; }
	}

	/// <summary>Gets and Sets limitFollowingSpeed property.</summary>
	public bool limitFollowingSpeed
	{
		get { return _limitFollowingSpeed; }
		set { _limitFollowingSpeed = value; }
	}

	/// <summary>Gets and Sets reachedTarget property.</summary>
	public bool reachedTarget
	{
		get { return _reachedTarget; }
		protected set { _reachedTarget = value; }
	}

	/// <summary>Gets and Sets ignoreAxes property.</summary>
	public Axes3D ignoreAxes
	{
		get { return _ignoreAxes; }
		set { _ignoreAxes = value; }
	}

	/// <summary>Gets and Sets ignoreFocusAxes property.</summary>
	public Axes3D ignoreFocusAxes
	{
		get { return _ignoreFocusAxes; }
		set { _ignoreFocusAxes = value; }
	}

	/// <summary>Gets and Sets followDuration property.</summary>
	public float followDuration
	{
		get { return _followDuration; }
		set { _followDuration = value; }
	}

	/// <summary>Gets and Sets maxFollowSpeed property.</summary>
	public float maxFollowSpeed
	{
		get { return limitFollowingSpeed ? _maxFollowSpeed : Mathf.Infinity; }
		set { _maxFollowSpeed = value; }
	}

	/// <summary>Gets reachTolerance property.</summary>
	public float reachTolerance { get { return _reachTolerance; } }

	/// <summary>Gets and Sets followingDirection property.</summary>
	public Vector3 followingDirection
	{
		get { return _followingDirection; }
		protected set { _followingDirection = value; }
	}

	/// <summary>Gets and Sets desiredNonIgnoredDirection property.</summary>
	public Vector3 desiredNonIgnoredDirection
	{
		get { return _desiredNonIgnoredDirection; }
		set { _desiredNonIgnoredDirection = value; }
	}

	/// <summary>Gets and Sets viewportOffset property.</summary>
	public Vector3 viewportOffset
	{
		get { return _viewportOffset; }
		set { _viewportOffset = value; }
	}

	/// <summary>Gets and Sets position property.</summary>
	public Vector3 position
	{
		get { return voidlessCamera.updateCameraAt == LoopType.FixedUpdate ? voidlessCamera.rigidbody.position : transform.position; }
		set
		{
			switch(voidlessCamera.updateCameraAt)
			{
				case LoopType.Update:
				case LoopType.LateUpdate:
				transform.position = value;
				break;

				case LoopType.FixedUpdate:
				voidlessCamera.rigidbody.position = value;
				break;
			}
		}
	}

	/// <summary>Gets and Sets rotation property.</summary>
	public Quaternion rotation
	{
		get { return voidlessCamera.updateCameraAt == LoopType.FixedUpdate ? voidlessCamera.rigidbody.rotation : transform.rotation; }
		set
		{
			switch(voidlessCamera.updateCameraAt)
			{
				case LoopType.Update:
				case LoopType.LateUpdate:
				transform.rotation = value;
				break;

				case LoopType.FixedUpdate:
				voidlessCamera.rigidbody.rotation = value;
				break;
			}
		}
	}
#endregion

#region UnityMethods:
	/// <summary>Callback called at end of each frame.</summary>
	public void OnLateUpdate()
	{
		//UpdateFollowingDirection();
		FollowTarget();
		EvaluateTargetProximity();
	}

	/// <summary>Updates CameraFollow's instance at each Physics Thread's frame.</summary>
	public void OnFixedUpdate()
	{
		//UpdateFollowingDirection();
		FollowTarget();
		EvaluateTargetProximity();
	}

	/// <summary>Resets Component.</summary>
	protected virtual void Reset()
	{
		followMode = FollowMode.Smooth;
		relativeToTarget = true;
		limitFollowingSpeed = true;
		ignoreAxes = Axes3D.None;
		ignoreFocusAxes = Axes3D.None;
		followDuration = 1.0f;
		maxFollowSpeed = 100.0f;
	}
#endregion

#region FollowTargetOverrides:
	/// <summary>Follows target.</summary>
	public void FollowTarget()
	{
		if(voidlessCamera.target == null) return;
		
		FollowTarget(position, voidlessCamera.GetTargetPosition());
	}

	/// <summary>Follows target [overrides Target's transform].</summary>
	/// <param name="_target">Target to follow.</param>
	public void FollowTarget(Vector3 _target)
	{	
		FollowTarget(position, _target);
	}

	/// <summary>Follows target [overrides Target's transform].</summary>
	/// <param name="_from">Origin's Point.</param>
	/// <param name="_target">Target to follow.</param>
	public abstract void FollowTarget(Vector3 _from, Vector3 _to);
#endregion

#region GetFollowingDirectionOverrides:
	/// <summary>Gets Following's Direction.</summary>
	/// <param name="_target">Target to follow.</param>
	/// <returns>Following direction from camera's position towards target's point.</returns>
	protected Vector3 GetFollowingDirection()
	{
		return GetFollowingDirection(position, voidlessCamera.target != null ? voidlessCamera.GetTargetPosition() : Vector3.zero);
	}

	/// <summary>Gets Following's Direction.</summary>
	/// <returns>Following direction from camera's position towards target's point.</returns>
	protected Vector3 GetFollowingDirection(Vector3 _target)
	{
		return GetFollowingDirection(position, _target);
	}

	/// <summary>Gets Following's Direction.</summary>
	/// <param name="_from">Observer's Point.</param>
	/// <param name="_to">Target's Point.</param>
	/// <returns>Following direction from given origin towards target's point.</returns>
	protected abstract Vector3 GetFollowingDirection(Vector3 _from, Vector3 _to);
#endregion

	/// <summary>Evaluates Target's Proximity.</summary>
	protected abstract void EvaluateTargetProximity();

	/// <returns>Focus' Direction, ignoring the flagged axes.</returns>
	public Vector3 GetCenterFocusDirection()
	{
		return transform.IgnoreVectorAxes(voidlessCamera.centerFocusDirection, ignoreFocusAxes, true);
	}

	/// <summary>Copies this componen's stats into another CameraFollow component.</summary>
	/// <param name="_cameraFollow">Component that will have the same stats as this component.</param>
	public void CopyStatsTo(CameraFollow _cameraFollow)
	{
		_cameraFollow.followMode = followMode;
		_cameraFollow.limitFollowingSpeed = limitFollowingSpeed;
		_cameraFollow.followDuration = followDuration;
		_cameraFollow.maxFollowSpeed = maxFollowSpeed;
	}
}
}