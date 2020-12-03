using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Flags]
public enum Easings
{
	Linear = 0, 																		/// <summary>Linear Easing.</summary>
	EaseIn = 1, 																		/// <summary>Ease-In Easing.</summary>
	EaseOut = 2, 																		/// <summary>Ease-Out Easing.</summary>
	Arc = 4, 																			/// <summary>Arc Easing.</summary>
	Sigmoid = 8, 																		/// <summary>Sigmoid Easing.</summary>

	EaseInOut = EaseIn | EaseOut 														/// <summary>Ease-In-Out Easing.</summary>
}

public enum CoordinatesModes 															/// <summary>Coordinates Modes.</summary>
{
	XY, 																				/// <summary>X and Y Coordinate Mode.</summary>
	YX, 																				/// <summary>Y and X Coordinate Mode.</summary>
	XZ, 																				/// <summary>X and Z Coordinate Mode.</summary>
	ZY, 																				/// <summary>Z and Y Coordinate Mode.</summary>
	YZ, 																				/// <summary>Y and Z Coordinate Mode.</summary>
	ZX 																					/// <summary>Z and X Coordinate Mode.</summary>
}

/// <summary>Normalized Property parametric function.</summary>
/// <param name="t">Time, normalized between -1f and 1f.</param>
public delegate float NormalizedPropertyFunctionOC(float t, float x = 0.0f);

public delegate float ParameterizedNormalizedPropertyFunctionOC(float t, float x);

public static class VMath
{
	public const float PI = 3.1415926535897932384626433832795028841971693993751058f; 	/// <summary>Pi's Constant.</summary>
	public const float PHI = 1.61803398874989484820458683436563811772030917980576f; 	/// <summary>Golden Ratio Constant.</summary>
	public const float E = 2.71828182845904523536028747135266249775724709369995f; 		/// <summary>Euler's Number Constant</summary>
	public const float DEG_TO_RAD = PI / 180.0f; 										/// <summary>Degrees to Radians' conversion.</summary>
	public const float RAD_TO_DEG = 180.0f / PI; 										/// <summary>Radians to Degrees' conversion</summary>

	public static readonly float[] sinTable; 											/// <summary>Sine's Lookup Table.</summary>
	public static readonly float[] cosTable; 											/// <summary>Cosine's Lookup Table.</summary>
	public static readonly float[] tanTable; 											/// <summary>Tangents's Lookup Table.</summary>

	/// <summary>VMath's Static Constructor.</summary>
	static VMath()
	{
		sinTable = new float[181];
		cosTable = new float[181];
		tanTable = new float[181];

		InitializeSinLookupTable();
		InitializeCosLookupTable();
		InitializeTanLookupTable();
	}

#region TrigonometricLookupTables:
	/// <summary>Initializes Sine's Lookup Table.</summary>
	private static void InitializeSinLookupTable()
	{
		sinTable[0] = 1.0f;
		sinTable[1] = 0.0175f; 		sinTable[62] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[2] = 0.0175f; 		sinTable[63] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[3] = 0.0175f; 		sinTable[64] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[4] = 0.0175f; 		sinTable[65] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[5] = 0.0175f; 		sinTable[66] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[6] = 0.0175f; 		sinTable[67] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[7] = 0.0175f; 		sinTable[68] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[8] = 0.0175f; 		sinTable[69] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[9] = 0.0175f; 		sinTable[70] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[10] = 0.0175f; 	sinTable[71] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[11] = 0.0175f; 	sinTable[72] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[12] = 0.0175f; 	sinTable[73] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[13] = 0.0175f; 	sinTable[74] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[14] = 0.0175f; 	sinTable[75] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[15] = 0.0175f; 	sinTable[76] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[16] = 0.0175f; 	sinTable[77] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[17] = 0.0175f; 	sinTable[78] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[18] = 0.0175f; 	sinTable[79] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[19] = 0.0175f; 	sinTable[80] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[20] = 0.0175f; 	sinTable[81] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[21] = 0.0175f; 	sinTable[82] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[22] = 0.0175f; 	sinTable[83] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[23] = 0.0175f; 	sinTable[84] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[24] = 0.0175f; 	sinTable[85] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[25] = 0.0175f; 	sinTable[86] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[26] = 0.0175f; 	sinTable[87] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[27] = 0.0175f; 	sinTable[88] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[28] = 0.0175f; 	sinTable[89] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[29] = 0.0175f; 	sinTable[90] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[30] = 0.0175f; 	sinTable[91] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[31] = 0.0175f; 	sinTable[92] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[32] = 0.0175f; 	sinTable[93] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[33] = 0.0175f; 	sinTable[94] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[34] = 0.0175f; 	sinTable[95] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[35] = 0.0175f; 	sinTable[96] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[36] = 0.0175f; 	sinTable[97] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[37] = 0.0175f; 	sinTable[98] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[38] = 0.0175f; 	sinTable[99] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[39] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[40] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[41] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[42] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[43] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[44] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[45] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[46] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[47] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[48] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[49] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[51] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[52] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[53] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[54] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[55] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[56] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[57] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[58] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[59] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[60] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[61] = 0.0175f; 	sinTable[1] = 0.0175f; 	sinTable[1] = 0.0175f;
		sinTable[90] = 0.0f;
	}

