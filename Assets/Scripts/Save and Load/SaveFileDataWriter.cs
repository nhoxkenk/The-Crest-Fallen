using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileDataWriter
{
    public string saveFileDataPath = "";
    public string saveFileName = "";

    public bool CheckIfSaveFileExists()
    {
        return false;
    }

    public void DeleteSaveFile()
    {

    }

    public void CreateNewCharacterSaveFile(CharacterSaveData saveData)
    {
        string path = Path.Combine(saveFileDataPath, saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string dataStore = JsonUtility.ToJson(saveData);

            using(FileStream stream = new FileStream(path, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataStore);
                }
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    public CharacterSaveData LoadCharacterSaveFile()
    {
        CharacterSaveData saveData = null;

        string path = Path.Combine(saveFileDataPath , saveFileName);

        if(File.Exists(path))
        {
            try
            {
                string dataLoad = "";

                using(FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataLoad = reader.ReadToEnd();
                    }
                }

                saveData = JsonUtility.FromJson<CharacterSaveData>(dataLoad);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        return saveData;
    }
}
