using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct Line
{
	public Vector3 a; 	/// <summary>Vector A.</summary>
	public Vector3 b; 	/// <summary>Vector B.</summary>

	/// <summary>Implicit Line to Ray Operator.</summary>
	public static implicit operator Ray(Line _line) { return new Ray(_line.a, (_line.b - _line.a)); }

	/// <summary>Implicit Ray to Line Operator.</summary>
	public static implicit operator Line(Ray _ray) { return new Line(_ray.origin, (_ray.origin + _ray.direction)); }

	/// <summary>Line's Constructor.</summary>
	/// <param name="_a">Vector A.</param>
	/// <param name="_b">Vector B.</param>
	public Line(Vector3 _a, Vector3 _b)
	{
		a = _a;
		b = _b;
	}

	/// <summary>Interpolates Line's points.</summary>
	/// <param name="t">Time parameter.</param>
	/// <returns>Interpolation between Line's points in t time.</returns>
	public Vector3 Lerp(float t)
	{
		return a + ((b - a) * t);
	}
}
}