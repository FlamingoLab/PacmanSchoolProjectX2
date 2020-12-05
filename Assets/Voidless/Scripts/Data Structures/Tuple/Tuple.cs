using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
public class Tuple<T1, T2>
{
	public T1 value1; 	/// <summary>Item 1.</summary>
	public T2 value2; 	/// <summary>Item 2.</summary>

	/// <summary>Tuple default constructor.</summary>
	/// <param name="_value1">Item 1.</param>
	/// <param name="_value2">Item 2.</param>
	public Tuple(T1 _value1, T2 _value2)
	{
		value1 = _value1;
		value2 = _value2;
	}
}
}