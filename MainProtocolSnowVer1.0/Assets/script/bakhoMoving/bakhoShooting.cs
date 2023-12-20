using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bakhoShooting : MonoBehaviour
{
  
    [SerializeField]
    public GameObject missile = null;//총알 오브젝트 생성. 빌드 후 캐릭터 안에
    [SerializeField]
    Transform missileSpawn = null;

    public Image img_Skill;
    private float timer;

    private bool grounded;

    private float m_Time = 0;

    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            if (Input.GetKeyDown(KeyCode.Q) && grounded == false)
            {
                StartCoroutine(CoolTime(5f)); // 쿨타임 5초
                GameObject Launcher = Instantiate(missile, missileSpawn.position, Quaternion.identity);
                Launcher.GetComponent<Rigidbody>().velocity = Vector3.up * 5f;

                timer = 0;
            }

        }



        IEnumerator CoolTime(float cool)
        {
            while (cool > 1.0f)
            {
                cool -= Time.deltaTime;
                img_Skill.fillAmount = (1.0f / cool); //amound filling 

                yield return new WaitForFixedUpdate(); //다음 실행까지 대기.

            }

        }



    }
    

}



