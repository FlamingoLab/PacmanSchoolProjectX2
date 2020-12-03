using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum LaserState
{
	Deactivated,
	Discharging,
	Engaging,
	Activated,
	OnCooldown
}

[RequireComponent(typeof(LineRenderer))]
public class DischargeLaserBeam : MonoBehaviour, IFiniteStateMachine<LaserState>
{
	public event OnStateChange<LaserState> onStateChange; 	/// <summary>OnStateChange's event delegate.</summary>

	[SerializeField] private Transform _muzzle; 			/// <summary>Muzzle's Origin.</summary>
	[Space(5f)]
	[Header("Laser's Attributes:")]
	[SerializeField] private float _length; 				/// <summary>Laser Beam's Length.</summary>
	[SerializeField] private float _radius; 				/// <summary>Laser Beam's Radius.</summary>
	[SerializeField] private float _dischargeDuration; 		/// <summary>Laser Shooting's Duration.</summary>
	[SerializeField] private float _engageDuration; 		/// <summary>Laser Engage's Duration.</summary>
	[SerializeField] private float _beamDuration; 			/// <summary>Lazer Beam's Duration.</summary>
	[SerializeField] private float _cooldownDuration; 		/// <summary>Cooldown's Duration.</summary>
	[Space(5f)]
	[SerializeField] private float _damage; 				/// <summary>Damage that this laser deals.</summary>
	[SerializeField] private float _followingSpeed; 		/// <summary>Following's Speed.</summary>
	[SerializeField] private LayerMask _affectable; 		/// <summary>LayerMask that says which GameObjects are affected by this Laser.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 			/// <summary>Gizmos' Color.</summary>
#endif
	private float _currentLength; 							/// <summary>Laser's current Length.</summary>
	private LaserState _state; 								/// <summary>Current State.</summary>
	private LaserState _previousState; 						/// <summary>Previous State.</summary>
	private LineRenderer _lineRenderer; 					/// <summary>LineRenderer's Component.</summary>
	private Coroutine coroutine; 							/// <summary>General Coroutine reference.</summary>
	private Cooldown _cooldown; 							/// <summary>Ability's Cooldown.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets muzzle property.</summary>
	public Transform muzzle
	{
		get { return _muzzle; }
		set { _muzzle = value; }
	}

	/// <summary>Gets and Sets length property.</summary>
	public float length
	{
		get { return _length; }
		set { _length = value; }
	}

	/// <summary>Gets and Sets radius property.</summary>
	public float radius
	{
		get { return _radius; }
		set
		{
			_radius = value;

			float halfRadius = _radius * 0.5f;
			lineRenderer.SetWidth(halfRadius, halfRadius);
		}
	}

	/// <summary>Gets and Sets dischargeDuration property.</summary>
	public float dischargeDuration
	{
		get { return _dischargeDuration; }
		set { _dischargeDuration = value; }
	}

	/// <summary>Gets and Sets engageDuration property.</summary>
	public float engageDuration
	{
		get { return _engageDuration; }
		set { _engageDuration = value; }
	}

	/// <summary>Gets and Sets damage property.</summary>
	public float damage
	{
		get { return _damage; }
		set { _damage = value; }
	}

	/// <summary>Gets and Sets followingSpeed property.</summary>
	public float followingSpeed
	{
		get { return _followingSpeed; }
		set { _followingSpeed = value; }
	}

	/// <summary>Gets and Sets beamDuration property.</summary>
	public float beamDuration
	{
		get { return _beamDuration; }
		set { _beamDuration = value; }
	}

	/// <summary>Gets and Sets cooldownDuration property.</summary>
	public float cooldownDuration
	{
		get { return _cooldownDuration; }
		set { _cooldownDuration = value; }
	}

	/// <summary>Gets and Sets currentLength property.</summary>
	public float currentLength
	{
		get { return _currentLength; }
		private set { _currentLength = value; }
	}

