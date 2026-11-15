using Godot;

namespace GArkanoid.Systems
{
    public class SettingsData
    {
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SFXVolume { get; set; }
        public Vector2I Resolution { get; set; }
        public bool Fullscreen { get; set; }
        public static SettingsData Default => new SettingsData
        {
            MasterVolume = -6.0f,
            MusicVolume  = -6.0f,
            SFXVolume    = -6.0f,
            Resolution   = new Vector2I(640, 360),
            Fullscreen   = false,
        };

        public SettingsData() { }
        public SettingsData(SettingsData other)
        {
            MasterVolume = other.MasterVolume;
            MusicVolume  = other.MusicVolume;
            SFXVolume    = other.SFXVolume;
            Resolution   = other.Resolution;
            Fullscreen   = other.Fullscreen;
        }
    }
}