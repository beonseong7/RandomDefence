using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public class MobScript : MonoBehaviour
{
    int corner = 0;
    Animator animator;
    AnimatorStateInfo animStateInfo;
    [Header("Status")]
    public float speed;
    public float physicArmor;
    public float magicArmor;
    private float Hp;
    public Transform field;
    public Slider HP_Slider;
    public float mob_Hp
    {
        get
        {
            return Hp;
        }
        set
        {
            Hp = value;
        }
    }

    public bool Is_Damage(float damage)
    {
        Hp -= damage;
        HP_Slider.value = Hp;
        if (Hp <= 0)
        {
            tag = "Dead";
            StopAllCoroutines();
            StartCoroutine(Is_Die());
            return true;
        }
         return false;
    }
    void Start()
    {
        HP_Slider.maxValue = Hp;
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
    private void LateUpdate()
    {
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        HP_Slider.transform.forward=Camera.main.transform.forward;
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
    IEnumerator Is_Die()
    {
        
        animator.SetInteger("status", 4);
        yield return new WaitUntil(() => animStateInfo.IsName("Dying") && animStateInfo.normalizedTime >= 1.0f);
        Destroy(this.gameObject);
    }

}
