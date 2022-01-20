using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using OptionData = UnityEngine.UI.Dropdown;

public class NodeFields : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _scenesDropdown;

    public SceneNode SelectedSceneNode
    {
        set
        {
            _scenesDropdown.value = value.CurrentSceneIndex;
            _scenesDropdown.onValueChanged.RemoveAllListeners();
            _scenesDropdown.onValueChanged.AddListener(delegate { value.SetScene(_scenesDropdown.value); });
        }
    }

    public void SetSceneOptions(BGScene[] objects)
    {
        _scenesDropdown.ClearOptions();
        _scenesDropdown.AddOptions(objects.Select(obj => obj.name).ToList());
    }
}
