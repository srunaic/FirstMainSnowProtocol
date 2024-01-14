using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public float moveSpeed = 2f; // �̵� �ӵ�
    public Vector2 spawnPosition; // ���� ��ġ
    public Vector2 spawnPos2; // ���� ��ġ
    public Transform targetPoint;

    void Start()
    {
        StartCoroutine(ChangeMovementPattern());
    }

    public IEnumerator ChangeMovementPattern()
    {  //�ε巴�� �����̱�.
        while (true)
        {
            // �̵� ��� �� ��ǥ �������� �̵�
            yield return new WaitForSeconds(5f);
            StartCoroutine(MoveSmoothly(targetPoint.position, 1f));

            yield return new WaitForSeconds(3f);

            // �¿�� �̵�
            StartCoroutine(MoveSmoothly(new Vector2(Random.Range(-1f, 1f), 0f), 1f));

            yield return new WaitForSeconds(3f);

            // �밢������ �̵�
            StartCoroutine(MoveSmoothly(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 1f));

            yield return new WaitForSeconds(3f);

            // �Ʒ� ���� �̵�
            StartCoroutine(MoveSmoothly(new Vector2(0f, Random.Range(-0f, 2f)), 1f));

            // ���� ��ġ�� ���ƿ���
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

        // ������ �����ӿ��� ��ġ ��Ȯ�� ����
        transform.position = startPosition + targetPosition;


    }
}
