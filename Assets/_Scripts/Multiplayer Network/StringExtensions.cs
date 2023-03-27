using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
public static class StringExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool HasValue(this string value)
	{
		return string.IsNullOrEmpty(value) == false;
	}
}
