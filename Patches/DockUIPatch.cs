using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winch.Core;

namespace FirstPerson.Patches;

[HarmonyPatch(typeof(DockUI))]
internal class DockUIPatch
{
    [HarmonyPatch("OnPlayerDockedToggled")]
    static bool Prefix(Dock dock, DockUI __instance)
    {
        WinchCore.Log.Debug("Calling alt dockUI");
        if (dock)
        {
            if (GameManager.Instance.IsPlaying)
            {
                __instance.Show(dock, 0.2f);
            }
        }
        else
        {
            __instance.HideUI();
        }

        return false;
    }

    [HarmonyPatch("RefreshVCams")]
    static bool Prefix()
    {
        return false;
    }
}
