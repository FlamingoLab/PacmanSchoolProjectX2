using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct CollectionIndex
{
	public int index; 			/// <summary>Collection's Index.</summary>
#if UNITY_EDITOR
	public int objectIndex; 		/// <summary>Persistent Object's Index.</summary>
	public int collectionIndex; 	/// <summary>Collection's Index.</summary>
#endif

	/// <summary>Implicit CollectionIndex to int operator.</summary>
	public static implicit operator int(CollectionIndex _collectionIndex) { return _collectionIndex.index; }

	/// <summary>Implicit int to CollectionIndex operator.</summary>
	public static implicit operator CollectionIndex(int _index) { return new CollectionIndex(_index); }

	/// <summary>CollectionIndex's Constructor.</summary>
	/// <param name="_index">Collection's Index.</param>
	public CollectionIndex(int _index) : this()
	{
		index = _index;
	}
}
}