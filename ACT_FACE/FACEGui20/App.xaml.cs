using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;


using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Async;
using Serilog.Sinks.Grafana.Loki;
using System.Threading.Tasks;

namespace Act.Face.FACEGui20
{
   
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // give the mutex a  unique name
        private const string MutexName = "FACEGui20Win";
        // declare the mutex
        private readonly Mutex _mutex;
        // overload the constructor
        bool createdNew;

    
        public App()
        {
            // overloaded mutex constructor which outs a boolean
            // telling if the mutex is new or not.
            // see http://msdn.microsoft.com/en-us/library/System.Threading.Mutex.aspx
            _mutex = new Mutex(true, MutexName, out createdNew);
            if (!createdNew)
            {
                // if the mutex already exists, notify and quit
                MessageBox.Show("This program is already running","",MessageBoxButton.OK,MessageBoxImage.Information);
                Application.Current.Shutdown(0);
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!createdNew) return;


            var monitor = new MonitorConfiguration();

            //Serilog.Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Verbose()
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .WriteTo.GrafanaLoki(
            //        "http://localhost:3100",
            //        labels: new List<LokiLabel>() { new LokiLabel { Key = "app", Value = "FACEGui" } },
            //        credentials: null)
            //    //.WriteTo.File("./logs/log.txt",
            //    //    rollingInterval: RollingInterval.Day,
            //    //    fileSizeLimitBytes: 1000000)
            //    //.WriteTo.File(new CompactJsonFormatter(),
            //    //    "./logs/structed.json")
            //    //.WriteTo.Async(a=>a.Seq("http://localhost:5341"), monitor:monitor)
            //    .CreateLogger();

          
            //Serilog.Log.ForContext("Application", "FACEGui20Win").Information("Serilog up!");

            //Serilog.Debugging.SelfLog.Enable(Console.Error);

           

            // overload the OnStartup so that the main window 
            // is constructed and visible
            FACEGui20Win mw = new FACEGui20Win();
            mw.Show();
        }

      
       
    }

    // Example check: log message to an out of band alarm channel if logging is showing signs of getting overwhelmed
   

    class MonitorConfiguration : IAsyncLogEventSinkMonitor
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static CancellationToken token = cancellationTokenSource.Token;

        public void StartMonitoring(IAsyncLogEventSinkInspector inspector) =>
            ForEachWithDelay(() => ExecuteAsyncBufferCheck(inspector), 60000);

        public void StopMonitoring(IAsyncLogEventSinkInspector inspector)
        {
            cancellationTokenSource.Cancel();
            /* reverse of StartMonitoring */
        }

        public static async void ForEachWithDelay(Func<bool> action, int interval)
        {

            await Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {


                    try
                    {
                        action();
                    }
                    catch
                    {


                    }

                    System.Threading.Thread.Sleep(interval);
                }
            }, token);


            System.Diagnostics.Debug.WriteLine("ForEachWithDelay fineeeeeee");


        }

        bool ExecuteAsyncBufferCheck(IAsyncLogEventSinkInspector inspector)
        {
            var usagePct = inspector.Count * 100 / inspector.BufferSize;
            if (usagePct > 50) Serilog.Debugging.SelfLog.WriteLine("Log buffer exceeded {0:p0} usage (limit: {1})", usagePct, inspector.BufferSize);

            System.Diagnostics.Debug.WriteLine("run ExecuteAsyncBufferCheck");

            return true;
        }

    }






}
