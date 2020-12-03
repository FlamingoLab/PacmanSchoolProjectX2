using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct FuzzySubset<T> where T : struct
{
	public FloatRange range; 				/// <summary>Set's Range.</summary>
	public FloatRange valueRange; 			/// <summary>Value's Range [by default {x, x}].</summary>
	public FloatRange intersectionRange; 	/// <summary>Intersection's Range.</summary>
	public T value; 						/// <summary>Set's Value.</summary>

	/// <summary>FuzzySubset's Constructor.</summary>
	/// <param name="_range">Set's Range.</param>
	/// <param name="_valueRange">Set's Value Range.</param>
	/// <param name="_value">Set's Value.</param>
	public FuzzySubset(FloatRange _range, FloatRange _valueRange, T _value) : this()
	{
		range = _range;
		valueRange = _valueRange;
		value = _value;
	}

	/// <summary>FuzzySubset's Constructor.</summary>
	/// <param name="_range">Set's Range.</param>
	/// <param name="_value">Set's Value.</param>
	public FuzzySubset(FloatRange _range, T _value) : this(_range, 0.0f, _value) { /*...*/ }

	/// <summary>Gets a ray pointing from the core's minimum's range value towards the support minimum's range.</summary>
	/// <returns>Ray pointing from value towards minimum's threshold.</returns>
	public Ray2D GetMinRangeRay()
	{
		Vector2 rayOrigin = new Vector2(valueRange.min, 1.0f);
		Vector2 rayDirection = (new Vector2(range.min, 0.0f) - rayOrigin);
		return new Ray2D(rayOrigin, rayDirection);
	}

	/// <summary>Gets a ray pointing from the core's maximum's range value towards the support maximum's range.</summary>
	/// <returns>Ray pointing from value towards maximum's threshold.</returns>
	public Ray2D GetMaxRangeRay()
	{
		Vector2 rayOrigin = new Vector2(valueRange.max, 1.0f);
		Vector2 rayDirection = (new Vector2(range.max, 0.0f) - rayOrigin);
		return new Ray2D(rayOrigin, rayDirection);
	}

	/// <returns>String representing Fuzzy Set.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(value.ToString());
		builder.Append("'s Fuzzy Subset: ");
		builder.Append("\n");
		builder.Append("{ ");
		builder.Append("\n");
		builder.Append("\tMin-Max Range: ");
		builder.Append(range.ToString());
		builder.Append(", ");
		builder.Append("\n");
		builder.Append("\tValue Range: ");
		builder.Append(valueRange.ToString());
		builder.Append(", ");
		builder.Append("\n");
		builder.Append("\tIntersection Range: ");
		builder.Append(intersectionRange.ToString());
		builder.Append("\n");
		builder.Append("} ");

		return builder.ToString();
	}
}
}