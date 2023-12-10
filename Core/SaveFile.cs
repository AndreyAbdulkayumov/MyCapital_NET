using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Core
{
    public static class SaveFile
    {
        // Файл сохраняется в скрытой дериктории /data/user/0/com.XSoft.MyCapital/files/.config
        // Для того, чтобы найти этот файл вручную на накопителе необходимы root права

        private readonly static string FilesFolder_Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);


        public static void Save(List<CapitalValue> AllParts, string FilePath)
        {
            try
            {
                var path = Path.Combine(FilesFolder_Path, FilePath);

                ClearDataFromFile(FilePath);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    JsonSerializer.Serialize(stream, AllParts, options);
                }
            }

            catch (Exception error)
            {
                ClearDataFromFile(FilePath);

                throw new Exception("Ошибка записи в файл сохранений. Файл сохранений был очищен.\n\n" + error.Message);
            }
        }

        public static List<CapitalValue>? GetAllParts(string FilePath)
        {
            try
            {
                List<CapitalValue>? AllParts;

                var path = Path.Combine(FilesFolder_Path, FilePath);

                if (File.Exists(path) == false)
                {
                    File.Create(path).Close();
                }

                string Text = File.ReadAllText(path);

                // Если файл пуст, то выходим
                if (Text == String.Empty)
                {
                    return null;
                }

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    AllParts = JsonSerializer.Deserialize<List<CapitalValue>>(stream);
                }

                return AllParts;
            }
            
            catch (Exception error)
            {
                ClearDataFromFile(FilePath);

                throw new Exception("Ошибка чтения файла сохранений. Файл сохранений был очищен.\n\n" + error.Message);
            }
        }

        private static void ClearDataFromFile(string FilePath)
        {
            var path = Path.Combine(FilesFolder_Path, FilePath);

            File.WriteAllText(path, String.Empty);
        }
    }
}
