using System;
using System.Linq;
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
         if (effectId is >= 37 and <= 52) //se.1 to se.16 
         {
             if (InFrontlines() && PluginServices.Config.Frontlines)
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
        uint m = PluginServices.ClientState.TerritoryType;
        var ccTerritories = new[] { 149, 1032, 1033, 1034, 1058, 1059, 1060, 1116, 1117, 1138, 1139, 1293, 1294 };
        return ccTerritories.Contains((int)m);
    }

    private bool InOverworld()
    {
        return !PluginServices.ClientState.IsPvP && !PluginServices.DutyState.IsDutyStarted;
    }

    private bool InFrontlines()
    {
        uint m = PluginServices.ClientState.TerritoryType;
        var flTerritories = new[] { 376, 431, 554, 888, 1273, 1313 };
        return flTerritories.Contains((int)m);    
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