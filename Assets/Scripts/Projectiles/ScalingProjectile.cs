using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class ScalingProjectile : Projectile
{
	[Space(5f)]
	[Header("Scaling Projectile's Attributes:")]
	[SerializeField] private Transform _projectileChild; 	/// <summary>Projectile's Child.</summary>
	[SerializeField] private float _scaleDuration; 			/// <summary>Scaling's Duration.</summary>
	private Coroutine scaling; 								/// <summary>Scaling's Coroutine Reference.</summary>

	/// <summary>Gets projectileChild property.</summary>
	public Transform projectileChild { get { return _projectileChild; } }

	/// <summary>Gets scaleDuration property.</summary>
	public float scaleDuration { get { return _scaleDuration; } }

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public override void OnObjectReset()
	{
		base.OnObjectReset();
		OnSacleEnds();
		transform.localScale = Vector3.zero;
		this.StartCoroutine(projectileChild.RegularScale(1.0f, scaleDuration, OnSacleEnds), ref scaling);
	}

	/// <summary>Callabak internally invoked when the scaling coroutine ends.</summary>
	private void OnSacleEnds()
	{
		this.DispatchCoroutine(ref scaling);
	}
}
}