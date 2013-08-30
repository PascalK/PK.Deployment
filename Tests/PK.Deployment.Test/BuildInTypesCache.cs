using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Microsoft.Web.Deployment;
namespace PK.Deployment
{
    public static class BuiltInTypesCache
    {
        private static List<string> _builtInFactories;
        private static List<Type> _builtInLinkExtensionTypes;
        private static List<Type> _builtInMethodTypes;
        private static List<Type> _builtInRuleHandlerTypes;
        private static List<Type> _builtInSkipDirectiveTypes;
        private static Dictionary<Type, bool> _customType;
        private static List<Exception> _loaderExceptions;
        public static IEnumerable<string> Factories
        {
            get
            {
                return BuiltInTypesCache._builtInFactories;
            }
        }
        public static IEnumerable<Type> LinkExtensionTypes
        {
            get
            {
                return BuiltInTypesCache._builtInLinkExtensionTypes;
            }
        }
        public static IEnumerable<Type> MethodTypes
        {
            get
            {
                return BuiltInTypesCache._builtInMethodTypes;
            }
        }
        public static IEnumerable<Type> RuleHandlerTypes
        {
            get
            {
                return BuiltInTypesCache._builtInRuleHandlerTypes;
            }
        }
        public static IEnumerable<Type> SkipDirectiveTypes
        {
            get
            {
                return BuiltInTypesCache._builtInSkipDirectiveTypes;
            }
        }
        public static List<Exception> LoaderExceptions
        {
            get
            {
                if (BuiltInTypesCache._loaderExceptions == null)
                {
                    BuiltInTypesCache._loaderExceptions = new List<Exception>();
                }
                return BuiltInTypesCache._loaderExceptions;
            }
        }
        public static IEnumerable<string> GetExtensiblityAssemblyFiles()
        {
            return BuiltInTypesCache.GetExtensiblityAssemblyFiles(true);
        }
        private static IEnumerable<Assembly> GetExtensiblityAssembliesWithDuplicates()
        {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\IIS Extensions\\MSDeploy\\3\\Extensibility", false))
            {
                if (registryKey != null)
                {
                    try
                    {
                        string[] subKeyNames = registryKey.GetSubKeyNames();
                        for (int i = 0; i < subKeyNames.Length; i++)
                        {
                            string name = subKeyNames[i];
                            Assembly assembly = BuiltInTypesCache.LoadExtensibilityAssembly(name);
                            if (assembly != null)
                            {
                                yield return assembly;
                            }
                        }
                    }
                    finally
                    {
                    }
                }
            }
            foreach (string current in BuiltInTypesCache.GetExtensiblityAssemblyFiles(true))
            {
                Assembly assembly2 = BuiltInTypesCache.LoadExtensibilityAssemblyFromPath(current);
                if (assembly2 != null)
                {
                    yield return assembly2;
                }
            }
            yield break;
        }
        private static IEnumerable<string> GetExtensiblityAssemblyFiles(bool fullName)
        {
            string mSDeployInstallOrBinPath = AssemblyEnvironment.MSDeployInstallOrBinPath;
            string path = Path.Combine(mSDeployInstallOrBinPath, "Extensibility");
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfoEx = new DirectoryInfo(path);
                try
                {
                    FileInfo[] files = directoryInfoEx.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileInfo fileInfoEx = files[i];
                        if (!string.IsNullOrEmpty(fileInfoEx.Name) && fileInfoEx.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                        {
                            if (fullName)
                            {
                                yield return fileInfoEx.FullName;
                            }
                            else
                            {
                                yield return fileInfoEx.Name;
                            }
                        }
                    }
                }
                finally
                {
                }
            }
            yield break;
        }
        private static Assembly LoadExtensibilityAssemblyFromPath(string path)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFile(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Tracer.TraceError(DeploymentTraceSource.Framework, Resources.DeploymentAgentException, new object[]
                //{
                //    ex
                //});
                return null;
            }
            if (!assembly.GlobalAssemblyCache)
            {
                Console.WriteLine("GacAssemblyProviderNoGacDir: " + path);
                //Tracer.TraceWarning(DeploymentTraceSource.Framework, Resources.GacAssemblyProviderNoGacDir, new object[]
                //{
                //    path
                //});
            }
            return assembly;
        }
        private static Assembly LoadExtensibilityAssembly(string name)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.Load(name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Tracer.TraceError(DeploymentTraceSource.Framework, Resources.DeploymentAgentException, new object[]
                //{
                //    ex
                //});
                return null;
            }
            if (!assembly.GlobalAssemblyCache)
            {
                Console.WriteLine("GacAssemblyProviderNoGacDir: " + name);
                //Tracer.TraceWarning(DeploymentTraceSource.Framework, Resources.GacAssemblyProviderNoGacDir, new object[]
                //{
                //    name
                //});
            }
            return assembly;
        }
        private static IEnumerable<Assembly> GetExtensiblityAssemblies()
        {
            HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            hashSet.Add(Assembly.GetExecutingAssembly().FullName);
            foreach (Assembly current in BuiltInTypesCache.GetExtensiblityAssembliesWithDuplicates())
            {
                if (!hashSet.Contains(current.FullName))
                {
                    hashSet.Add(current.FullName);
                    yield return current;
                }
                else
                {
                    Console.WriteLine("UnexpectedDuplicateElement: " + current.FullName);
                    //Tracer.TraceWarning(DeploymentTraceSource.Framework, Resources.UnexpectedDuplicateElement, new object[]
                    //{
                    //    current.FullName
                    //});
                }
            }
            yield break;
        }
        public static bool isCustomType(Type type)
        {
            return BuiltInTypesCache._customType.ContainsKey(type);
        }
        public static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            try
            {
                Type[] allTypesFromAssembly = assembly.GetTypes();// ReflectionHelper.GetAllTypesFromAssembly(assembly, ref BuiltInTypesCache._loaderExceptions);
                for (int i = 0; i < allTypesFromAssembly.Length; i++)
                {
                    Type type = allTypesFromAssembly[i];
                    if (type != null)
                    {
                        yield return type;
                    }
                }
            }
            finally
            {
            }
            yield break;
        }
        static BuiltInTypesCache()
        {
            BuiltInTypesCache._builtInFactories = new List<string>();
            BuiltInTypesCache._builtInLinkExtensionTypes = new List<Type>();
            BuiltInTypesCache._builtInMethodTypes = new List<Type>();
            BuiltInTypesCache._builtInRuleHandlerTypes = new List<Type>();
            BuiltInTypesCache._builtInSkipDirectiveTypes = new List<Type>();
            BuiltInTypesCache._customType = new Dictionary<Type, bool>();
            BuiltInTypesCache._loaderExceptions = new List<Exception>();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            BuiltInTypesCache.InspectTypesForWebDeployAttributes(BuiltInTypesCache.GetTypes(executingAssembly), executingAssembly.FullName);
            foreach (Assembly current in BuiltInTypesCache.GetExtensiblityAssemblies())
            {
                BuiltInTypesCache.InspectTypesForWebDeployAttributes(BuiltInTypesCache.GetTypes(current), current.FullName, true);
            }
            //BuiltInTypesCache._builtInFactories.Sort(new Comparison<DeploymentProviderFactory>(BuiltInTypesCache.CompareIDeploymentNameDescriptionObjectsByName));
            BuiltInTypesCache._builtInLinkExtensionTypes.Sort(new Comparison<Type>(BuiltInTypesCache.CompareTypesByFullName));
            BuiltInTypesCache._builtInMethodTypes.Sort(new Comparison<Type>(BuiltInTypesCache.CompareTypesByFullName));
            BuiltInTypesCache._builtInRuleHandlerTypes.Sort(new Comparison<Type>(BuiltInTypesCache.CompareTypesByFullName));
            BuiltInTypesCache._builtInSkipDirectiveTypes.Sort(new Comparison<Type>(BuiltInTypesCache.CompareTypesByFullName));
        }
        private static void InspectTypesForWebDeployAttributes(IEnumerable<Type> types, string dllName, bool isCustomType)
        {
            bool flag = false;
            foreach (Type current in types)
            {
                if (AssemblyEnvironment.DoesTypeHaveAttribute<DeploymentProviderFactoryAttribute>(current))
                {
                    if (isCustomType)
                    {
                        BuiltInTypesCache._customType.Add(current, true);
                    }
                    //DeploymentProviderFactory item = DeploymentProviderFactory.Create(current);
                    BuiltInTypesCache._builtInFactories.Add(current.FullName);
                    flag = true;
                }
                if (AssemblyEnvironment.DoesTypeHaveAttribute<DeploymentLinkExtensionAttribute>(current))
                {
                    if (isCustomType)
                    {
                        BuiltInTypesCache._customType.Add(current, true);
                    }
                    BuiltInTypesCache._builtInLinkExtensionTypes.Add(current);
                    flag = true;
                }
                if (AssemblyEnvironment.DoesTypeHaveAttribute<DeploymentRuleHandlerAttribute>(current))
                {
                    if (isCustomType)
                    {
                        BuiltInTypesCache._customType.Add(current, true);
                    }
                    BuiltInTypesCache._builtInRuleHandlerTypes.Add(current);
                    flag = true;
                }
                if (AssemblyEnvironment.DoesTypeHaveAttribute<DeploymentSkipDirectiveAttribute>(current))
                {
                    if (isCustomType)
                    {
                        BuiltInTypesCache._customType.Add(current, true);
                    }
                    BuiltInTypesCache._builtInSkipDirectiveTypes.Add(current);
                    flag = true;
                }
                if (AssemblyEnvironment.DoesTypeHaveAttribute<DeploymentMethodAttribute>(current))
                {
                    if (isCustomType)
                    {
                        BuiltInTypesCache._customType.Add(current, true);
                    }
                    BuiltInTypesCache._builtInMethodTypes.Add(current);
                    flag = true;
                }
            }
            if (!flag)
            {
                Console.WriteLine("CouldNotFindWebDeployTypes: " + dllName);
                //Trace.TraceWarning(MessageTable.FormatString(Resources.CouldNotFindWebDeployTypes, new object[]
                //{
                //    dllName
                //}));
            }
        }
        private static void InspectTypesForWebDeployAttributes(IEnumerable<Type> types, string dllName)
        {
            BuiltInTypesCache.InspectTypesForWebDeployAttributes(types, dllName, false);
        }
        private static int CompareIDeploymentNameDescriptionObjectsByName(IDeploymentNameDescription x, IDeploymentNameDescription y)
        {
            if (x == y)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }
        private static int CompareTypesByFullName(Type x, Type y)
        {
            if (x == y)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            return string.Compare(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
        }
    }
    internal static class AssemblyEnvironment
    {
        private static FileVersionInfo _assemblyVersion;
        private static Version _minimumSupportedVersion;
        private static Version _maximumSupportedVersion;
        private static string _exePath;
        private static string _msdeployInstallPath;
        private static string _installOrBinPath;
        internal static string ExePath
        {
            get
            {
                if (string.IsNullOrEmpty(AssemblyEnvironment._exePath))
                {
                    AssemblyEnvironment._exePath = AssemblyEnvironment.GetExecutablePath();
                }
                return AssemblyEnvironment._exePath;
            }
        }
        internal static string CurrentAssemblyPath
        {
            get
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string result;
                if (!executingAssembly.GlobalAssemblyCache)
                {
                    result = Path.GetDirectoryName(executingAssembly.Location);
                }
                else
                {
                    result = string.Empty;
                }
                return result;
            }
        }
        internal static string MSDeployInstallPath
        {
            get
            {
                if (string.IsNullOrEmpty(AssemblyEnvironment._msdeployInstallPath))
                {
                    AssemblyEnvironment._msdeployInstallPath = AssemblyEnvironment.GetMSDeployInstallPath();
                }
                return AssemblyEnvironment._msdeployInstallPath;
            }
        }
        internal static string MSDeployV1InstallPath
        {
            get
            {
                string result;
                if (!string.IsNullOrEmpty(AssemblyEnvironment.MSDeployInstallPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(AssemblyEnvironment.MSDeployInstallPath);
                    result = Path.Combine(directoryInfo.Parent.FullName, "Microsoft Web Deploy");
                }
                else
                {
                    result = string.Empty;
                }
                return result;
            }
        }
        internal static string MSDeployInstallOrBinPath
        {
            get
            {
                if (string.IsNullOrEmpty(AssemblyEnvironment._installOrBinPath))
                {
                    string text = AssemblyEnvironment.MSDeployInstallPath;
                    if (string.IsNullOrEmpty(text))
                    {
                        text = AssemblyEnvironment.CurrentAssemblyPath;
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        text = Path.GetDirectoryName(AssemblyEnvironment.ExePath);
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        text = Environment.CurrentDirectory;
                    }
                    AssemblyEnvironment._installOrBinPath = text;
                }
                return AssemblyEnvironment._installOrBinPath;
            }
        }
        private static FileVersionInfo AssemblyFileVersionInfo
        {
            get
            {
                if (AssemblyEnvironment._assemblyVersion == null)
                {
                    Assembly executingAssembly = Assembly.GetExecutingAssembly();
                    AssemblyEnvironment._assemblyVersion = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
                }
                return AssemblyEnvironment._assemblyVersion;
            }
        }
        internal static string ProductBuildVersionString
        {
            get
            {
                return AssemblyEnvironment.AssemblyFileVersionInfo.ProductVersion;
            }
        }
        internal static Version MaximumSupportedVersion
        {
            get
            {
                if (AssemblyEnvironment._maximumSupportedVersion == null)
                {
                    AssemblyEnvironment._maximumSupportedVersion = new Version(9, 0, 0, 0);
                    //Version maximumSupportedVersion = DeploymentRegistryValues.MaximumSupportedVersion;
                    //if (maximumSupportedVersion == null)
                    //{
                    //    AssemblyEnvironment._maximumSupportedVersion = AssemblyEnvironment.AssemblyFileVersionInfo.Version;
                    //}
                    //else
                    //{
                    //    AssemblyEnvironment._maximumSupportedVersion = maximumSupportedVersion;
                    //}
                }
                return AssemblyEnvironment._maximumSupportedVersion;
            }
        }
        internal static Version MinimumSupportedVersion
        {
            get
            {
                if (AssemblyEnvironment._minimumSupportedVersion == null)
                {
                    AssemblyEnvironment._minimumSupportedVersion = new Version(7, 1, 600, 0);
                    //Version minimumSupportedVersion = DeploymentRegistryValues.MinimumSupportedVersion;
                    //if (minimumSupportedVersion == null)
                    //{
                    //    AssemblyEnvironment._minimumSupportedVersion = AssemblyVersion.MSDeploy_RTW_Version;
                    //}
                    //else
                    //{
                    //    AssemblyEnvironment._minimumSupportedVersion = minimumSupportedVersion;
                    //}
                }
                return AssemblyEnvironment._minimumSupportedVersion;
            }
        }
        private static string GetExecutablePath()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            string result;
            if (entryAssembly == null)
            {
                string[] commandLineArgs = Environment.GetCommandLineArgs();
                if (commandLineArgs.Length == 0)
                {
                    result = null;
                }
                else
                {
                    result = commandLineArgs[0];
                }
            }
            else
            {
                result = entryAssembly.Location;
            }
            return result;
        }
        private static string GetMSDeployInstallPath()
        {
            string text;
            if (IntPtr.Size == 8)//ProcessHelper.Is64Bit
            {
                text = @"C:\Program Files\IIS\Microsoft Web Deploy V3\";
            }
            else
            {
                //if (ProcessHelper.Is32BitWOW)
                {
                    //throw new InvalidOperationException("Oink");
                    //text = DeploymentRegistryValues.InstallPath_x64;
                }
                //else
                {
                    text = @"C:\Program Files\IIS\Microsoft Web Deploy V3\";
                }
            }
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
            //Tracer.TraceWarning(DeploymentTraceSource.Framework, Resources.InvalidRegistryKey, new object[]
            //{
            //    "Software\\Microsoft\\IIS Extensions\\MSDeploy\\3@install path"
            //});
            return string.Empty;
        }
        public static bool DoesTypeHaveAttribute<T>(Type type) where T : Attribute
        {
            object[] customAttributes = type.GetCustomAttributes(typeof(T), false);
            return customAttributes.Length > 0;
        }
    }
}