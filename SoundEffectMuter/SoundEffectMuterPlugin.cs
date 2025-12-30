using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

namespace SoundEffectMuter;

public class SoundEffectMuterPlugin: IDalamudPlugin
{
    private Hooking h;
    private WindowSystem WindowSystem = new("SoundEffectMuter");

    private ConfigWindow ConfigWindow { get; init; }

    public SoundEffectMuterPlugin(IDalamudPluginInterface pluginInterface)
    {
        PluginServices.Initialize(pluginInterface);
        ConfigWindow = new ConfigWindow();
        WindowSystem.AddWindow(ConfigWindow);
        h = new Hooking();
        pluginInterface.UiBuilder.Draw += DrawUi;
        PluginServices.ClientState.Login += PluginUpdateMessage;
        pluginInterface.UiBuilder.OpenConfigUi += ToggleConfigWindow;
        PluginServices.CommandManager.AddHandler("/ptogglesem", new CommandInfo(ToggleCommand)
        {
            HelpMessage = "Enable/Disable plugin",
        });
        PluginUpdateMessage();

    }

    private void ToggleCommand(string command, string arguments)
    {
        PluginServices.Config.Enabled = !PluginServices.Config.Enabled;
        PluginServices.Config.Save();
        PluginServices.ChatGui.Print((PluginServices.Config.Enabled) ? "Enabled!" : "Disabled!", 
            "SoundEffectMuter");
    }

    private void DrawUi()
    {
        WindowSystem.Draw();
    }
    
    public static void PluginUpdateMessage()
    {
        if (PluginServices.Config.ShowNotification)
        {
            if (PluginServices.ClientState.IsLoggedIn == false)
            {
                return;
            }
            PluginServices.ChatGui.Print(
                "New Update! This plugin can now be toggled between Frontlines, CC, other duties, or everywhere.", "SoundEffectMuter", 15);
            PluginServices.Config.ShowNotification = false;
            PluginServices.Config.Save();
        }
           
    }
    private void ToggleConfigWindow()
    {
        ConfigWindow.Toggle();
    }
    
    public void Dispose()
    {
        PluginServices.ClientState.Login -= PluginUpdateMessage;
        PluginServices.DalamudPluginInterface.UiBuilder.Draw -= DrawUi;
        PluginServices.DalamudPluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigWindow;

        h.Dispose(); 
    }

}