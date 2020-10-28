using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace TelegramCw
{
    /// <summary>
    /// Класс, производящий обработку события запуска всех процессов.
    /// </summary>
    public class ProcessListener
    {
        /// <summary>
        /// Думаю, и так понятно.
        /// </summary>
        private const string EXE = ".exe";
        
        /// <summary>
        /// Следит за появлением нового процесса.
        /// </summary>
        private ManagementEventWatcher _startWatch;

        /// <summary>
        /// Список заблокированных процессов.
        /// </summary>
        private List<string> _blockedProcesses;
        
        public ProcessListener(List<string> blockedProcesses)
        {
            _startWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));

            _blockedProcesses = blockedProcesses;
            
            _startWatch.EventArrived += OnProcessStarted;
            _startWatch.Start();
        }

        /// <summary>
        /// Остановить отслеживание запуска процессов.
        /// </summary>
        public void StopListen() => _startWatch.Stop();

        /// <summary>
        /// Остановить процесс.
        /// </summary>
        /// <param name="name">Имя процесса.</param>
        public void StopProcess(string name)
        {
            var processes = Process.GetProcessesByName(name);

            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        /// <summary>
        /// Обработчик события начала нового процесса.
        /// </summary>
        private void OnProcessStarted(object sender, EventArrivedEventArgs e)
        {
            var name = e.NewEvent.Properties["ProcessName"].Value
                .ToString()
                .Replace(EXE, string.Empty);
            
            if (_blockedProcesses.Contains(name))
            {
                StopProcess(name);
            }
        }
    }
}