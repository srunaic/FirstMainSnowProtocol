using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public enum SpawnType{Point,Circle,Square}

public class EnemySpawn : MonoBehaviour
{
    public bool randomSpawn = true; //���̸� ��������, �����̸� ��������
    public GameObject[] spawnObjects;  //������ ��ü(�׷�)
    public float spawnDist = 1f; // ��������

    public SpawnType spawnType = SpawnType.Point;

    [Header("RandomPos(Circle)")]
    public float CircleRange = 1f; //���� ���� ������

    [Header("RandomPos(Square)")]
    public Vector2 SquareRange = new Vector2(1, 1); //���� �簢�� ������

    void Start()
    {
        StartCoroutine(Spawn());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);

        if (spawnType == SpawnType.Square)
            Gizmos.DrawCube(transform.position, (Vector3)SquareRange);
        //�簢�� ����
        else if (spawnType == SpawnType.Circle)
            Gizmos.DrawSphere(transform.position, CircleRange);
        //�� ����
    }

    IEnumerator Spawn()
    {
        int selectNum = 0;
        while (true)
        {
            //1. ������ ��ü ����
            GameObject selectObject;

            if (randomSpawn == true)
            {
                selectNum = Random.Range(0, spawnObjects.Length);
                selectObject = spawnObjects[selectNum];
            }
            else
            {
                if (selectNum >= spawnObjects.Length)
                    selectNum = 0;

                selectObject = spawnObjects[selectNum];
                selectNum++; //*���ٰ� �ڵ� ���� ���� 
            }

            //2. ������ ��ġ ����
            Vector3 spawnPos = transform.position; //Point
            Vector2 addRandPos;

            //�簢 �����϶�,
            if (spawnType == SpawnType.Square)
            {
                float randX = Random.Range(0, SquareRange.x);
                float randY = Random.Range(0, SquareRange.y);

                addRandPos = new Vector2(randX, randY);
                addRandPos -= SquareRange * 0.5f;

                spawnPos = transform.position + (Vector3)addRandPos;
            }
            //�� ���� �϶�,
            else if (spawnType == SpawnType.Circle)
            {
                addRandPos = Random.insideUnitCircle * CircleRange; //2D �϶� ���
                //addRandPos = Random.insideUnitSphere * CircleRange; 

                spawnPos = transform.position + (Vector3)addRandPos;
            }
            //����
            Instantiate(selectObject, spawnPos, Quaternion.identity);
            //�������
            yield return new WaitForSeconds(spawnDist);
        }
    }


}
