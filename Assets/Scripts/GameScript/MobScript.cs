using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class MobScript : MonoBehaviour
{
    int corner = 0;
    Animator animator;
    AnimatorStateInfo animStateInfo;
    [Header("Status")]
    public float speed;
    public float Hp;
    public float physicArmor;
    public float magicArmor;
    public Transform field;
    void Start()
    {
        field = GameObject.Find("Player1Field").transform;
        transform.position=field.GetChild(3).position;
        animator=this.GetComponent<Animator>();
        animator.SetInteger("status", 1);
        StartCoroutine(MobMove());
    }
    private void Update()
    {
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    // Update is called once per frame
    IEnumerator MobMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, field.GetChild(corner).transform.position, speed);
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(IsArrive());
    }

    IEnumerator IsArrive()
    {
        if (transform.position == field.GetChild(corner).transform.position)
        {
            animator.SetTrigger("turn");
            yield return new WaitUntil(() => animStateInfo.IsName("Left turn") && animStateInfo.normalizedTime >= 1.0f);
            if(corner==3)
            {
                corner = 0;
            }
            else
            {
                corner++;
            }
            transform.Rotate(new Vector3(0, -90, 0));
        }
        StartCoroutine(MobMove());
    }

}
