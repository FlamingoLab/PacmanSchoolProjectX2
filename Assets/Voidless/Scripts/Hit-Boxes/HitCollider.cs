using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event Invoked when this Hit Collider Triggers with another Collider.</summary>
/// <param name="_collider">Collider involved on the Trigger Event.</param>
public delegate void OnColliderEvent(Collider _collider);

/// <summary>Event invoked when this Hit Collider Collides with another Collider.</summary>
/// <param name="_collistion">Collision Data.</param>
public delegate void OnCollisionEvent(Collision _collision);

public class HitCollider : MonoBehaviour
{
	public event OnColliderEvent onTriggerEnter; 							/// <summary>OnTriggerEnter's Event Delegate.</summary>
	public event OnColliderEvent onTriggerStay; 							/// <summary>OnTriggerStay's Event Delegate.</summary>
	public event OnColliderEvent onTriggerExit; 							/// <summary>OnTriggerExit's Evetn Delegate.</summary>
	public event OnCollisionEvent onCollisionEnter; 						/// <summary>OnCollisionEnter's Event Delegate.</summary>
	public event OnCollisionEvent onCollisionStay; 							/// <summary>OnCollisionStay's Event Delegate.</summary>
	public event OnCollisionEvent onCollisionExit; 							/// <summary>OnCollisionExit's Evetn Delegate.</summary>

	[SerializeField] private HitColliderEventTypes _detectableHitEvents; 	/// <summary>Detectablie Hit's Events.</summary>
	private Collider _collider; 											/// <summary>Collider's Component.</summary>

	/// <summary>Gets and Sets detectableHitEvents property.</summary>
	public HitColliderEventTypes detectableHitEvents
	{
		get { return _detectableHitEvents; }
		set { _detectableHitEvents = value; }
	}

	/// <summary>Gets and Sets collider Component.</summary>
	public new Collider collider
	{ 
		get
		{
			if(_collider == null) _collider = GetComponent<Collider>();
			return _collider;
		}
	}

	/// <summary>Activates Hit Collider.</summary>
	public void Activate()
	{
		gameObject.SetActive(true);
	}

	/// <summary>Deactivates Hit Collider.</summary>
	public void Deactivate()
	{
		gameObject.SetActive(false);
	}

	/// <summary>Sets the Collider either as trigger or as a collision collider.</summary>
	/// <param name="_trigger">Set collider as trigger?.</param>
	public void SetTrigger(bool _trigger)
	{
		collider.isTrigger = _trigger;
	}

#region TriggerCallbakcs:
	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;
		if(onTriggerEnter != null) onTriggerEnter(col);
	}

	/// <summary>Event triggered when this Collider stays with another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerStay(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		if(onTriggerStay != null) onTriggerStay(col);
	}

	/// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerExit(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;
		if(onTriggerExit != null) onTriggerExit(col);
	}
#endregion

#region CollisionCallbacks:
	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionEnter(Collision col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;
		if(onCollisionEnter != null) onCollisionEnter(col);
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionStay(Collision col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		if(onCollisionExit != null) onCollisionExit(col);
	}

	/// <summary>Event triggered when this Collider/Rigidbody began having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionExit(Collision col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;
		if(onCollisionExit != null) onCollisionExit(col);
	}
#endregion
}
}