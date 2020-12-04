using UnityEngine;
using System.Collections;
using System;

/*============================================================
**
** Class:  VCoroutines
**
** Purpose: This static class contains utility properties and methods for Coroutines:
** 
** 	- General-purpose IEnumerators
**  - Static YieldInstructions. The following are the reasons:
**		- Unity's Coroutine Manager does reflection on what the IEnumerators returns.
**		- So, the same Manager does all the logic just by interpreting the YieldInstructions' Types.
**		- With this, we don't need to be initializing some YieldInstructions, such as the ones being declared.
** 	- Methods for safe startups and dispatchments of Coroutines
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Voidless
{
public static class VCoroutines
{
	public static readonly WaitForEndOfFrame WAIT_MAIN_THREAD; 		/// <summary>Wait for end of main thread's Yield Instruction.</summary>
	public static readonly WaitForFixedUpdate WAIT_PHYSICS_THREAD; 	/// <summary>Wait for end of physics thread's Yield Instruction.</summary>

	/// <summary>VoidlessCoroutine's Static Constructor.</summary>
	static VCoroutines()
	{
		WAIT_MAIN_THREAD = new WaitForEndOfFrame(); 	
		WAIT_PHYSICS_THREAD = new WaitForFixedUpdate();
	}

	/// <summary>Stops reference's Coroutine and then  sets it to null [if the Coroutine is different than null]. Starts Coroutine.</summary>
	/// <param name="_monoBehaviour">Extension MonoBehaviour, used to call StopCoroutine and StartCoroutine.</param>
	/// <param name="_iterator">Coroutine's Iterator.</param>
	/// <param name="_coroutine">Coroutine to dispatch and to initialize.</param>
	public static Coroutine StartCoroutine(this MonoBehaviour _monoBehaviour, IEnumerator _iterator, ref Coroutine _coroutine)
	{
		if(_coroutine != null)
		{
			_monoBehaviour.StopCoroutine(_coroutine);
			_coroutine = null;
		}
		return _coroutine = _monoBehaviour.StartCoroutine(_iterator);
	}

	/// <summary>Stops reference's Coroutine and then  sets it to null [if the Coroutine is different than null].</summary>
	/// <param name="_monoBehaviour">Extension MonoBehaviour, used to call StopCoroutine.</param>
	/// <param name="_coroutine">Coroutine to dispatch.</param>
	public static void DispatchCoroutine(this MonoBehaviour _monoBehaviour, ref Coroutine _coroutine)
	{
		if(_coroutine != null)
		{
			_monoBehaviour.StopCoroutine(_coroutine);
			_coroutine = null;
		}
	}

	/// <summary>Creates new Asynchronous Behavior as a Coroutine.</summary>
	/// <param name="_monoBehavior">MonoBehavior that will start the coroutine.</param>
	/// <param name="_iterator">Coroutine's Iterator.</param>
	/// <param name="_startAutomagically">Start the coroutine as soon as the Behavior is initialized [true by default].</param>
	/// <returns>Created Behavior.</returns>
	public static Behavior StartBehaviorCoroutine(this MonoBehaviour _monoBehaviour, IEnumerator _iterator, bool _startAutomagically = true)
	{
		return new Behavior(_monoBehaviour, _iterator, _startAutomagically);
	}

	/// <summary>Ends Behaviour's reference, and then sets it to null.</summary>
	/// <param name="_behavior">Behavior to dispatch.</param>
	public static void DispatchBehavior(ref Behavior _behavior)
	{
		if(_behavior != null)
		{
			_behavior.EndBehavior();
			_behavior = null;
		}
	}

#region IEnumerators:
	/// <summary>Waits for a certain ILoadable instance to be loaded.</summary>
	/// <param name="_monoBehaviour">Requesting MonoBehaviour.</param>
	/// <param name="_loadable">Expected ILoadable instance to load.</param>
	/// <param name="onObjectLoaded">Optional callback invoked when the object is loaded.</param>
	public static IEnumerator WaitForLoadable<T>(this MonoBehaviour _monoBehaviour, T _loadable, Action onObjectLoaded = null) where T : MonoBehaviour, ILoadable
	{
		while(!_loadable.Loaded) { yield return null; }
		if(onObjectLoaded != null) onObjectLoaded();
	}

	/// <summary>Wait for some seconds, and then invoke a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onIEnumeratorEnds">Callback invoked when IEnumerator ends.</param>
	public static IEnumerator WaitSeconds(this MonoBehaviour _monoBehaviour, float _seconds, Action onWaitEnds = null)
	{
		SecondsDelayWait wait = new SecondsDelayWait(_seconds);
		while(wait.MoveNext()) yield return null;
		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Wait for some random seconds, and then invoke a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_secondsRange">Random range of seconds.</param>
	/// <param name="onIEnumeratorEnds">Callback invoked when IEnumerator ends.</param>
	public static IEnumerator WaitRandomSeconds(this MonoBehaviour _monoBehaviour, FloatRange _secondsRange, Action onWaitEnds = null)
	{
		float randomDuration = UnityEngine.Random.Range(_secondsRange.min, _secondsRange.max);
		SecondsDelayWait wait = new SecondsDelayWait(randomDuration);
		while(wait.MoveNext()) yield return null;
		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Does action while waiting some seconds.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_seconds">Seconds to wait.</param>
	/// <param name="doWhileAction">Action to do while waiting seconds.</param>
	/// <param name="onWaitEnds">Optional callback invoked when the wait ends.</param>
	public static IEnumerator DoWhileWaitingSeconds(this MonoBehaviour _monoBehaviour, float _seconds, Action doWhileAction, Action onWaitEnds = null)
	{
		SecondsDelayWait wait = new SecondsDelayWait(_seconds);

		while(wait.MoveNext())
		{
			doWhileAction();
			yield return null;
		}

		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Does action while waiting some random seconds.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_secondsRange">Random range of seconds.</param>
	/// <param name="doWhileAction">Action to do while waiting seconds.</param>
	/// <param name="onWaitEnds">Optional callback invoked when the wait ends.</param>
	public static IEnumerator DoWhileWaitingSeconds(this MonoBehaviour _monoBehaviour, FloatRange _secondsRange, Action doWhileAction, Action onWaitEnds = null)
	{
		float randomDuration = UnityEngine.Random.Range(_secondsRange.min, _secondsRange.max);
		SecondsDelayWait wait = new SecondsDelayWait(randomDuration);

		while(wait.MoveNext())
		{
			doWhileAction();
			yield return null;
		}

		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Waits until a condition is false, to then invoke a callbakc when it is done.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_condition">Condition iterator.</param>
	/// <param name="onConditionEnds">Callback invoked when the condition ends.</param>
	public static IEnumerator WaitUntilCondition(this MonoBehaviour _monoBehaviour, Func<bool> _condition, Action onWaitEnds = null)
	{
		while(!_condition()) { yield return null; }
		if(onWaitEnds != null) onWaitEnds();
	}

	/// <summary>Waits until a condition is false, to then invoke a callbakc when it is done.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_condition">Condition iterator.</param>
	/// <param name="onConditionEnds">Callback invoked when the condition ends.</param>
	public static IEnumerator WaitUntilCondition(this MonoBehaviour _monoBehaviour, IEnumerator _condition, Action onConditionEnds)
	{
		while(_monoBehaviour.enabled && _condition.MoveNext()) { yield return null; }
		onConditionEnds();
	}

	/// <summary>Moves Transform towards position.</summary>
	/// <param name="_transform">Transform to move.</param>
	/// <param name="_position">Position to move the transform to.</param>
	/// <param name="_duration">Displacement's duration.</param>
	/// <param name="onMoveEnds">Optional Callback invoked when the displacement ends.</param>
	public static IEnumerator DisplaceToPosition(this Transform _transform, Vector3 _position, float _duration, Action onMoveEnds = null)
	{
		Vector3 originalPosition = _transform.localPosition;
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			_transform.localPosition = Vector3.Lerp(originalPosition, _position, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		if(onMoveEnds != null) onMoveEnds();
	}

	/// <summary>Moves Rigidbody towards given target at given speed.</summary>
	/// <param name="_rigidbody">RTigidbody to move.</param>
	/// <param name="_target">Destination target.</param>
	/// <param name="_speed">Movement's Speed.</param>
	/// <param name="_distance">Distance's threshold [0.0f by default].</param>
	/// <param name="onMovementEnds">Optional callback to invoke when the movement is finished.</param>
	public static IEnumerator MoveRigidbodyTowards(this Rigidbody _rigidbody, Vector3 _target, float _speed, float _distance = 0.0f, Action onMovementEnds = null)
	{
		Vector3 direction = _target - _rigidbody.position;
		float squareDistance = _distance * _distance;

		while(direction.sqrMagnitude > squareDistance)
		{
			_rigidbody.MovePosition(_rigidbody.position + (direction.normalized * _speed * Time.fixedDeltaTime));
			direction = _target - _rigidbody.position;
			yield return WAIT_PHYSICS_THREAD;
		}

		_rigidbody.MovePosition(_target);
		if(onMovementEnds != null) onMovementEnds();
	}

	/// <summary>Interpolates Transform's Rotation in to desired rotation.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="_rotation">Desired rotation.</param>
	/// <param name="_duration">Interpolation's duration.</param>
	/// <param name="onRotationEnds">Optional callback invoked when the rotation ends.</param>
	public static IEnumerator PivotToRotation(this Transform _transform, Quaternion _rotation, float _duration, Action onRotationEnds = null)
	{
		Quaternion originalRotation = _transform.rotation;
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			_transform.rotation = Quaternion.Lerp(originalRotation, _rotation, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		if(onRotationEnds != null) onRotationEnds();
	}

	/// <summary>Rotates Transform around given axis, at a fixed time step.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="_axis">Relative to what axis to rotate.</param>
	/// <param name="_rotation">Rotation step given at each frame.</param>
	/// <param name="_rotation">Rotation's duration.</param>
	/// <param name="_space">Relative to which space to rotate.</param>
	/// <param name="onRotationEnds">Optional Callback invoked when the rotation ends.</param>
	public static IEnumerator RotateOnAxis(this Transform _transform, Vector3 _axis, float _rotation, float _duration, Space _space = Space.Self, Action onRotationEnds = null)
	{
		float rotationSplit = (_rotation / _duration);
		float n = 0.0f;

		while(n < (1.0f + Mathf.Epsilon))
		{
			_transform.Rotate(_axis, rotationSplit * Time.deltaTime, _space);
			n += Time.deltaTime;
			yield return null;
		}

		if(onRotationEnds != null) onRotationEnds();
	}

	/// <summary>Rotates Tranform by given Vector, at a fixed time step.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="_rotationVector">Rotation vector to add to this Transform's rotation each frame.</param>
	/// <param name="_duration">Rotation's duration.</param>
	/// <param name="_space">Relative to which space to rotate.</param>
	/// <param name="onRotationEnds">Optional Callback invoked when the rotation ends.</param>
	public static IEnumerator RotateVector3(this Transform _transform, Vector3 _rotationVector, float _duration, Space _space = Space.Self, Action onRotationEnds = null)
	{
		Vector3 rotationSplit = ((_rotationVector * Time.deltaTime) / _duration);
		float n = 0.0f;

		while(n < (1.0f + Mathf.Epsilon))
		{
			_transform.Rotate(rotationSplit, _space);
			n += Time.deltaTime;
			yield return null;
		}

		if(onRotationEnds != null) onRotationEnds();
	}

	/// <summary>Scales given Transform by a regular Vector of the given value at a duration, invokes an optional callback when finished scaling.</summary>
	/// <param name="_transform">Transform to scale.</param>
	/// <param name="_scaleNormal">Value that will define the regular vector this transform will be scaled to.</param>
	/// <param name="_duration">Scaling's duration.</param>
	/// <param name="onScaleEnds">Optional Callback invoked when the scaling ends.</param>
	public static IEnumerator RegularScale(this Transform _transform, float _scaleNormal, float _duration, Action onScaleEnds = null)
	{
		Vector3 originalScale = _transform.localScale;
		Vector3 destinyScale = VVector3.Regular(_scaleNormal);
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			_transform.localScale = Vector3.Lerp(originalScale, destinyScale, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		if(onScaleEnds != null) onScaleEnds();
	}

	/// <summary>Scales given Transform by given Vector3 at a duration, invokes an optional callback when finished scaling.</summary>
	/// <param name="_transform">Transform to scale.</param>
	/// <param name="_scaleVector">Vector this transform will be scaled to.</param>
	/// <param name="_duration">Scaling's duration.</param>
	/// <param name="onScaleEnds">Optional Callback invoked when the scaling ends.</param>
	public static IEnumerator IrregularScale(this Transform _transform, Vector3 _scaleVector, float _duration, Action onScaleEnds = null)
	{
		Vector3 originalScale = _transform.localScale;
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			_transform.localScale = Vector3.Lerp(originalScale, _scaleVector, t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		if(onScaleEnds != null) onScaleEnds();
	}

	/// <summary>Shakes Transform's position.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_duration">Shake's Duration.</param>
	/// <param name="_speed">Sake's Speed.</param>
	/// <param name="_magnitude">Shake's Magnitude.</param>
	/// <param name="onShakeEnds">Action invoked when the shaking ends.</param>
	public static IEnumerator ShakePosition(this Transform _transform, float _duration, float _speed, float _magnitude, Action onShakeEnds = null)
	{
		Vector3 originalPosition = _transform.localPosition;
		float elapsedTime = 0.0f;
		float scaledSpeed = 0.0f;

		while((elapsedTime < (_duration + Mathf.Epsilon)) && (_transform != null))
		{
			scaledSpeed = (Time.time * _speed);

			_transform.localPosition = originalPosition.WithAddedXAndY
			(
				((Mathf.PerlinNoise(scaledSpeed, 0.0f) * _magnitude) - (_magnitude * 0.5f)),
				((Mathf.PerlinNoise(0.0f, scaledSpeed) * _magnitude) - (_magnitude * 0.5f))
			);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_transform.localPosition = originalPosition;
		if(onShakeEnds != null) onShakeEnds();
	}

	/// <summary>Shakes Transform's rotation.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_duration">Shake's Duration.</param>
	/// <param name="_speed">Sake's Speed.</param>
	/// <param name="_magnitude">Shake's Magnitude.</param>
	/// <param name="onShakeEnds">Action invoked when the shaking ends.</param>
	public static IEnumerator ShakeRotation(this Transform _transform, float _duration, float _speed, float _magnitude, Action onShakeEnds = null)
	{
		Vector3 originalEulerRotation = _transform.localRotation.eulerAngles;
		float elapsedTime = 0.0f;
		float scaledSpeed = 0.0f;

		while((elapsedTime < (_duration + Mathf.Epsilon)) && (_transform != null))
		{
			scaledSpeed = (Time.time * _speed);

			_transform.localRotation = Quaternion.Euler(originalEulerRotation + new Vector3(
				((Mathf.PerlinNoise(scaledSpeed, 0.0f) * _magnitude) - (_magnitude * 0.5f)),
				((Mathf.PerlinNoise(0.0f, scaledSpeed) * _magnitude) - (_magnitude * 0.5f)),
				((Mathf.PerlinNoise(0.5f, (scaledSpeed * 0.5f)) * _magnitude) - (_magnitude * 0.5f))));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		_transform.localRotation = Quaternion.Euler(originalEulerRotation);
		if(onShakeEnds != null) onShakeEnds();
	}

	/// <summary>Does actions taking a normalized time t.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_duration">Normlaized Time's duration.</param>
	/// <param name="action">Action taking the normalized time t each frame.</param>
	/// <param name="onActionEnds">Optional callbakc invoked when the normalized time reaches 1.0f.</param>
	public static IEnumerator DoOnNormalizedTime(this MonoBehaviour _monoBehaviour, float _duration, Action<float> action, Action onActionEnds = null)
	{
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;

		while(t < (1.0f + Mathf.Epsilon))
		{
			action(t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		if(t != 1.0f) action(1.0f);
		if(onActionEnds != null) onActionEnds();
	}

	/// <summary>Changes multiuple Material colors simultaneously at a given duration.</summary>
	/// <param name="_material">Target Material.</param>
	/// <param name="_duration">Color change duration.</param>
	/// <param name="onChangeEnds">Optional callback to invoke when the color changing ends.</param>
	/// <param name="_colorTuples">Tuples that contains both the Material tag and its new destiny color.</param>
	public static IEnumerator ChangeColors(this Material _material, float _duration, Action onChangeEnds = null, params ValueTuple<MaterialTag, Color>[] _colorTuples)
	{
		float t = 0.0f;
		float inverseDuration = 1.0f / _duration;
		Color[] colors = new Color[_colorTuples.Length];

		for(int i = 0; i < colors.Length; i++)
		{
			colors[i] = _material.GetColor(_colorTuples[i].value1);
		}

		while(t < 1.0f)
		{
			for(int i = 0; i < colors.Length; i++)
			{
				ValueTuple<MaterialTag, Color> tuple = _colorTuples[i];
				Color color = Color.Lerp(colors[i], tuple.value2, t);

				_material.SetColor(tuple.value1, color);
			}

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		for(int i = 0; i < colors.Length; i++)
		{
			ValueTuple<MaterialTag, Color> tuple = _colorTuples[i];
			_material.SetColor(tuple.value1, tuple.value2);
		}

		if(onChangeEnds != null) onChangeEnds();
	}

	/// <summary>Oscilates Renderer's Material Main Color between its original and a desired color, interpolating back and forth.</summary>
	/// <param name="_renderer">Renderer to apply the Colro oscillation effect.</param>
	/// <param name="_color">Desired Color.</param>
	/// <param name="_duration">Oscillation process's duration.</param>
	/// <param name="_cycles">Number of back and forth cycles during the oscillation.</param>
	/// <param name="_propertyTag">Property tag referrinf to the color ["_Color" as default].</param>
	/// <param name="onColorOscillation">Optional callback invoked when the effect ends.</param>
	public static IEnumerator OscillateRendererMainColor(this Renderer _renderer, Color _color, float _duration, float _cycles, string _propertyTag = "_Color", Action onColorOscillationEnds = null)
	{
		FloatRange sinRange = new FloatRange(-1.0f, 1.0f);
		int propertyID = Shader.PropertyToID(_propertyTag);
		Color originalColor = _renderer.material.GetColor(propertyID);
		Color newColor = new Color(0f, 0f, 0f, 0f);
		float inverseDuration = 1.0f / _duration;
		float t = 0.0f;
		float x = (360f * _cycles * Mathf.Deg2Rad);

		while(t < (1.0f + Mathf.Epsilon))
		{
			newColor = Color.Lerp(originalColor, _color, VMath.RemapValueToNormalizedRange(Mathf.Sin(t * x), sinRange));
			_renderer.material.SetColor(propertyID, newColor);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_renderer.material.SetColor(propertyID, originalColor);
		if(onColorOscillationEnds != null) onColorOscillationEnds();
	}
#endregion

}
}