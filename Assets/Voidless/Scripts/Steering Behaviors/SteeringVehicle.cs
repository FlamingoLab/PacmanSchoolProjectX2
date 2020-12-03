using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  SteeringVehicle
**
** Purpose: This Component contains the main attributes of a Vehicle model defined by Craig W. Reynolds, plus
** a radius attribute (defined as a FloatRange) for Arrival Behaviors.
** 
** This Component gives the container the capacity to get the following Steering Behaviors:
** 
** 	- Individual:
** 		- Seek
** 		- Flee
** 		- Pursuit
** 		- Evasion
** 	- Groupal:
** 		- Separation
** 		- Cohesion
** 		- Alignment
** 
** NOTE: This Component does not displace/rotate the  GameObject, it just returns calculated forces. Therefore,
** the choice of how to displace/rotate the GameObject is up to the user.
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Voidless
{
[RequireComponent(typeof(Rigidbody))]
public class SteeringVehicle : MonoBehaviour, ISteeringVehicle
{
	[Header("Vehicle's Attributes:")]
	[SerializeField] private float _maxSpeed; 			/// <summary>Vehicle's Maximum Speed.</summary>
	[SerializeField] private float _maxSteeringForce; 	/// <summary>Vehicle's Maximum Steering Force.</summary>
	[Space(5f)]
	[SerializeField] private FloatRange _radiusRange; 	/// <summary>Vehicle's Radius' Range.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 		/// <summary>Gizmos' Color.</summary>
#endif
	private Vector3 _velocity; 							/// <summary>Velocit's Vector..</summary>
	private Rigidbody _rigidbody; 						/// <summary>Rigidbody's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets maxSpeed property.</summary>
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set { _maxSpeed = value; }
	}

	/// <summary>Gets and Sets maxSteeringForce property.</summary>
	public float maxSteeringForce
	{
		get { return _maxSteeringForce; }
		set { _maxSteeringForce = value; }
	}

	/// <summary>Gets and Sets mass property.</summary>
	public float mass
	{
		get { return rigidbody.mass; }
		set { rigidbody.mass = value; }
	}

	/// <summary>Gets and Sets radiusRange property.</summary>
	public FloatRange radiusRange
	{
		get { return _radiusRange; }
		set { _radiusRange = value; }
	}

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector3 velocity
	{
		get { return _velocity; }
		set { _velocity = value; }
	}

	/// <summary>Gets and Sets rigidbody Component.</summary>
	public new Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}
#endregion

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor; 
		Gizmos.DrawRay(transform.position, velocity);
		Gizmos.DrawWireSphere(transform.position, radiusRange.min);
		Gizmos.DrawWireSphere(transform.position, radiusRange.max);
	}
#endif

#region IndividualBehaviors:
	/// <summary>Gets Seek's Steering Force towards Target.</summary>
	/// <param name="_target">Destination Target.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Seek's Steering Force.</returns>
	public Vector3 GetSeekForce(Vector3 _target, float _weight = 1.0f)
	{
		Vector3 desiredForce = (_target - transform.position).normalized * maxSpeed;
		Vector3 steeringForce = (desiredForce - velocity).normalized * maxSteeringForce;

		if(mass != 1.0f) steeringForce /= mass; 

		return steeringForce * _weight;
	}

	/// <summary>Gets Flee's Steering Force towards Target.</summary>
	/// <param name="_target">Destination Target.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Flee's Steering Force.</returns>
	public Vector3 GetFleeForce(Vector3 _target, float _weight = 1.0f)
	{
		Vector3 desiredForce = (transform.position - _target).normalized * maxSpeed;
		Vector3 steeringForce = (desiredForce - velocity).normalized * maxSteeringForce;

		if(mass != 1.0f) steeringForce /= mass;

		return steeringForce * _weight;
	}

	/// <summary>Gets Pursuit's Steering Force Towards Destination.</summary>
	/// <param name="_target">Target's Destination.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Pursuit's Steering Force.</returns>
	public Vector3 GetPursuitForce(Vector3 _target, Vector3 _velocity, float _weight = 1.0f)
	{
		return GetSeekForce(_target + _velocity, _weight);
	}

	/// <summary>Gets Pursuit's Steering Force Towards Rigidbody.</summary>
	/// <param name="_target">Target's Rigidbody.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Pursuit's Steering Force.</returns>
	public Vector3 GetPursuitForce(Rigidbody _target, float _weight = 1.0f)
	{
		return GetSeekForce(_target.position + _target.velocity, _weight);
	}

	/// <summary>Gets Evasion's Steering Force Towards Destination.</summary>
	/// <param name="_target">Target's Destination.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Evasion's Steering Force.</returns>
	public Vector3 GetEvasionForce(Vector3 _target, Vector3 _velocity, float _weight = 1.0f)
	{
		return GetSeekForce(_target + _velocity, _weight);
	}

	/// <summary>Gets Evasion's Steering Force Towards Rigidbody.</summary>
	/// <param name="_target">Target's Rigidbody.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Evasion's Steering Force.</returns>
	public Vector3 GetEvasionForce(Rigidbody _target, float _weight = 1.0f)
	{
		return GetFleeForce(_target.position + _target.velocity, _weight);
	}