	/// <summary>Initializes Cosine's Lookup Table.</summary>
	private static void InitializeCosLookupTable()
	{
		cosTable[0] = 1.0f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f; 	cosTable[1] = 0.0175f;
		cosTable[90] = 0.0f;
	}

	/// <summary>Initializes Tangent's Lookup Table.</summary>
	private static void InitializeTanLookupTable()
	{
		tanTable[0] = 1.0f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f; 	tanTable[1] = 0.0175f;
		tanTable[90] = 0.0f;
	}
#endregion

#region NormalizedPropertyFunctionOCs:
	/*public static float Interpolate(float _initialPoint, float _finalPoint, float _t)
	{
		return _initialPoint + (_t * (_finalPoint - _initialPoint));
	}*/

	public static NormalizedPropertyFunctionOC GetEasing(Easings _easing)
	{
		switch(_easing)
		{
			case Easings.Linear: 	return null;
			case Easings.EaseIn: 	return EaseIn;
			case Easings.EaseOut: 	return EaseOut;
			case Easings.Arc: 		return Arc;
			case Easings.Sigmoid: 	return Sigmoid;
		}

		return null;
	}

	/// <summary>Calculates a number to a given exponential.</summary>
	/// <param name="t">Number to elevate to given exponent.</param>
	/// <param name="_exponential">Exponential to raise number to [2 by default].</param>
	/// <returns>Number raised to given exponential.</returns>
	public static float EaseIn(float t, float _exponential = 2.0f)
	{
		return (1.0f - Mathf.Abs(EaseOut(t - 1.0f, _exponential)));
	}

	/// <summary>Calculates a number to a given exponential.</summary>
	/// <param name="t">Number to elevate to given exponent.</param>
	/// <param name="_exponential">Exponential to raise number to [2 by default].</param>
	/// <returns>Number raised to the inverse of the given exponential.</returns>
	public static float EaseOut(float t, float _exponential = 2.0f)
	{
		if(_exponential == 0.0f) return 1.0f;
		else if(_exponential == 1.0f) return t;
		else return Power(t, _exponential);
	}

	public static float EaseInEaseOut(float t, float _exponential = 2.0f)
	{
		return Blend(EaseIn(t, _exponential), EaseOut(t, _exponential), t);
	}

	public static float EaseInEaseOut(float t, float _easeInExponential = 2.0f, float _easeOutExponential = 2.0f)
	{
		return Blend(EaseIn(t, _easeInExponential), EaseOut(t, _easeOutExponential), t);
	}

	public static float Blend(float a, float b, float weightB)
	{
		return a + (weightB * (b - a));
		/*
		X^2.2 = Blend(t*t, t*t*t, 0.2);
		*/
	}

	/// <summary>Raises a number to a given power.</summary>
	/// <param name="x">Number to raise.</param>
	/// <param name="p">Power to raise the number to.</param>
	/// <returns>Number raised to power.</returns>
	public static float PowerInteger(float x, int p)
	{
		if(p == 0) return x != 0.0f ? 1.0f : 0.0f;
		else if(p == 1.0f) return x;

		for(int i = 0; i < p - 1; i++)
		{
			x *= x;
		}

		return p > 1 ? x : (1.0f / x);
	}

	/// <summary>Raises a number to a given power.</summary>
	/// <param name="x">Number to raise.</param>
	/// <param name="p">Power to raise the number to.</param>
	/// <returns>Number raised to power.</returns>
	public static float Power(float x, float p)
	{
		if(p == 0.0f) return x != 0.0f ? 1.0f : 0.0f;
		else if(p == 1.0f) return x;

		int floorPower = (int)(p);
		float difference = p - floorPower;
		float result = PowerInteger(x, floorPower);

		return (difference > 0.0f) ? Blend(result, result * (p > 1 ? x : (1.0f / x)), difference) : result;
	}

	/// <summary>Scales a t function to t.</summary>
	/// <param name="function">Function evaluating t.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Evaluated t scaled to t.</returns>
	public static float Scale(Func<float, float> function, float t)
	{
		return t * function(t);
	}

	/// <summary>Inverts a scaled t function.</summary>
	/// <param name="function">Function evaluating t.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Function scaled to t inverted.</returns>
	public static float ReverseScale(Func<float, float> function, float t)
	{
		return  (1.0f - t) * function(t);
	}

	/// <summary>Calculates position of Arc of a given t.</summary>
	/// <param name="t">Current time.</param>
	/// <returns>Time relative to t value.</returns>
	public static float Arc(float t, float _x = 0.0f)
	{
		return (t * (1.0f - t));
	}

