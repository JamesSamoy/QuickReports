using System;
using Topshelf;

namespace WindowsServices
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<DbUpdateMonitor>(s =>
                {
                    s.ConstructUsing(dbUpdateMonitor => new DbUpdateMonitor());
                    s.WhenStarted(dbUpdateMonitor => dbUpdateMonitor.Start());
                    s.WhenStopped(dbUpdateMonitor => dbUpdateMonitor.Stop());
                });
                
                x.RunAsLocalSystem();
                
                x.SetServiceName("DbUpdateMonitorService");
                x.SetDisplayName("Database Update Monitor Service");
                x.SetDescription("Checks the db and notifies once new record is added or updated");
            });

            int exitCodeValue = (int) Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}