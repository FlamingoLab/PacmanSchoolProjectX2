using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum LoopType
{
	Update,
	LateUpdate,
	FixedUpdate
}

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(CameraViewportHandler))]
[RequireComponent(typeof(CameraOcclusionHandler))]
[RequireComponent(typeof(OrientationNormalAdjuster))]
[RequireComponent(typeof(Rigidbody))]
public abstract class VCamera : Singleton<VCamera>
{
	public const float ANGLE_LOOK_AT_EACH_OTHER = 180.0f; 					/// <summary>Look at each other's angle.</summary>

	[SerializeField] private LoopType _updateCameraAt; 						/// <summary>Loop to update the camera at.</summary>
	[Space(5f)]
	[Header("Camera's Attributes:")]
	[SerializeField] private Transform _target; 							/// <summary>Camera's Focus Point.</summary>
	[SerializeField] private Rigidbody _physicsTarget; 						/// <summary>Physics' Target as a Rigidbody.</summary>
	[SerializeField] private FloatRange _distanceRange; 					/// <summary>Distance between camera and target's Range.</summary>
	[SerializeField]
	[Range(0.0f, 90.0f)] private float _angleTolerance; 					/// <summary>Tolerance of degrees if the Third Person Character is heading towards the camera.</summary>
	[Space(5f)]
	[Header("Distance's Attributes:")]
	[SerializeField] private bool _limitDistanceChangeSpeed; 				/// <summary>Limit Distance's Change Speed?.</summary>
	[SerializeField]
	[Range(0.1f, 1.0f)] private float _distanceChangeDuration; 				/// <summary>Distance Change's Duration.</summary>
	[SerializeField] private float _maxDistanceChangeSpeed; 				/// <summary>Maximum Distance's Change Speed.</summary>
	private CameraViewportHandler _viewportHandler; 						/// <summary>CameraViewportHandler's Component.</summary>
	private CameraOcclusionHandler _occlusionHandler; 						/// <summary>CameraOcclusionHandler's Component.</summary>
	private OrientationNormalAdjuster _orientationNormalAdjuster; 			/// <summary>OrientationNormalAdjuster's Component.</summary>
	private Rigidbody _rigidbody; 											/// <summary>Rigidbody's Component.</summary>
	private Behavior _cameraEffect; 										/// <summary>Coroutine controller for the actual Camera's effect.</summary>
	private Camera _camera; 												/// <summary>Camera's Component.</summary>
	private TransformData _targetData;
	private Vector3 _centerFocusDirection; 									/// <summary>Center Focus' Direction.</summary>
	private float _distance; 												/// <summary>Actual distance between camera and target.</summary>
	protected float currentDistance; 										/// <summary>Current Distance's Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets updateCameraAt property.</summary>
	public LoopType updateCameraAt
	{
		get { return _updateCameraAt; }
		protected set { _updateCameraAt = value; }
	}

	/// <summary>Gets and Sets target property.</summary>
	public Transform target
	{
		get { return _target; }
		set { _target = value; }
	}

	/// <summary>Gets and Sets physicsTarget property.</summary>
	public Rigidbody physicsTarget
	{
		get { return _physicsTarget; }
		set { _physicsTarget = value; }
	}

	/// <summary>Gets and Sets targetData property.</summary>
	public TransformData targetData
	{
		get { return _targetData; }
		set { _targetData = value; }
	}

	/// <summary>Gets and Sets distanceRange property.</summary>
	public FloatRange distanceRange
	{
		get { return _distanceRange; }
		set { _distanceRange = value; }
	}

	/// <summary>Gets and Sets angleTolerance property.</summary>
	public float angleTolerance
	{
		get { return _angleTolerance; }
		set { _angleTolerance = Mathf.Clamp(0.0f, 90.0f, value); }
	}

	/// <summary>Gets and Sets distanceChangeDuration property.</summary>
	public float distanceChangeDuration
	{
		get { return _distanceChangeDuration; }
		set { _distanceChangeDuration = value; }
	}

	/// <summary>Gets and Sets maxDistanceChangeSpeed property.</summary>
	public float maxDistanceChangeSpeed
	{
		get { return limitDistanceChangeSpeed ? _maxDistanceChangeSpeed : Mathf.Infinity; }
		set { _maxDistanceChangeSpeed = value; }
	}

	/// <summary>Gets and Sets distance property.</summary>
	public float distance
	{
		get { return _distance; }
		set { _distance = Mathf.Clamp(value, distanceRange.Min(), distanceRange.Max()); }
	}

