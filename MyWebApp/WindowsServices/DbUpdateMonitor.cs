using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using EmployeeDataAccess;

namespace WindowsServices
{
    public class DbUpdateMonitor
    {
        private readonly Timer _timer;
        private static int timeIntervalInMinutes = 2;

        public DbUpdateMonitor()
        {
            _timer = new Timer(60000*timeIntervalInMinutes) {AutoReset = true};
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var intervalTime = DateTime.Now.Subtract(TimeSpan.FromMinutes(timeIntervalInMinutes));
            var timeNow = DateTime.Now;
            var records = new List<Employee>();

            using (var ctx = new EmployeeDBEntities())
            {
                records =
                    ctx.Employees.Where(emp => emp.UpdateTimestamp >= intervalTime && emp.UpdateTimestamp <= timeNow).ToList();
            }

            if (records.Any())
            {
                foreach (var record in records)
                {
                    string[] line = {$"New Employee record added/updated: Name[{record.FirstName} {record.LastName}] Salary:[{record.Salary}] Gender:[{record.Gender}]"};
                    // insert into file
                    File.AppendAllLines(@"C:\test\test.txt", line);
                }
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}