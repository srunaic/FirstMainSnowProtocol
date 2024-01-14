using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public enum SpawnType{Point,Circle,Square}

public class EnemySpawn : MonoBehaviour
{
    public bool randomSpawn = true; //참이면 랜덤생성, 거짓이면 순차생성
    public GameObject[] spawnObjects;  //생성할 물체(그룹)
    public float spawnDist = 1f; // 생성간격

    public SpawnType spawnType = SpawnType.Point;

    [Header("RandomPos(Circle)")]
    public float CircleRange = 1f; //랜덤 지름 사이즈

    [Header("RandomPos(Square)")]
    public Vector2 SquareRange = new Vector2(1, 1); //랜덤 사각형 사이즈

    void Start()
    {
        StartCoroutine(Spawn());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);

        if (spawnType == SpawnType.Square)
            Gizmos.DrawCube(transform.position, (Vector3)SquareRange);
        //사각형 범위
        else if (spawnType == SpawnType.Circle)
            Gizmos.DrawSphere(transform.position, CircleRange);
        //원 범위
    }

    IEnumerator Spawn()
    {
        int selectNum = 0;
        while (true)
        {
            //1. 생성할 물체 결정
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
                selectNum++; //*윗줄과 코드 순서 주의 
            }

            //2. 생성할 위치 결정
            Vector3 spawnPos = transform.position; //Point
            Vector2 addRandPos;

            //사각 범위일때,
            if (spawnType == SpawnType.Square)
            {
                float randX = Random.Range(0, SquareRange.x);
                float randY = Random.Range(0, SquareRange.y);

                addRandPos = new Vector2(randX, randY);
                addRandPos -= SquareRange * 0.5f;

                spawnPos = transform.position + (Vector3)addRandPos;
            }
            //원 범위 일때,
            else if (spawnType == SpawnType.Circle)
            {
                addRandPos = Random.insideUnitCircle * CircleRange; //2D 일때 사용
                //addRandPos = Random.insideUnitSphere * CircleRange; 

                spawnPos = transform.position + (Vector3)addRandPos;
            }
            //생성
            Instantiate(selectObject, spawnPos, Quaternion.identity);
            //생성대기
            yield return new WaitForSeconds(spawnDist);
        }
    }


}
