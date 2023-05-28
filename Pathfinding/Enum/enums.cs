namespace Global.Enum
{
    public enum NodeStatus
    {
        Open = 0,
        Blocked,
        LightTerrain,
        MediumTerrain,
        HeavyTerrain
    }

    public enum MouseEventTypes
    {
        Enter,
        Exit,
        Hover,
        ButtonDown,
        ButtonUp,
        ButtonHeld,
        Delta
    }

    public enum MouseButtons
    {
        Left,
        Right,
        Middle,
        Empty
    }

    #region Commander

    public enum CommandType
    {
        TimerCommand,
        SceneCommand,
        SoundCommand,
        PlayerInteractCommand,
        PlantCommand
    }

    public enum CropType
    {
        None = -1,
        Corn,
        PlowedLand,
        Cashed,
        Tomato,
        Pepper,
        Cabbage
    }
    
    public enum SoundType
    {
        SoundFX,
        BackgroundMusic
    }

    public enum Sound
    {
        WorldMusicDay,
        WorldMusicNight,
        BuildingMusicDay,
        BuildingMusicNight
    }

    public enum Scene
    {
        First,
        Second
    }
    #endregion
}