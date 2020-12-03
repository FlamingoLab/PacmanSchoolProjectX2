using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{

public enum Format
{
	Normal = 1,
	Bold = 2,
	Italic = 4,

	BoldAndItalic = Bold | Italic,
}

public static class VDebug
{
	/// <summary>Debugs Message on given Format and Color.</summary>
	/// <param name="_message">Message to debug to the console.</param>
	/// <param name="_format">Text's Format [Normal by default].</param>
	/// <param name="_color">Text's Color [Black by default].</param>
	public static void Log(string _message, Format _format = Format.Normal, Color _color = default(Color))
	{
		StringBuilder builder = new StringBuilder();
		if(_color == default(Color)) _color = Color.black;

		builder.Append("<color=");
		builder.Append(ColorUtility.ToHtmlStringRGBA(_color));
		builder.Append(">");
		if((_format | Format.Bold) == _format) builder.Append("<b>");
		if((_format | Format.Italic) == _format) builder.Append("<i>");
		builder.Append(_message);
		if((_format | Format.Bold) == _format) builder.Append("</b>");
		if((_format | Format.Italic) == _format) builder.Append("</i>");
		builder.Append("</color>");

		Debug.Log(builder.ToString());
	}
}
}