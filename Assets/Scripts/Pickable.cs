using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(TransformRotation))]
public class Pickable : PoolGameObject
{
	[SerializeField] private LayerMask _interactable; 	/// <summary>Mask that dictates which GameObjects this collectible can interact with.</summary>
	[SerializeField] private PickableType _type; 		/// <summary>Type of Pickable.</summary>
	[SerializeField] private float _value; 				/// <summary>Value of this Pickable.</summary>
	private TransformRotation _rotation; 				/// <summary>TransformRotation's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets interactable property.</summary>
	public LayerMask interactable { get { return _interactable; } }

	/// <summary>Gets type property.</summary>
	public PickableType type { get { return _type; } }

	/// <summary>Gets value property.</summary>
	public float value { get { return _value; } }

	/// <summary>Gets rotation Component.</summary>
	public TransformRotation rotation
	{ 
		get
		{
			if(_rotation == null) _rotation = GetComponent<TransformRotation>();
			return _rotation;
		}
	}
#endregion

	/// <summary>Updates Pickable's instance at each frame.</summary>
	private void Update()
	{
		rotation.RotateInAxes(Vector3.up);
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;
	
		if((interactable | obj.layer << 1) == interactable)
		{
			switch(type)
			{
				case PickableType.NormalCollectible:
				GameplayController.OnNormalCollectiblePicked(value);
				break;

				case PickableType.RareCollectible:
				GameplayController.OnRareCollectiblePicked(value);
				break;

				case PickableType.Health:
				break;

				case PickableType.Secret:
				break;
			}
			
			OnObjectDeactivation();
		}
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		transform.rotation = Quaternion.identity;
	}
}
}