	/// <summary>Interpolates linearly an initial value to a final value, on a normalized time, following the formula P = P0 + t(Pf - P0).</summary>
	/// <param name="_initialPoint">Initial value [P0].</param>
	/// <param name="_finalPoint">Destiny value [Pf].</param>
	/// <param name="_time">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Interpolated value on given normalized time.</returns>
	public static float Lerp(float _initialPoint, float _finalPoint, float _time)
	{
		_time = Mathf.Clamp(_time, -1.0f, 1.0f);

		return _initialPoint + (_time * (_finalPoint - _initialPoint)); 
	}

	/// <summary>Evaluates sigmoid function by given x.</summary>
	/// <param name="_x">Number to evaluate.</param>
	/// <param name="e">Exponential, 'e' constant by default.</param>
	/// <returns>Sigmoid evaluation.</returns>
	public static float Sigmoid(float _x, float e = E)
	{
		return (1.0f / (1.0f + (1.0f / Mathf.Pow(e, _x))));
	}

	public static float FunctionsSumatories(float _t, params NormalizedPropertyFunctionOC[] _functions)
	{
		float proportion = (1 / _functions.Length);

		for(int i = 0; i < (_functions.Length - 1); i++)
		{
			if((_t >= (proportion * i)) && (_t < (proportion * (i + 1)))) return _functions[i](_t);
		}

		return _t;
	}
#endregion

#region TrigonometricFunctions:
	/// https://stackoverflow.com/questions/38917692/sin-cos-funcs-without-math-h

	/*/// sin(x) = x - (x^3)/3! + (x^5)/5! - (x^7)/7! .......
	public static float Sin(float d)
	{
		d *= DEG_TO_RAD;
		float x = d;

		for(float i = 1.0f; i < 7.0f; i++)
		{
			float di = (1 * 2.0f);
			float c = Power(x, di + 1.0f);

			c /= Factorial(di);
			i += ((int)(c) & 0x01) ? -c : c;
		}

		return x;
	}

	/// cos(x) = 1 - (x^2)/2! + (x^4)/4! - (x^6)/6! .......
	public static float Cos(float d)
	{
		d *= DEG_TO_RAD;
		float x = 1.0f;

		for(float i = 1.0f; i < 7.0f; i++)
		{
			float di = (1 * 2.0f);
			float c = Power(x, di);

			c /= Factorial(di - 1.0f);
			i += ((int)(c) & 0x01) ? -c : c;
		}

		return x;
	}*/
#endregion

	/// <summary>Calculates the spatial relationship of 2 given segments.</summary>
	/// <param name="aMin">Segment A's minimum value.</param>
	/// <param name="aMax">Segment A's maximum value.</param>
	/// <param name="bMin">Segment B's minimum value.</param>
	/// <param name="bMax">Segment B's maximum value.</param>
	/// <returns>Spatial relationship of 2 segments A and B.</returns>
	public static SpatialRelationship Get1DSpatialRelationship(float aMin, float aMax, float bMin, float bMax)
	{
		if(bMin > aMin && bMax < aMax) return SpatialRelationship.AContainsB;
		if(aMin > bMin && aMax < bMax) return SpatialRelationship.BContainsA;
		if((aMin <= bMin && aMax <= bMax) || (bMin <= aMin && bMax <= aMax)) return SpatialRelationship.Intersection;

		float minAMaxBMax = Mathf.Min(aMax, bMax);
		float maxAMinBMin = Mathf.Max(aMin, bMin);

		if(minAMaxBMax < maxAMinBMin) return SpatialRelationship.NonIntersection;
		if(minAMaxBMax == maxAMinBMin) return SpatialRelationship.Contact;

		Debug.LogWarning("[VMath] Spatial Relationship's calculations went wrong, returning undefined relationship...");
		return SpatialRelationship.Undefined;
	}

	/// <summary>Gets size of segment that fits two given segments.</summary>
	/// <param name="aMin">Segment A's minimum value.</param>
	/// <param name="aMax">Segment A's maximum value.</param>
	/// <param name="bMin">Segment B's minimum value.</param>
	/// <param name="bMax">Segment B's maximum value.</param>
	/// <returns>Size of segment that fits 2 segments A and B.</returns>
	public static float GetSizeToFitSegments(float aMin, float aMax, float bMin, float bMax)
	{
		float min = Mathf.Min(aMin, bMin);
		float max = Mathf.Max(aMax, bMax);

		return max - min;

		/* All of this was unnecesary, though I'll keep it since it was a fun exercise to 
		leave in vain...

		Nonetheless, both methods reach the same results...*/

		SpatialRelationship relationship = Get1DSpatialRelationship(aMin, aMax, bMin, bMax);
		float sizeA = aMax - aMin;
		float sizeB = bMax - bMin;

		switch(relationship)
		{
			case SpatialRelationship.AContainsB:
			case SpatialRelationship.BContainsA:
			return Mathf.Max(sizeA, sizeB);

			case SpatialRelationship.Intersection:
			return (sizeA + sizeB) - (Mathf.Min(aMax, bMax) - Mathf.Max(aMin, bMin));

			case SpatialRelationship.NonIntersection:
			return (sizeA + sizeB) + (Mathf.Max(aMin, bMin) - Mathf.Min(aMax, bMax));

			case SpatialRelationship.Contact:
			return sizeA + sizeB;

			default:
			Debug.LogWarning("[VMath] Something went wring, returning '0.0f'...");
			return 0.0f;
		}
	}

