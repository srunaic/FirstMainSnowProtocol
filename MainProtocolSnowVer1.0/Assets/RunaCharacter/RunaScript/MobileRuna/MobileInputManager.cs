using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobileInputManager : MonoBehaviour
{
    //모바일 pc 사용 가능
    //두개 이상의 터치시 하나로 취급.
    //콜라이더 UI가 반드시 필요
    //동일한 콜라이더에 컴퍼넌트가 들어가야지 쓸수 있음.

    public Text debugTxt;
    /*
    private void OnMouseDown()
    {
        debugTxt.text = "마우스 올려놓음";
    }
    private void OnMouseDrag()
    {
        debugTxt.text = "마우스 드래그";
    }
    private void OnMouseUp()
    {
        debugTxt.text = "마우스 땜";
    }
    */

    private void Update()
    {
        //4.input의 touch 이용하기.
        //모바일 가능
        //여러개의 터치를 개별 파악해서 다양한 기능 개발 가능
        //touch_1.pahse를 이용해서 입력 상태도 파악 가능.
        //두개 이상의 터치를 파악할 상황이 아니라면 굳이 쓸 필요없다.

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
            T = "터치없음.";
        }

        debugTxt.text = T;
    }

}
