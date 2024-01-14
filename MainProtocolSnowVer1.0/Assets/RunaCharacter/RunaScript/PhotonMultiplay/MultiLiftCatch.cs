using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class MultiLiftCatch : MonoBehaviour, IPunObservable
{
    [Header("���԰� ������ ������� ����.")]
    bool IsTongsHoldingDoll = false;
    public LiftMultiDoll _LiftMove; //��� ��ü ����.

    [Header("���� ��Ʈ��")]
    private Rigidbody dollRigidbody;//������ ������ �ٵ�.
    private Collider dollCollider;

    [Header("���� ��Ʈ��")]
    private Collider legDollPos2Collider;

    [Header("�ִϸ��̼� �߰�")]
    public Animator LiftAnim;
    private Transform _LiftArmTr;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    private void Start()
    {
        _LiftArmTr = GetComponent<Transform>();
        dollRigidbody = _LiftMove._Doll.GetComponent<Rigidbody>();
        dollCollider = _LiftMove._Doll.GetComponent<Collider>();
        legDollPos2Collider = _LiftMove._LegDollPos2.GetComponent<Collider>();

        LiftAnim = GetComponent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //���� ���۹��.
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_LiftArmTr.position);
            stream.SendNext(_LiftArmTr.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
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
    }

    private IEnumerator DisableColliderForDuration(float duration)
    {
        legDollPos2Collider.enabled = false;
        yield return new WaitForSeconds(duration);
        legDollPos2Collider.enabled = true;
    }
  
}
