using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class ShootParabolaProjectile : ShootProjectile
{
	/// <summary>Shoots Parabola Projectile towards given target at given time.</summary>
	/// <param name="_target">Target destination.</param>
	/// <param name="_time">Duration of the trayectory.</param>
	public void Shoot(Vector3 _target, float _time)
	{
        Cooldown cooldown = cooldowns[projectileIndex];

		if(muzzles == null || cooldown.onCooldown) return;

        Vector3 centerPoint = GetMuzzlesCenter() + (transform.forward * centerDistance);
        Vector3 velocity = Vector3.zero;
        ParabolaProjectile projectile = null;
        Quaternion rotation = Quaternion.identity;

        if(shootingType != ShootingType.AllMuzzles)
        {
            Transform muzzle = null;

            switch(shootingType)
            {
                case ShootingType.OneByOne:
                muzzle = muzzles[muzzleIndex];
                AdvanceMuzzleIndex();
                break;

                case ShootingType.RandomMuzzle:
                muzzle = muzzles[UnityEngine.Random.Range(0, muzzles.Length)];
                break;
            }
        
            velocity = VPhysics.ProjectileDesiredVelocity(_time, muzzle.position, _target, Game.data.gravity);
            rotation = orientationType == OrientationType.MuzzlesCenter ?
                    Quaternion.LookRotation(centerPoint - muzzle.position) : muzzle.rotation;

            projectile = PoolManager.RequestParabolaProjectile(faction, projectileID, muzzle.position, rotation);
            projectile.velocity = velocity;

        } else foreach(Transform muzzle in muzzles)
    	{
    		velocity = VPhysics.ProjectileDesiredVelocity(_time, muzzle.position, _target, Game.data.gravity);
            rotation = orientationType == OrientationType.MuzzlesCenter ?
                    Quaternion.LookRotation(centerPoint - muzzle.position) : muzzle.rotation;
                    
    		projectile = PoolManager.RequestParabolaProjectile(faction, projectileID, muzzle.position, rotation);
    		projectile.velocity = velocity;
    	}

        if(projectile.cooldownDuration <= 0.0f) return;

        cooldown.duration = projectile.cooldownDuration;
        cooldown.Begin();
	}
}
}