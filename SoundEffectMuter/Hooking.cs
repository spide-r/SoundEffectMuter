using System;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace SoundEffectMuter;

public class Hooking: IDisposable
{

    [Signature("E8 ?? ?? ?? ?? 45 0F B7 C5", DetourName = nameof(PlaySoundEffectDetour))]
    private readonly Hook<UIGlobals.Delegates.PlaySoundEffect> _playSoundEffectHook = null!;
     public void PlaySoundEffectDetour(uint effectId, nint a2, nint a3, byte a4)
     {
         if ((effectId is >= 37 and <= 52 ) //se.1 to se.16 
             || (effectId is >= 1000096 and <= 1000104)) //cc quickchat 
         {
             if (PluginServices.ClientState.IsPvP)
             {
                 //PluginServices.PluginLog.Debug($"{effectId}, {a2}, {a3}, {a4}");
                 return;
             }
         }
         _playSoundEffectHook.Original(effectId, a2, a3, a4);

     }

    public Hooking()
    {
        PluginServices.GameInteropProvider.InitializeFromAttributes(this);
        _playSoundEffectHook.Enable();
    }

    public void Dispose()
    {
        _playSoundEffectHook.Disable();
        _playSoundEffectHook.Dispose();
    }
}