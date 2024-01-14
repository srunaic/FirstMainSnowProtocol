using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class MultiLiftCatch : MonoBehaviour, IPunObservable
{
    [Header("집게가 인형을 잡았을때 판정.")]
    bool IsTongsHoldingDoll = false;
    public LiftMultiDoll _LiftMove; //기계 본체 접촉.

    [Header("인형 컨트롤")]
    private Rigidbody dollRigidbody;//인형의 리지드 바디.
    private Collider dollCollider;

    [Header("집게 컨트롤")]
    private Collider legDollPos2Collider;

    [Header("애니메이션 추가")]
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //원격 전송방식.
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
        if (IsTongsHoldingDoll)//인형을 잡는 중인지?
        {
            _LiftMove._Doll.position = _LiftMove.ZilePos.position;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachDollFromTongs();
        }
    }

    private void AttachDollToTongs()//인형을 잡고있다면,
    {
        IsTongsHoldingDoll = true;
        _LiftMove._Doll.position = _LiftMove.ZilePos.position;

        LiftAnim.SetTrigger("ZileLift");

        dollRigidbody.isKinematic = true;
        dollCollider.enabled = false;
    }

    private void DetachDollFromTongs()//인형을 놓아주는 부분
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
