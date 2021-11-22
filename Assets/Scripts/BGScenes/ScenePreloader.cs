using UnityEngine;

public class ScenePreloader : ISceneController
{
    public void FadeIn(float seconds)
    {
        
    }

    public void FadeOut(float seconds)
    {
        
    }

    public void ShakeScreen(float intensity, float duration, bool isBlocking)
    {
        
    }

    public void SetScene(string background)
    {
        Debug.Log($"Loaded {background}.");
        Resources.Load(background);
    }

    public void SetCameraPos(Vector2Int position)
    {
        
    }

    public void PanCamera(float seconds, Vector2Int finalPosition)
    {
        
    }

    public void PanToActorSlot(int oneBasedSlotIndex, float seconds)
    {
        
    }

    public void JumpToActorSlot(int oneBasedSlotIndex)
    {
        
    }

    public void ShowItem(string item, ItemDisplayPosition position)
    {
        Debug.Log($"Loaded {item}.");
        Resources.Load(item);
    }

    public void ShowActor()
    {
        
    }

    public void HideActor()
    {
        
    }

    public void Wait(float seconds)
    {
        
    }

    public void HideItem()
    {
         
    }

    public void PlayAnimation(string animationName)
    {
        
    }
}
