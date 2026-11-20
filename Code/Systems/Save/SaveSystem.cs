using System;
using Godot;
using Godot.Collections;

namespace Garkanoid.Save
{
    public class SaveSystem
    {
        public const string SaveFileExtension = ".json";
        public const string SaveDirectory = "user://saves/";
        public const string QuickSaveSlot = "QuickSave"; // File name for quick save file 
        string relativePath;

        public Dictionary<string, Variant> LatestLoadData
        {
            get;
            private set;
        } = null;

        //convenient method for quick saving
        public bool QuickSave(Dictionary<string, Variant> saveData)
        {
            return Save(QuickSaveSlot, saveData);
        }

        public Dictionary<string, Variant> QuickLoad()
        {
            return Load(QuickSaveSlot);
        }

        public bool Save(string saveSlot, Dictionary<string, Variant> saveData)
        {
            string SaveDirectoryPath = ProjectSettings.GlobalizePath(SaveDirectory);

            if (!DirAccess.DirExistsAbsolute(SaveDirectoryPath))
            {
                DirAccess.MakeDirRecursiveAbsolute(SaveDirectoryPath);
            } //makes sure the save directory exists

            string saveFilePath = SaveDirectory + saveSlot + SaveFileExtension;

            //Serialize save data
            string jsonData = Json.Stringify(saveData);

            using(FileAccess file = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Write))
            {
                if(file == null)
                {
                    Error error = FileAccess.GetOpenError();
                    GD.PushError($"Failed to open save file for writing: {saveFilePath} {error}");
                    return false;
                } 

                file.StoreString(jsonData);
                return true;
            }

        }

        public Dictionary<string, Variant> Load(string saveSlotName)
        {
            string saveFilePath = SaveDirectory + saveSlotName + SaveFileExtension;

            if (!FileAccess.FileExists(saveFilePath))
            {
                GD.PushError($"Save file not found: {saveFilePath}");
                return null;
            }

            using (FileAccess saveFile = FileAccess.Open(saveFilePath, FileAccess.ModeFlags.Read))
            {
                string serializedData = saveFile.GetAsText();
                LatestLoadData = (Dictionary<string, Variant>) Json.ParseString(serializedData);

            }

            return LatestLoadData;
        }

        public void LoadingDone()
        {
            LatestLoadData = null;
        }

    }
}