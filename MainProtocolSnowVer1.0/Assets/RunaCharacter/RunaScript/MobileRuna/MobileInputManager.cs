using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobileInputManager : MonoBehaviour
{
    //����� pc ��� ����
    //�ΰ� �̻��� ��ġ�� �ϳ��� ���.
    //�ݶ��̴� UI�� �ݵ�� �ʿ�
    //������ �ݶ��̴��� ���۳�Ʈ�� ������ ���� ����.

    public Text debugTxt;
    /*
    private void OnMouseDown()
    {
        debugTxt.text = "���콺 �÷�����";
    }
    private void OnMouseDrag()
    {
        debugTxt.text = "���콺 �巡��";
    }
    private void OnMouseUp()
    {
        debugTxt.text = "���콺 ��";
    }
    */

    private void Update()
    {
        //4.input�� touch �̿��ϱ�.
        //����� ����
        //�������� ��ġ�� ���� �ľ��ؼ� �پ��� ��� ���� ����
        //touch_1.pahse�� �̿��ؼ� �Է� ���µ� �ľ� ����.
        //�ΰ� �̻��� ��ġ�� �ľ��� ��Ȳ�� �ƴ϶�� ���� �� �ʿ����.

        string T = "";

        if (Input.touchCount > 0)
        {
            Touch touch_1 = Input.GetTouch(0);
            T += touch_1.position;

            if (Input.touchCount > 1)
            {
                Touch touch_2 = Input.GetTouch(1);
                T += "\n 2:" + touch_2.position;

                T += "\n dist:"
                    + Vector3.Distance(touch_1.position, touch_2.position);
            }
        }
        else
        {
            T = "��ġ����.";
        }

        debugTxt.text = T;
    }

}
