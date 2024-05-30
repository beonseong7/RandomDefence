using PlayFab.EconomyModels;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Character_ : MonoBehaviour
{
    public enum status {walking,idle,attack };
    public status nowstatus;
    public Transform target;
    public Animator animator;
    public AnimatorStateInfo animStateInfo;
    [Header("Status")]
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
        if (target == null)
        {
            animator.SetInteger("status", 0);
        }
    }
    IEnumerator Charac_Anim()
    {
        if (target != null && nowstatus != status.walking)
        {
            transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
            animator.SetInteger("status", 2);
            yield return new WaitUntil(() => animStateInfo.IsName("standing attack") && animStateInfo.normalizedTime >= 1.0f);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(this.Charac_Anim());
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "mob")
            if (target ==null)
            {
                target = other.transform;
                Debug.Log(target.gameObject.name);
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == target.gameObject.name)
        {
            Debug.Log("sdfg");
            target = null;
        }
    }
    public void Allstop()
    {
        StopAllCoroutines();
    }
    public IEnumerator Charac_Move(Vector3 target)
    {
        nowstatus = status.walking;
            while(Vector3.Distance(transform.position,target)>0.1f)
            { 
            transform.position = Vector3.MoveTowards(transform.position,new Vector3(target.x,transform.position.y,target.z),speed);
            yield return new WaitForSeconds(0.1f);
            }
    }
}
