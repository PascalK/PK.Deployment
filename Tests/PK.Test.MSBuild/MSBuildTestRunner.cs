using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace PK.Test.MSBuild
{
    public class MSBuildTestRunner
    {
        public ILogger Logger { get; private set; }

        public MSBuildTestRunner(ILogger logger)
        {
            Logger = logger;
        }

        public BuildResult TestBuild(MsBuildParameters parameters)
        {
            return TestBuild(parameters, null);
        }
        public BuildResult TestBuild(MsBuildParameters parameters, MsDeployPublishProfile publishProfile)
        {
            parameters = parameters ?? new MsBuildParameters();
            publishProfile = publishProfile ?? new MsDeployPublishProfile();
            return TestBuild(parameters.BuildFile, parameters as IDictionary<string, string>, publishProfile);
        }
        private BuildResult TestBuild(
            FileInfo buildFile,
            IDictionary<string, string> buildProperties,
            MsDeployPublishProfile publishProfile)
        {
            ProjectCollection projectCollection;
            BuildRequestData buildRequest;
            BuildParameters buildParameters;

            projectCollection = new ProjectCollection();
            buildParameters = new BuildParameters(projectCollection);
            buildParameters.LogInitialPropertiesAndItems = true;
            buildParameters.LogTaskInputs = true;
            buildParameters.ResetCaches = true;
            buildParameters.DetailedSummary = true;
            buildParameters.Loggers = new List<ILogger>()
            {
                Logger,
                //new MsBuildToConsoleLogger()
                //{
                //    Verbosity = LoggerVerbosity.Diagnostic
                //},
            };
            buildProperties.Union(publishProfile).ToDictionary(p => p.Key, p => p.Value).ToList().ForEach(kv => Console.WriteLine(string.Format("{0}:{1}", kv.Key, kv.Value)));
            buildRequest = new BuildRequestData(
                new ProjectInstance(
                    buildFile.FullName,
                    buildProperties.Union(publishProfile).ToDictionary(p => p.Key, p => p.Value),
                    null),
                new string[0],
                null,
                BuildRequestDataFlags.None);

            return BuildManager.DefaultBuildManager.Build(
                buildParameters,
                buildRequest);
        }
    }
}
