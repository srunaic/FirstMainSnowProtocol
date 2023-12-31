using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LiftMoving : MonoBehaviour
{
    //������� ������ ����
    [Header("������� ��ġ")]
    public Transform LiftPos;
    public GameObject DollGameCam;//���� ķ ��ġ.

    [SerializeField]
    private InterManager _interManger;//��ȣ�ۿ��� ��ũ��Ʈ

    [Header("���� ������ ��ġ")]
    public Transform _LegDollPos;//�� ��ü�� �ڽĵ��� �ѹ��� �����̱�.
    public Transform _LegDollPos2;
    [Header("���� ��ġ")]
    public Transform _Doll;
    public Transform ZilePos; //������ �ö� ��ġ

    [Header("������ ���ǵ� ����.")]
    public float speed = 0.10f;
    public float Downspeed = 0.08f;

    public Vector3 LiftVec;

    void Awake()
    {
       _interManger = FindObjectOfType<InterManager>();
    }

    void Update()
    {
       DollGame();
    }
    public void DollGame()
    {
        if (_interManger._JoyGame == GameJoy.DollGame)
        { 
            if (Input.GetKey(KeyCode.R))
            {
                _LegDollPos2.Translate(Vector3.forward * Downspeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.T))
            {
                _LegDollPos2.Translate(Vector3.back * Downspeed * Time.deltaTime);
            }

            else if(Input.GetKey(KeyCode.D))
            {
                _LegDollPos.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _LegDollPos.Translate(Vector3.right * speed * Time.deltaTime);
            }
       
            else if (Input.GetKey(KeyCode.W))
            {
                _LegDollPos2.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _LegDollPos2.Translate(Vector3.right * speed * Time.deltaTime);
            }
         
        }

    }


}
