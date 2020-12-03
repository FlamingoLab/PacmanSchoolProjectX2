using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public abstract class BaseControllerSetup<T>
{
	protected const string ASSET_MENU_PATH = "Controller Setups / "; 	/// <summary>Controller Setup creation path.</summary>

#region Properties:
	[SerializeField] private T[] _keyMapping; 							/// <summary>Input Controller Key's mappings.</summary>
#endregion

#region Getters:
	/// <summary>Gets and Sets keyMapping property.</summary>
	public T[] keyMapping
	{
		get { return _keyMapping; }
		set { _keyMapping = value; }
	}

	/// <summary>Gets leftAxisX property.</summary>
	public abstract float leftAxisX { get; }

	/// <summary>Gets leftAxisY property.</summary>
	public abstract float leftAxisY { get; }

	/// <summary>Gets rightAxisX property.</summary>
	public abstract float rightAxisX { get; }

	/// <summary>Gets rightAxisY property.</summary>
	public abstract float rightAxisY { get; }

	/// <summary>Gets leftTrigger property.</summary>
	public abstract float leftTrigger { get; }

	/// <summary>Gets rightTrigger property.</summary>
	public abstract float rightTrigger { get; }	

	/// <summary>Gets dPadAxisX property.</summary>
	public abstract float dPadAxisX { get; }

	/// <summary>Gets dPadAxisY property.</summary>
	public abstract float dPadAxisY { get; }
#endregion

	/// <summary>BaseControllerSetup constructor.</summary>
	public BaseControllerSetup(int _size = 1)
	{
		keyMapping = new T[_size];
	}
	
	/// <summary>Resizes Keys' Mapping.</summary>	
	/// <param name="_newSize">Mapping's New Size.</param>
	public void ResizeMapping(int _newSize)
	{
		Array.Resize(ref _keyMapping, _newSize);
	}

	/// <returns>String representing this Controller's Setup.</returns>
	public override string ToString()
	{
		if(keyMapping != null && keyMapping.Length > 0)
		{
			StringBuilder builder = new StringBuilder();
			int index = 0;

			builder.AppendLine("Mapped Keys: ");
			builder.AppendLine();

			foreach(T key in keyMapping)
			{
				builder.Append("Key with ID #");
				builder.Append(index.ToString());
				builder.Append(": ");
				builder.AppendLine(key.ToString());
				index++;
			}

			return builder.ToString();

		} else return "No Mapped Keys on this Controller's Setup.";
	}
}
}