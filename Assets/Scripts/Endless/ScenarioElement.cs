using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum DrawMode
{
	None,
	GridCell,
	NodeCells
}

public class ScenarioElement : MonoBehaviour
{
	[SerializeField] private Renderer[] _renderers; 	/// <summary>Renderer's contained on this object.</summary>
	[SerializeField] private LayerMask _interactable; 	/// <summary>Interactable's LayerMask.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes")]
	[SerializeField] private DrawMode _drawMode; 		/// <summary>Gizmos' Draw Mode.</summary>
#endif
	private NodeCell[] _nodeCells;

	/// <summary>Gets renderers property.</summary>
	public Renderer[] renderers { get { return _renderers; } }

	/// <summary>Gets interactable property.</summary>
	public LayerMask interactable { get { return _interactable; } }

	/// <summary>Gets and Sets nodeCells property.</summary>
	public NodeCell[] nodeCells
	{
		get { return _nodeCells; }
		private set { _nodeCells = value; }
	}

#if UNITY_EDITOR
	/// <summary>Gets drawMode property.</summary>
	public DrawMode drawMode { get { return _drawMode; } }

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		switch(drawMode)
		{
			case DrawMode.GridCell:
			DrawGirdCell();
			break;

			case DrawMode.NodeCells:
			DrawNodes();
			break;
		}
	}
#endif

	/// <summary>Calculates Cell Nodes.</summary>
	public void CalculateCellNodes()
	{
		Bounds bounds = VBounds.GetBoundsToFitSet(renderers);
		Vector3 cellDimensions = Game.data.cellDimensions;
		Vector3 cellsNeeded = VVector3.Division(bounds.size, cellDimensions).Ceiled();
		Vector3 maxDimensions = Vector3.Scale(cellDimensions, cellsNeeded);
		Bounds[] gridCells = bounds.GetCartesianGridBounds(cellsNeeded);

		if(gridCells == null) return;

		int length = gridCells.Length;
		Collider[] colliders = null;
		Bounds current = default(Bounds);
		Bounds newBounds = new Bounds();
		NodeType type = NodeType.Traversable;
		
		nodeCells = new NodeCell[length];

		for(int i = 0; i < length; i++)
		{
			current = gridCells[i];
			newBounds.center = current.center - transform.position;
			newBounds.size = current.size;

			colliders = Physics.OverlapBox(transform.position + newBounds.center, newBounds.extents, transform.rotation, interactable);

			if(colliders == null || colliders.Length == 0) type = NodeType.Traversable;
			else type = NodeType.NonTraversable;

			nodeCells[i] = new NodeCell(newBounds, type);
		}
	}

#if UNITY_EDITOR
	/// <summary>Draws Grid cell (without nodes).</summary>
	private void DrawGirdCell()
	{
		if(renderers == null || Game.Instance == null) return;

		Gizmos.color = Game.data.gizmosColor;
		Bounds bounds = VBounds.GetBoundsToFitSet(renderers);
		Vector3 cellDimensions = Game.data.cellDimensions;
		Vector3 cellsNeeded = VVector3.Division(bounds.size, cellDimensions).Ceiled();
		Vector3 maxDimensions = Vector3.Scale(cellDimensions, cellsNeeded);

		VGizmos.DrawWireGridCube(bounds.center, maxDimensions, cellsNeeded);

		if(renderers.Length < 2) return;

		foreach(Renderer renderer in renderers)
		{
			if(renderer == null) return;
			VGizmos.DrawBounds(renderer.bounds);
		}

		VGizmos.DrawBounds(bounds);
	}

	/// <summary>Draws Cell Nodes on Gizmos' Mode.</summary>
	private void DrawNodes()
	{
		if(nodeCells == null || Game.Instance == null) return;
		
		foreach(NodeCell cell in nodeCells)
		{
			Gizmos.color = cell.type == NodeType.Traversable ? Game.data.traversableColor : Game.data.nonTraversableColor;
			Bounds bounds = cell.bounds;

			bounds.center = transform.position + bounds.center;
			VGizmos.DrawBounds(bounds, cell.type == NodeType.Traversable ? GizmosDrawType.Wired : GizmosDrawType.Solid);
		}
	}
#endif

}
}