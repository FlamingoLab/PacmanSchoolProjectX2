using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class PoolManager : Singleton<PoolManager>
{
	private GameObjectPool<Projectile>[] _playerProjectilesPools; 					/// <summary>Player Projectiles' Pool.</summary>
	private GameObjectPool<Projectile>[] _enemyProjectilesPools; 					/// <summary>Enemy Projectiles' Pool.</summary>
	private GameObjectPool<ParabolaProjectile>[] _enemyParabolaProjectilesPools; 	/// <summary>Enemy ParabolaProjectiles' Pool.</summary>

	/// <summary>Gets and Sets playerProjectilesPools property.</summary>
	public GameObjectPool<Projectile>[] playerProjectilesPools
	{
		get { return _playerProjectilesPools; }
		private set { _playerProjectilesPools = value; }
	}

	/// <summary>Gets and Sets enemyProjectilesPools property.</summary>
	public GameObjectPool<Projectile>[] enemyProjectilesPools
	{
		get { return _enemyProjectilesPools; }
		private set { _enemyProjectilesPools = value; }
	}

	/// <summary>Gets and Sets enemyParabolaProjectilesPools property.</summary>
	public GameObjectPool<ParabolaProjectile>[] enemyParabolaProjectilesPools
	{
		get { return _enemyParabolaProjectilesPools; }
		private set { _enemyParabolaProjectilesPools = value; }
	}

	/// <summary>PoolManager's instance initialization.</summary>
	protected override void OnAwake()
	{
		/// Create Player Projectiles's Pool:
		playerProjectilesPools = new GameObjectPool<Projectile>[Game.data.playerProjectiles.Length];

		for(int i = 0; i < playerProjectilesPools.Length; i++)
		{
			playerProjectilesPools[i] = new GameObjectPool<Projectile>(Game.data.playerProjectiles[i]);
		}

		/// Create Enemy Projectiles's Pool:
		enemyProjectilesPools = new GameObjectPool<Projectile>[Game.data.enemyProjectiles.Length];

		for(int i = 0; i < enemyProjectilesPools.Length; i++)
		{
			enemyProjectilesPools[i] = new GameObjectPool<Projectile>(Game.data.enemyProjectiles[i]);
		}

		/// Create Enemy ParabolaProjectiles's Pool:
		enemyParabolaProjectilesPools = new GameObjectPool<ParabolaProjectile>[Game.data.enemyParabolaProjectiles.Length];

		for(int i = 0; i < enemyParabolaProjectilesPools.Length; i++)
		{
			enemyParabolaProjectilesPools[i] = new GameObjectPool<ParabolaProjectile>(Game.data.enemyParabolaProjectiles[i]);
		}
	}

	/// <summary>Gets a Projectile from the Projectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_projectileID">Projectile's ID.</param>
	/// <param name="_position">Spawn position for the Projectile.</param>
	/// <param name="_rotation">Spawn rotation for the Projectile.</param>
	/// <returns>Requested Projectile.</returns>
	public static Projectile RequestProjectile(Faction _faction, int _projectileID, Vector3 _position, Quaternion _rotation)
	{
		GameObjectPool<Projectile> factionPool = _faction == Faction.Ally ?
			Instance.playerProjectilesPools[_projectileID] : Instance.enemyProjectilesPools[_projectileID];
		
		return factionPool.Recycle(_position, _rotation);
	}

	/// <summary>Gets a ParabolaProjectile from the ParabolaProjectiles' Pools.</summary>
	/// <param name="_faction">Faction of the requester.</param>
	/// <param name="_projectileID">ParabolaProjectile's ID.</param>
	/// <param name="_position">Spawn position for the ParabolaProjectile.</param>
	/// <param name="_rotation">Spawn rotation for the ParabolaProjectile.</param>
	/// <returns>Requested ParabolaProjectile.</returns>
	public static ParabolaProjectile RequestParabolaProjectile(Faction _faction, int _projectileID, Vector3 _position, Quaternion _rotation)
	{
		/*GameObjectPool<ParabolaProjectile> factionPool = _faction == Faction.Ally ?
			Instance.playerParabolaProjectilesPools[_projectileID] : Instance.enemyParabolaProjectilesPools[_projectileID];
		
		return factionPool.Recycle(_position, _rotation);*/

		return Instance.enemyParabolaProjectilesPools[_projectileID].Recycle(_position, _rotation);
	}
}
}