	/// <summary>Gets size of segment that fits two given segments [both with minimum value at '0'].</summary>
	/// <param name="sizeA">Size of Segment A.</param>
	/// <param name="sizeB">Size of Segment B.</param>
	/// <returns>Size of segment that fits 2 segments A and B.</returns>
	public static float GetSizeToFitSegments(float sizeA, float sizeB)
	{
		return GetSizeToFitSegments(0.0f, sizeA, 0.0f, sizeB);
	}

	/// <summary>Calculates segment [represented as Float Range] that better fits 2 given segments A and B.</summary>
	/// <param name="aMin">Segment A's minimum value.</param>
	/// <param name="aMax">Segment A's maximum value.</param>
	/// <param name="bMin">Segment B's minimum value.</param>
	/// <param name="bMax">Segment B's maximum value.</param>
	/// <returns>Segment that fits segments A and B [as FloatRange].</returns>
	public static FloatRange GetSegmentThatFitsPair(float aMin, float aMax, float bMin, float bMax)
	{
		return new FloatRange(Mathf.Min(aMin, bMin), Mathf.Max(aMax, bMax));
	}

	/// <summary>Calculates AABBs that fit into a pair of Bounds.</summary>
	/// <param name="a">A's Bounds.</param>
	/// <param name="b">B's Bounds.</param>
	/// <returns>Bounds' that fits into pair of bounds.</returns>
	public static Bounds GetBoundsToFitPair(Bounds a, Bounds b)
	{
		Vector3 center = new Vector3(
			GetMedian(Mathf.Min(a.min.x, b.min.x), Mathf.Max(a.max.x, b.max.x)),
			GetMedian(Mathf.Min(a.min.y, b.min.y), Mathf.Max(a.max.y, b.max.y)),
			GetMedian(Mathf.Min(a.min.z, b.min.z), Mathf.Max(a.max.z, b.max.z))
		);
		Vector3 size = new Vector3(
			VMath.GetSizeToFitSegments(a.min.x, a.max.x, b.min.x, b.max.x),
			VMath.GetSizeToFitSegments(a.min.y, a.max.y, b.min.y, b.max.y),
			VMath.GetSizeToFitSegments(a.min.z, a.max.z, b.min.z, b.max.z)
		);

		return new Bounds(center, size);
	}

	/// <summary>Calculates the Rule of 3 given three values A, B and C (A -> C; B -> X).</summary>
	/// <param name="a">Value A (Extreme).</param>
	/// <param name="b">Value B (Mean).</param>
	/// <param name="c">Value C (Mean).</param>
	/// <returns>Rule of 3 between given values.</returns>
	public static float RuleOf3(float a, float b, float c)
	{
		return (b * c) / a;
	}

	/// \TODO The formula fails when calculating the quotient, since it is multiplicating the divisor's inverse.
	/// <summary>Calculates the Modulo of X and Y, given that Y is a multiplicative inverse.</summary>
	/// <param name="x">Dividend value.</param>
	/// <param name="y">Multiplicative inverse of a divisor.</param>
	/// <returns>Modulo of X mod Y.</returns>
	public static float InverseDivisorMod(float x, float y)
	{
		float q = Mathf.Floor(x * y);
		return x - (q * y);
	}

	/// <summary>Calculates the Logarithm of an odd function.</summary>
	/// <param name="x">Variable to calculate the Logot.</param>
	/// <param name="b">Base [2 by default].</param>
	/// <returns>Logot function.</returns>
	public static float Logot(float x, float b = 2.0f)
	{
		return Log((x / (1.0f - x)), b);
	}

	/// <summary>Calculates the logarithm of a given number.</summary>
	/// <param name="x">Number to calculate logarithm.</param>
	/// <param name="b">Logarithm's base [2 by default].</param>
	/// <returns>Logarithm of number.</returns>
	public static float Log(float x, float b = 2.0f)
	{
		float count = 0.0f;

		while(x > (b - 1.0f))
		{
			x /= b;
			count++;
		}

		return count;
	}

#region Vector3Utilities:
	/// <summary>Calculates angle between 2 vectors around given axis.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <param name="axis">Axis.</param>
	/// <returns>Signed angle around given axis.</returns>
	public static float AnlgeAroundAxis(Vector3 a, Vector3 b, Vector3 axis)
	{
		a = a - Vector3.Project(a, axis);
		b = b - Vector3.Project(b, axis);

		Vector3 cross = Vector3.Cross(a, b);
		float angle = Vector3.Angle(a, b);

		return angle * (Vector3.Dot(axis, cross) > 0.0f ? -1.0f : 1.0f);
	}