	/// <summary>Gets and Sets limitDistanceChangeSpeed property.</summary>
	public bool limitDistanceChangeSpeed
	{
		get { return _limitDistanceChangeSpeed; }
		set { _limitDistanceChangeSpeed = value; }
	}
	
	/// <summary>Gets and Sets centerFocusDirection property.</summary>
	public Vector3 centerFocusDirection
	{
		get { return _centerFocusDirection; }
		set { _centerFocusDirection = value; }
	}

	/// <summary>Gets and Sets cameraEffect property.</summary>
	public Behavior cameraEffect
	{
		get { return _cameraEffect; }
		set { _cameraEffect = value; }
	}

	/// <summary>Gets and Sets camera Component.</summary>
	public new Camera camera
	{ 
		get
		{
			if(_camera == null) _camera = GetComponent<Camera>();
			return _camera;
		}
	}

	/// <summary>Gets and Sets viewportHandler Component.</summary>
	public CameraViewportHandler viewportHandler
	{ 
		get
		{
			if(_viewportHandler == null) _viewportHandler = GetComponent<CameraViewportHandler>();
			return _viewportHandler;
		}
	}

	/// <summary>Gets and Sets occlusionHandler Component.</summary>
	public CameraOcclusionHandler occlusionHandler
	{ 
		get
		{
			if(_occlusionHandler == null) _occlusionHandler = GetComponent<CameraOcclusionHandler>();
			return _occlusionHandler;
		}
	}

	/// <summary>Gets and Sets orientationNormalAdjuster Component.</summary>
	public OrientationNormalAdjuster orientationNormalAdjuster
	{ 
		get
		{
			if(_orientationNormalAdjuster == null) _orientationNormalAdjuster = GetComponent<OrientationNormalAdjuster>();
			return _orientationNormalAdjuster;
		}
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}

	/// <summary>Gets and Sets position property.</summary>
	public Vector3 position
	{
		get { return updateCameraAt == LoopType.FixedUpdate ? rigidbody.position : transform.position; }
		set
		{
			switch(updateCameraAt)
			{
				case LoopType.Update:
				case LoopType.LateUpdate:
				transform.position = value;
				break;

				case LoopType.FixedUpdate:
				rigidbody.position = value;
				break;
			}
		}
	}

	/// <summary>Gets and Sets rotation property.</summary>
	public Quaternion rotation
	{
		get { return updateCameraAt == LoopType.FixedUpdate ? rigidbody.rotation : transform.rotation; }
		set
		{
			switch(updateCameraAt)
			{
				case LoopType.Update:
				case LoopType.LateUpdate:
				transform.rotation = value;
				break;

				case LoopType.FixedUpdate:
				rigidbody.rotation = value;
				break;
			}
		}
	}

	/// <summary>Implicit VCamera to Camera operator.</summary>
	public static implicit operator Camera(VCamera _baseCamera) { return _baseCamera.camera; }
#endregion

#region UnityMethods:
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		//Gizmos.DrawRay(transform.position, centerFocusDirection);
	}

	/// <summary>Resets Component.</summary>
	private void Reset()
	{
		currentDistance = 0.0f;
		rigidbody.isKinematic = true;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}
	
	/// <summary>VCamera's tick at the end of each frame.</summary>
	private void LateUpdate()
	{
		switch(updateCameraAt)
		{
			case LoopType.Update:
			case LoopType.LateUpdate:
			OnUpdate();
			UpdateCamera();
			break;
		}
	}

	/// <summary>Updates VCamera's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		switch(updateCameraAt)
		{
			case LoopType.FixedUpdate:
			OnUpdate();
			FixedUpdateCamera();
			break;
		}
	}
