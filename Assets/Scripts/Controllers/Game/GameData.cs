using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[CreateAssetMenu]
public class GameData : ScriptableObject
{
	[Header("Play Area's Attributes:")]
	[SerializeField] private Vector3 _playAreaDimensions; 						/// <summary>Playe Area's Dimensions.</summary>
	[SerializeField] private Vector3 _playAreaDivisions; 						/// <summary>Play Area's Divisions.</summary>
	[SerializeField] private float _zLimits; 									/// <summary>Boundary Limits on the Z Axis.</summary>
	[Space(5f)]
	[Header("Physics' Attributes:")]
	[SerializeField] private float _gravityScale; 								/// <summary>Gravity's Scale.</summary>
	[Space(5f)]
	[Header("Layer's Masks & Values:")]
	[SerializeField] private LayerValue _playerLayer; 							/// <summary>Player's Value.</summary>
	[SerializeField] private LayerValue _enemyLayer; 							/// <summary>Enemy's Layer.</summary>
	[Space(5f)]
	[Header("Projectiles:")]
	[SerializeField] private Projectile[] _playerProjectiles; 					/// <summary>Player's Projectiles.</summary>
	[SerializeField] private Projectile[] _enemyProjectiles; 					/// <summary>Enemies' Projectiles.</summary>	
	[SerializeField] private ParabolaProjectile[] _enemyParabolaProjectiles; 	/// <summary>Enemies' Projectiles.</summary>	
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color _gizmosColor; 								/// <summary>General Gizmos' Color.</summary>
	[SerializeField] private Color _traversableColor; 							/// <summary>Traversable Nodes' Color.</summary>
	[SerializeField] private Color _nonTraversableColor; 						/// <summary>Non-Traversable Nodes' Color.</summary>
#endif

#region Getters:
	/// <summary>Gets zLimits property.</summary>
	public float zLimits { get { return _zLimits; } }

	/// <summary>Gets playAreaDimensions property.</summary>
	public Vector3 playAreaDimensions { get { return _playAreaDimensions; } }

	/// <summary>Gets playAreaDivisions property.</summary>
	public Vector3 playAreaDivisions { get { return _playAreaDivisions; } }

	/// <summary>Gets gravityScale property.</summary>
	public float gravityScale { get { return _gravityScale; } }

	/// <summary>Gets playerLayer property.</summary>
	public LayerValue playerLayer { get { return _playerLayer; } }

	/// <summary>Gets enemyLayer property.</summary>
	public LayerValue enemyLayer { get { return _enemyLayer; } }

	/// <summary>Gets playerProjectiles property.</summary>
	public Projectile[] playerProjectiles { get { return _playerProjectiles; } }

	/// <summary>Gets enemyProjectiles property.</summary>
	public Projectile[] enemyProjectiles { get { return _enemyProjectiles; } }

	/// <summary>Gets enemyParabolaProjectiles property.</summary>
	public ParabolaProjectile[] enemyParabolaProjectiles { get { return _enemyParabolaProjectiles; } }

	/// <summary>Gets cellDimensions property.</summary>
	public Vector3 cellDimensions { get { return VVector3.Division(playAreaDimensions, playAreaDivisions); } }

	/// <summary>Gets gravity property.</summary>
	public Vector3 gravity { get { return Physics.gravity * gravityScale; } }
#endregion

#if UNITY_EDITOR
	/// <summary>Gets gizmosColor property.</summary>
	public Color gizmosColor { get { return _gizmosColor; } }

	/// <summary>Gets traversableColor property.</summary>
	public Color traversableColor { get { return _traversableColor; } }

	/// <summary>Gets nonTraversableColor property.</summary>
	public Color nonTraversableColor { get { return _nonTraversableColor; } }
#endif

}
}