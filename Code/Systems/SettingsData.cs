using Godot;

namespace Garkanoid.Systems
{
    // MODEL
        public class SettingsData
    {

        // return default data 
        public static SettingsData Default
        {
            get
            {
                return new SettingsData()
                {
                    MasterVolume = -6.0f,
                    MusicVolume = -6.0f,
                    SFXVolume = -6.0f,
                    Resolution = new Vector2I(640, 360), // replace this magic number
                    Fullscreen = false
                };
            }
        }
        public float MasterVolume
        {
            get;
            set;
        }

        public float MusicVolume
        {
            get;
            set;
        }

        public float SFXVolume
        {
            get;
            set;
        }


        public Vector2I Resolution
        {
            get;
            set;
        }

        public bool Fullscreen
        {
            get;
            set;
        }

        public SettingsData(){}

        // Copy-Constructor: copies existing object to a new one (other: object to be copied) 
        public SettingsData(SettingsData other)
        {
            MasterVolume = other.MasterVolume;
            MusicVolume = other.MusicVolume;
            SFXVolume = other.SFXVolume;
            Resolution = other.Resolution;
            Fullscreen = other.Fullscreen;
        }
    }

    }
