using UnityEngine;

public class FootstepEffect : MonoBehaviour
{
    public ParticleSystem footstepParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            
            // ���� ���� �� ����Ʈ�� Ȱ��ȭ
            footstepParticles.Play();

          
        }
    }
}