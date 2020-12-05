using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
public struct ValueTuple<T1, T2> where T1 : struct where T2 : struct
{
	public T1 value1; 	/// <summary>Item 1.</summary>
	public T2 value2; 	/// <summary>Item 2.</summary>

	/// <summary>Implicit ValueTuple to Tuple converter.</summary>
	public static implicit operator Tuple<T1, T2>(ValueTuple<T1, T2> _tuple) { return new Tuple<T1, T2>(_tuple.value1, _tuple.value2); }

	/// <summary>Tuple default constructor.</summary>
	/// <param name="_value1">Item 1.</param>
	/// <param name="_value2">Item 2.</param>
	public ValueTuple(T1 _value1, T2 _value2)
	{
		value1 = _value1;
		value2 = _value2;
	}

	/// <summary>Creates a Tuple reference from this ValueTuple.</summary>
	/// <param name="_value1">Item 1.</param>
	/// <param name="_value2">Item 2.</param>
	public Tuple<T1, T2> ToTuple()
	{
		return new Voidless.Tuple<T1, T2>(value1, value2);
	}
}
}