using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flamingo
{
public enum NodeType
{
	Traversable,
	NonTraversable
}

[Serializable]
public struct NodeCell
{
	public Bounds bounds; 	/// <summary>Bounds that define the grid's cell.</summary>
	public NodeType type; 	/// <summary>Type of Node.</summary>

	/// <summary>NodeCell's Constructor.</summary>
	/// <param name="_bounds">Bounds that define the Grid's position [relative to a point] and dimensions.</param>
	/// <param name="_type">Type of Node.</param>
	public NodeCell(Bounds _bounds, NodeType _type)
	{
		bounds = _bounds;
		type = _type;
	}
}
}