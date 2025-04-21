using Cinemachine;
using UnityEngine;
using Winch.Config;

namespace FirstPerson;

internal class FPCamera : MonoBehaviour
{
    Player player;
    CinemachineVirtualCamera cam;
    CinemachineTransposer transposer;

    ModConfig Config => ModConfig.GetConfig();

    private void Start()
    {
        player = GameManager.Instance.Player;
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        HandleCamOffset();
        HandleFOV();
        HandleOceanSmoothing();
    }

    private void HandleCamOffset()
    {
        switch (player.BoatModelProxy.name) {
            case "Boat1":
                transposer.m_FollowOffset = new Vector3(0f, 1.1f, 0.3f);
                break;
            case "Boat2":
                transposer.m_FollowOffset = new Vector3(0f, 1.25f, 0.3f);
                break;
            case "Boat3":
                transposer.m_FollowOffset = new Vector3(0f, 1.3f, 0.2f);
                break;
            case "Boat4":
                transposer.m_FollowOffset = new Vector3(0f, 1.4f, 0.45f);
                break;
            case "Boat5":
                transposer.m_FollowOffset = new Vector3(0f, 1.6f, 0.8f);
                break;
            default:
                break;
        }
    }

    private void HandleFOV()
    {
        cam.m_Lens.FieldOfView = Config.GetProperty<float>("fov");
    }

    private void HandleOceanSmoothing()
    {
        bool shouldHaveWaves = Config.GetProperty<bool>("waves");

        if (shouldHaveWaves)
        {
            DestabilizeOcean();
        } else
        {
            StabilizeOcean();
        }
    }

    // thanks megapiggy :)
    // https://github.com/DREDGE-Mods/Winch/blob/master/Winch.Examples/ExampleItems/TestAbility.cs

    private void StabilizeOcean()
    {
        GameManager.Instance.WaveController.steepness = 0f;
        Shader.SetGlobalFloat("_WaveSteepness", 0);
        typeof(WaveController).GetField("wavelength", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(GameManager.Instance.WaveController, 1f);
        Shader.SetGlobalFloat("_WaveLength", 1);
        typeof(WaveController).GetField("speed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(GameManager.Instance.WaveController, 0.1f);
        Shader.SetGlobalFloat("_WaveSpeed", 0);
        typeof(WaveController).GetField("waveDirections", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(GameManager.Instance.WaveController, new float[4] { 0, 0, 0, 0 });
        Shader.SetGlobalVector("_WaveDirections", Vector4.zero);
    }

    private void DestabilizeOcean()
    {
        GameManager.Instance.WaveController.steepness = 0.1f;
        Shader.SetGlobalFloat("_WaveSteepness", 0.1f);
        typeof(WaveController).GetField("wavelength", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(GameManager.Instance.WaveController, 6f);
        Shader.SetGlobalFloat("_WaveLength", 6);
        typeof(WaveController).GetField("speed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(GameManager.Instance.WaveController, 0.1f);
        Shader.SetGlobalFloat("_WaveSpeed", 0.1f);
        typeof(WaveController).GetField("waveDirections", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(GameManager.Instance.WaveController, new float[4] { 0.1f, 0.4f, 0.2f, 0.3f });
        Shader.SetGlobalVector("_WaveDirections", new Vector4(0.1f, 0.4f, 0.2f, 0.3f));
    }
}
