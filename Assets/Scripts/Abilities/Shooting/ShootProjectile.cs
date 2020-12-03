using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum OrientationType
{
    Muzzle,
    MuzzlesCenter
}

public enum ShootingType
{
    AllMuzzles,
    OneByOne,
    RandomMuzzle
}

public class ShootProjectile : MonoBehaviour
{
    [SerializeField] private OrientationType _orientationType;      /// <summary>Muzzle's Shooting Type.</summary>
    [SerializeField] private ShootingType _shootingType;            /// <summary>Projectiles' Shooting Type.</summary>
    [SerializeField] private Transform[] _muzzles;                  /// <summary>Muzzles.</summary>
    [SerializeField] private CollectionIndex[] _projectilesIDs;     /// <summary>Projectiles' IDs.</summary>
    [SerializeField] private Faction _faction;                      /// <summary>Shooter's Faction.</summary>
    [SerializeField] private int _projectileIndex;                  /// <summary>Projectile's ID Index.</summary>
    [SerializeField] private float _centerDistance;                 /// <summary>Center's Distance from the average position of the muzzles.</summary>
#if UNITY_EDITOR
    [Space(5f)]
    [Header("Gizmos' Attributes:")]
    [SerializeField] private Color gizmosColor;                     /// <summary>Gizmos' Color.</summary>
#endif
    private Cooldown[] _cooldowns;                                  /// <summary>Projectiles' Cooldowns.</summary>
    private int _muzzleIndex;                                       /// <summary>Current Muzzle's index [in case ShootingType.OneByOne is selected].</summary>

#region Getters/Setters:
    /// <summary>Gets and Sets orientationType property.</summary>
    public OrientationType orientationType
    {
        get { return _orientationType; }
        set { _orientationType = value; }
    }

    /// <summary>Gets and Sets shootingType property.</summary>
    public ShootingType shootingType
    {
        get { return _shootingType; }
        set { _shootingType = value; }
    }

    /// <summary>Gets and Sets muzzles property.</summary>
    public Transform[] muzzles
    {
        get { return _muzzles; }
        set { _muzzles = value; }
    }

    /// <summary>Gets and Sets projectilesIDs property.</summary>
    public CollectionIndex[] projectilesIDs
    {
        get { return _projectilesIDs; }
        set { _projectilesIDs = value; }
    }

    /// <summary>Gets and Sets faction property.</summary>
    public Faction faction
    {
        get { return _faction; }
        set { _faction = value; }
    }

    /// <summary>Gets and Sets projectileIndex property.</summary>
    public int projectileIndex
    {
        get { return _projectileIndex; }
        protected set { _projectileIndex = Mathf.Clamp(value, 0, cooldowns != null ? cooldowns.Length - 1 : 0); }
    }

    /// <summary>Gets and Sets centerDistance property.</summary>
    public float centerDistance
    {
        get { return _centerDistance; }
        set { _centerDistance = value; }
    }

    /// <summary>Gets and Sets muzzleIndex property.</summary>
    public int muzzleIndex
    {
        get { return _muzzleIndex; }
        protected set { _muzzleIndex = value; }
    }

    /// <summary>Gets and Sets cooldowns property.</summary>
    public Cooldown[] cooldowns
    {
        get { return _cooldowns; }
        protected set { _cooldowns = value; }
    }

    /// <summary>Gets projectileID property.</summary>
    public int projectileID { get { return projectilesIDs != null ? projectilesIDs[projectileIndex].index : 0; } }

    /// <summary>Gets onCooldown property.</summary>
    public bool onCooldown { get { return cooldowns[projectileIndex].onCooldown; } }
#endregion

#if UNITY_EDITOR
    /// <summary>Callback invoked when a GizmosDraw evetn happens.</summary>
    private void OnDrawGizmosSelected()
    {
        if(muzzles == null) return;

        Vector3 center = GetMuzzlesCenter();
        Vector3 centerPoint = center + (transform.forward * centerDistance);

        Gizmos.color = gizmosColor;
        Gizmos.DrawLine(center, centerPoint);
        
        foreach(Transform muzzle in muzzles)
        {
            if(muzzle == null) continue;

            Vector3 origin = muzzle.position;

            switch(orientationType)
            {
                case OrientationType.Muzzle:
                Gizmos.DrawRay(origin, muzzle.forward * centerDistance);
                break;

                case OrientationType.MuzzlesCenter:
                Gizmos.DrawLine(muzzle.position, centerPoint);
                break;
            }
        }
    }
#endif

    /// <summary>ShootProjectile's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        cooldowns = new Cooldown[projectilesIDs.Length];

        for(int i = 0; i < cooldowns.Length; i++)
        {
            cooldowns[i] = new Cooldown(this, 0.0f);
        }
    }

    /// <returns>Average position of all muzzles' positions.</returns>
    protected Vector3 GetMuzzlesCenter()
    {
    	Vector3 center = Vector3.zero;

    	foreach(Transform muzzle in muzzles)
    	{
            if(muzzle != null)
    		center += muzzle.position;
    	}

    	return center / (float)(muzzles.Length);
    }

    /// <summary>Changes Projectile's ID equal to the specific ID provided.</summary>
    /// <param name="_ID">New projectile's ID.</param>
    public void ChangeProjectileIndex(int _ID)
    {
        projectileIndex = _ID;
    }

    /// <summary>Advances to the next projectile's ID.</summary>
    public void AdvanceProjectileIndex()
    {
        int advancedID = projectileIndex + 1;

        projectileIndex = advancedID < cooldowns.Length ? advancedID : 0;
    }

    /// <summary>Changes Projecrtile's ID into a random one.</summary>
    public void ChangeRandomProjectileIndex()
    {
        projectileIndex = UnityEngine.Random.Range(0, projectilesIDs.Length);
    }

    /// <summary>Advances Muzzle's Index.</summary>
    protected void AdvanceMuzzleIndex()
    {
        int advancedIndex = muzzleIndex + 1;
        muzzleIndex = advancedIndex < muzzles.Length ? advancedIndex : 0;
    }

    /// <summary>Shoots projectiles from all muzzles.</summary>
    public void Shoot()
    {
        Cooldown cooldown = cooldowns[projectileIndex];

    	if(muzzles == null || cooldown.onCooldown) return;

    	Vector3 centerPoint = GetMuzzlesCenter() + (transform.forward * centerDistance);
        Projectile projectile = null;
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

            rotation = orientationType == OrientationType.MuzzlesCenter ?
                    Quaternion.LookRotation(centerPoint - muzzle.position) : muzzle.rotation;

            projectile = PoolManager.RequestProjectile(faction, projectileID, muzzle.position, rotation);

        } else foreach(Transform muzzle in muzzles)
    	{
            rotation = orientationType == OrientationType.MuzzlesCenter ?
                Quaternion.LookRotation(centerPoint - projectile.rigidbody.position) : muzzle.rotation;

    		projectile = PoolManager.RequestProjectile(faction, projectileID, muzzle.position, rotation);
    	}

        if(projectile.cooldownDuration <= 0.0f) return;

        cooldown.duration = projectile.cooldownDuration;
        cooldown.Begin();
    }
}
}