namespace NetworkLib.Utilities
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class ReflectionHelper
    {
        private static Dictionary<string, Assembly> _assemblyPool = new Dictionary<string, Assembly>();

        private ReflectionHelper()
        {
        }

        public static CompilerResults CompileAssemblyFromFile(string language, CompilerParameters parameters, string[] fileNames)
        {
            return CodeDomProvider.CreateProvider(language).CompileAssemblyFromFile(parameters, fileNames);
        }

        public static CompilerResults CompileAssemblyFromSource(string language, CompilerParameters parameters, string source)
        {
            return CodeDomProvider.CreateProvider(language).CompileAssemblyFromSource(parameters, new string[] { source });
        }

        public static object CreateInstance(Type type, object[] parameters, bool errorOnMissing)
        {
            ConstructorInfo constructor = type.GetConstructor(GetTypes(parameters));
            if (constructor != null)
            {
                return constructor.Invoke(parameters);
            }
            return null;
        }

        public static CompilerParameters GetCompilerParameters(string[] references, string output, bool excutable)
        {
            CompilerParameters parameters = new CompilerParameters(references, output);
            parameters.GenerateInMemory = false;
            parameters.CompilerOptions = "/optimize";
            parameters.GenerateExecutable = excutable;
            parameters.ReferencedAssemblies.Add("System.dll");
            return parameters;
        }

        public static Type GetType(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                foreach (Assembly assembly in _assemblyPool.Values)
                {
                    type = assembly.GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return type;
        }

        public static Type GetTypeFormPool(string assemblyName, string typeName)
        {
            return _assemblyPool[assemblyName].GetType(typeName, false);
        }

        public static Type[] GetTypes(object[] objects)
        {
            Type[] typeArray = new Type[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                typeArray[i] = objects[i].GetType();
            }
            return typeArray;
        }

        public static object InvokeMethod(object obj, string methodName, object[] parameters, bool errorOnMissing)
        {
            return InvokeMethod(obj, methodName, parameters, GetTypes(parameters), errorOnMissing);
        }

        public static object InvokeMethod(object obj, string methodName, object[] parameters, Type[] types, bool errorOnMissing)
        {
            MethodInfo method = obj.GetType().GetMethod(methodName, types);
            if (method != null)
            {
                return method.Invoke(obj, parameters);
            }
            if (errorOnMissing)
            {
                throw new MissingMethodException(string.Format("找不到指定方法 : {0}", methodName));
            }
            return null;
        }

        public static object InvokeStaticMethod(Type type, string methodName, object[] parameters, bool errorOnMissing)
        {
            return InvokeStaticMethod(type, methodName, parameters, GetTypes(parameters), errorOnMissing);
        }

        public static object InvokeStaticMethod(Type type, string methodName, object[] parameters, Type[] types, bool errorOnMissing)
        {
            MethodInfo method = type.GetMethod(methodName, types);
            if (method != null)
            {
                return method.Invoke(null, parameters);
            }
            if (errorOnMissing)
            {
                throw new MissingMethodException(string.Format("找不到指定方法 : {0}", methodName));
            }
            return null;
        }

        public static string LoadAssembly(Assembly assembly)
        {
            _assemblyPool[assembly.FullName] = assembly;
            return assembly.FullName;
        }

        public static string LoadAssembly(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            if (info.Exists)
            {
                try
                {
                    return LoadAssembly(Assembly.LoadFile(info.FullName));
                }
                catch (Exception exception)
                {
                    Logger.LogException(exception);
                }
            }
            return null;
        }
    }
}

