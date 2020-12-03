using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
/// <summary>Factions in the Game.</summary>
public enum Faction
{
	Ally,
	Enemy
}

/// <summary>Types of Pickables.</summary>
public enum PickableType
{
    NormalCollectible,
    RareCollectible,
    Health,
    Secret
}

/// <summary>Event invoked when state changes.</summary>
/// <param name="_state">New State [of type T].</param>
public delegate void OnStateChange<T>(T _state);

public class Game : Singleton<Game>
{
    public const int ID_EVENT_PLAYER_OFFLIMITS = 0;     /// <summary>Player Off-Limits' Event ID.</summary>

	[Space(5f)]
    [SerializeField] private GameData _data;            /// <summary>Game's Data.</summary>
    [Space(5f)]
	[Header("Game's Attributes:")]
    [SerializeField] private Player _player;            /// <summary>Player's Reference.</summary>

    /// <summary>Gets data property.</summary>
    public static GameData data { get { return Instance._data; } }

    /// <summary>Gets and Sets player property.</summary>
    public static Player player
    {
    	get { return Instance._player; }
    	set { Instance._player = value; }
    }

#if UNITY_EDITOR
    /// <summary>Draws Gizmos on Editor mode.</summary>
    private void OnDrawGizmos()
    {
        if(player == null || data == null) return;

        Gizmos.color = data.gizmosColor;
        Gizmos.DrawWireCube(player.transform.position, data.playAreaDimensions);
        VGizmos.DrawWireGridCube(player.transform.position, data.playAreaDimensions, data.playAreaDivisions);
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * data.zLimits);
    }
#endif

    /// <summary>Callback internally called right on Awake's callback.</summary>
    protected override void OnAwake()
    {
        
    }

    /// <summary>Repositions Player and Scenario [used if player passed the Z's limits].</summary>
    /// <param name="_player">Player's Reference.</param>
    /// <param name="_scenarioGroup">Transform that contains all the scenario elements.</param>
    public static void RepositionPlayer(Player _player, Transform _scenarioGroup = null)
    {
        Vector3 position = new Vector3(0.0f, 1.0f, 1.0f);

        if(_scenarioGroup != null)
        {
            Vector3 offset = _player.transform.position - _scenarioGroup.position;
            offset.y = 0.0f;
            _scenarioGroup.position = position - offset;
        }

        _player.transform.position = position;
    }
}
}