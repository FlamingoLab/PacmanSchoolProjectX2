using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event invoked when this Hit Collider2D hits a gameobject.</summary>
/// <param name="_gameObject">GameObject that was involved on the Hit Event.</param>
/// <param name="_eventType">Type of the event.</param>
public delegate void OnHitColliderEvent2D(GameObject _gameObject, HitColliderEventTypes _eventType);

public class HitCollider2D : MonoBehaviour, ILayerAffecter, ITagsAffecter
{
	public event OnHitColliderEvent2D onHitColliderEvent2D; 				/// <summary>OnHitColliderEvent event delegate.</summary>

	[SerializeField] private HitColliderEventTypes _detectableHitEvents; 	/// <summary>Detectablie Hit's Events.</summary>
	[SerializeField] private LayerMask _affectedLayer;  					/// <summary>Affected LayerMasks.</summary>
	[SerializeField] private string[] _affectedTags;  						/// <summary>Affected Tags.</summary>
	private Collider2D _collider; 											/// <summary>Collider2D's Component.</summary>

	/// <summary>Gets and Sets detectableHitEvents property.</summary>
	public HitColliderEventTypes detectableHitEvents
	{
		get { return _detectableHitEvents; }
		set { _detectableHitEvents = value; }
	}

	/// <summary>Gets and Sets affectedLayer property.</summary>
	public LayerMask affectedLayer
	{
		get { return _affectedLayer; }
		set { _affectedLayer = value; }
	}

	/// <summary>Gets and Sets affectedTags property.</summary>
	public string[] affectedTags
	{
		get { return _affectedTags; }
		set { _affectedTags = value; }
	}

	/// <summary>Gets and Sets collider Component.</summary>
	public new Collider2D collider
	{ 
		get
		{
			if(_collider == null) _collider = GetComponent<Collider2D>();
			return _collider;
		}
	}

	/// <summary>Sets the Collider2D either as trigger or as a collision collider.</summary>
	/// <param name="_trigger">Set collider as trigger?.</param>
	public void SetTrigger(bool _trigger)
	{
		collider.isTrigger = _trigger;
	}

#region TriggerCallbakcs:
	/// <summary>Event triggered when this Collider2D enters another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	void OnTriggerEnter2D(Collider2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;

		GameObject obj = col.gameObject;
	
		if(obj.IsInLayerMask(affectedLayer)) if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Enter);
		else
		{
			if(affectedTags != null)
			for(int i = 0; i < affectedTags.Length; i++)
			{
				if(obj.CompareTag(affectedTags[i]))
				if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Enter);
			}
		}
	}

	/// <summary>Event triggered when this Collider2D stays with another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	void OnTriggerStay2D(Collider2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		
		GameObject obj = col.gameObject;
	
		if(obj.IsInLayerMask(affectedLayer)) if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Stays);
		else
		{
			if(affectedTags != null)
			for(int i = 0; i < affectedTags.Length; i++)
			{
				if(obj.CompareTag(affectedTags[i]))
				if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Stays);
			}
		}
	}

	/// <summary>Event triggered when this Collider2D exits another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	void OnTriggerExit2D(Collider2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;

		GameObject obj = col.gameObject;
	
		if(obj.IsInLayerMask(affectedLayer)) if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Exit);
		else
		{
			if(affectedTags != null)
			for(int i = 0; i < affectedTags.Length; i++)
			{
				if(obj.CompareTag(affectedTags[i]))
				if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Exit);
			}
		}
	}
#endregion

#region CollisionCallbacks:
	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	void OnCollisionEnter2D(Collision2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;

		GameObject obj = col.gameObject;
	
		if(obj.IsInLayerMask(affectedLayer)) if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Enter);
		else
		{
			if(affectedTags != null)
			for(int i = 0; i < affectedTags.Length; i++)
			{
				if(obj.CompareTag(affectedTags[i]))
				if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Enter);
			}
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	void OnCollisionStay2D(Collision2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		
		GameObject obj = col.gameObject;
	
		if(obj.IsInLayerMask(affectedLayer)) if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Stays);
		else
		{
			if(affectedTags != null)
			for(int i = 0; i < affectedTags.Length; i++)
			{
				if(obj.CompareTag(affectedTags[i]))
				if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Stays);
			}
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody began having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	void OnCollisionExit2D(Collision2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;

		GameObject obj = col.gameObject;
	
		if(obj.IsInLayerMask(affectedLayer)) if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Exit);
		else
		{
			if(affectedTags != null)
			for(int i = 0; i < affectedTags.Length; i++)
			{
				if(obj.CompareTag(affectedTags[i]))
				if(onHitColliderEvent2D != null) onHitColliderEvent2D(obj, HitColliderEventTypes.Exit);
			}
		}
	}
#endregion
}
}