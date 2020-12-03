using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VDictionary
{
#region Dictionaries:
	/// <summary>Initializes Dictionary from Collection of SerializableKeyValuePair of the same KeyValuePair.</summary>
	/// <param name="_dictionary">Dictionary to initialize.</param>
	/// <param name="_SerializableKeyValuePairs">Collection of SerializableKeyValuePair of the same KeyValuePair as the SerializableKeyValuePair.</param>
	/// <param name="onInitializationEnds">[Optional] Action to invoke after the Initialization ends.</param>
	/// <returns>Initialized Dictionary.</returns>
	public static Dictionary<T, U> InitializeFrom<T, U>(this Dictionary<T, U> _dictionary, List<SerializableKeyValuePair<T, U>> _SerializableKeyValuePairs, System.Action onInitializationEnds)
	{
		_dictionary = new Dictionary<T, U>();

		for(int i = 0; i < _SerializableKeyValuePairs.Count; i++)
		{
			_dictionary.Add(_SerializableKeyValuePairs[i].key, _SerializableKeyValuePairs[i].value);
		}

		if(onInitializationEnds != null) onInitializationEnds();

		return _dictionary;
	}

	/// <summary>Initializes Dictionary from Collection of SerializableKeyValuePair of the same KeyValuePair.</summary>
	/// <param name="_dictionary">Dictionary to initialize.</param>
	/// <param name="_SerializableKeyValuePairs">Collection of SerializableKeyValuePair of the same KeyValuePair as the SerializableKeyValuePair.</param>
	/// <param name="onInitializationEnds">[Optional] Action to invoke after the Initialization ends.</param>
	/// <returns>Initialized Dictionary.</returns>
	public static Dictionary<T, U> InitializeFrom<T, U>(this Dictionary<T, U> _dictionary, SerializableKeyValuePair<T, U>[] _SerializableKeyValuePairs, System.Action onInitializationEnds)
	{
		_dictionary = new Dictionary<T, U>();

		for(int i = 0; i < _SerializableKeyValuePairs.Length; i++)
		{
			_dictionary.Add(_SerializableKeyValuePairs[i].key, _SerializableKeyValuePairs[i].value);
		}

		if(onInitializationEnds != null) onInitializationEnds();

		return _dictionary;
	}
#endregion
}
}