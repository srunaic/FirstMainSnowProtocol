using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{
    [Header("타겟 지정")]
    public Transform target; //플레이어 위치 저장할 Transform 생성
    private NavMeshAgent nav; //네비메쉬 생성

    public LineRenderer lineRenderer;

    [Header("애니메이터")]
    public Animator _RunaAnim;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _RunaAnim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>(); //네비 컴포넌트 참조.
    }
    void Update()
    {
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(MouseRay.origin, MouseRay.direction * 20f, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(MouseRay, out RaycastHit hit))
            {
                //Instantiate(Point, hit.point, Quaternion.identity);//터치 되는 부분 effect 생성.
                target.position = hit.point;
                nav.SetDestination(target.position); //네비게이션 타겟 위치 추적.
            }
        }

        NavigationDrawLine();
    }

    private void NavigationDrawLine()
    {
        int count = nav.path.corners.Length;
        lineRenderer.positionCount = count;
        lineRenderer.positionCount = nav.path.corners.Length;

        for (int i = 0; i < count; i++)
        {
            lineRenderer.SetPosition(i, nav.path.corners[i] + Vector3.up * 1f);
        }

        StartCoroutine(CheckAnimDraw(2f));

    }
    IEnumerator CheckAnimDraw(float EndAnimSpeed)
    {
        yield return new WaitForSeconds(EndAnimSpeed);
        float animSpeed = nav.velocity.magnitude / nav.speed; //네비게이션 애니메이션 속도 공식
        _RunaAnim.SetFloat("InputY", animSpeed);
    }

}
