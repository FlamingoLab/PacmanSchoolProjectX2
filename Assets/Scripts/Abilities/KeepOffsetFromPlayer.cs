using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(SteeringVehicle))]
public class KeepOffsetFromPlayer : MonoBehaviour
{
	[SerializeField] private Vector3 _offset; 				/// <summary>Offset from player [unscaled].</summary>
	[SerializeField] private Axes3D _ignoreAxes; 			/// <summary>Axes to ignore.</summary>
	[SerializeField]
	[Range(0.0f, 0.5f)] private float _distanceThreshold; 	/// <summary>Distance's threshold.</summary>
	private SteeringVehicle _vehicle; 						/// <summary>SteeringVehicle's Component.</summary>

	/// <summary>Gets and Sets offset property.</summary>
	public Vector3 offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>Gets and Sets ignoreAxes property.</summary>
	public Axes3D ignoreAxes
	{
		get { return _ignoreAxes; }
		set { _ignoreAxes = value; }
	}

	/// <summary>Gets distanceThreshold property.</summary>
	public float distanceThreshold { get { return _distanceThreshold; } }

	/// <summary>Gets vehicle Component.</summary>
	public SteeringVehicle vehicle
	{ 
		get
		{
			if(_vehicle == null) _vehicle = GetComponent<SteeringVehicle>();
			return _vehicle;
		}
	}

	/// <summary>Keeps offset from Player.</summary>
	public void KeepOffset()
	{
		Vector3 direction = ((Game.player.rigidbody.position + offset) - vehicle.rigidbody.position);

		if(offset.x == 0.0f || (ignoreAxes | Axes3D.X) == ignoreAxes) direction.x = 0.0f;
		if(offset.y == 0.0f || (ignoreAxes | Axes3D.Y) == ignoreAxes) direction.y = 0.0f;
		if(offset.z == 0.0f || (ignoreAxes | Axes3D.Z) == ignoreAxes) direction.z = 0.0f;
		
		if(direction.sqrMagnitude > (distanceThreshold * distanceThreshold))
		vehicle.rigidbody.MovePosition(vehicle.rigidbody.position + direction.normalized * vehicle.maxSpeed * Time.fixedDeltaTime);
	}
}
}