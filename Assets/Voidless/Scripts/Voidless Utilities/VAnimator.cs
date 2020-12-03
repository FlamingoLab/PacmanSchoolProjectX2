using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidlessUtilies
{
public static class VAnimator
{
	/// <summary>Sets IK position of this Joint if it is one.</summary>
	/// <param name="_animator">Animator to Extend.</param>
	/// <param name="_IKGoal">IK Goal to displace.</param>
	/// <param name="_position">Position.</param>
	/// <param name="_weight">Position's Weight [1.0f by default].</param>
	public static void SetIKPosition(this Animator _animator, AvatarIKGoal _IKGoal, Vector3 _position, float _weight = 1.0f)
	{
		_animator.SetIKPosition(_IKGoal, _position);
        _animator.SetIKPositionWeight(_IKGoal, _weight);
	}
}
}