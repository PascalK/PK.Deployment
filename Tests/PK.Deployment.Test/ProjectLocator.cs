using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PK.Deployment
{
    public class ProjectLocator
    {
        public static FileInfo GetProjectFile(string projectName, int parentFolderSearchLimit = 5)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException("projectName");
            }
            if (!Path.GetExtension(projectName).EndsWith("proj"))
            {
                projectName = projectName + @"*csproj";
            }

            return GetFile(projectName, parentFolderSearchLimit);

        }
        public static FileInfo GetSolutionFile(string solutionName, int parentFolderSearchLimit = 5)
        {
            if (string.IsNullOrWhiteSpace(solutionName))
            {
                throw new ArgumentNullException("solutionName");
            }
            if (Path.GetExtension(solutionName) != "sln")
            {
                solutionName = solutionName + @"\.sln";
            }

            return GetFile(solutionName, parentFolderSearchLimit);
        }

        private static FileInfo GetFile(string fileName, int parentFolderSearchLimit)
        {
            int directoryLevel = 0;
            string currentLocation = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo directory = Directory.GetParent(currentLocation);
            while (directoryLevel < parentFolderSearchLimit)
            {
                IEnumerable<FileInfo> foundFiles;
                foundFiles = directory.GetFiles(fileName, SearchOption.AllDirectories);
                if (foundFiles.Any())
                {
                    return foundFiles.Single();
                }
                directory = directory.Parent;
                directoryLevel++;
            }

            throw new FileNotFoundException("Projectfile not found", fileName);
        }
    }
}
