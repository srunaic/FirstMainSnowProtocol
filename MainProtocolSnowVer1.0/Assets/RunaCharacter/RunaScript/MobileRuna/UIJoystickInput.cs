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

        //�Է°� ���
        //(~1, 1 �������� �۵��ϵ��� �����Ͽ��� ��.)

        //1. ������ ����� ��츦 ����
        Vector2 inputValue = touchDir;
        if (touchDir.magnitude > size)
            inputValue = touchDir.normalized * size;

        //2. ���� �ִ방�� �̿��� 0~1(-1) ũ��� ����
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
        //�Է°� ���
        //(~1, 1 �������� �۵��ϵ��� �����Ͽ��� ��.)

        //1. ������ ����� ��츦 ����
        Vector2 inputValue = touchDir;
        if (touchDir.magnitude > size)
            inputValue = touchDir.normalized * size;

        //2. ���� �ִ방�� �̿��� 0~1(-1) ũ��� ����
        inputValue = inputValue / size;

        debugText.text =
            "" + inputValue + "(" + inputValue.magnitude + ")";
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        stick.position = transform.position;
    }
}
