using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private float TimeLessParticle;//���� ��ƼŬ ���� �ð�
    private float ETime;//���� �ı��ǰ� �� �� ���ð�.

    private void Update()
    {
        TimeParticle();
    }

    void TimeParticle()
    {
        TimeLessParticle += Time.deltaTime;
        if(ETime <= TimeLessParticle)
        {
            Destroy(gameObject,1f);
        }
    }
}
