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

        Collider[] t_cols = Physics.OverlapSphere(transform.position, 1000f, m_layerMask); //100m 전방 검출

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

        yield return new WaitForSeconds(5f); //날았는데 아무 일이 없다면 5초 뒤 파괴.
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
            if (m_currentSpeed <= m_speed) // 현재 속력 <= 미사일 속력 비례
                m_currentSpeed += m_speed * Time.deltaTime; //미사일 실행 여부

           transform.position += transform.up * m_currentSpeed * Time.deltaTime; //위로 잠시 올랐다가 날아가라.
            Vector3 t_dir = (Target.position - transform.position).normalized; 
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy")) //적 객체.
        {
            Destroy(col.gameObject); //파괴 하시오.
            Destroy(gameObject);
        }

    }
       
}
