using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

/*public enum Axes3D
{
	None = 0,
	X = 1,
	Y = 2,
	Z = 4,

	XAndY = X | Y,
	XAndZ = X | Z,
	YAndZ = Y | Z,
	All = X | Y | Z
}*/

[RequireComponent(typeof(Rigidbody))]
public class SteeringBehavior : MonoBehaviour
{
	[SerializeField] private float _maxSpeed;
	[SerializeField] private float _maxSteeringForce;
	[SerializeField] private float _arrivalRadius;
	[SerializeField] private Axes3D _movementAxes;
	[SerializeField] private Axes3D _rotationAxes;
	private Rigidbody _rigidbody;

#region Getters/Setters:
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set { _maxSpeed = value; }
	}

	public float maxSteeringForce
	{
		get { return _maxSteeringForce; }
		set { _maxSteeringForce = value; }
	}

	public float arrivalRadius
	{
		get { return _arrivalRadius; }
		set { _arrivalRadius = value; }
	}

	public Axes3D movementAxes
	{
		get { return _movementAxes; }
		set { _movementAxes = value; }
	}

	public Axes3D rotationAxes
	{
		get { return _rotationAxes; }
		set { _rotationAxes = value; }
	}

	public Rigidbody rigidbody
	{
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}
#endregion

	public Vector3 SeekVelocity(Vector3 _target)
	{
		Vector3 desired = (_target - rigidbody.position).normalized * maxSpeed;
		Vector3 steering = desired - (transform.forward * maxSpeed);

		return (steering.normalized * maxSteeringForce) * ArrivalScalar(_target);
	}

	public float ArrivalScalar(Vector3 _target)
	{
		Vector3 direction = _target - rigidbody.position;

		return Mathf.Clamp(direction.magnitude / arrivalRadius, 0.0f, 1.0f);
	}

	public void SeekBehavior(Vector3 _target)
	{
		Vector3 seekVelocity = SeekVelocity(_target);
		Vector3 movement = new Vector3(
			(movementAxes & Axes3D.X) == Axes3D.X ? seekVelocity.x : 0.0f,
			(movementAxes & Axes3D.Y) == Axes3D.Y ? seekVelocity.y : 0.0f,
			(movementAxes & Axes3D.Z) == Axes3D.Z ? seekVelocity.z : 0.0f
		);
		Vector3 rotation = new Vector3(
			(rotationAxes & Axes3D.X) == Axes3D.X ? seekVelocity.x : 0.0f,
			(rotationAxes & Axes3D.Y) == Axes3D.Y ? seekVelocity.y : 0.0f,
			(rotationAxes & Axes3D.Z) == Axes3D.Z ? seekVelocity.z : 0.0f
		);

		Debug.DrawRay(transform.position, seekVelocity, Color.magenta, Time.fixedDeltaTime * 5.0f);

		rigidbody.MovePosition(rigidbody.position + (movement * Time.fixedDeltaTime));
		//rigidbody.MoveRotation(Quaternion.LookRotation(rotation * Time.fixedDeltaTime));
		rotation = Vector3.Lerp(transform.forward, _target - rigidbody.position, maxSteeringForce * Time.fixedDeltaTime);
		transform.rotation = Quaternion.LookRotation(rotation);
	}
}