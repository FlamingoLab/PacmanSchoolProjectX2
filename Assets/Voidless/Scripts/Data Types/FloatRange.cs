using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct FloatRange : IRange<float>, ISerializationCallbackReceiver
{
	[SerializeField] private float _min; 	/// <summary>Range's Minimum value.</summary>
	[SerializeField] private float _max; 	/// <summary>Range's maximum value.</summary>

	/// <summary>Gets and Sets min property.</summary>
	public float min
	{
		get { return _min; }
		set { _min = value; }
	}

	/// <summary>Gets and Sets max property.</summary>
	public float max
	{
		get { return _max; }
		set { _max = value; }
	}

	/// <summary>Implicit float value to FloatRange operator.</summary>
	public static implicit operator FloatRange(float _value) { return new FloatRange(_value); }

	/// <summary>FloatRange's Constructor.</summary>
	/// <param name="_min">Minimum's value.</param>
	/// <param name="_max">Maximum's value.</param>
	public FloatRange(float _min, float _max) : this()
	{
		min = _min;
		max = _max;
	}

	/// <summary>FloatRange's constructor.</summary>
	/// <param name="_value">Same minimum and maximum's value for the FloatRange.</param>
	public FloatRange(float _value) : this(_value, _value) { /*...*/ }

	/// <returns>Range's Median.</returns>
	public float GetMedian()
	{
		return (min + (GetLength() * 0.5f));
	}

	/// <returns>Range's Length.</returns>
	public float GetLength()
	{
		return (max - min/* + 1.0f*/);
	}

	/// <returns>Maximum Value.</returns>
	public float Max()
	{
		return Mathf.Max(min, max);
	}

	/// <returns>Minimum Value.</returns>
	public float Min()
	{
		return Mathf.Min(min, max);
	}

	/// <returns>Random value between the range.</returns>
	public float Random()
	{
		return UnityEngine.Random.Range(Min(), Max());
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