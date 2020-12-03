using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(SteeringVehicle))]
public class SteeringMovementAlongRadius : MonoBehaviour
{
	[SerializeField] private Axes3D _axes; 			/// <summary>Movement's Axes.</summary>
	[SerializeField] private float _radius; 		/// <summary>Following's Radius.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 	/// <summary>Gizmos' Color.</summary>
#endif
	private Vector3 _initialPosition; 				/// <summary>Initial Position [constant position for the radius].</summary>
	private SteeringVehicle _steeringVehicle; 		/// <summary>SteeringVehicle's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets axes property.</summary>
	public Axes3D axes
	{
		get { return _axes; }
		set { _axes = value; }
	}

	/// <summary>Gets and Sets radius property.</summary>
	public float radius
	{
		get { return _radius; }
		set { _radius = value; }
	}

	/// <summary>Gets and Sets initialPosition property.</summary>
	public Vector3 initialPosition
	{
		get { return _initialPosition; }
		set { _initialPosition = value; }
	}

	/// <summary>Gets steeringVehicle Component.</summary>
	public SteeringVehicle steeringVehicle
	{ 
		get
		{
			if(_steeringVehicle == null) _steeringVehicle = GetComponent<SteeringVehicle>();
			return _steeringVehicle;
		}
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireSphere(Application.isPlaying ? initialPosition : transform.position, radius);
	}
#endif

	/// <summary>SteeringFollowAlongRadius's instance initialization.</summary>
	private void Awake()
	{
		initialPosition = transform.position;
	}

	/// <summary>Performs Seek's Steering Behavior towards given Vehicle.</summary>
	/// <param name="_vehicle">Vehicle to Seek.</param>
	/// <param name="_type">Type of Time's delta [Time.deltaTime by default].</param>
	public void Seek(SteeringVehicle _vehicle, DeltaTimeType _type = DeltaTimeType.DeltaTime)
	{
		Vector3 vehiclePosition = _type == DeltaTimeType.DeltaTime ? _vehicle.transform.position : _vehicle.rigidbody.position;
		Vector3 position = _type == DeltaTimeType.DeltaTime ? transform.position : steeringVehicle.rigidbody.position;
		Vector3 radiusPosition = initialPosition + (vehiclePosition - position).normalized * radius;
		Vector3 direction = VVector3.DirectionWithAxes(radiusPosition, position, axes);
		float deltaTime = VExtensionMethods.GetDeltaTime(_type);
		float arrivalWeight = steeringVehicle.GetArrivalWeight(position + direction, _vehicle.radiusRange, deltaTime);
		Vector3 seek = steeringVehicle.GetSeekForce(position + direction, arrivalWeight);

		steeringVehicle.rigidbody.MovePosition(position + (seek * deltaTime));
		steeringVehicle.UpdateVelocity(seek);
	}

	/*
	public void Evade(SteeringVehicle _vehicle, DeltaTime _type = DeltaTimeType)
	{
		float delta = VExtensionMethods.GetDeltaTime(_type);
	}*/
}
}