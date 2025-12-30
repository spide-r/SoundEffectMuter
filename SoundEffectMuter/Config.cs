using System;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace SoundEffectMuter;

public class Config: IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool Cc = true;
    public bool Frontlines = true;
    public bool DisableAllSoundEffectsInCc = false;
    public bool Duties = false;
    public bool Overworld = false;
    public bool ShowNotification = true;
    public bool Enabled = true;
    
    [NonSerialized]
    private IDalamudPluginInterface? _pluginInterface;

    public void Initialize(IDalamudPluginInterface pluginInterface)
    {
        _pluginInterface = pluginInterface;
    }
    
    public void Save()
    {
        _pluginInterface?.SavePluginConfig(this);
    }
    


}