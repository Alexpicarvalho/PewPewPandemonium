#region assembly UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files\Unity\Hub\Editor\2021.3.10f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace UnityEngine.Assertions
{
    //
    // Resumo:
    //     The Assert class contains assertion methods for setting invariants in the code.
    [DebuggerStepThrough]
    public static class Assert
    {
        //
        // Resumo:
        //     Obsolete. Do not use.
        [Obsolete("Future versions of Unity are expected to always throw exceptions and not have this field.")]
        public static bool raiseExceptions;

        //
        // Resumo:
        //     Assert the values are approximately equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual, float tolerance, string message);
        //
        // Resumo:
        //     Assert the values are approximately equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual, float tolerance);
        //
        // Resumo:
        //     Assert the values are approximately equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual);
        //
        // Resumo:
        //     Assert the values are approximately equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreApproximatelyEqual(float expected, float actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(ushort expected, ushort actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(ushort expected, ushort actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(byte expected, byte actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(uint expected, uint actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(char expected, char actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(char expected, char actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(sbyte expected, sbyte actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(sbyte expected, sbyte actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(int expected, int actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(int expected, int actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(uint expected, uint actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(byte expected, byte actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(short expected, short actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(short expected, short actual);
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer);
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual<T>(T expected, T actual, string message);
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual<T>(T expected, T actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(ulong expected, ulong actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(ulong expected, ulong actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(Object expected, Object actual, string message);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(long expected, long actual);
        //
        // Resumo:
        //     Assert that the values are equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreEqual(long expected, long actual, string message);
        //
        // Resumo:
        //     Asserts that the values are approximately not equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual, string message);
        //
        // Resumo:
        //     Asserts that the values are approximately not equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance, string message);
        //
        // Resumo:
        //     Asserts that the values are approximately not equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual, float tolerance);
        //
        // Resumo:
        //     Asserts that the values are approximately not equal.
        //
        // Parâmetros:
        //   tolerance:
        //     Tolerance of approximation.
        //
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotApproximatelyEqual(float expected, float actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(short expected, short actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(short expected, short actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(long expected, long actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(uint expected, uint actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(ushort expected, ushort actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(ushort expected, ushort actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(int expected, int actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(int expected, int actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(long expected, long actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(uint expected, uint actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(sbyte expected, sbyte actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(char expected, char actual);
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual<T>(T expected, T actual);
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual<T>(T expected, T actual, string message);
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual<T>(T expected, T actual, string message, IEqualityComparer<T> comparer);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(char expected, char actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(Object expected, Object actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(byte expected, byte actual, string message);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(byte expected, byte actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(ulong expected, ulong actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(sbyte expected, sbyte actual);
        //
        // Resumo:
        //     Assert that the values are not equal.
        //
        // Parâmetros:
        //   expected:
        //     The assumed Assert value.
        //
        //   actual:
        //     The exact Assert value.
        //
        //   message:
        //     The string used to describe the Assert.
        //
        //   comparer:
        //     Method to compare expected and actual arguments have the same value.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AreNotEqual(ulong expected, ulong actual, string message);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Assert.Equals should not be used for Assertions", true)]
        public static bool Equals(object obj1, object obj2);
        //
        // Resumo:
        //     Return true when the condition is false. Otherwise return false.
        //
        // Parâmetros:
        //   condition:
        //     true or false.
        //
        //   message:
        //     The string used to describe the result of the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsFalse(bool condition, string message);
        //
        // Resumo:
        //     Return true when the condition is false. Otherwise return false.
        //
        // Parâmetros:
        //   condition:
        //     true or false.
        //
        //   message:
        //     The string used to describe the result of the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsFalse(bool condition);
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsNotNull<T>(T value) where T : class;
        //
        // Resumo:
        //     Assert that the value is not null.
        //
        // Parâmetros:
        //   value:
        //     The Object or type being checked for.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsNotNull(Object value, string message);
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsNotNull<T>(T value, string message) where T : class;
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsNull<T>(T value, string message) where T : class;
        //
        // Resumo:
        //     Assert the value is null.
        //
        // Parâmetros:
        //   value:
        //     The Object or type being checked for.
        //
        //   message:
        //     The string used to describe the Assert.
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsNull(Object value, string message);
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsNull<T>(T value) where T : class;
        //
        // Resumo:
        //     Asserts that the condition is true.
        //
        // Parâmetros:
        //   message:
        //     The string used to describe the Assert.
        //
        //   condition:
        //     true or false.
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsTrue(bool condition, string message);
        //
        // Resumo:
        //     Asserts that the condition is true.
        //
        // Parâmetros:
        //   message:
        //     The string used to describe the Assert.
        //
        //   condition:
        //     true or false.
        [Conditional("UNITY_ASSERTIONS")]
        public static void IsTrue(bool condition);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Assert.ReferenceEquals should not be used for Assertions", true)]
        public static bool ReferenceEquals(object obj1, object obj2);
    }
}
