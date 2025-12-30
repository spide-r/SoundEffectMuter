using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;

namespace SoundEffectMuter;

public class ConfigWindow: Window, IDisposable
{
  
    private readonly Config _config;

    public ConfigWindow() : base("SoundEffectMuter", ImGuiWindowFlags.NoResize)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(300, 180),
            MaximumSize = new Vector2(300, 180),
            
        };
        

        SizeCondition = ImGuiCond.Always;
        

        _config = PluginServices.Config;
    }

    public override void Draw()
    {
        var enabled = _config.Enabled;
        var cc = _config.Cc;
        var frontlines = _config.Frontlines;
        var disableAllSoundEffectsInCc = _config.DisableAllSoundEffectsInCc;
        var duties = _config.Duties;
        var overworld = _config.Overworld;
        
        if (ImGui.Checkbox("Enable plugin?", ref enabled))
        {
            _config.Enabled = enabled;
            _config.Save();
        }

        
        if (ImGui.Checkbox("Mute CC Quickchat?", ref cc))
        {
            _config.Cc = cc;
            _config.Save();
        }

        if (ImGui.Checkbox("Mute Sound Effects in Frontlines?", ref frontlines))
        {
            _config.Frontlines = frontlines;
            _config.Save();
        }

        if (ImGui.Checkbox("Mute Sound Effects in Overworld?", ref overworld))
        {
            _config.Overworld = overworld;
            _config.Save();
            
        }

        if (ImGui.Checkbox("Mute Sound Effects in Duties?", ref duties))
        {
            _config.Duties = duties;
            _config.Save();
        }

        if (ImGui.Checkbox("Mute SE.X sounds in Crystalline conflict?", ref disableAllSoundEffectsInCc))
        {
            _config.DisableAllSoundEffectsInCc = disableAllSoundEffectsInCc;
            _config.Save();
        }
        ImGuiComponents.HelpMarker("Some SE.X sound effects are present in CC through UI elements and match state." +
                                   "\nThese do not come from players as chat is impossible." +
                                   "\nThis toggle does NOT affect CC QuickChat, only sound effects that come from the CC UI itself. ");
    }

    public void Dispose()
    {
        
    }
}