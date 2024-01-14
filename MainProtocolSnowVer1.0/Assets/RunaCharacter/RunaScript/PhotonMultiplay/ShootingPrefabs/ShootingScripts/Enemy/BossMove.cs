using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    public Vector2 spawnPosition; // 스폰 위치
    public Vector2 spawnPos2; // 스폰 위치
    public Transform targetPoint;

    void Start()
    {
        StartCoroutine(ChangeMovementPattern());
    }

    public IEnumerator ChangeMovementPattern()
    {  //부드럽게 움직이기.
        while (true)
        {
            // 이동 대기 후 목표 지점으로 이동
            yield return new WaitForSeconds(5f);
            StartCoroutine(MoveSmoothly(targetPoint.position, 1f));

            yield return new WaitForSeconds(3f);

            // 좌우로 이동
            StartCoroutine(MoveSmoothly(new Vector2(Random.Range(-1f, 1f), 0f), 1f));

            yield return new WaitForSeconds(3f);

            // 대각선으로 이동
            StartCoroutine(MoveSmoothly(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 1f));

            yield return new WaitForSeconds(3f);

            // 아래 위로 이동
            StartCoroutine(MoveSmoothly(new Vector2(0f, Random.Range(-0f, 2f)), 1f));

            // 스폰 위치로 돌아오기
            yield return new WaitForSeconds(1f);
            StartCoroutine(MoveSmoothly(spawnPosition, 1f));
            yield return new WaitForSeconds(3f);
            StartCoroutine(MoveSmoothly(spawnPos2, 1f));

        }
    }

    IEnumerator MoveSmoothly(Vector2 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector2 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPosition, startPosition + targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 프레임에서 위치 정확히 설정
        transform.position = startPosition + targetPosition;


    }
}
