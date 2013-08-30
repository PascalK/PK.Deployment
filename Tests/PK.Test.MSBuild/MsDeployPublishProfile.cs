using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PK.Test.MSBuild
{
    public class MsDeployPublishProfile : MSBuildParametersBase
    {
        public string WebPublishMethod
        {
            get
            {
                return Get("WebPublishMethod");
            }
            set
            {
                Set("WebPublishMethod", value);
            }
        }
        public bool? PackageAsSingleFile
        {
            get
            {
                return GetBool("PackageAsSingleFile");
            }
            set
            {
                SetBool("PackageAsSingleFile", value);
            }
        }
        public bool? UseMsdeployExe
        {
            get
            {
                return GetBool("UseMsdeployExe");
            }
            set
            {
                SetBool("UseMsdeployExe", value);
            }
        }
        public bool? AutoParameterizationWebConfigConnectionStrings
        {
            get
            {
                return GetBool("AutoParameterizationWebConfigConnectionStrings");
            }
            set
            {
                SetBool("AutoParameterizationWebConfigConnectionStrings", value);
            }
        }
        public bool? GenerateSampleDeployScript
        {
            get
            {
                return GetBool("GenerateSampleDeployScript");
            }
            set
            {
                SetBool("GenerateSampleDeployScript", value);
            }
        }
        public DirectoryInfo PackageLocation
        {
            get
            {
                return Get<DirectoryInfo>("PackageLocation", s => new DirectoryInfo(s));
            }
            set
            {
                Set("PackageLocation", value == null ? null : value.FullName);
            }
        }
        public string PackagePath
        {
            get
            {
                return Get("PackagePath");
            }
            set
            {
                Set("PackagePath", value);
            }
        }
    }
}