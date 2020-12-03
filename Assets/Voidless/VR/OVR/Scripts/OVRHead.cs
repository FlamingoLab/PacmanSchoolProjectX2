using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.VR
{
public class OVRHead : MonoBehaviour
{
	[Header("Hands:")]
	[SerializeField] private Transform _leftHandAnchor; 	/// <summary>Left Hand's Anchor.</summary>
	[SerializeField] private Transform _rightHandAnchor; 	/// <summary>Right Hand's Anchor.</summary>

	/// <summary>Gets and Sets leftHandAnchor property.</summary>
	public Transform leftHandAnchor
	{
		get { return _leftHandAnchor; }
		set { _leftHandAnchor = value; }
	}

	/// <summary>Gets and Sets rightHandAnchor property.</summary>
	public Transform rightHandAnchor
	{
		get { return _rightHandAnchor; }
		set { _rightHandAnchor = value; }
	}
}
}