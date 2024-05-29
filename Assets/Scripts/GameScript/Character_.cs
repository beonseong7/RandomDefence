using PlayFab.EconomyModels;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character_ : MonoBehaviour
{
    public enum status {walking,idle,attack };
    public status nowstatus;
    public Transform target;
    public Animator animator;
    public AnimatorStateInfo animStateInfo;
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
    }
    IEnumerator Charac_Anim()
    {
        if (target != null && nowstatus != status.walking)
        {
            transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
            animator.SetInteger("status", 2);
            yield return new WaitUntil(() => animStateInfo.IsName("standing attack") && animStateInfo.normalizedTime >= 1.0f);
        }
        else if (target == null&&nowstatus == status.idle)
        {
            animator.SetInteger("status", 0);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(this.Charac_Anim());
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
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
}