	/// <summary>Interpolates linearly an initial Vector3 to a final Vector3, on a normalized time, following the formula P = P0 + t(Pf - P0).</summary>
	/// <param name="_initialPoint">Initial Vector3 [P0].</param>
	/// <param name="_finalPoint">Destiny Vector3 [Pf].</param>
	/// <param name="_time">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Interpolated Vector3 on given normalized time.</returns>
	public static Vector3 Lerp(Vector3 _initialPoint, Vector3 _finalPoint, float _time)
	{
		_time = Mathf.Clamp(_time, -1.0f, 1.0f);

		return _initialPoint + (_time * (_finalPoint - _initialPoint)); 
	}

	public static Vector3 SoomthStartN(Vector3 _initialPoint, Vector3 _finalPoint, int _exponential, float _time)
	{
		_time = Mathf.Clamp(_time, -1.0f, 1.0f);

		return _initialPoint + (Mathf.Pow(_time, _exponential) * (_finalPoint - _initialPoint));
	}

	public static Vector3 SoomthEndN(Vector3 _initialPoint, Vector3 _finalPoint, int _exponential, float _time)
	{
		_time = Mathf.Clamp(_time, -1.0f, 1.0f);

		return _initialPoint + ((1f - (1f - Mathf.Pow(_time, _exponential))) * (_finalPoint - _initialPoint));
	}

	/// <summary>Calculates a Linear Beizer Curve point relative to the time, following the formula [B(t) = (1-t)P0 + tPf].</summary>
	/// <param name="_initialPoint">Initial value [P0].</param>
	/// <param name="_finalPoint">Destiny value [Pf].</param>
	/// <param name="_time">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Linear Beizer Curve point relative to given normalized time.</returns>
	public static Vector3 LinearBeizer(Vector3 _initialPoint, Vector3 _finalPoint, float _time)
	{
		_time = Mathf.Clamp(_time, -1.0f, 1.0f);

		return ((1.0f - _time) * _initialPoint) + (_time * _finalPoint);
	}

	/// \TODO Clean the following Beizer Curve functions to a formula that doesn't call Linear Beizer n times.
	/// <summary>Calculates a Cuadratic Beizer Curve point relative to the time, following the formula [B(P0,P1,P2,t) = (1-t)B(P0,P1,t) + tB(P1,P2,t)].</summary>
	/// <param name="_initialPoint">Initial value [P0].</param>
	/// <param name="_finalPoint">Destiny value [Pf].</param>
	/// <param name="_tangent">Tanget vector between initialPoint and finalPoint [P1].</param>
	/// <param name="_time">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Cuadratic Beizer Curve point relative to given normalized time.</returns>
	public static Vector3 CuadraticBeizer(Vector3 _initialPoint, Vector3 _finalPoint, Vector3 _tangent, float _time)
	{
		_time = Mathf.Clamp(_time, -1.0f, 1.0f);

		return LinearBeizer(LinearBeizer(_initialPoint, _tangent, _time), LinearBeizer(_tangent, _finalPoint, _time), _time);
	}

	/// <summary>Calculates a Cubic Beizer Curve point relative to the time, following the formula [B(P0,P1,P2,t) = (1-t)B(P0,P1,t) + tB(P1,P2,t)].</summary>
	/// <param name="_initialPoint">Initial value [P0].</param>
	/// <param name="_finalPoint">Destiny value [Pf].</param>
	/// <param name="_startTangent">First tanget vector between initialPoint and finalPoint [P1].</param>
	/// <param name="_endTangent">Second tangent vector petween initialPoint and finalPoint [P2].</param>
	/// <param name="_time">Normalized t, clamped internally between -1 and 1.</param>
	/// <returns>Cubic Beizer Curve point relative to given normalized time.</returns>
	public static Vector3 CubicBeizer(Vector3 _initialPoint, Vector3 _finalPoint, Vector3 _startTangent, Vector3 _endTangent, float _time)
	{
		_time = Mathf.Clamp(_time, -1.0f, 1.0f);

		return LinearBeizer(CuadraticBeizer(_initialPoint, _startTangent, _endTangent, _time), CuadraticBeizer(_startTangent, _endTangent, _finalPoint, _time), _time);
	}

	/// <summary>Gets middle point between n number of points (positions).</summary>
	/// <param name="_points">The points from where the middle point will be calculated.</param>
	/// <returns>Middle point between n points.</returns>
	public static Vector3 GetMiddlePointBetween(params Vector3[] _points)
	{
		Vector3 middlePoint = Vector3.zero;

		for(int i = 0; i < _points.Length; i++)
		{
			middlePoint += _points[i];
		}

		return (middlePoint / _points.Length);
	}

