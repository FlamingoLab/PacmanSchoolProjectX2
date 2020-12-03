using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class TestSceneController : Singleton<TestSceneController>
{
	[SerializeField] private Player _player; 	/// <summary>Player's Reference.</summary>
	[SerializeField] private Transform _stage; 	/// <summary>Stage's Main Transform.</summary>

	/// <summary>Gets and Sets player property.</summary>
	public Player player 
	{
		get { return _player; }
		set { _player = value; }
	}

	/// <summary>Gets and Sets stage property.</summary>
	public Transform stage 
	{
		get { return _stage; }
		set { _stage = value; }
	}

	/// <summary>PlayerController's instance initialization.</summary>
	protected override void OnAwake()
	{
		player.onPlayerEvent += OnPlayerEvent;
	}

	/// <summary>Callback internally called before this gets passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		player.onPlayerEvent -= OnPlayerEvent;
	}

	/// <summary>Callback invoked when a Player's event takes place.</summary>
	/// <param name="_ID">Event's ID.</param>
	private void OnPlayerEvent(int _ID)
	{
		switch(_ID)
		{
			case Game.ID_EVENT_PLAYER_OFFLIMITS:
			Game.RepositionPlayer(player, stage);
			break;
		}
	}
}
}