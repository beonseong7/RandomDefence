using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Script : MonoBehaviourPunCallbacks
{
    private AnimatorStateInfo animStateInfo;
    private Animator animator;
    float previousNormalizedTime=-0.1f;
    private float Damage;
    public void Set_Damage(float Damage)
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
       animator = GetComponent<Animator>();
        if (photonView.IsMine)
        {
            Damage=GameManager.instance.charac_Data.Get_Skill_status(this.gameObject.name.Split("(Clone)")[0]).Damage;
            this.GetComponent<CapsuleCollider>().enabled = true;
        }
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
                    PhotonNetwork.Destroy(this.transform.parent.gameObject);
                }
                previousNormalizedTime = currentNormalizedTime;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.gameObject.tag == "mob")
        {
            var tmp = other.GetComponent<MobScript>().Is_Damage(Damage);
            if (this.gameObject.name == "Arrow") PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