	/// <summary>Gets normalized point between n number of points (positions).</summary>
	/// <param name="_normalizedValue">The normal of the points sumatory.</param>
	/// <param name="_points">The points from where the middle point will be calculated.</param>
	/// <returns>Normalized point between n points.</returns>
	public static Vector3 GetNormalizedPointBetween(float _normalizedValue, params Vector3[] _points)
	{
		Vector3 middlePoint = Vector3.zero;

		for(int i = 0; i < _points.Length; i++)
		{
			middlePoint += _points[i];
		}

		return (middlePoint * _normalizedValue.Clamp(-1.0f, 1.0f));
	}

	public static float DotProductAngle(Vector3 a, Vector3 b)
	{
		return Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude));
	}

	/// <summary>Gets interpoilation's t, given an output.</summary>
	/// <param name="_initialInput">Interpolation's initial value [P0].</param>
	/// <param name="_finalInput">Interpolation's final value [Pf].</param>
	/// <param name="_output">Interpolation's Output.</param>
	/// <returns>T deducted from the given output and original interpolation's data.</returns>
	public static float GetInterpolationTime(float _initialInput, float _finalInput, float _output)
	{
		return ((_output - _initialInput) / (_finalInput - _initialInput));
	}
#endregion

#region Ray2DOperations:
	/// <summary>Calculates a 2X2 determinant, given two bidimensional Rays.</summary>
	/// <param name="_rayA">Ray A.</param>
	/// <param name="_rayB">Ray B.</param>
	/// <returns>2X2's determinant of Ray A and Ray B.</returns>
	public static float Determinant(Ray2D _rayA, Ray2D _rayB)
	{
		return ((_rayA.direction.y * _rayB.direction.x) - (_rayA.direction.x * _rayB.direction.y));
		//return ((_rayB.direction.x * _rayA.direction.y) - (_rayB.direction.y * _rayA.direction.x));
	}

	/// <summary>Interpolates ray towards direction, given a time t.</summary>
	/// <param name="_ray">Ray to interpolate.</param>
	/// <param name="t">Time reference.</param>
	/// <returns>Interpolation between Ray's origin and direction on t time, as a Vector2.</returns>
	public static Vector2 Lerp(this Ray2D _ray, float t)
	{
		return (_ray.origin + (t * _ray.direction));
	}

	/// <summary>Calculates for intersection between Ray A and B.</summary>
	/// <param name="_rayA">Ray A.</param>
	/// <param name="_rayB">Ray B.</param>
	/// <returns>Intersection between Rays A and B if there is, null otherwise.</returns>
	public static Vector2? CalculateIntersectionBetween(Ray2D _rayA, Ray2D _rayB)
	{
		float determinant = Determinant(_rayA, _rayB);
		if(determinant == 0.0f) return null;
		float determinantMultiplicativeInverse = (1.0f / determinant);
		float deltaX = (_rayA.origin.x - _rayB.origin.x);
		float deltaY = (_rayB.origin.y - _rayA.origin.y);
		float tA = ((deltaY * _rayB.direction.x) + (deltaX * _rayB.direction.y)) * determinantMultiplicativeInverse;
		float tB = ((deltaY * _rayA.direction.x) + (deltaX * _rayA.direction.y)) * determinantMultiplicativeInverse;

		return (tA >= 0.0f && tB >= 0.0f) ? _rayA.Lerp(tA) : (Vector2?)null;
	}
#endregion

#region RandomOperations:
	/// <summary>Gets a unique [not duplicate] set of random integers.</summary>
	/// <param name="_range">Random's Range [max gets excluded].</param>
	/// <param name="_count">Size of the set.</param>
	/// <returns>Set of random sorted unique integers.</returns>
	public static int[] GetUniqueRandomSet(Range<int> _range, int _count)
	{
		HashSet<int> numbersSet = new HashSet<int>();

		for(int i = (_range.max - _count); i < _range.max; i++)
		{
			if(!numbersSet.Add(UnityEngine.Random.Range(_range.min, (i + 1))))
			numbersSet.Add(i);
		}

		int[] result = numbersSet.ToArray();

		for(int i = (result.Length - 1); i > 0; i--)
		{
			int n = UnityEngine.Random.Range(_range.min, (i + 1));
			int x = result[n];
			result[n] = result[i];
			result[i] = x;			
		}

		return result;
	}

	/// <summary>Gets a unique [not duplicate] set of random integers, from 0 to given count.</summary>
	/// <param name="_count">Size of the set.</param>
	/// <returns>Set of random sorted unique integers.</returns>
	public static int[] GetUniqueRandomSet(int _count)
	{
		HashSet<int> numbersSet = new HashSet<int>();

		for(int i = 0; i < _count; i++)
		{
			if(!numbersSet.Add(UnityEngine.Random.Range(0, (i + 1))))
			numbersSet.Add(i);
		}

		int[] result = numbersSet.ToArray();

		for(int i = (result.Length - 1); i > 0; i--)
		{
			int n = UnityEngine.Random.Range(0, (i + 1));
			int x = result[n];
			result[n] = result[i];
			result[i] = x;			
		}

		return result;
	}
