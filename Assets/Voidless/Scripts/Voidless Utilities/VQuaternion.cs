using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VQuaternion
{
	/// <summary>Gets the rotations (Quaternion) from a list of Transforms.</summary>
	/// <param name="_list">The list of Transforms from where the Quaternion list will be created.</param>
	/// <returns>List of the Transform rotation (Quaternion).</returns>
	public static List<Quaternion> GetRotations(this List<Transform> _list)
	{
		List <Quaternion> newList = new List<Quaternion>();

		foreach(Transform _transform in _list)
		{
			newList.Add(_transform.rotation);
		}

		return newList;
	}

	/// <summary>Gets the rotations (Quaternion) from a list of GameObjects.</summary>
	/// <param name="_list">The list of GameObjects from where the Quaternion list will be created.</param>
	/// <returns>List of the Transform rotation (Quaternion).</returns>
	public static List<Quaternion> GetRotations(this List<GameObject> _list)
	{
		List <Quaternion> newList = new List<Quaternion>();

		foreach(GameObject _gameObject in _list)
		{
			if(_gameObject != null) newList.Add(_gameObject.transform.rotation);
		}

		return newList;
	}

	/// <summay>Sets Quaternion.Euler Y component.</summary>
	/// <param name="_quaternion">Queternion that will have its eulerAnglles.y modified.</param>
	/// <param name="_y">Ne Y component value.</param>
	/// <returns>Quaternion with eulerAngles.y modified.</returns>
	public static Quaternion SetY(this Quaternion _quaternion, float _y)
	{
		return Quaternion.Euler(_quaternion.eulerAngles.x, _y, _quaternion.eulerAngles.z);
	}

	/// <summary>Calculates the difference between two Quaternions.</summary>
	/// <param name="a">Quaternion A.</param>
	/// <param name="b">Quaternion B.</param>
	/// <returns>Difference between two given Quaternions.</returns>
	public static Quaternion Delta(Quaternion a, Quaternion b)
	{
		return a * Quaternion.Inverse(b);
	}

	/// <returns>Random Unit Quaternion.</returns>
	public static Quaternion Random()
	{
		return Quaternion.Euler(new Vector3(
			UnityEngine.Random.Range(0.0f, 360.0f),
			UnityEngine.Random.Range(0.0f, 360.0f),
			UnityEngine.Random.Range(0.0f, 360.0f)
		));
	}
}
}