using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAI : MonoBehaviour
{
    [Header("Ÿ�� ����")]
    public Transform target; //�÷��̾� ��ġ ������ Transform ����
    private NavMeshAgent nav; //�׺�޽� ����

    public LineRenderer lineRenderer;

    [Header("�ִϸ�����")]
    public Animator _RunaAnim;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _RunaAnim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>(); //�׺� ������Ʈ ����.
    }
    void Update()
    {
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(MouseRay.origin, MouseRay.direction * 20f, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(MouseRay, out RaycastHit hit))
            {
                //Instantiate(Point, hit.point, Quaternion.identity);//��ġ �Ǵ� �κ� effect ����.
                target.position = hit.point;
                nav.SetDestination(target.position); //�׺���̼� Ÿ�� ��ġ ����.
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
        float animSpeed = nav.velocity.magnitude / nav.speed; //�׺���̼� �ִϸ��̼� �ӵ� ����
        _RunaAnim.SetFloat("InputY", animSpeed);
    }

}
