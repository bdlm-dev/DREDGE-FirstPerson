using HarmonyLib;
using Cinemachine;

namespace FirstPerson.Patches;

[HarmonyPatch(typeof(SpyglassAbility), "Init")]
internal class SpyglassAbilityPatch
{
    static void Prefix(SpyglassAbility __instance)
    {
        CinemachineVirtualCamera cam = (CinemachineVirtualCamera)__instance.GetType().GetField("spyglassVCam", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(__instance);
        cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new UnityEngine.Vector3(0, 1.2f, 0.2f);
    }
}
