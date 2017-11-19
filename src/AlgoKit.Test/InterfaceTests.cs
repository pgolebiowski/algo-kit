using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace AlgoKit.Test
{
    public class InterfaceTests
    {
        [Fact]
        // It is crucial for parameter names to match when inheriting docs.
        public void Overridden_methods_should_have_same_parameter_names()
        {
            var algoKitType = typeof(AlgoKit.Collections.Heaps.IHeap<int, int>);

            var wrongMethods = Assembly.GetAssembly(algoKitType)
                .GetTypes()
                .Where(x => !x.GetCustomAttributes(typeof(CompilerGeneratedAttribute)).Any())
                .SelectMany(type => type.GetMethods().Select(method => new
                {
                    Method = method,
                    ParentType = type
                }))
                .Where(x =>
                {
                    var a = x.Method.GetParameters().Select(p => p.Name);
                    var b = x.Method.GetBaseDefinition().GetParameters().Select(p => p.Name);

                    return !a.SequenceEqual(b);
                })
                .ToArray();

            if (!wrongMethods.Any())
                return;

            foreach (var method in wrongMethods)
                Console.WriteLine($"{method.ParentType.Name}.{method.Method.Name}");

            throw new Exception($"There are {wrongMethods.Length} methods with wrong parameter names.");
        }
    }
}