#endregion

	/// <summary>Calculates Rectified Linear Unit of given number.</summary>
	/// <param name="x">Unit to rectify.</param>
	/// <returns>Number rectified.</returns>
	public static float RectifiedLinearUnit(float x)
	{
		return (x >= 0.0f ? x : 0.0f);
	}

	/// <summary>Checks if a dot product between 2 vectors is between an angle of tolerance.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector b.</param>
	/// <param name="degreeTolerance">Degree Tolerance.</param>
	/// <returns>True if the dot product between two vectors is between given tolerance angle.</returns>
	public static bool DotProductWithinAngle(Vector3 a, Vector3 b, float degreeTolerance)
	{
		float dot = Vector3.Dot(a, b);
		float angleToDot = Mathf.Cos(degreeTolerance * DEG_TO_RAD);

		return dot >= 0.0f ? dot >= angleToDot : dot <= angleToDot;
	}

#region NumberUtilities:
	/// <summary>Remaps given input from map into given range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_map">Original values mapping.</param>
	/// <param name="_range">Range to map the input to.</param>
	/// <returns>Input mapped into given range.</returns>
	public static float RemapValue(float _input, FloatRange _map, FloatRange _range)
	{
		return (((_range.max - _range.min) * (_input - _map.min)) / (_map.max - _map.min)) + _range.min;
	}

	/// <summary>Remaps given input from map into given range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_mapMin">Original values mapping's minimum value.</param>
	/// <param name="_mapMax">Original values mapping's maximum value.</param>
	/// <param name="_rangeMin">Range's minimum value.</param>
	/// <param name="_rangeMax">Range's maximum value.</param>
	/// <returns>Input mapped into given range.</returns>
	public static float RemapValue(float _input, float _mapMin, float _mapMax, float _rangeMin, float _rangeMax)
	{
		return (((_rangeMax - _rangeMin) * (_input - _mapMin)) / (_mapMax - _mapMin)) + _rangeMin;
	}

	/// <summary>Remaps given input from map into normalized range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_mapMin">Original values mapping's minimum value.</param>
	/// <param name="_mapMax">Original values mapping's maximum value.</param>
	/// <returns>Input mapped into normalizedRange.</returns>
	public static float RemapValueToNormalizedRange(float _input, FloatRange _map)
	{
		return ((_input - _map.min) / (_map.max - _map.min));
	}

	/// <summary>Remaps given input from map into normalized range.</summary>
	/// <param name="_input">Input value to remap.</param>
	/// <param name="_map">Original values mapping.</param>
	/// <returns>Input mapped into normalizedRange.</returns>
	public static float RemapValueToNormalizedRange(float _input, float _mapMin, float _mapMax)
	{
		return ((_input - _mapMin) / (_mapMax - _mapMin));
	}

	/// <summary>Calculates normalized t from input and given range.</summary>
	/// <param name="x">Input.</param>
	/// <param name="min">Range's Minimum Value.</param>
	/// <param name="max">Range's Maximum Value.</param>
	/// <returns>Normalized T.</returns>
	public static float T(float x, float min, float max)
	{
		return (x - min) / (max - min);
	}

	public static float Clamp(this float x, float min, float max)
	{
		return Mathf.Clamp(x, min, max);
	}

	public static int Clamp(this int x, int min, int max)
	{
		return Mathf.Clamp(x, min, max);
	}

	/// <summary>Sets Integer to clamped value.</summary>
	/// <param name="x">Integer that will be clamped.</param>
	/// <param name="min">Minimum value clamped.</param>
	/// <param name="max">Maximum value clamped.</param>
	/// <returns>Integer clamped (as int).</returns>
	public static int ClampSet(ref int x, int min, int max)
	{
		return x = x < min ? min : x > max ? max : x;
	}

	/// <summary>Sets float to clamped value.</summary>
	/// <param name="x">Float that will be clamped.</param>
	/// <param name="min">Minimum value clamped.</param>
	/// <param name="max">Maximum value clamped.</param>
	/// <returns>Float clamped (as float).</returns>
	public static float ClampSet(ref float x, float min, float max)
	{
		return x = x < min ? min : x > max ? max : x;
	}

	/// <summary>Calculates negative absolute of given number.</summary>
	/// <param name="x">Value to convert to negative absolute.</param>
	/// <returns>Number passed to negative absolute.</returns>
	public static float NegativeAbs(float x)
	{
		return (x < 0.0f ? x : (x * -1.0f));
	}	
#endregion

	/// <summary>Gets Range's Median.</summary>
	/// <param name="min">Minimum value.</param>
	/// <param name="max">Minimum value.</param>
	/// <returns>Range's Median.</returns>
	public static float GetMedian(float min, float max)
	{
		return min + ((max - min) * 0.5f);
	}

