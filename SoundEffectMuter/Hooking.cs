using System;
using System.Linq;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Sound;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace SoundEffectMuter;

public class Hooking: IDisposable
{

    [Signature("E8 ?? ?? ?? ?? 45 0F B7 C5", DetourName = nameof(PlaySoundEffectDetour))]
    private readonly Hook<UIGlobals.Delegates.PlaySoundEffect> _playSoundEffectHook = null!;
     public unsafe void PlaySoundEffectDetour(uint effectId, SoundData** a2, SoundData** a3, byte a4)
     {
         if (!PluginServices.Config.Enabled)
         {
             _playSoundEffectHook.Original(effectId, a2, a3, a4);
             return;
             
         }
         if (effectId is >= 37 and <= 52) //se.1 to se.16 
         {
             if (InFrontlinesAndRw() && PluginServices.Config.Frontlines)
             {
                 return;
             }
             if (InDuty() && PluginServices.Config.Duties)
             {
                 return;
             }
             if (InOverworld() && PluginServices.Config.Overworld)
             {
                 return;
             }
             if (InCc() && PluginServices.Config.DisableAllSoundEffectsInCc)
             {
                 return;
             }
         }
         
         if (effectId is >= 1000096 and <= 1000104) //cc quickchat 
         {
             if (InCc() && PluginServices.Config.Cc)
             {
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

    private bool InCc()
    {
        return PluginServices.ClientState.IsPvPExcludingDen && !PluginServices.PartyList.IsAlliance;
    }
    private bool InOverworld()
    {
        return !PluginServices.ClientState.IsPvPExcludingDen && !PluginServices.DutyState.IsDutyStarted;
    }

    private bool InFrontlinesAndRw()
    {
        return PluginServices.ClientState.IsPvPExcludingDen && PluginServices.PartyList.IsAlliance;
  
    }

    private bool InDuty()
    {
        return PluginServices.DutyState.IsDutyStarted;
    }
    public void Dispose()
    {
        _playSoundEffectHook.Disable();
        _playSoundEffectHook.Dispose();
    }
}