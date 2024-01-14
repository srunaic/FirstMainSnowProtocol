using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCamTransparent : MonoBehaviour
{
    [Header("캐릭터가 지나갈 때 투명한 오브젝트 처리")]
    private Renderer[] renderers; //렌더링 
    private Color[] originalColors;
    public float transparentAlpha = 0.2f; // 투명도 값 (0에서 1까지)

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>(); // 자식 오브젝트의 모든 렌더러를 가져옴

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
        // 오브젝트의 월드 좌표 가져오기
        Vector3 objectWorldPos = transform.position;

        // 화면에 보이는지 확인
        if (renderers != null && IsVisibleOnScreen(objectWorldPos))
        {
            // 투명도 처리
            foreach (Renderer renderer in renderers)
            {
                Color newColor = renderer.material.color;
                newColor.a = transparentAlpha; // 알파 값을 설정하여 투명하게 만듦
                renderer.material.color = newColor;
            }
        }
        else if (renderers != null)
        {
            // 카메라 화면 밖에 있는 경우 원래 색상으로 복원
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = originalColors[i];
            }
        }
    }

    private bool IsVisibleOnScreen(Vector3 worldPos)
    {
        // 카메라가 스프라이트의 X, Y 좌표를 포함하면 화면에 있는 것으로 간주
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(worldPos);
            return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
        }
        return false;
    }
}
