using UnityEngine;
using UnityEngine.EventSystems;

public class IgnoreMouse : MonoBehaviour
{
    GameObject lastSelected;

    void Start()
    {
        lastSelected = gameObject;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(lastSelected);
        else
            lastSelected = EventSystem.current.currentSelectedGameObject;
    }
}