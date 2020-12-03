using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class Ability : MonoBehaviour
{
	[SerializeField] private float _cooldownDuration; 	/// <summary>Cooldown's Duration.</summary>
	private Cooldown _cooldown; 						/// <summary>Cooldown's Reference.</summary>

	/// <summary>Gets and Sets cooldownDuration property.</summary>
	public float cooldownDuration
	{
		get { return _cooldownDuration; }
		set { _cooldownDuration = value; }
	}

	/// <summary>Gets and Sets cooldown property.</summary>
	public Cooldown cooldown
	{
		get { return _cooldown; }
		private set { _cooldown = value; }
	}

#region UnityMethods:
	/// <summary>Ability's instance initialization.</summary>
	private void Awake()
	{
		
	}

	/// <summary>Ability's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		
	}
	
	/// <summary>Ability's tick at each frame.</summary>
	private void Update ()
	{
		
	}
#endregion
}
}