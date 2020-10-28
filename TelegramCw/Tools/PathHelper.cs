using System.Reflection;

namespace TelegramCw.Tools
{
    public static class PathHelper
    {
        /// <summary>
        /// Возвращает путь к файлу, необходимому для работы программы.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        public static string GetFilePath(string fileName)
        {
            var path = Assembly.GetEntryAssembly()?.Location;
            
            var parts = path.Split('\\');

            path = string.Empty;
            
            for (int i = 0; i < parts.Length - 1; i++)
            {
                path += parts[i] + "\\";
            }

            path += fileName;
            
            return path;
        }
    }
}