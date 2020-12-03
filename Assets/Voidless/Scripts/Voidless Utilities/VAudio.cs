using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VAudio
{
	/// <summary>Stops AudioSource, then assigns and plays AudioClip.</summary>
	/// <param name="_audioSource">AudioSource to play sound.</param>
	/// <param name="_aucioClip">AudioClip to play.</param>
	/// <param name="_loop">Loop AudioClip? false as default.</param>
	public static void PlaySound(this AudioSource _audioSource, AudioClip _audioClip, bool _loop = false)
	{
		_audioSource.Stop();
		_audioSource.clip = _audioClip;
		_audioSource.Play();
		_audioSource.loop = _loop;
	}

	/// <summary>Stops AudioSource, then assigns and plays AudioClip.</summary>
	/// <param name="_audioSource">AudioSource to play sound.</param>
	/// <param name="_aucioClip">AudioClip to play.</param>
	/// <param name="_volumeScale">Normalized Volume's Scale.</param>
	public static void PlaySoundOneShot(this AudioSource _audioSource, AudioClip _audioClip, float _volumeScale = 1.0f)
	{
		_audioSource.PlayOneShot(_audioClip, _volumeScale);
	}
}
}