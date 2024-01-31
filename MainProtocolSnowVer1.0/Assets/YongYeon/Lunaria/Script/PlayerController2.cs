using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2: MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;  //�����̴� �ӵ�

    [SerializeField]
    public float rotationSpeed = 500f; //ȸ���ӵ�

    [SerializeField]
    public float gravity_multiplier = 2f; //�߷� �¼� 

    [SerializeField]
    public float jump_force = 10f;




    private CharacterController character;  //ĳ���� ��Ʈ�ѷ�
    private float downward_velocity; //����ӵ�
    // �÷��̾��� �ӵ��� y�࿡ ����Ǹ�, 




    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();


    }


    //ī�޶� ȸ���� �÷��̾��� �ӵ��� ȥ���Ͽ� �÷��̾��� ���� ������ �׻� 
    //ī�޶� ���ϴ� ������ �ǵ��� �ϴ°��Դϴ�. �� �����ӿ��� ���ʹϾ� Ŭ���� �н�
    //�� ���� ȸ�� ����� ����Ͽ� �̸� �޼��� �� �ֽ��ϴ�. 
    //y�� ��� x0�� ���� ī�޶��� ���� x ���� z������ ī�޶��� ���� z ���� ����ϴ� ���� 3
    // ȸ�� ����� �ӵ��� ���ϸ� ���ʹϾ𿡼� �ٽ� ��� ������ ���� 3���� ��ȯ�˴ϴ�.

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float move_amount = Mathf.Abs(h) + Mathf.Abs(v);
        //�� �ٸ� �ε� �Ҽ����� �߰��� ���ϴ�. �� �̵����� ȣ���ϼ���
        //���� �� ���ΰ� ���� 1�� ��� ������ ���� ��ȯ�ؾ� �ϱ� �����Դϴ�.
        //���밪�� ���� ���� �̸� �ջ��մϴ�
        //�̵��� ������ ���� �Ǵ� ���� �Է��̼��ŵ��� �ʴ� �� �׻� 0�� ��ȯ�մϴ�
        // �̵����� 0���� ū�� Ȯ���Ͽ� �����϶��� �÷��̷��� ȸ���Ϸ��� �մϴ�.

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
                    downward_velocity *= 0.5f; //���� �ӵ��� �������� �پ���
                    //���� ���� ��Ŀ������ ������ ������ �� �ְ� �ȴ�
                    //�� ���� ���� �Կ� �������� ��Ȯ���� �����Ͽ� ���� �÷��̸� ��� ��ų �� �ִ�.


                }

            }

            //��ư�� ���� ��� ���� �ڵ� �ٿ��� ����� ���� �߷¿� �����ϱ� ����
            //�Ʒ��� �ӵ��� ����� �Ҵ��ؾ� �մϴ�.


        }
        else
        {

            downward_velocity += Physics.gravity.y * gravity_multiplier * Time.deltaTime;

        }

        velocity.y = downward_velocity;


        /*
           ĳ���� ��Ʈ�ѷ��� �̵� �޼ҵ带 ȣ���ϱ� ������ ������Ʈ �޼ҵ忡
           ����˴ϴ�. �÷��̾ ���鿡 �ֽ��ϴ�.
           �÷��̰� ���鿡 ������ true�� ��ȯ�ϰ� �׷��� ������ false�� ��ȯ�մϴ�.
           ���鿡 �ణ�� �ӵ��� �߰��ϸ� Ư�� ��� �ó��������� �÷��̰� ���鿡 �����˴ϴ�
           ��������� ���߿����� ����� �߷��� ���� �ӵ� ������ �߰��Ͽ� �ð� ��Ʈ ��Ÿ
           �ð��� ���Ͽ� �����Ӽӵ��� �����ؾ� �մϴ�.
           ��üȿ������ �߷��� �����ϱ� ���� �������� �����Ϸ��� �ӵ��� y���� ������ ���� �����ϰڽ��ϴ�.
           ĳ���� ��Ʈ�ѷ����� �̵� �޼��带 ȣ���ϱ� ���� ���� �ӵ��� ����Ͽ�
           �߷��� ���� ��Ƽ�� �շ��� ���� �� �� �ֽ��ϴ�.
           �ӵ��� �� ������ �ذ��ϱ� ���� �߷¿� y���� ����ؾ� �� �������� ���� ȸ���� ����
           �Ű������� �����ҽ��ϴ�.
        */



        character.Move(velocity * Time.deltaTime);

        if (move_amount > 0)
        {
            var target_rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                target_rotation, rotationSpeed * Time.deltaTime);

            //y�࿡ ���� x�� 0�� �ӵ� x ���� Z���� �����ϴ� ���ο� ���� 3�� �����ϰڽ��ϴ�.
            //Z���� �ӵ��� ���� 

            //�����ϱ����� ����� ���� �߷� �¼��� �߰��غ��ڽ��ϴ�. 
            //ȸ�� �ӵ�

            //Quaternion Ŭ������ ȸ�� ���� �޼��� ��ü�� �� �������� �ٸ� ������ 
            //���� �ܰ踸ŭ ȸ���մϴ�. ���� �ð� ��Ʈ ��Ÿ �ð��� ���ϴ°�.
        }



    }
}
