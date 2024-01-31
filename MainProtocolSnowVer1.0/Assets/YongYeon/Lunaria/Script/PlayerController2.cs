using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2: MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;  //움직이는 속도

    [SerializeField]
    public float rotationSpeed = 500f; //회전속도

    [SerializeField]
    public float gravity_multiplier = 2f; //중력 승수 

    [SerializeField]
    public float jump_force = 10f;




    private CharacterController character;  //캐릭터 컨트롤러
    private float downward_velocity; //하향속도
    // 플레이어의 속도가 y축에 저장되며, 




    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();


    }


    //카메라 회전을 플레이어의 속도에 혼합하여 플레이어의 전방 방향이 항상 
    //카메라가 향하는 방향이 되도록 하는것입니다. 새 프레임에서 쿼터니언 클래스 패스
    //의 보기 회전 방법을 사용하여 이를 달성할 수 있습니다. 
    //y의 경우 x0에 대한 카메라의 전방 x 값과 z에대한 카메라의 전방 z 값을 사용하는 벡터 3
    // 회전 방법에 속도를 곱하면 쿼터니언에서 다시 사용 가능한 벡터 3으로 변환됩니다.

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float move_amount = Mathf.Abs(h) + Mathf.Abs(v);
        //또 다른 부동 소수점을 추가해 봅니다. 이 이동량을 호출하세요
        //가로 및 세로가 음수 1과 양수 사이의 값을 반환해야 하기 때문입니다.
        //절대값을 구한 다음 이를 합산합니다
        //이동량 변수는 수평 또는 수직 입력이수신되지 않는 한 항상 0을 반환합니다
        // 이동량이 0보다 큰지 확인하여 움직일때만 플레이러를 회전하려고 합니다.

        Vector3 velocity = new Vector3(h, 0f, v).normalized * moveSpeed;
        velocity = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.
            main.transform.forward.z)) * velocity;

        if (character.isGrounded)
        {
            downward_velocity = -2f;

            if (Input.GetButtonDown("Jump"))
            {

                downward_velocity = jump_force;

                if (Input.GetButtonUp("Jump") && downward_velocity > 0f)
                {
                    downward_velocity *= 0.5f; //점프 속도가 절반으로 줄어든다
                    //제어 점프 메커니즘을 완전히 제어할 수 있게 된다
                    //더 많은 선택 읮와 움직임의 정확성을 제공하여 게임 플레이를 향상 시킬 수 있다.


                }

            }

            //버튼을 누른 경우 위의 코드 줄에서 적용된 음의 중력에 대응하기 위해
            //아래쪽 속도에 양수를 할당해야 합니다.


        }
        else
        {

            downward_velocity += Physics.gravity.y * gravity_multiplier * Time.deltaTime;

        }

        velocity.y = downward_velocity;


        /*
           캐릭터 컨트롤러의 이동 메소드를 호출하기 직전에 업데이트 메소드에
           저장됩니다. 플레이어가 지면에 있습니다.
           플레이거 지면에 닿으면 true를 반환하고 그렇지 않으면 false를 반환합니다.
           지면에 약간의 속도를 추가하면 특정 경사 시나리오에서 플레이가 지면에 고정됩니다
           기술적으로 공중에서는 내장된 중력을 하향 속도 변수에 추가하여 시간 도트 델타
           시간을 곱하여 프레임속도를 보장해야 합니다.
           전체효과에서 중력을 관찰하기 위해 독립성을 유지하려면 속도의 y축을 다음과 같게 설정하겠습니다.
           캐릭터 컨트롤러에서 이동 메서드를 호출하기 전에 하향 속도를 사용하여
           중력이 이제 파티에 합류한 것을 볼 수 있습니다.
           속도를 이 문제를 해결하기 위해 중력에 y축을 사용해야 할 때까지는 보기 회전에 대한
           매개변수가 괜찮았습니다.
        */



        character.Move(velocity * Time.deltaTime);

        if (move_amount > 0)
        {
            var target_rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                target_rotation, rotationSpeed * Time.deltaTime);

            //y축에 대한 x축 0의 속도 x 값과 Z값을 전달하는 새로운 벡터 3을 생성하겠습니다.
            //Z축의 속도에 대해 

            //점프하기전에 사용자 정의 중력 승수를 추가해보겠습니다. 
            //회전 속도

            //Quaternion 클래스의 회전 방향 메서는 객체를 한 각도에서 다른 각도로 
            //각도 단계만큼 회전합니다. 값에 시간 도트 델타 시간을 곱하는것.
        }



    }
}
