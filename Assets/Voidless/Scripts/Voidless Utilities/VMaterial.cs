using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VMaterial
{
	public static readonly MaterialTag MATERIAL_TAG_COLOR;
	public static readonly MaterialTag MATERIAL_TAG_ALBEDO;

	static VMaterial()
	{
		MATERIAL_TAG_COLOR = "_Color";
		MATERIAL_TAG_ALBEDO = "_Albedo";
	}
}
}