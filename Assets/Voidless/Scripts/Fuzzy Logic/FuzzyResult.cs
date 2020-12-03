using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct FuzzyResult<T> where T : struct
{
	public T value; 				/// <summary>Result's Value.</summary>
	public float membershipDegree; 	/// <summary>Result's MembershipDegree.</summary>

	/// <summary>FuzzyResult constructor.</summary>
	/// <param name="_value">Value associated with the input.</param>
	/// <param name="_mermbershipDegree">Membership's Degree.</param>
	public FuzzyResult(T _value, float _membershipDegree)
	{
		value = _value;
		membershipDegree = _membershipDegree;
	}

	/// <returns>String representing this Fuzzy Subset's result.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Value: ");
		builder.Append(value.ToString());
		builder.Append("\n");
		builder.Append("With Membership Degree: ");
		builder.Append(membershipDegree.ToString());

		return builder.ToString();
	}
}
}