#region ShortFunctions:
	/// <summary>Clamps short to a maximum value.</summary>
	/// <param name="x">Short to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <returns>Clamped short value.</returns>
	public static short Max(short x, short max)
	{
		return x > max ? max : x;
	}

	/// <summary>Clamps short to a minimum value.</summary>
	/// <param name="x">Short to clamp.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped short value.</returns>
	public static short Min(short x, short min)
	{
		return x < min ? min : x;
	}

	/// <summary>Clamps short between a minimum and maximum value.</summary>
	/// <param name="x">Short to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped short value.</returns>
	public static short Clamp(short x, short min, short max)
	{
		return x < min ? min : x > max ? max : x;
	}
#endregion

#region LongFunctions:
	/// <summary>Clamps long to a maximum value.</summary>
	/// <param name="x">Long to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <returns>Clamped long value.</returns>
	public static long Max(long x, long max)
	{
		return x > max ? max : x;
	}

	/// <summary>Clamps long to a minimum value.</summary>
	/// <param name="x">Long to clamp.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped long value.</returns>
	public static long Min(long x, long min)
	{
		return x < min ? min : x;
	}

	/// <summary>Clamps long between a minimum and maximum value.</summary>
	/// <param name="x">Long to clamp.</param>
	/// <param name="max">Maximum value possible.</param>
	/// <param name="min">Minimum value possible.</param>
	/// <returns>Clamped long value.</returns>
	public static long Clamp(long x, long min, long max)
	{
		return x < min ? min : x > max ? max : x;
	}
#endregion

#region ComparissonUtilities:
	public static int Factorial(int x)
	{
		if(x <= 0)
		{
			throw new Exception("No Value below 1 can be received");
		
		} else if(x == 1) return 1;
		else return x * Factorial((x - 1));
	
		int f = x;

		while(x > 1)
		{
			f *= --x;
		}

		return f;
	}

	public static float Factorial(float x)
	{
		if(x <= 0.0f)
		{
			throw new Exception("No Value below 1 can be received");

		} else if(x == 1.0f) return 1.0f;
		else return x * Factorial((x - 1.0f));

		float f = x;

		while(x > 1.0f)
		{
			f *= --x;
		}

		return f;
	}

	/// <summary>Checks if to float values are equal [below or equal the difference tolerance].</summary>
	/// <param name="_f1">First float value.</param>
	/// <param name="_f2">Second float value.</param>
	/// <param name="_differenceTolerance">Difference's Tolerance between the two float values [Epsilon by default].</param>
	/// <returns>True if both float values are equal between epsilon's range.</returns>
	public static bool Equal(float _f1, float _f2, float _differenceTolerance = 0.0f)
	{
		return Mathf.Abs(_f1 - _f2) <= (_differenceTolerance == 0.0f ? Mathf.Epsilon : _differenceTolerance);
	}

	/// <summary>Checks if to float values are different [above or equal the difference tolerance].</summary>
	/// <param name="_f1">First float value.</param>
	/// <param name="_f2">Second float value.</param>
	/// <param name="_differenceTolerance">Difference's Tolerance between the two float values [Epsilon by default].</param>
	/// <returns>True if both float values are different between epsilon's range.</returns>
	public static bool Different(float _f1, float _f2, float _differenceTolerance = 0.0f)
	{
		return Mathf.Abs(_f1 - _f2) >= (_differenceTolerance == 0.0f ? Mathf.Epsilon : _differenceTolerance);
	}
#endregion

	/// <summary>Gets 360 system angle between 2 points.</summary>
	/// <param name="_fromPoint">Point from where the angle starts.</param>
	/// <param name="_toPoint">Point the origin point is pointing towards.</param>
	/// <param name="_coordinatesMode">Coordinates Mode.</param>
	/// <returns>360 range angle (as float).</returns>
	public static float Get360Angle(Vector3 _fromPoint, Vector3 _toPoint, CoordinatesModes _coordinatesMode)
	{
		Vector2 direction = Vector2.zero;

		switch(_coordinatesMode)
		{
			case CoordinatesModes.XY:
			direction = new Vector2((_fromPoint.x - _toPoint.x), (_fromPoint.y - _toPoint.y));
			break;

			case CoordinatesModes.YX:
			direction = new Vector2((_fromPoint.y - _toPoint.y), (_fromPoint.x - _toPoint.x));
			break;

			case CoordinatesModes.XZ:
			direction = new Vector2((_fromPoint.x - _toPoint.x), (_fromPoint.z - _toPoint.z));
			break;

			case CoordinatesModes.ZY:
			direction = new Vector2((_fromPoint.z - _toPoint.z), (_fromPoint.y - _toPoint.y));
			break;

			case CoordinatesModes.YZ:
			direction = new Vector2((_fromPoint.y - _toPoint.y), (_fromPoint.z - _toPoint.z));
			break;

			case CoordinatesModes.ZX:
			direction = new Vector2((_fromPoint.z - _toPoint.z), (_fromPoint.x - _toPoint.x));
			break;
		}

		return direction.y < 0f || direction.x < 0f &&direction.y < 0f ? (Mathf.Atan2(direction.y, direction.x) + (PI * 2)) * RAD_TO_DEG : Mathf.Atan2(direction.y, direction.x) * RAD_TO_DEG;
	}
}
}