using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public enum Fade
{
	In,
	Out
}

public class ScreenFaderGUI : BaseGUI<Fade>
{
	[SerializeField] private Image _screen; 		/// <summary>Fade's Screen.</summary>
	[Space(5f)]
	[Header("Fade's Attributes:")]
	[SerializeField] private Color _inColor; 		/// <summary>Fade-In's color.</summary>
	[SerializeField] private Color _outColor; 		/// <summary>Fade-Out's color.</summary>
	[SerializeField] private float _inDuration; 	/// <summary>Default Fade-In's duration.</summary>
	[SerializeField] private float _outDuration; 	/// <summary>Default Fade-Out's duration.</summary>
	private Behavior _fadeEffect; 					/// <summary>Fade Effect's Coroutine controller.</summary>

	/// <summary>Gets and Sets screen property.</summary>
	public Image screen
	{
		get { return _screen; }
		set { _screen = value; }
	}

	/// <summary>Gets and Sets inColor property.</summary>
	public Color inColor
	{
		get { return _inColor; }
		set { _inColor = value; }
	}

	/// <summary>Gets and Sets outColor property.</summary>
	public Color outColor
	{
		get { return _outColor; }
		set { _outColor = value; }
	}

	/// <summary>Gets and Sets inDuration property.</summary>
	public float inDuration
	{
		get { return _inDuration; }
		set { _inDuration = value; }
	}

	/// <summary>Gets and Sets outDuration property.</summary>
	public float outDuration
	{
		get { return _outDuration; }
		set { _outDuration = value; }
	}

	/// <summary>Gets state property.</summary>
	public Fade state { get { return screen.color.a > 0.5f ? Fade.In : Fade.Out; } }

	/// <summary>Gets and Sets fadeEffect property.</summary>
	public Behavior fadeEffect
	{
		get { return _fadeEffect; }
		private set { _fadeEffect = value; }
	}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_effectMode">Fade Effect's Mode.</param>
	public override void UpdateGUI(Fade _effectMode)
	{
		if(fadeEffect != null) fadeEffect.EndBehavior();
		fadeEffect = new Behavior(this, _effectMode == Fade.In ? FadeIn(inDuration) : FadeOut(outDuration));
	}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_effectMode">Fade Effect's Mode.</param>
	/// <param name="_duration">Fade Effect's duration.</param>
	/// <param name="onFadeEnds">Callback invoked when the fade ends.</param>
	public void UpdateGUI(Fade _effectMode, float _duration = 0f, Action onFadeEnds = null)
	{
		if(fadeEffect != null) fadeEffect.EndBehavior();
		fadeEffect = new Behavior(this, _effectMode == Fade.In ? FadeIn(_duration <= 0f ? inDuration : _duration, onFadeEnds) : FadeOut(_duration <= 0f ? outDuration : _duration, onFadeEnds));
	}

	/// <summary>Fades Screen in.</summary>
	/// <param name="_duration">Fade Effect's duration.</param>
	/// <param name="onFadeEnds">Callback invoked when the fade ends.</param>
	private IEnumerator FadeIn(float _duration, Action onFadeEnds = null)
	{
		float n = 0.0f;
		screen.color = screen.color.WithAlpha(0f);

		while(n < (1f + Mathf.Epsilon))
		{
			screen.color = screen.color.WithAlpha(n);
			n += (Time.deltaTime / _duration);
			yield return null;
		}

		screen.color = screen.color.WithAlpha(1f);
		if(onFadeEnds != null) onFadeEnds();
	}

	/// <summary>Fades Screen out.</summary>
	/// <param name="_duration">Fade Effect's duration.</param>
	/// <param name="onFadeEnds">Callback invoked when the fade ends.</param>
	private IEnumerator FadeOut(float _duration, Action onFadeEnds = null)
	{
		float n = 0.0f;
		screen.color = screen.color.WithAlpha(1f);

		while(n < (1f + Mathf.Epsilon))
		{
			screen.color = screen.color.WithAlpha(1.0f - n);
			n += (Time.deltaTime / _duration);
			yield return null;
		}

		screen.color = screen.color.WithAlpha(0f);
		if(onFadeEnds != null) onFadeEnds();
	}
}
}