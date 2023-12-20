using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bakhoShooting : MonoBehaviour
{
  
    [SerializeField]
    public GameObject missile = null;//�Ѿ� ������Ʈ ����. ���� �� ĳ���� �ȿ�
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
                StartCoroutine(CoolTime(5f)); // ��Ÿ�� 5��
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

                yield return new WaitForFixedUpdate(); //���� ������� ���.

            }

        }



    }
    

}



