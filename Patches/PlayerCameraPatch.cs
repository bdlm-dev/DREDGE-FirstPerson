using HarmonyLib;
using UnityEngine;
using Cinemachine;
using Winch.Core;

namespace FirstPerson.Patches;

[HarmonyPatch(typeof(PlayerCamera), "Start")]
class PlayerCameraPatch
{
    public static CinemachineVirtualCamera? fpCam;

    static void Prefix(PlayerCamera __instance)
    {
        WinchCore.Log.Debug("Applying PlayerCamera Patch");
        Player player = GameManager.Instance.Player;

        CinemachineFreeLook freeLook = __instance.CinemachineCamera;

        freeLook.enabled = false;

        GameObject obj = new GameObject("FPCamera");
        obj.AddComponent<FPCamera>();

        CinemachineVirtualCamera cam = obj.AddComponent<CinemachineVirtualCamera>();
        cam.transform.parent = player.transform;
        cam.Follow = player.transform;
        cam.LookAt = player.transform;

        fpCam = cam;

        CinemachineImpulseListener listener = obj.AddComponent<CinemachineImpulseListener>();
        cam.AddExtension(listener);
        listener.m_ChannelMask = 1;
        listener.m_Gain = 1;

        CinemachinePOV pov = cam.AddCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.m_AccelTime = 0.1f;
        pov.m_HorizontalAxis.m_DecelTime = 0.1f;
        pov.m_HorizontalAxis.m_MaxSpeed = 2f;
        pov.m_HorizontalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
        pov.m_HorizontalAxis.m_MaxValue = 180f;
        pov.m_HorizontalAxis.m_MinValue = -180f;

        pov.m_VerticalAxis.m_AccelTime = 0.1f;
        pov.m_VerticalAxis.m_DecelTime = 0.1f;
        pov.m_VerticalAxis.m_MaxSpeed = 1f;
        pov.m_VerticalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;

        CinemachineTransposer transposer = cam.AddCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = new Vector3(0, 1.1f, 0.2f);
        transposer.m_XDamping = 0f;
        transposer.m_YDamping = 0f;
        transposer.m_ZDamping = 0f;

        WinchCore.Log.Debug("Applied PlayerCamera Patch");
    }
}
