using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

/*============================================================
**
** Class:  PlayerController
**
** Purpose: This class has the single responsibility of handling the inputs and calling the Player's methods.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Flamingo
{
public class PlayerController : Singleton<Player>
{
	[Space(5f)]
	[SerializeField] private Player _player; 						/// <summary>Player's Reference.</summary>
	[Space(5f)]
	[Header("Input Mapping:")]
	[SerializeField] private OVRInput.Button _shootButton; 			/// <summary>Shoot's Button.</summary>
	[SerializeField] private OVRInput.Button _barrelRollButton; 	/// <summary>Barrell Roll's Button.</summary>

	/// <summary>Gets and Sets player property.</summary>
	public Player player
	{
		get { return _player; }
		set { _player = value; }
	}

	/// <summary>Gets and Sets shootButton property.</summary>
	public OVRInput.Button shootButton
	{
		get { return _shootButton; }
		set { _shootButton = value; }
	}

	/// <summary>Gets and Sets barrelRollButton property.</summary>
	public OVRInput.Button barrelRollButton
	{
		get { return _barrelRollButton; }
		set { _barrelRollButton = value; }
	}

#region UnityMethods:
	/// <summary>PlayerController's instance initialization.</summary>
	protected override void OnAwake()
	{
		if(player == null) player = Game.player;
	}
	
	/// <summary>PlayerController's tick at each frame.</summary>
	private void Update ()
	{
		if(OVRInput.GetDown(shootButton)) player.ShootProjectile();
		if(OVRInput.GetDown(barrelRollButton)) player.DoBarrelRoll();
	}
#endregion
}
}