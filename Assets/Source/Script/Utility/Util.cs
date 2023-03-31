using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;


public class Util : MonoBehaviour
{
    public static bool ClickOverUI()
    {
        return IsPointerOverUIElement();
    }
    
    public static async void Delay(float time, Action action)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        action();
    }

    public static async void ActivateLastFrame(Action action)
    {
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        action();
    }

    private static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
   
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {
        int UILayer = LayerMask.NameToLayer("UI");
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[index];
            if (curRaycastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
 
 
    //Gets all event system raycast results of current mouse or touch position.
    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
 
    //Returns 'true' if we touched or hovering on Unity UI element.
    
    
}
