using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NUGETManager
{
    public class SaveManager
    {
        public static string AppDataPath;
        static SaveManager()
        {
            AppDataPath = "data";
            Directory.CreateDirectory(AppDataPath);
            Directory.CreateDirectory(Path.Combine(AppDataPath, "Projects"));
        }

        public static void Save(string name, object toSave)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream file = new FileStream(Path.Combine(AppDataPath, name + ".bin"), FileMode.Create))
            {
                formatter.Serialize(file, toSave);
            }
        }

        public static Type Load<Type>(string name)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Path.Combine(AppDataPath, name + ".bin");
            Type obj = default;
            if (File.Exists(path))
                using (FileStream file = new FileStream(path, FileMode.Open))
                {
                    obj = (Type)formatter.Deserialize(file);
                }

            return obj;
        }
    }
}