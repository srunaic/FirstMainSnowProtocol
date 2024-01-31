using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //SerializeField ������ȯ
    public Transform follow_target;
    [SerializeField] float distance = 7.5f; //�÷��̾�� ī�޶��� �Ÿ�
    [SerializeField] float rotation_speed = 2; //ī�޶� ȸ���ϴ� ȸ�� �ӵ�.
    [SerializeField] float min_v_angle = 0; //Vertical x�� ȸ�� �ּ� ���� ������ ���� 
    [SerializeField] float max_v_angle = 90; //Vertical x �� ȸ�� ����


    private Vector2 rotation;


    // Update is called once per frame
    void Update()
    {
        rotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotation_speed;
        rotation.x = Mathf.Clamp(rotation.x, min_v_angle, max_v_angle);

        //�� ���͵� �����ϰ� x�࿡ ���� ���콺 y �Է� ���� y �࿡ ���� ���콺 x �Է� ���� ������ 
        //���� ���⿡ ȸ���ӵ��� ���մϴ�. X�࿡�� ī�޶� ȸ���� �����մϴ�.
        //���� Ŭ������ Ŭ���� �޼��带 ����Ͽ� �ּ� �� �ִ� ���� ������ ����

        // �ӽ� ����, ȸ�� ������ ��ȯ�� ���� ���ʹϾ����� �����ϱ� ���� ��� ȸ�� �������� ī�޶���
        // ������ �����մϴ�. ��ġ�� ���󰡴� ����� ��ġ���� ����� �� ȸ���� ���ο� ���� 3�� ���ϰ� 
        // �Ÿ� ������ z�࿡ ���޵˴ϴ�.

        // ī�޶��� ȸ���� ��� ȸ�� ������ �����ϰ� �����մϴ�.


        var target_rotation = Quaternion.Euler(rotation);


        transform.position = follow_target.position - target_rotation * new Vector3(0f, 0f, distance);
        transform.rotation = target_rotation;
    }
}
