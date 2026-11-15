using GArkanoid.Entities;
using Godot;

namespace GArkanoid.Systems                                  
{
	public static class Settings                            
	{
		public const string SettingsFilePath= "user://settings.cfg";
		private static SettingsData _current = SettingsData.Default;
		public static SettingsData Current => new SettingsData(_current);
		public static void Initialize()
		{
			_current = Load();
			Apply(_current);
		}

		public static void Apply(SettingsData settingsData)  
		{
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.MasterSection), settingsData.MasterVolume);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.MusicSection), settingsData.MusicVolume);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex(Config.SFXSection), settingsData.SFXVolume);

			if (settingsData.Fullscreen)
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				return;
			}

			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			DisplayServer.WindowSetSize(settingsData.Resolution);

			int screen = DisplayServer.WindowGetCurrentScreen();
			Vector2I screenPosition = DisplayServer.ScreenGetPosition(screen);
			Vector2I screenSize = DisplayServer.ScreenGetSize(screen);
			DisplayServer.WindowSetPosition(screenPosition + (screenSize - settingsData.Resolution) / 2);
		}



		public static void Save(SettingsData settingsData)  
		{
			_current = new SettingsData(settingsData);      

			ConfigFile settingsFile = new ConfigFile();
			settingsFile.SetValue(Config.AudioSection, Config.MasterVolumeKey, settingsData.MasterVolume);   // ← DEĞİŞTİ (_settingsData → settingsData)
			settingsFile.SetValue(Config.AudioSection, Config.MusicVolumeKey, settingsData.MusicVolume);
			settingsFile.SetValue(Config.AudioSection, Config.SFXVolumeKey, settingsData.SFXVolume);

			settingsFile.SetValue(Config.VideoSection, Config.ResolutionKey, settingsData.Resolution);
			settingsFile.SetValue(Config.VideoSection, Config.FullscreenKey, settingsData.Fullscreen);

			settingsFile.Save(SettingsFilePath);
		}

		// TODO: Return loaded data
		private static SettingsData Load()                    
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
			  Fullscreen = (bool) savedSettings.GetValue(Config.VideoSection, Config.FullscreenKey, defaultData.Fullscreen),   // ← DEĞİŞTİ (ResolutionKey → FullscreenKey)

			};
		}
	}

}
