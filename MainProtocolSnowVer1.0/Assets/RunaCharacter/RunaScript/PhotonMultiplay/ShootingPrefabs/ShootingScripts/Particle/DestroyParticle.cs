using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private float TimeLessParticle;//기존 파티클 동작 시간
    private float ETime;//적이 파괴되고 난 후 대기시간.

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
