using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace PK.Test.MSBuild
{
    public class MsBuildToConsoleLogger : Logger
    {
        public override void Initialize(IEventSource eventSource)
        {
            eventSource.AnyEventRaised += eventSource_AnyEventRaised;
            eventSource.ErrorRaised += eventSource_ErrorRaised;
        }

        void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            Console.Error.WriteLine(e.Message);
        }

        void eventSource_AnyEventRaised(object sender, BuildEventArgs e)
        {
            //BuildEventContext
            //HelpKeyword
            //Message
            //SenderName
            //ThreadId
            //Timestamp
            Console.Out.WriteLine(e.Message);
        }
    }
}