#endregion

#region SeparationBehaviors:
	/// <summary>Gets Separation's Steering Force.</summary>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Separation's Steering Force.</returns>
	public Vector3 GetSeparationForce(ICollection<SteeringVehicle> _vehicles, float _weight = 1.0f)
	{
		Vector3 steeringForce = Vector3.zero;

		foreach(SteeringVehicle vehicle in _vehicles)
		{
			steeringForce += GetFleeForce(vehicle.transform.position);
		}

		return _vehicles.Count > 0 ? (steeringForce / _vehicles.Count) * _weight : steeringForce;
	}

	/// <summary>Gets Separation's Steering Force.</summary>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <returns>Separation's Steering Force.</returns>
	public Vector3 GetSeparationForce(float _weight = 1.0f, params SteeringVehicle[] _vehicles)
	{
		return GetSeparationForce(_vehicles, _weight);
	}

	/// <summary>Gets Separation's Steering Force.</summary>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Separation's Steering Force.</returns>
	public Vector3 GetSeparationForce(ICollection<Vector3> _targets, float _weight = 1.0f)
	{
		Vector3 steeringForce = Vector3.zero;

		foreach(Vector3 target in _targets)
		{
			steeringForce += GetFleeForce(target);
		}

		return _targets.Count > 0 ? (steeringForce / _targets.Count) * _weight : steeringForce;
	}

	/// <summary>Gets Separation's Steering Force.</summary>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <param name="_targets">Vehicles' Group.</param>
	/// <returns>Separation's Steering Force.</returns>
	public Vector3 GetSeparationForce(float _weight = 1.0f, params Vector3[] _targets)
	{
		return GetSeparationForce(_targets, _weight);
	}
#endregion

#region CohesionBehaviors:
	/// <summary>Gets Cohesion's Steering Force.</summary>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Cohesion's Steering Force.</returns>
	public Vector3 GetCohesionForce(ICollection<SteeringVehicle> _vehicles, float _weight = 1.0f)
	{
		Vector3 steeringForce = Vector3.zero;

		foreach(SteeringVehicle vehicle in _vehicles)
		{
			steeringForce += GetSeekForce(vehicle.transform.position);
		}

		return _vehicles.Count > 0 ? (steeringForce / _vehicles.Count) * _weight : steeringForce;
	}

	/// <summary>Gets Cohesion's Steering Force.</summary>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <returns>Cohesion's Steering Force.</returns>
	public Vector3 GetCohesionForce(float _weight = 1.0f, params SteeringVehicle[] _vehicles)
	{
		return GetCohesionForce(_vehicles, _weight);
	}

	/// <summary>Gets Cohesion's Steering Force.</summary>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Cohesion's Steering Force.</returns>
	public Vector3 GetCohesionForce(ICollection<Vector3> _targets, float _weight = 1.0f)
	{
		Vector3 steeringForce = Vector3.zero;

		foreach(Vector3 target in _targets)
		{
			steeringForce += GetSeekForce(target);
		}

		return _targets.Count > 0 ? (steeringForce / _targets.Count) * _weight : steeringForce;
	}

	/// <summary>Gets Cohesion's Steering Force.</summary>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <param name="_targets">Vehicles' Group.</param>
	/// <returns>Cohesion's Steering Force.</returns>
	public Vector3 GetCohesionForce(float _weight = 1.0f, params Vector3[] _targets)
	{
		return GetCohesionForce(_targets, _weight);
	}
