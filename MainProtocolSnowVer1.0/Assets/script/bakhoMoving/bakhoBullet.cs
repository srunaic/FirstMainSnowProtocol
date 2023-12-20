using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bakhoBullet : MonoBehaviour
{
    private Rigidbody rigid = null;
    private Transform Target = null;

    [SerializeField] float m_speed = 0f;
    float m_currentSpeed = 0f;
    [SerializeField] LayerMask m_layerMask = 0;
    [SerializeField] ParticleSystem m_psEffect = null;


    void SearchEnemy()
    {

        Collider[] t_cols = Physics.OverlapSphere(transform.position, 1000f, m_layerMask); //100m ���� ����

        if (t_cols.Length > 0)
        {
            Target = t_cols[Random.Range(0, t_cols.Length)].transform;
        }



    }

    IEnumerator LaunchDelay() 
    {
        yield return new WaitUntil(() => rigid.velocity.y < 0f);
        yield return new WaitForSeconds(0.1f);

        SearchEnemy();
        m_psEffect.Play();

        yield return new WaitForSeconds(5f); //���Ҵµ� �ƹ� ���� ���ٸ� 5�� �� �ı�.
        Destroy(gameObject);

   }

    void Start() 
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(LaunchDelay());
    
    }

    void Update()
    {
        if (Target != null)
        {
            if (m_currentSpeed <= m_speed) // ���� �ӷ� <= �̻��� �ӷ� ���
                m_currentSpeed += m_speed * Time.deltaTime; //�̻��� ���� ����

           transform.position += transform.up * m_currentSpeed * Time.deltaTime; //���� ��� �ö��ٰ� ���ư���.
            Vector3 t_dir = (Target.position - transform.position).normalized; 
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy")) //�� ��ü.
        {
            Destroy(col.gameObject); //�ı� �Ͻÿ�.
            Destroy(gameObject);
        }

    }
       
}
