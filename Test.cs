using System;
using System.Collections.Generic;
using System.Reflection;

public partial class UnitTests
{
    public static List<int> ExpectedValues = new List<int> {
        int.MinValue, int.MinValue +1, -1, 0, 1, 99, 100, 101, 999, 1000, 1001, 9999, 10000, 10001
    };

    public static int ExtraValuesCount = 0;
    public static bool IsException = false;

    public static double GetDiscountedPrice(int n)
    {
        IsException = false;

        if (ExpectedValues.Contains(n))
        {
            ExpectedValues.Remove(n);
            if (n < 0)
            {
                IsException = true;
                throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            ExtraValuesCount++;
        }

        if (n < 100) return n;
        else if (n < 1000) return Math.Round(n * 0.9, 1);
        else if (n < 10000) return Math.Round(n * 0.8, 1);
        else return Math.Round(n * 0.7, 1);
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class TestClassAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class TestMethodAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class ExpectedExceptionAttribute : Attribute
{
    public Type ExceptionType { get; }
    public ExpectedExceptionAttribute(Type exceptionType) { ExceptionType = exceptionType; }
}

public static class Assert
{
    public static void AreEqual<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            UnitTests.IsException = true;
            Console.WriteLine($"Expected {expected} but got {actual}");
        }
    }
}

public class Program
{
    public static void Main()
    {
        var testInstance = new UnitTests();
        var methods = typeof(UnitTests).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        int passed = 0, failed = 0;
        Array.Sort(methods, (a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));

        foreach (var method in methods)
        {
            if (method.GetCustomAttribute(typeof(TestMethodAttribute)) != null)
            {
                string testName = method.Name;
                var expectedExceptionAttr = method.GetCustomAttribute<ExpectedExceptionAttribute>();

                try
                {
                    method.Invoke(testInstance, null);

                    if (expectedExceptionAttr != null)
                    {
                        Console.WriteLine($"{testName} FAILED: Expected exception {expectedExceptionAttr.ExceptionType.Name} was not thrown.");
                        failed++;
                    }
                    else
                    {
                        Console.WriteLine($"{testName} passed.");
                        passed++;
                    }
                }
                catch (TargetInvocationException ex)
                {
                    if (expectedExceptionAttr != null && ex.InnerException?.GetType() == expectedExceptionAttr.ExceptionType)
                    {
                        Console.WriteLine($"{testName} passed.");
                        passed++;
                    }
                    else
                    {
                        Console.WriteLine($"{testName} FAILED: {ex.InnerException?.Message}");
                        failed++;
                    }
                }
            }
        }

        Console.WriteLine("All tests completed.");
        Console.WriteLine($"Summary: {passed} passed, {failed} failed.");
        Console.WriteLine($"Missing test values count: {UnitTests.ExpectedValues.Count}");
        Console.WriteLine($"Extra test values count: {UnitTests.ExtraValuesCount}");
    }
}
