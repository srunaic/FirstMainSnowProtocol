using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StepDust : MonoBehaviourPunCallbacks
{
    public Transform Foot;

    public Vector3 checkoffset;
    public float checkHeight;
    public float checkSize = 2f;

    //private ParticleSystem effect;
    private PhotonView Pv;
    private bool onCheckFoot= false;

    private void Start()
    {
        Pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        Pv.RPC("checkStep", RpcTarget.All);
    }

    [PunRPC]
    void checkStep()
    {
        if (Foot != null)
        {
            Vector3 FootStep1 = Foot.position + Vector3.zero;

            Ray checkRay = new Ray(Foot.transform.position + transform.rotation * checkoffset, Vector3.down);
            if (Physics.SphereCast(checkRay, checkSize, out RaycastHit hit, checkHeight))
            {
                if (!onCheckFoot)
                {
                    //Instantiate(effect, hit.point, Quaternion.identity);
                    PhotonNetwork.Instantiate("FootStepParticle",Vector3.zero + hit.point,Quaternion.identity);
                    onCheckFoot = true;
                }

            }
            else
            {
                onCheckFoot = false;
              
            }
        }
      
    }
}
