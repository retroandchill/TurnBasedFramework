using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace UnrealSharp.Test.Utils;

public static class ModuleExtensions
{
    public static SequencePoint? GetFirstSequencePoint(this MethodInfo methodInfo)
    {
        try
        {
            var methodBody = methodInfo.GetMethodBody();
            if (methodBody == null)
                return null;

            var methodToken = methodInfo.MetadataToken;
            var debugInfo = methodInfo.Module.GetDebugInfoOrNull(methodToken);
            var sequencePoints = debugInfo?.GetSequencePoints();
            return sequencePoints?.FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    public static MethodDebugInformation? GetDebugInfoOrNull(this Module module, int methodToken)
    {
        try
        {
            var pdbPath = module.Assembly.Location + ".pdb";
            if (!File.Exists(pdbPath))
                return null;

            using var pdbStream = File.OpenRead(pdbPath);
            using var provider = MetadataReaderProvider.FromPortablePdbStream(pdbStream);
            var reader = provider.GetMetadataReader();

            var methodDebugHandle = MetadataTokens.MethodDebugInformationHandle(methodToken);
            return reader.GetMethodDebugInformation(methodDebugHandle);
        }
        catch
        {
            return null;
        }
    }
}
