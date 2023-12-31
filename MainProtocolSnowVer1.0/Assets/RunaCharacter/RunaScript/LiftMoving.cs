using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LiftMoving : MonoBehaviour
{
    //오락기기 움직임 관리
    [Header("오락기기 위치")]
    public Transform LiftPos;
    public GameObject DollGameCam;//게임 캠 위치.

    [SerializeField]
    private InterManager _interManger;//상호작용할 스크립트

    [Header("집게 포지션 위치")]
    public Transform _LegDollPos;//이 객체와 자식들을 한번에 움직이기.
    public Transform _LegDollPos2;
    [Header("인형 위치")]
    public Transform _Doll;
    public Transform ZilePos; //인형이 올라갈 위치

    [Header("움직임 스피드 관리.")]
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
