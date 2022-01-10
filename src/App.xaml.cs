﻿using NLog;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace fcrd
{
    public partial class App : Application
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject);

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception);
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception);
                e.SetObserved();
            };
        }

        private void LogUnhandledException(Exception exception)
        {
            MessageBox.Show(exception.Message);
            _logger.Error(exception);
            _logger.Error(exception.InnerException);
            _logger.Error(exception.StackTrace);
        }

        private void Failure(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "error.log";
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                streamWriter.WriteLine("Error @ " + DateTime.Now.ToString("R"));
                streamWriter.WriteLine(e.Exception.ToString());
            }
            e.Handled = true;
            int num = (int)MessageBox.Show("Произолша ошибка, отправьте разработчикусодержимое файла " + path, "Ошибка!");
        }
    }
}