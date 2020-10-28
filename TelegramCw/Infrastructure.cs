using System;
using System.Collections.Generic;

namespace TelegramCw
{
    /// <summary>
    /// Класс, хранящий в себе постоянные данные, необходимые для работы системы.
    /// </summary>
    public static class Infrastructure
    {
        /// <summary>
        /// Конфигурационные данные.
        /// </summary>
        [Serializable]
        public class Config
        {
            /// <summary>
            /// Токен, необходимый для идентификации бота.
            /// </summary>
            public string Token { get; set; }

            /// <summary>
            /// Список заблокированных процессов.
            /// </summary>
            public List<string> BlockedProcesses { get; } = new List<string>();
        }

        /// <summary>
        /// Данные, необходимые для организации хранения данных.
        /// </summary>
        public static class DataStore
        {
            /// <summary>
            /// Имя файла, хранящего в себе некоторую информацию, необходимую для процесса работы программы.
            /// </summary>
            public const string TMP_IMAGE_FILE_NAME = "image.png";

            /// <summary>
            /// Имя конфигурационного файла.
            /// </summary>
            public const string CONFIG_FILE_NAME = "config";
        }

        /// <summary>
        /// Команды, передаваемые пользователем и обрабатываемые на сервере.
        /// </summary>
        public static class Commands
        {
            /// <summary>
            /// Команда получения всех запущенных процессов.
            /// </summary>
            public const string GET_PROCESSES = "/get_processes";

            /// <summary>
            /// Команда выхода из системы.
            /// </summary>
            public const string UNLOG = "/unlog";

            /// <summary>
            /// Команда получения скриншота.
            /// </summary>
            public const string GET_SCREEN = "/get_screen";

            /// <summary>
            /// Команда получения снимка с камеры.
            /// </summary>
            public const string GET_CAM = "/get_cam";

            /// <summary>
            /// Команда получения списка USB-устройств.
            /// </summary>
            public const string GET_USB = "/get_usb";

            /// <summary>
            /// Команда добавления процесса в список заблокированных.
            /// </summary>
            public const string ADD_BLOCK = "/add_block";

            /// <summary>
            /// Команда удаления процесса из списка заблокированных.
            /// </summary>
            public const string REMOVE_BLOCK = "/remove_block";
        }
    }
}