#endregion

	/// <summary>Callback called on either LateUpdate or FixedUpdate.</summary>
	protected virtual void OnUpdate()
	{
		UpdateDistance();
		UpdateCenterFocusDirection();
		UpdateTargetData();
	}

	/// <summary>Updates Camera.</summary>
	protected abstract void UpdateCamera();

	/// <summary>Updates Camera on Physics' Thread.</summary>
	protected abstract void FixedUpdateCamera();

	/// <summary>Calculates an adjusted direction given direction's axes.</summary>
	/// <param name="_x">Axis X.</param>
	/// <param name="_y">Axis Y.</param>
	/// <returns>Adjusted Direction.</returns>
	public virtual Vector3 GetAdjustedDirection(float _x, float _y)
	{
		return (transform.right * _x) + (orientationNormalAdjuster.forward * _y);
	}

	/// <returns>Target's Position.</returns>
	public virtual Vector3 GetTargetPosition()
	{
		if(updateCameraAt == LoopType.FixedUpdate && physicsTarget != null)
		{
			return physicsTarget.position;
		
		} else if(target != null)
		{
			return target.position;
		}
		else return targetData.position;
	}

	/// <returns>Target's Rotation.</returns>
	public virtual Quaternion GetTargetRotation()
	{
		if(updateCameraAt == LoopType.FixedUpdate && physicsTarget != null)
		{
			return physicsTarget.rotation;
		
		} else if(target != null)
		{
			return target.rotation;
		}
		else return targetData.rotation;
	}

	/// <returns>True if camera and target are faceing each other, considering the tolerance angle, false otherwise or if there is no target.</returns>
	public virtual bool CameraAndTargetLookingAtEachOther()
	{
		if(target != null)
		{
			float angle = Vector3.Angle(transform.forward, target.forward);
			return ((angle <= ANGLE_LOOK_AT_EACH_OTHER) && (angle >= ANGLE_LOOK_AT_EACH_OTHER - angleTolerance));
		}
		else return false;
	}

	/// <summary>Updates Target's Transform Data.</summary>
	protected virtual void UpdateTargetData()
	{
		_targetData.position = GetTargetPosition();
		_targetData.rotation = GetTargetRotation();
	}

	/// <summery>Sets Direction between Focus Center and Target's Position.</summery>
	protected virtual void UpdateCenterFocusDirection()
	{
		Ray viewportRay = camera.ViewportPointToRay(new Vector3(viewportHandler.gridAttributes.centerX, viewportHandler.gridAttributes.centerY, 0.0f));
		Ray centerRay = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
		Vector3 scaledViewportDirection = viewportRay.direction * distance;
		Vector3 centerProjection = VVector3.VectorProjection(scaledViewportDirection, centerRay.direction);
		centerFocusDirection = (centerProjection - (scaledViewportDirection));
		
		if(camera.orthographic)
		{
			Vector3 shift = transform.InverseTransformDirection(viewportRay.direction);
			shift.z = 0.0f;
			centerFocusDirection = transform.TransformVector(shift);
		}

		/*Debug.DrawRay(viewportRay.origin, scaledViewportDirection, Color.cyan);
		Debug.DrawRay(centerRay.origin, centerProjection, Color.magenta);
		Debug.DrawRay(position, centerFocusDirection, Color.yellow);*/
	}

	/// <summary>Updates Distance defined by occlusion's handling.</summary>
	public virtual void UpdateDistance()
	{
		float bestDistance = occlusionHandler.CalculateAdjustedDistance(GetTargetPosition());

		distance = Mathf.SmoothDamp(
			distance,
			Mathf.Clamp(bestDistance, distanceRange.Min(), distanceRange.Max()),
			ref currentDistance,
			distanceChangeDuration,
			limitDistanceChangeSpeed ? maxDistanceChangeSpeed : Mathf.Infinity,
			GetDeltaTime()
		);
	}

	/// <summary>Sets New Target.</summary>
	/// <param name="_target">Target's Transform.</param>
	public void SetTarget(Transform _target)
	{
		target = _target;
	}

	/// <summary>Sets New Target.</summary>
	/// <param name="_target">Target's Rigidbody.</param>
	public void SetTarget(Rigidbody _target)
	{
		if(_target == null) return;

		rigidbody.isKinematic = true;
		_target.interpolation = RigidbodyInterpolation.Interpolate;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		target = _target.transform;
		physicsTarget = _target;
	}

	/// <returns>Axes where the target is currently within.</returns>
	/// <param name="_point">Point to evaluate.</param>
	public Axes3D GetAxesWhereTargetIsWithin(Vector3 _point)
	{
		return viewportHandler.Axes3DWithinGridFocusArea(_point);
	}

	/// <returns>Gets Delta Time according to the Loop Type.</returns>
	public float GetDeltaTime()
	{
		switch(updateCameraAt)
		{
			case LoopType.Update:
			case LoopType.LateUpdate: 	return Time.deltaTime;
			case LoopType.FixedUpdate: 	return Time.fixedDeltaTime;
			default: 					return Time.smoothDeltaTime;
		}
	}
}
}