using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VPhysics
{
	/// <summary>Gets a list of Components inside an overlapping Sphere.</summary>
	/// <param name="_origin">Sphere's Origin.</param>
	/// <param name="_radius">Sphere's Radius.</param>
	/// <param name="action">Ection to do for each Component.</param>
	/// <param name="_mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="_queryTriggerInteractions">Hit Interactions to Allow.</param>
	/// <returns>List of Components inside an overlapping Sphere.</returns>
	public static List<T> GetAllComponentsInOverLapSphere<T>(Vector3 _origin, float _radius, int _mask = Physics.AllLayers, QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T : Component
	{
		T component = null;
		Collider[] colliders = Physics.OverlapSphere(_origin, _radius, _mask, _queryTriggerInteraction);

		if(colliders.Length > 1)
		{
			List<T> list = new List<T>();

			foreach(Collider collider in colliders)
			{
				component = collider.gameObject.GetComponent<T>();
				if(component != null) list.Add(component);
			}

			return list;
		}
		else return null;
	}

	/// <summary>Dictates actions to do for each component inside an overlapping sphere.</summary>
	/// <param name="_origin">Sphere's Origin.</param>
	/// <param name="_radius">Sphere's Radius.</param>
	/// <param name="action">Ection to do for each Component.</param>
	/// <param name="_mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="_queryTriggerInteractions">Hit Interactions to Allow.</param>
	public static void ForEachComponentInOverlapSphere<T>(Vector3 _origin, float _radius, Action<T> action, int _mask = Physics.AllLayers, QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T : UnityEngine.Object
	{
		T component = null;
		Collider[] colliders = Physics.OverlapSphere(_origin, _radius, _mask, _queryTriggerInteraction);

		for(int i = 0; i < colliders.Length; i++)
		{
			component = colliders[i].gameObject.GetComponent<T>();
			if(component != null) action(component);	
		}
	}

	/// <summary>Calculates a projectile's projection given a time t [pf = (g * (t^2/2)) + (v0 * t) + p0].</summary>
	/// <param name="t">Time t.</param>
	/// <param name="v0">Initial Velocity.</param>
	/// <param name="p0">Initial Position.</param>
	/// <param name="g">Gravity.</param>
	/// <returns>Projectile's Projection given time t.</returns>
	public static Vector3 ProjectileProjection(float t, Vector3 v0, Vector3 p0, Vector3 g)
	{
		return g * (0.5f * t * t) + (v0 * t) + p0;
	}

	/// <summary>Calculates desired Projectile's velocity to reach a point pf on time t.</summary>
	/// <param name="t">Time t.</param>
	/// <param name="p0">Initial Position.</param>
	/// <param name="pf">Desired Position.</param>
	/// <param name="g">Gravity.</param>
	/// <returns>Desired Projectile's Initial Velocity to reach pf on time t.</returns>
	public static Vector3 ProjectileDesiredVelocity(float t, Vector3 p0, Vector3 pf, Vector3 g)
	{
		return (pf - (g * (0.5f * t * t) + p0)) / t;
	}

	/// <summary>Cast a sphere along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="hit">Hit Information.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the sphere cast detected a Collider.</returns>
	public static bool SphereCast(Vector3 o, float r, Vector3 d, out RaycastHit hit, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = o - (d * r);
		Ray ray = new Ray(origin, d);

		return Physics.SphereCast(ray, r, out hit, l, mask, interactions);
	}

	/// <summary>Cast a sphere along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="hit">Hit Information.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <param name="additionalDirections">Additional Directions to cast the ray along.</param>
	/// <returns>True if the sphere cast detected a Collider.</returns>
	public static bool SphereCast(Vector3 o, float r, Vector3 d, out RaycastHit hit, Quaternion q, float maxD = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction i = QueryTriggerInteraction.UseGlobal, params Vector3[] additionalDirections)
	{
		d.Normalize();

		float diameter = r * 2.0f;
		Vector3 offsetOrigin = o - (q * d * diameter);
		if(maxD == Mathf.Infinity) maxD = diameter;

		if(Physics.SphereCast(offsetOrigin, r, d, out hit, maxD, mask, i)) return true;

		if(additionalDirections != null && additionalDirections.Length > 0)
		{
			foreach(Vector3 direction in additionalDirections)
			{
				direction.Normalize();
				offsetOrigin = o - (q * direction * diameter);
				if(Physics.SphereCast(offsetOrigin, r, d, out hit, maxD, mask, i)) return true;
			}
		}
		return false;
	}

	/*public static Vector3 ProjectileProjection(float t, Vector3 v0, Vector3 p0, params Vector3[] G, params float T)
	{
		if(G == null || T == null) return Vector3.zero;

		int n = Mathf.Min(G.Length, T.Length);
		Vector3 gravitySum = Vector3.zero;
		float tSum = 0.0f;

		for(int i = 0; i < n; i++)
		{
			if(t < tSum) break;

			Vector3 gx = G[i];
			float tx = T[i];
			float time = 

			gravitySum += ((i == 0) ? gx * (t * t * 0.05f) : )
			tSum += tx;
		}

		return gravitySum + (v0 * t) + p0;
	}*/
}
}