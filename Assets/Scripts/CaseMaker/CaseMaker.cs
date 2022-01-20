using UnityEngine;

public class CaseMaker : MonoBehaviour
{    
     [field: SerializeField] public NodeFields NodeFields { get; private set; }
     
     public BGScene[] BgScenes { get; private set; }

     private void Awake()
     {
          BgScenes = Resources.LoadAll<BGScene>("BGScenes");
          NodeFields.SetSceneOptions(BgScenes);
     }
}
