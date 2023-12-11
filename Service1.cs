using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Configuration;

namespace HelloWindowService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;
        private string serviceName;
        private string logFilePath;

        public Service1()
        {
            InitializeComponent();

            // Get the service name and log file path from app.config
            this.serviceName = ConfigurationManager.AppSettings["ServiceName"] ?? "ACS_SRB";
            this.logFilePath = ConfigurationManager.AppSettings["LogFilePath"] ?? "C:\\Logs\\HelloWorldServiceLog.txt";

            // Set the service name
            this.ServiceName = serviceName;
        }

        protected override void OnStart(string[] args)
        {
            // Set up the timer to trigger every 10 seconds
            timer = new Timer();
            timer.Interval = 10000; // 10 seconds
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Perform the action you want to repeat every 10 seconds
            LogMessage("Hello, World! - " + DateTime.Now);
        }

        protected override void OnStop()
        {
            // Stop the timer when the service is stopped
            timer.Stop();
            timer.Dispose();
        }

        private void LogMessage(string message)
        {
            try
            {
                // Ensure the directory of the log file exists
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Append the message to the log file
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during logging
                Console.WriteLine($"Error while logging: {ex.Message}");
            }
        }
    }
}
