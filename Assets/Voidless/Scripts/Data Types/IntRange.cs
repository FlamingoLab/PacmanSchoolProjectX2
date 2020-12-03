using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct IntRange : IRange<int>, ISerializationCallbackReceiver
{
	[SerializeField] private int _min; 	/// <summary>Range's Minimum value.</summary>
	[SerializeField] private int _max; 	/// <summary>Range's maximum value.</summary>

	/// <summary>Gets and Sets min property.</summary>
	public int min
	{
		get { return _min; }
		set { _min = value; }
	}

	/// <summary>Gets and Sets max property.</summary>
	public int max
	{
		get { return _max; }
		set { _max = value; }
	}

	/// <summary>Implicit int value to IntRange operator.</summary>
	public static implicit operator IntRange(int _value) { return new IntRange(_value); }

	/// <summary>IntRange's Constructor.</summary>
	/// <param name="_min">Minimum's value.</param>
	/// <param name="_max">Maximum's value.</param>
	public IntRange(int _min, int _max) : this()
	{
		min = _min;
		max = _max;
	}

	/// <summary>IntRange's constructor.</summary>
	/// <param name="_value">Same minimum and maximum's value for the IntRange.</param>
	public IntRange(int _value) : this(_value, _value) { /*...*/ }

	/// <returns>Range's Median.</returns>
	public int GetMedian()
	{
		return (min + (GetLength() / 2));
	}

	/// <returns>Range's Length.</returns>
	public int GetLength()
	{
		return (max - min + 1);
	}

	/// <returns>Maximum Value.</returns>
	public int Max()
	{
		return Mathf.Max(min, max);
	}

	/// <returns>Minimum Value.</returns>
	public int Min()
	{
		return Mathf.Min(min, max);
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	/*min = Min();
    	max = Max();*/
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	
    }

	/// <returns>String representing Range.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("[ ");
		builder.Append("Min: ");
		builder.Append(min.ToString());
		builder.Append(", Max: ");
		builder.Append(max.ToString());
		builder.Append(", Length: ");
		builder.Append(GetLength().ToString());
		builder.Append(", Median: ");
		builder.Append(GetMedian().ToString());
		builder.Append("]");

		return builder.ToString();
	}
}
}