	/// <summary>Gets and Sets affectable property.</summary>
	public LayerMask affectable
	{
		get { return _affectable; }
		set { _affectable = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public LaserState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public LaserState previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}

	/// <summary>Gets lineRenderer Component.</summary>
	public LineRenderer lineRenderer
	{ 
		get
		{
			if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
			return _lineRenderer;
		}
	}

	/// <summary>Gets and Sets cooldown property.</summary>
	public Cooldown cooldown
	{
		get { return _cooldown; }
		set { _cooldown = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
		{
			float halfRadius = radius * 0.5f;
			lineRenderer.SetWidth(halfRadius, halfRadius);
		}

		if(muzzle == null) return;

		Gizmos.color = gizmosColor;
		Gizmos.DrawRay(muzzle.position, muzzle.forward * length);
	}
#endif

#region UnityMethods:
	/// <summary>ShootLaserBeam's instance initialization.</summary>
	private void Awake()
	{
		cooldown = new Cooldown(this, cooldownDuration, OnCooldownEnds);
	}
	
	/// <summary>ShootLaserBeam's tick at each frame.</summary>
	private void Update ()
	{
		if(state == LaserState.Deactivated || cooldown.onCooldown) return;

		Vector3 end = muzzle.position + (muzzle.forward * currentLength);
		RaycastHit hit;

		if(VPhysics.SphereCast(muzzle.position, radius, muzzle.forward, out hit, currentLength, affectable))
		{
			Health health = hit.transform.GetComponent<Health>();
			if(health != null) health.GiveDamage(damage);
			end = hit.point;
		}

		lineRenderer.SetPosition(0, muzzle.position);
		lineRenderer.SetPosition(1, end);
	}
#endregion

	/// <summary>Enters LaserState State.</summary>
	/// <param name="_state">LaserState State that will be entered.</param>
	public void OnEnterState(LaserState _state)
	{
		switch(_state)
		{
			case LaserState.Deactivated:
			lineRenderer.enabled = false;
			break;

			case LaserState.Discharging:
			if(cooldown.onCooldown)
			{
				this.ChangeState(LaserState.Deactivated);
				break;
			}
			lineRenderer.enabled = true;
			this.StartCoroutine(ChangeBeamLength(true), ref coroutine);
			break;

			case LaserState.Engaging:
			lineRenderer.enabled = true;
			this.StartCoroutine(ChangeBeamLength(false), ref coroutine);
			break;

			case LaserState.Activated:
			lineRenderer.enabled = true;
			this.StartCoroutine(this.WaitSeconds(beamDuration, OnBeamDurationEnds), ref coroutine);
			break;

			case LaserState.OnCooldown:
			lineRenderer.enabled = false;
			cooldown.Begin();
			break;
		}

		if(onStateChange != null) onStateChange(_state);
	}
	
	/// <summary>Leaves LaserState State.</summary>
	/// <param name="_state">LaserState State that will be left.</param>
	public void OnExitState(LaserState _state)
	{
		switch(_state)
		{
			case LaserState.Discharging:
			case LaserState.Engaging:
			this.DispatchCoroutine(ref coroutine);
			break;
		}
	}

	/// <summary>Begins Laser Beam's Emission.</summary>
	public void Begin()
	{
		if(state == LaserState.Deactivated)
		this.ChangeState(LaserState.Discharging);
	}

	/// <summary>Points Laser Beam Towards Target.</summary>
	/// <param name="_target">Target's Point.</param>
	public void PointTowards(Vector3 _target)
	{
		Vector3 direction = _target - transform.position;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), followingSpeed * Time.deltaTime);
	}

	/// <summary>Callback internally invoked when the Beam's duration ends.</summary>
	private void OnBeamDurationEnds()
	{
		this.ChangeState(LaserState.Engaging);
	}

	/// <summary>Callback internally invoked when the cooldown ends.</summary>
	private void OnCooldownEnds()
	{
		this.ChangeState(LaserState.Deactivated);
	}

	/// <summary>Changes Beam's Length.</summary>
	/// <param name="_discharge">Discharge the beam [augment length]? false [dimisnish the length] otherwise.</param>
	private IEnumerator ChangeBeamLength(bool _discharge = true)
	{
		float t = 0.0f;
		float inverseDuration = 1.0f / (_discharge ? dischargeDuration : engageDuration);
		currentLength = _discharge ? 0.0f : length;

		while(t < 1.0f)
		{
			currentLength = Mathf.Lerp(0.0f, length, _discharge ? t : (1.0f - t));	

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		this.ChangeState(_discharge ? LaserState.Activated : LaserState.OnCooldown);
	}
}
}