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
    public string owner;
    [Header("Status")]
    public float attack;
    public float speed=3;
    // Start is called before the first frame update
    void Start()
    {
        nowstatus = status.idle;
        animator = GetComponent<Animator>();
        StartCoroutine(this.Charac_Anim());
    }

    // Update is called once per frame
    void Update()
    {
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (destination!=Vector3.zero)
        {
            nowstatus = status.walking;
        }
        else if(target !=null) 
        {
           nowstatus=status.attack;
        }
        else
        {
            nowstatus=status.idle;
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
                transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
                break;
            case status.walking:
                transform.GetComponent<CapsuleCollider>().enabled = false;
                while (Vector3.Distance(transform.position, destination) >110f)
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
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "mob")
            if (target ==null)
            {
                target = other.transform;
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag=="mob"&&other.gameObject.name == target.gameObject.name)
        {
            target = null;
        }
    }
}
