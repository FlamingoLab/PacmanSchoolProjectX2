using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class BarrelRoll : MonoBehaviour
{
	[Header("Roll's Attributes:")]
	[SerializeField] private int _spins; 						/// <summary>Number of full 360-Degree rolls made on the Barrel Roll.</summary>
	[SerializeField] private float _barrellRollDuration; 		/// <summary>Barrel Roll's Duration.</summary>
	[Space(5f)]
	[Header("Shield's Attributes:")]
	[SerializeField] private SphereCollider _sphereCollider; 	/// <summary>SphereCollider's Component.</summary>
	[SerializeField] private float _coverageRadius; 			/// <summary>Coverage Radius of the Roll's Shield.</summary>
	[SerializeField] private float _cooldownDuration; 			/// <summary>Barrel Roll's Cooldown Duration.</summary>
	private Coroutine cooldown; 								/// <summary>Cooldown's Coroutine Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets spins property.</summary>
	public int spins
	{
		get { return _spins; }
		set { _spins = value; }
	}

	/// <summary>Gets and Sets barrellRollDuration property.</summary>
	public float barrellRollDuration
	{
		get { return _barrellRollDuration; }
		set { _barrellRollDuration = value; }
	}

	/// <summary>Gets and Sets coverageRadius property.</summary>
	public float coverageRadius
	{
		get { return _coverageRadius; }
		set
		{
			_coverageRadius = value;
			sphereCollider.radius = _coverageRadius;
		}
	}

	/// <summary>Gets and Sets cooldownDuration property.</summary>
	public float cooldownDuration
	{
		get { return _cooldownDuration; }
		set { _cooldownDuration = value; }
	}

	/// <summary>Gets sphereCollider Component.</summary>
	public SphereCollider sphereCollider
	{ 
		get
		{
			if(_sphereCollider == null) _sphereCollider = GetComponent<SphereCollider>();
			return _sphereCollider;
		}
	}

	/// <summary>Gets onCooldown property.</summary>
	public bool onCooldown { get { return cooldown != null; } }
#endregion

	/// <summary>Draws Gizmos on Editor mode when BarrelRoll's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(Application.isPlaying && sphereCollider == null) return;
		sphereCollider.radius = coverageRadius;
	}

	/// <summary>BarelRoll's instance initialization.</summary>
	private void Awake()
	{
		sphereCollider.radius = coverageRadius;
		sphereCollider.gameObject.SetActive(false);
		BeginBarrelRoll();
	}

	/// <summary>Begins Barrel Roll [only if there is no cooldown activated].</summary>
	public void BeginBarrelRoll()
	{
		if(onCooldown) return;
		sphereCollider.gameObject.SetActive(true);
		StartCoroutine(BarrelRollCoroutine());
	}

	/// <summary>Callback internally called when the cooldown ends.</summary>
	private void OnCooldownEnds()
	{
		this.DispatchCoroutine(ref cooldown);
	}

	/// <summary>Barrel Roll's Coroutine. At the end of it the cooldown begins.</summary>
	private IEnumerator BarrelRollCoroutine()
	{
		float s = spins * 1.0f;
		float totalDegrees = 360.0f * s;
		float angularSpeed = totalDegrees / barrellRollDuration;
		float ellapsedTime = 0.0f;
		Vector3 eulerRotation = (Vector3.forward * angularSpeed);

		while(ellapsedTime < barrellRollDuration)
		{
			transform.Rotate(eulerRotation * Time.deltaTime, Space.Self);
			ellapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.localRotation = Quaternion.identity;
		sphereCollider.gameObject.SetActive(false);
		this.StartCoroutine(this.WaitSeconds(cooldownDuration), ref cooldown);
	}
}
}