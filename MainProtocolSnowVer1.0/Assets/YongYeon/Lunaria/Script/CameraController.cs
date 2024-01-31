using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //SerializeField 공개변환
    public Transform follow_target;
    [SerializeField] float distance = 7.5f; //플레이어와 카메라의 거리
    [SerializeField] float rotation_speed = 2; //카메라가 회전하는 회전 속도.
    [SerializeField] float min_v_angle = 0; //Vertical x축 회전 최소 수직 각도를 제어 
    [SerializeField] float max_v_angle = 90; //Vertical x 축 회전 제어


    private Vector2 rotation;


    // Update is called once per frame
    void Update()
    {
        rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotation_speed;
        rotation.x = Mathf.Clamp(rotation.x, min_v_angle, max_v_angle);

        //새 벡터도 생성하고 x축에 대한 마우스 y 입력 값과 y 축에 대한 마우스 x 입력 값을 전달한 
        //다음 여기에 회전속도를 곱합니다. X축에서 카메라 회전을 제한합니다.
        //수학 클래스의 클램프 메서드를 사용하여 최소 및 최대 수직 각도를 전달

        // 임시 변수, 회전 변수의 변환된 값을 쿼터니언으로 저장하기 위한 대상 회전 다음으로 카메라의
        // 각도를 설정합니다. 위치를 따라가는 대상의 위치에서 대상을 뺀 회전에 새로운 벡터 3을 곱하고 
        // 거리 변수가 z축에 전달됩니다.

        // 카메라의 회전을 대상 회전 변수와 동일하게 설정합니다.


        var target_rotation = Quaternion.Euler(rotation);


        transform.position = follow_target.position - target_rotation * new Vector3(0f, 0f, distance);
        transform.rotation = target_rotation;
    }
}
