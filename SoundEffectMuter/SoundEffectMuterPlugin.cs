using Dalamud.Game.Command;
using Dalamud.Plugin;

namespace SoundEffectMuter;

public class SoundEffectMuterPlugin: IDalamudPlugin
{
    private Hooking h;

    public SoundEffectMuterPlugin(IDalamudPluginInterface pluginInterface)
    {
        PluginServices.Initialize(pluginInterface);

        h = new Hooking();

    }
    
    public void Dispose()
    {
        h.Dispose(); 
    }

}