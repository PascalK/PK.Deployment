using System.IO;
using System.Linq;
using Microsoft.Build.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PK.Test.MSBuild;
using FluentAssertions;
using System;

namespace PK.Deployment
{
    [TestClass]
    public class WebApplicationTest
    {
        const string WebApplicationProjectName = @"Test.WebApp";
        private MSBuildTestRunner buildRunner;

        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        public WebApplicationTest()
        {
            buildRunner = new MSBuildTestRunner(new MsBuildToConsoleLogger());
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void normal_build_should_succeeds_without_exceptions()
        {
            var actualResult = TestBuild(new MsBuildParameters()
            {
                BuildFile = ProjectLocator.GetProjectFile(WebApplicationProjectName),
            });
            actualResult.Exception.Should().BeNull();
            actualResult.OverallResult.Should().Be(BuildResultCode.Success);
        }
        [TestMethod]
        public void package_adds_additionalmanifestfile_to_packagelocation()
        {
            var packageLocation = new DirectoryInfo(Path.Combine(TestContext.TestDir, TestContext.TestName));
            var actualResult = TestBuild(
                new MsBuildParameters()
                {
                    BuildFile = ProjectLocator.GetProjectFile(WebApplicationProjectName),
                    DeployOnBuild = true,
                },
                new MsDeployPublishProfile()
                {
                    WebPublishMethod = "Package",
                    PackageAsSingleFile = false,
                    PackageLocation = packageLocation,
                    GenerateSampleDeployScript = false,
                });

            actualResult.Exception.Should().BeNull();
            actualResult.OverallResult.Should().Be(BuildResultCode.Success);

            packageLocation.GetFiles("AdditionalManifestFile.txt").Should().HaveCount(1);
        }
        [TestMethod]
        public void package_as_single_file_adds_additionalmanifestfile_to_packagelocation()
        {
            var packageLocation = new DirectoryInfo(Path.Combine(TestContext.TestDir, TestContext.TestName));
            var actualResult = TestBuild(
                new MsBuildParameters()
                {
                    BuildFile = ProjectLocator.GetProjectFile(WebApplicationProjectName),
                    DeployOnBuild = true,
                },
                new MsDeployPublishProfile()
                {
                    WebPublishMethod = "Package",
                    PackageAsSingleFile = true,
                    PackageLocation = packageLocation,
                    GenerateSampleDeployScript =false,
                });

            actualResult.Exception.Should().BeNull();
            actualResult.OverallResult.Should().Be(BuildResultCode.Success);

            packageLocation.GetFiles("AdditionalManifestFile.txt").Should().HaveCount(1);
        }
        [TestMethod]
        public void package_places_content_in_directory_named_website()
        {
            var packageLocation = new DirectoryInfo(Path.Combine(TestContext.TestDir, TestContext.TestName));
            var actualResult = TestBuild(
                new MsBuildParameters()
                {
                    BuildFile = ProjectLocator.GetProjectFile(WebApplicationProjectName),
                    DeployOnBuild = true,
                },
                new MsDeployPublishProfile()
                {
                    WebPublishMethod = "Package",
                    PackageAsSingleFile = false,
                    PackageLocation = packageLocation,
                });

            actualResult.Exception.Should().BeNull();
            actualResult.OverallResult.Should().Be(BuildResultCode.Success);

            //Default package path = website
            packageLocation.GetDirectories("website", SearchOption.AllDirectories).Should().HaveCount(1);
        }
        [TestMethod]
        public void package_places_content_in_directory_defined_by_packagepath()
        {
            var packageLocation = new DirectoryInfo(Path.Combine(TestContext.TestDir, TestContext.TestName));
            string packagePath = "TestPackagePath";
            var actualResult = TestBuild(
                new MsBuildParameters()
                {
                    BuildFile = ProjectLocator.GetProjectFile(WebApplicationProjectName),
                    DeployOnBuild = true,
                },
                new MsDeployPublishProfile()
                {
                    WebPublishMethod = "Package",
                    PackageAsSingleFile = false,
                    PackageLocation = packageLocation,
                    PackagePath = packagePath,
                });

            actualResult.Exception.Should().BeNull();
            actualResult.OverallResult.Should().Be(BuildResultCode.Success);

            //Default package path = website
            packageLocation.GetDirectories(packagePath, SearchOption.AllDirectories).Should().HaveCount(1);
        }

        private BuildResult TestBuild(MsBuildParameters msBuildParameters)
        {
            return TestBuild(msBuildParameters, null);
        }
        private BuildResult TestBuild(MsBuildParameters msBuildParameters, MsDeployPublishProfile msDeployPublishProfile)
        {
            msBuildParameters = msBuildParameters ?? new MsBuildParameters();
            msDeployPublishProfile = msDeployPublishProfile ?? new MsDeployPublishProfile();
            if (msDeployPublishProfile.PackageLocation == null)
            {
                //Default
                msDeployPublishProfile.PackageLocation = new DirectoryInfo(Path.Combine(TestContext.TestDir, TestContext.TestName));
            }
            //TestContext.AddResultFile(parameters.PackageLocation.FullName);
            return buildRunner.TestBuild(msBuildParameters, msDeployPublishProfile);
        }
    }
}
