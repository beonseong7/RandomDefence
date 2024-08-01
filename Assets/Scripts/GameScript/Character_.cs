using Photon.Pun;
using PlayFab.EconomyModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.UI.GridLayoutGroup;

public class Character_ : MonoBehaviourPunCallbacks
{
    private enum status { idle = 0,walking =1,attack=2};

    private status nowstatus;
    private Transform target;
    private Vector3 destination=Vector3.zero;
    private Animator animator;
    private bool is_attack=false;
    private AnimatorStateInfo animStateInfo;
    private TextMeshProUGUI Name;
    private float previousNormalizedTime;
    [Header("Status")]
    CharacData data;
    // Start is called before the first frame update
    void Start()
    {
        data = GameManager.instance.charac_Data.Get_Charac_status("¾ðÄ¿¸Õ");
        nowstatus = status.idle;
        animator = this.GetComponent<Animator>();
        this.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("status", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Continuous);
        Name =this.transform.Find("Canvas").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Name.text = this.name;
        Name.color = Color.black;
        if (photonView.IsMine)StartCoroutine(this.Charac_Anim());
    }
    // Update is called once per frame
    void Update()
    {
        if (target!=null&& target.gameObject.tag == "Dead")
        {
            target = null;
            }
        if (photonView.IsMine)
        {
            animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (destination != Vector3.zero)
            {
                nowstatus = status.walking;
            }
            else if (target != null)
            {
                nowstatus = status.attack;
            }
            else
            {
                nowstatus = status.idle;
            }
            if (animStateInfo.IsName("standing attack"))
            {
                float currentNormalizedTime = animStateInfo.normalizedTime % 1.0f;
                if (currentNormalizedTime < previousNormalizedTime)
                {
                    is_attack = false;
                }
                previousNormalizedTime = currentNormalizedTime;
                if (!is_attack)
                {
                    switch(data.type)
                    {
                        case basicType.Melee:
                            this.Event_Attack();
                            break;
                        case basicType.Ranged:
                            PhotonNetwork.Instantiate("Effect/Model/Prefab/" + data.skills[0].name,this.transform.position,this.transform.rotation);
                            break;
                        default:
                            break;
                    }
                    is_attack = true;
                }
       
            }
        }
    }
    public void LateUpdate()
    {
        Name.transform.forward = Camera.main.transform.forward;
    }
    IEnumerator Charac_Anim()
    {
        animator.SetInteger("status",(int)nowstatus);
        switch(nowstatus)
        {
            case status.idle:
                break;
            case status.attack:
                if (target!=null)
                    transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
                break;
            case status.walking:
                transform.GetComponent<CapsuleCollider>().enabled = false;
                while (Vector3.Distance(transform.position, destination) >120f)
                {
                    transform.LookAt(new Vector3(destination.x, this.transform.position.y, destination.z));
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, transform.position.y, destination.z), data.Speed);
                    yield return new WaitForSeconds(0.01f);
                }
                transform.GetComponent<CapsuleCollider>().enabled = true;
                destination = Vector3.zero;
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(this.Charac_Anim());
    }
    private void OnCollisionStay(Collision collision)
    {
        if (photonView.IsMine)
        {
            Rigidbody otherRb = collision.collider.attachedRigidbody;

            if (otherRb != null)
            {
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.Normalize();

                otherRb.MovePosition(otherRb.transform.position + pushDirection * 2f);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (target == null&& photonView.IsMine)
            if (other.gameObject.tag == "mob")
            {
                target = other.transform;
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (target != null && photonView.IsMine)
            if (other.gameObject.tag=="mob"&&other.gameObject.name == target.gameObject.name)
                target = null;
    }
    public void Event_Attack()
    {
        if (target != null && photonView.IsMine)
        {
            target.GetComponent<MobScript>().Is_Damage(data.Damage);
            if (target.GetComponent<MobScript>().mob_Hp <= 0) target = null;
        }

    }
    public void Set_destination(Vector3 tmp)
    {
        destination = tmp;
    }
    [PunRPC]
    public void Cha_Destroy()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(this.gameObject);
    }
}
