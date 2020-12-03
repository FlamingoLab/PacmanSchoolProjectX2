using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// \TODO Make an extension method that draws the skeleton instead
/* Example:
	public const float RADIUS_JOINT = 0.05f;

	public static void DrawSkeletonRig(transform _transform, float _radius = RADIUS_JOINT)
	{
		...
	}
*/
public class SkeletonRig : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private Color color; 	/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float radius; 	/// <summary>Gizmos' Radius.</summary>

	/// <summary>Draws Gizmos on Editor mode when SkeletonRig's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = color;
		DrawJoints(transform);
	}

	/// <summary>Draws Bone's Joints.</summary>
	/// <param name="_bone">Parent Bone.</param>
	private void DrawJoints(Transform _bone)
	{
		foreach(Transform child in _bone)
		{
			DrawGizmoJoint(_bone, child);
			DrawJoints(child);
		}
	}

	/// <summary>Draws Gizmos' Joint.</summary>
	/// <param name="a">Bone A [father].</param>
	/// <param name="b">Bone B [child].</param>
	private void DrawGizmoJoint(Transform a, Transform b)
	{
		Vector3 p1 = a.TransformPoint(Vector3.left * radius);
		Vector3 p2 = a.TransformPoint(Vector3.right * radius);
		Vector3 p3 = a.TransformPoint(Vector3.back * radius);
		Vector3 p4 = a.TransformPoint(Vector3.forward * radius);

		Gizmos.DrawWireSphere(b.position, radius);
		Gizmos.DrawLine(p1, b.position);
		Gizmos.DrawLine(p2, b.position);
		Gizmos.DrawLine(p3, b.position);
		Gizmos.DrawLine(p4, b.position);
	}
#endif
}
}