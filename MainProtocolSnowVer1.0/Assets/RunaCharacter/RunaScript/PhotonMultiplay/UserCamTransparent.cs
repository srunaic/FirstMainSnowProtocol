using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamTransparent : MonoBehaviour
{
    [Header("ĳ���Ͱ� ������ �� ������ ������Ʈ ó��")]
    private Renderer[] renderers; //������ 
    private Color[] originalColors;
    public float transparentAlpha = 0.2f; // ���� �� (0���� 1����)

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>(); // �ڽ� ������Ʈ�� ��� �������� ������

        if (renderers != null && renderers.Length > 0)
        {
            originalColors = new Color[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                originalColors[i] = renderers[i].material.color;
            }
        }
    }

    private void Update()
    {
        // ������Ʈ�� ���� ��ǥ ��������
        Vector3 objectWorldPos = transform.position;

        // ȭ�鿡 ���̴��� Ȯ��
        if (renderers != null && IsVisibleOnScreen(objectWorldPos))
        {
            // ���� ó��
            foreach (Renderer renderer in renderers)
            {
                Color newColor = renderer.material.color;
                newColor.a = transparentAlpha; // ���� ���� �����Ͽ� �����ϰ� ����
                renderer.material.color = newColor;
            }
        }
        else if (renderers != null)
        {
            // ī�޶� ȭ�� �ۿ� �ִ� ��� ���� �������� ����
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = originalColors[i];
            }
        }
    }

    private bool IsVisibleOnScreen(Vector3 worldPos)
    {
        // ī�޶� ��������Ʈ�� X, Y ��ǥ�� �����ϸ� ȭ�鿡 �ִ� ������ ����
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(worldPos);
            return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
        }
        return false;
    }
}
