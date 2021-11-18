using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptImporter : MonoBehaviour
{
    [SerializeField] private TextAsset _narrativeScript;

    private ScriptDecoder _scriptDecoder;
    
    private void Awake()
    {
        _scriptDecoder = new ScriptDecoder(_narrativeScript);
    }
}
