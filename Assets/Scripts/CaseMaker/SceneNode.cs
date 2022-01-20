using UnityEngine;
using UnityEngine.EventSystems;

public class SceneNode : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform _scene;

    private Transform _transform;
    private Transform _bgSceneInstance;
    
    private void Awake()
    {
        _transform = transform;
        _bgSceneInstance = Instantiate(_scene, _transform.position, Quaternion.identity);
        _bgSceneInstance.localScale *= 2;
    }

    private void Update()
    {
        _bgSceneInstance.transform.position = _transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
