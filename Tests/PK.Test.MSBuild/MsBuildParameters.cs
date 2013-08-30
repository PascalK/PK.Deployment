using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PK.Test.MSBuild
{
    public class MsBuildParameters : MSBuildParametersBase
    {
        public FileInfo BuildFile { get; set; }
        public string Configuration
        {
            get
            {
                return Get("Configuration");
            }
            set
            {
                Set("Configuration", value);
            }
        }
        public string Platform
        {
            get
            {
                return Get("Platform");
            }
            set
            {
                Set("Platform", value);
            }
        }
        public bool? DeployOnBuild
        {
            get
            {
                return GetBool("DeployOnBuild");
            }
            set
            {
                SetBool("DeployOnBuild", value);
            }
        }
        public string PublishProfile
        {
            get
            {
                return Get("PublishProfile");
            }
            set
            {
                Set("PublishProfile", value);
            }
        }
        public int? VisualStudioVersion
        {
            get
            {
                return GetInt("VisualStudioVersion");
            }
            set
            {
                SetInt("VisualStudioVersion", value);
            }
        }
        public string PackageFileName
        {
            get
            {
                return Get("PackageFileName");
            }
            set
            {
                Set("PackageFileName", value);
            }
        }

        public MsBuildParameters()
        {
            Configuration = "Release";
            Platform = "AnyCPU";
            DeployOnBuild = true;
            //PublishProfile="Package";
            VisualStudioVersion = 12;
        }
    }
}
