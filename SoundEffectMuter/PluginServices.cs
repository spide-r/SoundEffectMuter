using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace SoundEffectMuter;

#pragma warning disable 8618
internal class PluginServices {
    
    [PluginService]
    internal static ICommandManager CommandManager { get; private set; }
    [PluginService]
    internal static IClientState ClientState { get; private set; }
    
    [PluginService]
    internal static IGameInteropProvider GameInteropProvider { get; private set; }
    
    [PluginService]
    internal static IPluginLog PluginLog { get; private set; }
    
    internal static void Initialize(IDalamudPluginInterface pluginInterface) {
        pluginInterface.Create<PluginServices>();
    }
}
#pragma warning restore 8618