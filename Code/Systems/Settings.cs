using Godot;

namespace Garkanoid.Systems
{
    public partial class Settings
    {
        public const string SettingsFilePath= "user://settings.cfg";
        private SettingsData _settingsData = null;

        public static void ApplySettings(SettingsData settingsData)
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), settingsData.MasterVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), settingsData.MusicVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), settingsData.SFXVolume);

            DisplayServer.WindowSetSize(settingsData.Resolution);
            DisplayServer.WindowMode windowMode = settingsData.Fullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed;
            DisplayServer.WindowSetMode(windowMode);
        }



        public void Save()
        {
            ConfigFile settingsFile = new ConfigFile();
            settingsFile.SetValue("Audio", "MasterVolume", _settingsData.MasterVolume);
            settingsFile.SetValue("Audio", "MusicVolume", _settingsData.MusicVolume);
            settingsFile.SetValue("Audio", "SFXVolume", _settingsData.SFXVolume);

            settingsFile.SetValue("Video", "Resolution", _settingsData.Resolution);
            settingsFile.SetValue("Video", "Fullscreen", _settingsData.Fullscreen);

            settingsFile.Save(SettingsFilePath);
        }

        // TODO: Return loaded data 
        public SettingsData Load()
        {
            SettingsData result = null;

            ConfigFile savedSettings = new ConfigFile();

            Error readError = savedSettings.Load(SettingsFilePath);

            SettingsData defaultData = SettingsData.Default;
            if (readError != Error.Ok)
            {
             result = defaultData;
             return result;
            }

            // initialization list method
            return new SettingsData()
            {
              MasterVolume = (float) savedSettings.GetValue("Audio", "MasterVolume", defaultData.MasterVolume),
              MusicVolume = (float) savedSettings.GetValue("Audio", "MusicVolume", defaultData.MusicVolume),
              SFXVolume = (float) savedSettings.GetValue("Audio", "SFXVolume", defaultData.SFXVolume), 

              Resolution = (Vector2I) savedSettings.GetValue("Video", "Resolution", defaultData.Resolution),
              Fullscreen = (bool) savedSettings.GetValue("Video", "Fullscreen", defaultData.Fullscreen),

            };
        }
    }
    
}