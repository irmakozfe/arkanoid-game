using GArkanoid.Entities;
using Godot;

namespace Garkanoid.Systems
{
    public partial class Settings
    {
        public const string SettingsFilePath= "user://settings.cfg";
        private SettingsData _settingsData = null;

        public static void ApplySettings(SettingsData settingsData)
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.MasterSection), settingsData.MasterVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.MusicSection), settingsData.MusicVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.SFXSection), settingsData.SFXVolume);

            DisplayServer.WindowSetSize(settingsData.Resolution);
            DisplayServer.WindowMode windowMode = settingsData.Fullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed;
            DisplayServer.WindowSetMode(windowMode);
        }



        public void Save()
        {
            ConfigFile settingsFile = new ConfigFile();
            settingsFile.SetValue(Config.AudioSection, Config.MasterVolumeKey, _settingsData.MasterVolume);
            settingsFile.SetValue(Config.AudioSection, Config.MusicVolumeKey, _settingsData.MusicVolume);
            settingsFile.SetValue(Config.AudioSection, Config.SFXVolumeKey, _settingsData.SFXVolume);

            settingsFile.SetValue(Config.VideoSection, Config.ResolutionKey, _settingsData.Resolution);
            settingsFile.SetValue(Config.VideoSection, Config.FullscreenKey, _settingsData.Fullscreen);

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
              MasterVolume = (float) savedSettings.GetValue(Config.AudioSection, Config.MasterVolumeKey, defaultData.MasterVolume),
              MusicVolume = (float) savedSettings.GetValue(Config.AudioSection, Config.MusicVolumeKey, defaultData.MusicVolume),
              SFXVolume = (float) savedSettings.GetValue(Config.AudioSection, Config.SFXVolumeKey, defaultData.SFXVolume), 

              Resolution = (Vector2I) savedSettings.GetValue(Config.VideoSection, Config.ResolutionKey, defaultData.Resolution),
              Fullscreen = (bool) savedSettings.GetValue(Config.VideoSection, Config.ResolutionKey, defaultData.Fullscreen),

            };
        }
    }
    
}