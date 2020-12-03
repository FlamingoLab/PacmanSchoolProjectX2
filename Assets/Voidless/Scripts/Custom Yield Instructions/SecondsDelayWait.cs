using System.Collections;
using UnityEngine;

namespace Voidless
{
public class SecondsDelayWait : VYieldInstruction
{
	private float _waitDuration; 	/// <summary>Wait Delay's Duration.</summary>
	private float _currentWait; 	/// <summary>Current Wait's Time.</summary>
	private float _timeScale; 		/// <summary>Time's Scale.</summary>

	/// <summary>Gets and Sets waitDuration property.</summary>
	public float waitDuration
	{
		get { return _waitDuration; }
		set { _waitDuration = value; }
	}

	/// <summary>Gets and Sets currentWait property.</summary>
	public float currentWait
	{
		get { return _currentWait; }
		protected set { _currentWait = value; }
	}

	/// <summary>Gets and Sets timeScale property.</summary>
	public float timeScale
	{
		get { return _timeScale; }
		set { _timeScale = value; }
	}

	/// <summary>SecondsDelayWait's constructor.</summary>
	/// <param name="_waitDuration">Wait's Duration.</param>
	public SecondsDelayWait(float _waitDuration, float _timeScale = 1.0f) : base()
	{
		waitDuration = _waitDuration;
		currentWait = 0.0f;
		timeScale = _timeScale;
	}

	/// <summary>Advances the enumerator to the next element of the collection.</summary>
	public virtual bool MoveNext()
	{
		timeScale = 1.0f;
		return base.MoveNext();
	}

	/// <summary>Iterates the current time.</summary>
	/// <param name="_timeScale">New Time Scale.</param>
	/// <returns>True if the iterator can keep moving, false otherwise.</returns>
	public bool MoveNext(float _timeScale)
	{
		timeScale = _timeScale;
		return base.MoveNext();
	}

	/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
	public override void Reset()
	{
		currentWait = 0.0f;
		enumerator = Operation();
	}

	/// <summary>Yield Instruction's Operation.</summary>
	protected override IEnumerator Operation()
	{
		while(currentWait < (waitDuration + Mathf.Epsilon))
		{
			currentWait += Time.deltaTime * timeScale;
			yield return null;
		}
	}
}
}