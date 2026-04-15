using Godot;

/// Autoload singleton — persists settings (volume, display) using ConfigFile.
/// Apply settings immediately when changed; also call SaveSettings() to persist.
public partial class SettingsManager : Node
{
    public static SettingsManager Instance { get; private set; } = null!;

    // Backing fields
    private float _masterVolume = 1f;
    private float _musicVolume = 0.8f;
    private float _sfxVolume = 1f;
    private bool _fullscreen = false;
    private float _mouseSensitivity = 1f;

    public float MasterVolume
    {
        get => _masterVolume;
        set { _masterVolume = Mathf.Clamp(value, 0f, 1f); ApplyMasterVolume(); }
    }

    public float MusicVolume
    {
        get => _musicVolume;
        set { _musicVolume = Mathf.Clamp(value, 0f, 1f); ApplyMusicVolume(); }
    }

    public float SfxVolume
    {
        get => _sfxVolume;
        set { _sfxVolume = Mathf.Clamp(value, 0f, 1f); ApplySfxVolume(); }
    }

    public bool Fullscreen
    {
        get => _fullscreen;
        set { _fullscreen = value; ApplyFullscreen(); }
    }

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set => _mouseSensitivity = Mathf.Clamp(value, 0.1f, 5f);
    }

    private const string SavePath = "user://settings.cfg";

    public override void _Ready()
    {
        Instance = this;
        LoadSettings();
    }

    public void SaveSettings()
    {
        var cfg = new ConfigFile();
        cfg.SetValue("audio", "master_volume", _masterVolume);
        cfg.SetValue("audio", "music_volume", _musicVolume);
        cfg.SetValue("audio", "sfx_volume", _sfxVolume);
        cfg.SetValue("display", "fullscreen", _fullscreen);
        cfg.SetValue("input", "mouse_sensitivity", _mouseSensitivity);
        cfg.Save(SavePath);
    }

    private void LoadSettings()
    {
        var cfg = new ConfigFile();
        if (cfg.Load(SavePath) != Error.Ok)
        {
            ApplyAll(); // apply defaults
            return;
        }

        _masterVolume = (float)cfg.GetValue("audio", "master_volume", 1f);
        _musicVolume  = (float)cfg.GetValue("audio", "music_volume",  0.8f);
        _sfxVolume    = (float)cfg.GetValue("audio", "sfx_volume",    1f);
        _fullscreen   = (bool)cfg.GetValue("display", "fullscreen",   false);
        _mouseSensitivity = (float)cfg.GetValue("input", "mouse_sensitivity", 1f);
        ApplyAll();
    }

    private void ApplyAll()
    {
        ApplyMasterVolume();
        ApplyMusicVolume();
        ApplySfxVolume();
        ApplyFullscreen();
    }

    private void ApplyMasterVolume()
    {
        // AudioServer bus 0 = Master
        if (AudioServer.GetBusCount() > 0)
            AudioServer.SetBusVolumeDb(0, Mathf.LinearToDb(_masterVolume));
    }

    private void ApplyMusicVolume()
    {
        int bus = AudioServer.GetBusIndex("Music");
        if (bus >= 0)
            AudioServer.SetBusVolumeDb(bus, Mathf.LinearToDb(_musicVolume));
    }

    private void ApplySfxVolume()
    {
        int bus = AudioServer.GetBusIndex("SFX");
        if (bus >= 0)
            AudioServer.SetBusVolumeDb(bus, Mathf.LinearToDb(_sfxVolume));
    }

    private void ApplyFullscreen()
    {
        if (_fullscreen)
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        else
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }
}
