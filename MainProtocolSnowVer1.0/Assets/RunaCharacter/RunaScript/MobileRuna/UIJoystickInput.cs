using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIJoystickInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    Transform stick;

    public Text debugText;
    public float size = 10f;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 touchDir = eventData.position
            - (Vector2)transform.position;

        if (touchDir.magnitude <= size)
        {
            stick.position = eventData.position;
        }
        else
        {
            stick.position = transform.position
                + (Vector3)touchDir.normalized * size;
        }

        //입력값 계산
        //(~1, 1 범위에서 작동하도록 가공하여야 함.)

        //1. 범위를 벗어나는 경우를 가공
        Vector2 inputValue = touchDir;
        if (touchDir.magnitude > size)
            inputValue = touchDir.normalized * size;

        //2. 값을 최대갑을 이용해 0~1(-1) 크기로 가공
        inputValue = inputValue / size;

        debugText.text =
            "" + inputValue + "(" + inputValue.magnitude + ")";
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDir = eventData.position
            - (Vector2)transform.position;

        if (touchDir.magnitude <= size)
        {
            stick.position = eventData.position;
        }
        else
        {
            stick.position = transform.position
                + (Vector3)touchDir.normalized * size;
        }
        //입력값 계산
        //(~1, 1 범위에서 작동하도록 가공하여야 함.)

        //1. 범위를 벗어나는 경우를 가공
        Vector2 inputValue = touchDir;
        if (touchDir.magnitude > size)
            inputValue = touchDir.normalized * size;

        //2. 값을 최대갑을 이용해 0~1(-1) 크기로 가공
        inputValue = inputValue / size;

        debugText.text =
            "" + inputValue + "(" + inputValue.magnitude + ")";
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        stick.position = transform.position;
    }
}
