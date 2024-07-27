using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Script : MonoBehaviourPunCallbacks
{
    private AnimatorStateInfo animStateInfo;
    private Animator animator;
    float previousNormalizedTime=-0.1f;
    // Start is called before the first frame update
    void Start()
    {
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine) {
            animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animStateInfo.IsName("firing"))
            {
                float currentNormalizedTime = animStateInfo.normalizedTime % 1.0f;
                if (currentNormalizedTime < previousNormalizedTime)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
                previousNormalizedTime = currentNormalizedTime;
            }
        }
    }
}
