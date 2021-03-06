﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voidless;

namespace Flamingo
{
public class TestSceneController : Singleton<TestSceneController>
{
	[SerializeField] private Player _player; 	/// <summary>Player's Reference.</summary>
	[SerializeField] private Transform _stage; 	/// <summary>Stage's Main Transform.</summary>
	[Space(5f)]
	[SerializeField] private Enemy[] _enemies; 	/// <summary>Enemies [Must be on scene].</summary>
	[SerializeField] private Boss[] _bosses; 	/// <summary>Bosses [Must be on scene].</summary>
	private Enemy _currentEnemy; 				/// <summary>Currently selected Enemy.</summary>
	private Boss _currentBoss; 					/// <summary>Currently selected Boss.</summary>
#if UNITY_EDITOR
	private Vector2 scrollPosition; 			/// <summary>GUI's Scroll Position.</summary>
	private string damageText;
	private float damage;
#endif

#region Getters/Setters:
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

	/// <summary>Gets and Sets enemies property.</summary>
	public Enemy[] enemies 
	{
		get { return _enemies; }
		set { _enemies = value; }
	}

	/// <summary>Gets and Sets bosses property.</summary>
	public Boss[] bosses 
	{
		get { return _bosses; }
		set { _bosses = value; }
	}

	/// <summary>Gets and Sets currentEnemy property.</summary>
	public Enemy currentEnemy
	{
		get { return _currentEnemy; }
		set { _currentEnemy = value; }
	}

	/// <summary>Gets and Sets currentBoss property.</summary>
	public Boss currentBoss
	{
		get { return _currentBoss; }
		set { _currentBoss = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Callback invoked for rendering and handling GUI events.</summary>
	private void OnGUI()
	{
		if(bosses == null) return;

		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		if(enemies != null)
		foreach(Enemy enemy in enemies)
		{
			if(GUILayout.Button(enemy.name)) SpawnEnemy(enemy);
		}

		if(bosses != null)
		foreach(Boss boss in bosses)
		{
			if(GUILayout.Button(boss.name)) SpawnBoss(boss);
		}

		if(currentEnemy != null) GUILayout.Label(currentEnemy.ToString());
		if(currentBoss != null) GUILayout.Label(currentBoss.ToString());

		if(currentEnemy != null)
		{
			GUILayout.Label("Damage to Apply: ");
			damage = VGUILayout.FloatField(damage);
			damage = GUILayout.HorizontalSlider(damage, 0.0f, currentBoss.health.maxHP);
			if(GUILayout.Button("Apply Damage")) currentBoss.health.GiveDamage(damage);
			if(GUILayout.Button("Reset")) currentBoss.Reset();
		}
		
		GUILayout.EndScrollView();
	}
#endif

	/// <summary>PlayerController's instance initialization.</summary>
	protected override void OnAwake()
	{
		player.onPlayerEvent += OnPlayerEvent;

		if(bosses != null) SpawnBoss(bosses[0]);
	}

	/// <summary>Callback internally called before this gets passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		player.onPlayerEvent -= OnPlayerEvent;
	}

	/// <summary>Spawns Enemy.</summary>
	/// <param name="_enemy">Enemy to spawn.</param>
	private void SpawnEnemy(Enemy _enemy)
	{
		if(_enemy == null) return;

		SpawnBoss(null);

		foreach(Enemy enemy in enemies)
		{
			if(enemy == _enemy)
			{
#if UNITY_EDITOR
				damage = 0.0f;
#endif
				currentEnemy = _enemy;
				currentEnemy.Reset();
				currentEnemy.OnObjectReset();
			}
			else enemy.OnObjectDeactivation();
		}
	}

	/// <summary>Spawns Boss.</summary>
	/// <param name="_boss">Boss to spawn.</param>
	private void SpawnBoss(Boss _boss)
	{
		if(_boss == null) return;

		SpawnEnemy(null);

		foreach(Boss boss in bosses)
		{
			if(boss == _boss)
			{
#if UNITY_EDITOR
				damage = 0.0f;
#endif
				currentBoss = _boss;
				currentBoss.Reset();
				currentBoss.OnObjectReset();
			}
			else boss.OnObjectDeactivation();
		}
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