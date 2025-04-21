using HarmonyLib;
using Winch.Core;

namespace FirstPerson.Patches;

[HarmonyPatch(typeof(BaseDestinationUI), "Show")]
class BaseDestinationPatch
{
    public static void Postfix(BaseDestination destination)
    {
        WinchCore.Log.Debug(destination);
        WinchCore.Log.Debug(destination.VCam);
        destination.VCam.enabled = false;
    }
}
