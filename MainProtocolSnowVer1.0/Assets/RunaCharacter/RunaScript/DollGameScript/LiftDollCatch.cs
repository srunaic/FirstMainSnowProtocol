using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiftDollCatch : MonoBehaviour
{
    [Header("���԰� ������ ������� ����.")]
    bool IsTongsHoldingDoll = false;
    public LiftMoving _LiftMove;

    [Header("���� ��Ʈ��")]
    private Rigidbody dollRigidbody;//������ ������ �ٵ�.
    private Collider dollCollider;

    [Header("���� ��Ʈ��")]
    private Collider legDollPos2Collider;

    [Header("�ִϸ��̼� �߰�")]
    public Animator LiftAnim;

    [Header("UI �ؽ�Ʈ �˸�")]
    public TextMeshProUGUI GetDollTxt;

    private void Start()
    {
        _LiftMove = FindObjectOfType<LiftMoving>();
        dollRigidbody = _LiftMove._Doll.GetComponent<Rigidbody>();
        dollCollider = _LiftMove._Doll.GetComponent<Collider>();
        legDollPos2Collider = _LiftMove._LegDollPos2.GetComponent<Collider>();

        LiftAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Doll"))
        {
            AttachDollToTongs();
        }

    }
    private void Update()
    {
        if (IsTongsHoldingDoll)//������ ��� ������?
        {
            _LiftMove._Doll.position = _LiftMove.ZilePos.position;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachDollFromTongs();
        }
    }

    private void AttachDollToTongs()//������ ����ִٸ�,
    {
        IsTongsHoldingDoll = true;
        _LiftMove._Doll.position = _LiftMove.ZilePos.position;

        LiftAnim.SetTrigger("ZileLift");

        dollRigidbody.isKinematic = true;
        dollCollider.enabled = false;
    }

    private void DetachDollFromTongs()//������ �����ִ� �κ�
    {
        IsTongsHoldingDoll = false;
        dollRigidbody.isKinematic = false;
        dollCollider.enabled = true;
        LiftAnim.SetTrigger("PutZile");
        StartCoroutine(DisableColliderForDuration(2f));

        //StartCoroutine(FallAndDisappear());
    }

    private IEnumerator DisableColliderForDuration(float duration)
    {
        legDollPos2Collider.enabled = false;
        yield return new WaitForSeconds(duration);
        legDollPos2Collider.enabled = true;
    }
    public IEnumerator TextShowDollCheck(float TextDuration)
    {
        yield return new WaitForSeconds(TextDuration);
        GetDollTxt.enabled = false;
    }

    /*private IEnumerator FallAndDisappear()
    {
        yield return new WaitForSeconds(2f);
        Destroy(_LiftMove._Doll.gameObject);
        
    }*/
}
