using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "PrototypeSettingsList", menuName = "ScriptableObjects/PrototypeSettingsList")]
public class PrototypeSettingsList : ScriptableObject
{
    public List<PrototypeSettings_Scriptable> settingsList = new List<PrototypeSettings_Scriptable>();

    [Dropdown("CurrentSetting")]
    public PrototypeSettings_Scriptable setting;
    private List<PrototypeSettings_Scriptable> CurrentSetting { get { return settingsList; } }
}
