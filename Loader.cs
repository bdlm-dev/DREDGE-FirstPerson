using Winch.Core;
using UnityEngine;
using Cinemachine;
using HarmonyLib;
using Winch.Util;
using System.Reflection;

namespace FirstPerson;

public class Loader
{
    public static void Initialize()
    {
        // initialize here init
        WinchCore.Log.Debug("Initializing FirstPerson");

        new Harmony("mmbluey.firstperson").PatchAll();

        ApplicationEvents.Instance.OnGameLoaded += OnGameLoadInitialisation;
    }

    private static void OnGameLoadInitialisation()
    {
        UpdateCamera();
        ClearDockVCams();
        DisableHarvesterVCam();
    }

    public static void DisableHarvesterVCam()
    {
        ((CinemachineClearShot)typeof(Harvester).GetField("harvestVCam", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(GameManager.Instance.Player.Harvester)).gameObject.SetActive(false);
        ((CinemachineVirtualCamera)typeof(Harvester).GetField("atrophyVCam", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(GameManager.Instance.Player.Harvester)).gameObject.SetActive(false);
    }

    private static void ClearDockVCams()
    {
        try
        {
            WinchCore.Log.Debug("Attempting to clean Dock VCAMS");
            FieldInfo dockVCam = typeof(Dock).GetField("dockVCam", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo speakerVCams = typeof(Dock).GetField("speakerVCams", BindingFlags.Instance | BindingFlags.NonPublic);
            WinchCore.Log.Debug("Fetched field info");

            DockUtil.GetAllDocks().ForEach(dock =>
            {
                if (dock != null && dock.gameObject != null)
                {
                    dock.gameObject.SetActive(false);

                    Dictionary<string, CinemachineVirtualCamera> speakerCams = (Dictionary<string, CinemachineVirtualCamera>)speakerVCams.GetValue(dock);
                    string[] speakerKeys = speakerCams.Keys.ToArray();
                    speakerKeys.ForEach(key =>
                    {
                        speakerCams[key].gameObject.SetActive(false);
                    });
                }
            });

            WinchCore.Log.Debug("VCam clean complete");

        } catch (Exception e)
        {
            WinchCore.Log.Debug("Error encountered cleaning Dock VCams.");
            WinchCore.Log.Debug(e.ToString());
        }
    }


    private static void UpdateCamera()
    {
        Player player = GameManager.Instance.Player;
        PlayerCamera cam = GameManager.Instance.PlayerCamera;

        if (player == null)
        {
            WinchCore.Log.Error("Failed updating camera: Player doesn't exist");
            return;
        }

        if (cam == null) {
            WinchCore.Log.Error("Failed updating camera: PlayerCamera doesn't exist");
            return;
        }

        cam.transform.localPosition = new Vector3(0, 0.8f, 0);
        cam.transform.localEulerAngles = Vector3.zero;

        GameObject camTargetParent = new GameObject("FPCameraTargetContainer");
        camTargetParent.transform.parent = player.transform;

        GameObject camTarget = new GameObject("FPCameraTarget");
        GameObject fpCamPosition = new GameObject("FPCameraRoot");
        fpCamPosition.transform.parent = camTargetParent.transform;
        camTarget.transform.parent = camTargetParent.transform;
    }
}