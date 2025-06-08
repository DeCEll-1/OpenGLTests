using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using OpenTK.Mathematics;
using System.Threading.Tasks;

namespace OpenglTestConsole.Classes.API.Misc
{
    public class ReflectionMisc
    {
        private static Dictionary<string, string> delegateCache = [];
        public static string GetDelegateeBody(Delegate del)
        {

            MethodInfo info = del.Method;
            string assemblyPath = info.Module.FullyQualifiedName;
            int token = info.MetadataToken;
            if (delegateCache.ContainsKey(assemblyPath + token.ToString()))
            {
                return delegateCache[assemblyPath + token.ToString()];
            }

            CSharpDecompiler decompiler = new CSharpDecompiler(assemblyPath, new());
            EntityHandle? entityHandle = MetadataTokenHelpers.TryAsEntityHandle(token);
            if (entityHandle == null)
                return "NO_HANDLE";

            string syntaxTree = decompiler.Decompile((EntityHandle)entityHandle).ToString();
            delegateCache.Add(assemblyPath + token.ToString(), syntaxTree);
            return syntaxTree;
        }

        private delegate string ToStringDelegate();
        private static Dictionary<Type, bool> stringOverrideCache = [];
        public static bool OverridesToString(object instance)
        {
            if (instance == null)
                return false;

            Type type = instance.GetType();

            // Check cache
            if (stringOverrideCache.TryGetValue(type, out bool cachedResult))
                return cachedResult;

            // Determine if ToString is overridden
            ToStringDelegate func = instance.ToString;
            bool isOverridden = func.Method.DeclaringType == type;

            // Cache the result
            stringOverrideCache[type] = isOverridden;

            return isOverridden;
        }
    }
}
