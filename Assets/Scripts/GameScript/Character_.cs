using Photon.Pun;
using PlayFab.EconomyModels;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Character_ : MonoBehaviour
{
    public enum status { idle = 0,walking =1,attack=2};
    public status nowstatus;
    public Transform target;
    public Vector3 destination=Vector3.zero;
    public Animator animator;
    public AnimatorStateInfo animStateInfo;
    public PhotonView Cha_photonview;
    public string owner;
    [Header("Status")]
    public float attack;
    public float speed=3;
    // Start is called before the first frame update
    void Start()
    {
        nowstatus = status.idle;
        animator = GetComponent<Animator>();
        this.GetComponent<PhotonAnimatorView>().SetParameterSynchronized("status", PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Continuous);
         if (Cha_photonview.IsMine)StartCoroutine(this.Charac_Anim());
    }
    // Update is called once per frame
    void Update()
    {
        if (target!=null&& target.gameObject.tag == "Dead")
        {
            target = null;
            }
        if (Cha_photonview.IsMine)
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
        }
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
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, transform.position.y, destination.z), speed);
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
        if (Cha_photonview.IsMine)
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
        if (target == null&& Cha_photonview.IsMine)
            if (other.gameObject.tag == "mob")
            {
                target = other.transform;
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (target != null && Cha_photonview.IsMine)
            if (other.gameObject.tag=="mob"&&other.gameObject.name == target.gameObject.name)
                target = null;
    }
    public void Event_Attack()
    {
        if (target != null && Cha_photonview.IsMine)
        {
            target.GetComponent<MobScript>().Is_Damage(attack);
            if (target.GetComponent<MobScript>().mob_Hp<=0)target = null;
        }
             
    }
}
