using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ThirdPersonCameraOffsetSetter : MonoBehaviour
{
	[SerializeField] private NormalizedVector3 _offset; 	/// <summary>Offset.</summary>
	[SerializeField] private bool _changeDistance; 			/// <summary>Change also distance?.</summary>
	[SerializeField] private float _distance; 				/// <summary>Distance.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color color; 					/// <summary>Gizmos' color.</summary>
	[SerializeField] private Vector3 targetPosition; 		/// <summary>Test Target's position.</summary>
	[SerializeField] private float radius; 					/// <summary>Gimos' radius.</summary>
#endif

	/// <summary>Gets and Sets offset property.</summary>
	public NormalizedVector3 offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>Gets and Sets changeDistance property.</summary>
	public bool changeDistance
	{
		get { return _changeDistance; }
		set { _changeDistance = value; }
	}

	/// <summary>Gets and Sets distance property.</summary>
	public float distance
	{
		get { return _distance; }
		set { _distance = value; }
	}

	void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Vector3 realTargetPosition = (transform.position + targetPosition);
		Gizmos.color = color;
		Gizmos.DrawWireSphere(realTargetPosition, radius);
		Gizmos.DrawLine(realTargetPosition, (realTargetPosition + (offset * distance)));
#endif
	}

	/// \TODO Move this method to the Inspector's class.
	/// <summary>Tests Camera on Editor.</summary>
	public void TestCamera(ThirdPersonCamera _camera)
	{
#if UNITY_EDITOR
        Vector3 targetPosition = new Vector3(0,0,0);
        Vector3 realTargetPosition = (transform.position + targetPosition);
		Vector3 offsetPosition = (realTargetPosition + (offset * distance));

		_camera.transform.position = offsetPosition;
		_camera.transform.LookAt(realTargetPosition);
#endif
	}
}
}