#endregion

#region AlignmentBehaviors:
	/// <summary>Gets Alignment's Steering Force.</summary>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Alignment's Steering Force.</returns>
	public Vector3 GetAlignmentForce(ICollection<SteeringVehicle> _vehicles, float _weight = 1.0f)
	{
		Vector3 steeringForce = Vector3.zero;

		foreach(SteeringVehicle vehicle in _vehicles)
		{
			steeringForce += GetSeekForce(vehicle.transform.position + vehicle.velocity);
		}

		return _vehicles.Count > 0 ? (steeringForce / _vehicles.Count) * _weight : steeringForce;
	}

	/// <summary>Gets Alignment's Steering Force.</summary>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <returns>Alignment's Steering Force.</returns>
	public Vector3 GetAlignmentForce(float _weight = 1.0f, params SteeringVehicle[] _vehicles)
	{
		return GetAlignmentForce(_vehicles, _weight);
	}

	/// <summary>Gets Alignment's Steering Force.</summary>
	/// <param name="_vehicles">Vehicles' Group.</param>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <returns>Alignment's Steering Force.</returns>
	public Vector3 GetAlignmentForce(ICollection<Rigidbody> _bodies, float _weight = 1.0f)
	{
		Vector3 steeringForce = Vector3.zero;

		foreach(Rigidbody body in _bodies)
		{
			steeringForce += GetSeekForce(body.position + body.velocity);
		}

		return _bodies.Count > 0 ? (steeringForce / _bodies.Count) * _weight : steeringForce;
	}

	/// <summary>Gets Alignment's Steering Force.</summary>
	/// <param name="_weight">Optional Weight [default 1.0f].</param>
	/// <param name="_bodies">Vehicles' Group.</param>
	/// <returns>Alignment's Steering Force.</returns>
	public Vector3 GetAlignmentForce(float _weight = 1.0f, params Rigidbody[] _bodies)
	{
		return GetAlignmentForce(_bodies, _weight);
	}
#endregion

#region ArrivalBehaviors:
	/// <summary>Gets Arrival's Weight.</summary>
	/// <param name="_target">Destination Target.</param>
	/// <param name="_radiusRange">Radius' Range.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public float GetArrivalWeight(Vector3 _target, FloatRange _radiusRange, float _deltaTime, bool _projectPosition = false)
	{
		if(_radiusRange.min == _radiusRange.max) _radiusRange.min = 0.0f;

		Vector3 relativeVelocity = transform.position;
		if(_projectPosition) relativeVelocity += (velocity * _deltaTime);
		Vector3 desiredTarget = _radiusRange.Min() == 0.0f ? _target : _target + (((relativeVelocity - _target).normalized * _radiusRange.Min()));
		Vector3 direction = (desiredTarget - relativeVelocity);
		
		/*float squareDistance = direction.sqrMagnitude;
		_radiusRange.min *= _radiusRange.min;
		_radiusRange.max *= _radiusRange.max;*/

		return Mathf.Clamp((direction.magnitude / (_radiusRange.Max() - _radiusRange.Min())), 0.0f, 1.0f);
	}
#endregion

	/// <summary>Updates Vehicle's Velocity.</summary>
	/// <param name="_steering">Steering Vector.</param>
	public void UpdateVelocity(Vector3 _steering)
	{
		velocity = _steering;
		//velocity = velocity.normalized * maxSpeed;
	}

#region StaticFunctions:
	/// <summary>Gets Arrival's Weight.</summary>
	/// <param name="_position">Position's Point.</param>
	/// <param name="_target">Destination Target.</param>
	/// <param name="_radiusRange">Radius' Range.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public static float GetArrivalWeight(Vector3 _position, Vector3 _target, FloatRange _radiusRange)
	{
		if(_radiusRange.min == _radiusRange.max) _radiusRange.min = 0.0f;

		_radiusRange.min *= _radiusRange.min;
		_radiusRange.max *= _radiusRange.max;

		Vector3 direction = _target - _position;
		float squareDistance = direction.sqrMagnitude;
		float squareRadius = _radiusRange.max - _radiusRange.min;

		return (Mathf.Min(squareDistance, squareRadius) / squareRadius);
	}
#endregion

}
}