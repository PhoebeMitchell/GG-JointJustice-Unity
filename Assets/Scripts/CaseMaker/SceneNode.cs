using UnityEngine;
using UnityEngine.EventSystems;

public class SceneNode : MonoBehaviour, IPointerDownHandler
{
    private CaseMaker _caseMaker;
    private Transform _transform;
    private BGScene _bgSceneInstance;
    
    public int CurrentSceneIndex { get; private set; }
    
    private void Awake()
    {
        _transform = transform;
        _caseMaker = _transform.parent.GetComponentInParent<CaseMaker>();
        SetScene(CurrentSceneIndex);
    }

    private void Update()
    {
        if (_bgSceneInstance != null)
        {
            _bgSceneInstance.transform.position = _transform.position;
        }
    }

    public void SetScene(int sceneIndex)
    {
        CurrentSceneIndex = sceneIndex;
        
        if (_bgSceneInstance != null)
        {
            Destroy(_bgSceneInstance.gameObject);
            Debug.Log("hello");
        }

        _bgSceneInstance = Instantiate(_caseMaker.BgScenes[sceneIndex], _transform.position, Quaternion.identity);
        _bgSceneInstance.transform.localScale *= 2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _caseMaker.NodeFields.SelectedSceneNode = this;
    }
}
