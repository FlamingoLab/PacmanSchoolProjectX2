using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct CameraViewportPlane
{
	public Vector3 centerPoint; 		/// <summary>Viewport's Center point.</summary>
	public Vector3 topLeftPoint;		/// <summary>Viewport's Top-Left point.</summary>
	public Vector3 topRightPoint;		/// <summary>Viewport's Top-Right point.</summary>
	public Vector3 bottomLeftPoint;		/// <summary>Viewport's Bottom-Left point.</summary>
	public Vector3 bottomRightPoint;	/// <summary>Viewport's Bottom-Right point.</summary>

	/// <summary>CameraViewportPlane contructor.</summary>
	/// <param name="_centerPoint">Viewport's Center point.</param>
	/// <param name="_topLeftPoint">Viewport's Top-Left point.</param>
	/// <param name="_topRightPoint">Viewport's Top-Right point.</param>
	/// <param name="_bottomLeftPoint">Viewport's Bottom-Left point.</param>
	/// <param name="_bottomRightPoint">Viewport's Bottom-Right point.</param>
	public CameraViewportPlane(Vector3 _centerPoint, Vector3 _topLeftPoint, Vector3 _topRightPoint, Vector3 _bottomLeftPoint, Vector3 _bottomRightPoint)
	{
		centerPoint = _centerPoint;
		topLeftPoint = _topLeftPoint;
		topRightPoint = _topRightPoint;
		bottomLeftPoint = _bottomLeftPoint;
		bottomRightPoint = _bottomRightPoint;
	}
}
}