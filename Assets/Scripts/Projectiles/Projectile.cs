using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum ProjectileType
{
	Normal,
	Parabola,
	Homing
}

[RequireComponent(typeof(Rigidbody))]
public class Projectile : PoolGameObject
{
	[Space(5f)]
	[SerializeField] private LayerMask _affectable; 	/// <summary>Mask that contains GameObjects affectable by this Projectile.</summary>
	[SerializeField] private float _damage; 			/// <summary>Damage that this Projectile is able to inflict.</summary>
	[SerializeField] private float _speed; 				/// <summary>Projectile's Speed.</summary>
	[SerializeField] private float _lifespan; 			/// <summary>Projectile's Lifespan.</summary>
	[SerializeField] private float _cooldownDuration; 	/// <summary>Cooldown's Duration.</summary>
	private float _currentLifeTime; 					/// <summary>Current Life Time.</summary>
	private Rigidbody _rigidbody; 						/// <summary>Rigidbody's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets affectable property.</summary>
	public LayerMask affectable
	{
		get { return _affectable; }
		set { _affectable = value; }
	}	

	/// <summary>Gets and Sets damage property.</summary>
	public float damage
	{
		get { return _damage; }
		set { _damage = value; }
	}

	/// <summary>Gets and Sets speed property.</summary>
	public float speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	/// <summary>Gets and Sets lifespan property.</summary>
	public float lifespan
	{
		get { return _lifespan; }
		set { _lifespan = value; }
	}

	/// <summary>Gets and Sets cooldownDuration property.</summary>
	public float cooldownDuration
	{
		get { return _cooldownDuration; }
		set { _cooldownDuration = value; }
	}

	/// <summary>Gets and Sets currentLifeTime property.</summary>
	public float currentLifeTime
	{
		get { return _currentLifeTime; }
		set { _currentLifeTime = value; }
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}
#endregion

	/// <summary>Updates Projectile's instance at each frame.</summary>
	private void Update()
	{
		if(currentLifeTime < lifespan)
		{
			currentLifeTime += Time.deltaTime;
			OnUpdate();

		} else OnObjectDeactivation();
	}

	/// <summary>Callback called each Physics' Time Step.</summary>
	private void FixedUpdate()
	{
		rigidbody.MovePosition(rigidbody.position + CalculateDisplacement());
	}

	/// <summary>Callback when a trigger event happens between a trigger attached to this GameObject and another one.</summary>
	/// <param name="_collider">Collider that this trigger intersected with.</param>
	private void OnTriggerEnter(Collider _collider)
	{
		GameObject obj = _collider.gameObject;
		int layerMask = 1 << obj.layer;

		if((affectable.value & layerMask) == layerMask)
		{
			Health health = obj.GetComponent<Health>();
			if(health != null) health.GiveDamage(damage);
			OnObjectDeactivation();
		}
	}

	/// <summary>Callback internally invoked insided Update.</summary>
	protected virtual void OnUpdate() {/*...*/}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		currentLifeTime = 0.0f;
	}

	/// <returns>Displacement acoording to the Projectile's Type.</returns>
	protected virtual Vector3 CalculateDisplacement()
	{
		return (transform.forward * speed * Time.fixedDeltaTime);
	}
}
}