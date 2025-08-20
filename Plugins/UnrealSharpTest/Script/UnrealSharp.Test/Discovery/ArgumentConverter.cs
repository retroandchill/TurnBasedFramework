using System.Reflection;

namespace UnrealSharp.Test.Discovery;

public static class ArgumentConverter
{
    public static object?[] ConvertProvidedArguments(MethodInfo methodInfo, object?[] arguments)
    {
        var parameters = methodInfo.GetParameters();
        var requiredParameters = parameters.TakeWhile(p => p.HasDefaultValue == false).Count();
        
        if (requiredParameters > arguments.Length)
        {
            throw new InvalidOperationException($"Test method {methodInfo.Name} has more parameters than provided");
        }
        
        if (parameters.Length < arguments.Length)
        {
            LogUnrealSharpTest.LogWarning($"Test method {methodInfo.Name} has less parameters than provided");
        }
        
        var convertedArguments = new object?[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            if (i < arguments.Length)
            {
                convertedArguments[i] = parameters[i].DefaultValue;
            }
            
            convertedArguments[i] = Convert.ChangeType(arguments[i], parameters[i].ParameterType);
        }

        return convertedArguments;
    }
}