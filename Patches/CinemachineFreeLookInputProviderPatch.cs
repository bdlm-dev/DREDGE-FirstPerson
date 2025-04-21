using HarmonyLib;

namespace FirstPerson.Patches;

// yes, the name is inconsistently cased
// FreeLook != Freelook, it's inconsistent in their codebase :(
[HarmonyPatch(typeof(CinemachineFreeLookInputProvider), "GetAxisCustom")]
internal class CinemachineFreeLookInputProviderPatch
{
    static void Prefix(CinemachineFreeLookInputProvider __instance)
    {
        __instance
            .GetType()
            .GetField("canMoveCamera", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .SetValue(__instance, true);
    }
}
