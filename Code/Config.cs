namespace GArkanoid.Entities
{
    
    public static class Config
    {
        // TODO: create a resource script for all of the config
        public const float MaxSpeed = 500; 
        public const float MinSpeed = 0;
        public const int InitialLives = 3;
        public const int InitialScore = 0;

        public const string PauseActionName = "Pause";
        public const string LaunchActionName = "Launch";
        public const string ClickToggleActionName = "ClickToggle";

        public const string axisNameLeft = "Left";
        public const string axisNameRight = "Right";


        public const string AudioSection = "Audio";
        public const string VideoSection = "Video";
        public const string MusicSection = "Music";
        public const string MasterSection = "Master";
        public const string SFXSection = "SFX";
        public const string MusicVolumeKey = MusicSection + "Volume";
        public const string MasterVolumeKey = MasterSection + "Volume";
        public const string SFXVolumeKey = SFXSection + "Volume";
        public const string ResolutionKey = "Resolution";
        public const string FullscreenKey = "Fullscreen";


        
        
    }

}