using UnityEngine;


public class IgnoreUiRaycastWhenInactive : MonoBehaviour//, ICanvasRaycastFilter   //gives an error -shrug-
{
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return gameObject.activeInHierarchy;
    }
}