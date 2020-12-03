using UnityEngine;

namespace Voidless
{
public static class VTransform
{
	/// <summary>Returns a Vector3 relative to the space given.</summary>
	/// <param name="_transform">Transform that requests the relative to space Vector.</param>
	/// <param name="_vector">Vector to evaluate.</param>
	/// <param name="_space">Space relativeness.</param>
	/// <returns>Vector relative to the given space, local space ig given 'Self', world space if 'World' given.</returns>
	public static Vector3 RelativeTo(this Transform _transform, Vector3 _vector, Space _space)
	{
		return _space == Space.World ? _vector : _transform.rotation * _vector;
	}

	/// <summary>Gets Vector with Axis ignored.</summary>
	/// <param name="_transform">Transoform that extends the method.</param>
	/// <param name="v">Vector to return.</param>
	/// <param name="_axes">Axes to ignore [Axes3D.None by default].</param>
	/// <param name="_zeroIgnoredAxes">Set to 0.0f all ignored axes? [if false, it will set the vector equal to the transform's respective component].</param>
	/// <returns>Vector with ignored axes.</returns>
	public static Vector3 IgnoreVectorAxes(this Transform _transform, Vector3 v, Axes3D _axes = Axes3D.None, bool _zeroIgnoredAxes = false)
	{
		if(_axes != Axes3D.None)
		{
			if((_axes | Axes3D.X) == _axes) v.x = _zeroIgnoredAxes ? 0.0f : _transform.position.x;
			if((_axes | Axes3D.Y) == _axes) v.y = _zeroIgnoredAxes ? 0.0f : _transform.position.y;
			if((_axes | Axes3D.Z) == _axes) v.z = _zeroIgnoredAxes ? 0.0f : _transform.position.z;
		}

		return v;
	}

	/// <summary>Gets Vector with Axis considered.</summary>
	/// <param name="_transform">Transoform that extends the method.</param>
	/// <param name="v">Vector to return.</param>
	/// <param name="_axes">Axes to consider [Axes3D.All by default].</param>
	/// <param name="_zeroIgnoredAxes">Set to 0.0f all ignored axes? [if false, it will set the vector equal to the transform's respective component].</param>
	/// <returns>Vector with with considered axes.</returns>
	public static Vector3 WithVectorAxes(this Transform _transform, Vector3 v, Axes3D _axes = Axes3D.All, bool _zeroIgnoredAxes = false)
	{
		if(_axes != Axes3D.All)
		{
			if((_axes & Axes3D.X) != _axes) v.x = _zeroIgnoredAxes ? 0.0f : _transform.position.x;
			if((_axes & Axes3D.Y) != _axes) v.y = _zeroIgnoredAxes ? 0.0f : _transform.position.y;
			if((_axes & Axes3D.Z) != _axes) v.z = _zeroIgnoredAxes ? 0.0f : _transform.position.z;
		}

		return v;
	}

	/// <summary>Activates/Deactivates children beneath given transform.</summary>
	/// <param name="_transform">Parent's Transform.</param>
	/// <param name="_activate">Activate Children? True by default.</param>
	public static void ActivateChildren(this Transform _transform, bool _activate = true)
	{
		foreach(Transform child in _transform)
		{
			child.gameObject.SetActive(_activate);
		}
	}

	/// <summary>Sets ReorientedTransform as parent of given Transform.</summary>
	/// <param name="_transform">Transform that will have a new Parent.</param>
	/// <param name="_reorientedParent">ReorientedTransform that will become the new Parent.</param>
	/// <param name="_worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
	public static void SetReorientedParent(this Transform _transform, ReorientedTransform _reorientedParent, bool _worldPositionStays = true)
	{
		if(_reorientedParent != null)
		_transform.SetParent(_reorientedParent.transform, _worldPositionStays);
	}

	/// <summary>Sets ReorientedTransform as parent of given Transform.</summary>
	/// <param name="_transform">Transform that will have a new Parent.</param>
	/// <param name="_reorientedParent">ReorientedTransform that will become the new Parent.</param>
	/// <param name="_setParentProperties">ReorientedParent's properties to give to new child.</param>
	public static void SetReorientedParent(this Transform _transform, ReorientedTransform _reorientedParent, TransformProperties _setParentProperties = TransformProperties.None)
	{
		if(_reorientedParent == null) return;

		_transform.SetParent(_reorientedParent.transform);

		if((_setParentProperties | TransformProperties.Position) == _setParentProperties) _transform.position = _reorientedParent.transform.position;
		if((_setParentProperties | TransformProperties.Rotation) == _setParentProperties) _transform.rotation = _reorientedParent.rotation;
		if((_setParentProperties | TransformProperties.Scale) == _setParentProperties) _transform.localScale = _reorientedParent.transform.localScale;
	}
}
}