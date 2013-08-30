//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace PK.Deployment
//{
//    [TestClass]
//    public class DiagnoseTestWebApplication : TestWebApplication
//    {
//        [TestMethod]
//        public void Diagnose()
//        {
//            //var types = BuiltInTypesCache.GetTypes(Assembly.GetExecutingAssembly());
//            //foreach (var type in types)
//            //{
//            //    Console.WriteLine(type);
//            //}

//            //Microsoft.Web.Deployment.DeploymentManager.DoNotCreateProcessForDacFx = true;
//            //Console.WriteLine(Assembly.GetExecutingAssembly().Location);
//            string currentLocation = Assembly.GetExecutingAssembly().Location;
//            DirectoryInfo currentDirectory = Directory.GetParent(currentLocation);
//            Console.WriteLine(Utility.GetProjectFile("Test.WebApp").FullName);

//            Console.WriteLine(string.Format("ResultsDirectory: '{0}'", TestContext.ResultsDirectory)); //In
//            Console.WriteLine(string.Format("TestDeploymentDir: '{0}'", TestContext.TestDeploymentDir)); //Out
//            Console.WriteLine(string.Format("TestDir: '{0}'", TestContext.TestDir));
//            Console.WriteLine(string.Format("TestName: '{0}'", TestContext.TestName));
//            Console.WriteLine(string.Format("TestResultsDirectory: '{0}'", TestContext.TestResultsDirectory));
//            Console.WriteLine(string.Format("TestRunDirectory: '{0}'", TestContext.TestRunDirectory));
//            Console.WriteLine(string.Format("TestRunResultsDirectory: '{0}'", TestContext.TestRunResultsDirectory));
//            Console.WriteLine(string.Format("Combined: '{0}'", Path.Combine(TestContext.TestDir, TestContext.TestName)));
//        }

//    }
//}
