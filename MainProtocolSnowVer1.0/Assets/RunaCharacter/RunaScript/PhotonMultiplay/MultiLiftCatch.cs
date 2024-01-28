using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class MultiLiftCatch : MonoBehaviour, IPunObservable
{
    public enum KindDoll { None, RabbitDoll1, RabbitDoll2 ,HumanDoll }

    [Header("catchKindDoll")]
    public KindDoll _kindDoll = KindDoll.None;

    [Header("Judgment when the tongs catch the doll.")]
    bool isTongsHoldingDoll = false;
    public LiftMultiDoll _LiftMove;

    [Header("Puppet Control")]
    private Rigidbody[] dollRigidbodies = new Rigidbody[2];
    private Collider[] dollColliders = new Collider[2];

    [Header("Pinch Control")]
    private Collider legDollPos2Collider;

    [Header("Add Animation")]
    public Animator LiftAnim;
    private Transform _LiftArmTr;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    private void Start()
    {
        _LiftArmTr = GetComponent<Transform>();

        for (int i = 0; i < 2; i++)
        {
            dollRigidbodies[i] = _LiftMove._Doll[i].GetComponent<Rigidbody>();
            dollColliders[i] = _LiftMove._Doll[i].GetComponent<Collider>();
        }

        legDollPos2Collider = _LiftMove._LegDollPos2.GetComponent<Collider>();
        LiftAnim = GetComponent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RabbitDoll1"))
        {
            _kindDoll = KindDoll.RabbitDoll1;
            AttachDollToTongs(0);
        }

        else if (other.CompareTag("RabbitDoll2"))
        {
            _kindDoll = KindDoll.RabbitDoll2;
            AttachDollToTongs(1);
        }

    }

    private void Update()
    {
        if (isTongsHoldingDoll)
        {
            int dollIndex = (_kindDoll == KindDoll.RabbitDoll1) ? 0 : 1;
            _LiftMove._Doll[dollIndex].position = _LiftMove.ZilePos.position;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachDollFromTongs();
            _kindDoll = KindDoll.None;
        }
    }

    private void AttachDollToTongs(int dollIndex)
    {
        if (!isTongsHoldingDoll)
        {
            isTongsHoldingDoll = true;

            _LiftMove._Doll[dollIndex].position = _LiftMove.ZilePos.position;
            LiftAnim.SetTrigger("ZileLift");
            
            dollRigidbodies[dollIndex].isKinematic = true;
            dollColliders[dollIndex].enabled = false;
        }
    }

    private void DetachDollFromTongs()
    {
        isTongsHoldingDoll = false;

        for (int i = 0; i < 2; i++)
        {
            dollRigidbodies[i].isKinematic = false;
            dollColliders[i].enabled = true;
        }
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