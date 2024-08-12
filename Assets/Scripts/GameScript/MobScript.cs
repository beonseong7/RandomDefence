using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobScript : MonoBehaviourPunCallbacks
{
    int corner = 0;
    Animator animator;
    AnimatorStateInfo animStateInfo;
    [Header("Status")]
    public float speed;
    public float physicArmor;
    public float magicArmor;
    private float Hp;
    public Transform field=null;
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
        if (Hp > 0)
        {
            Hp -= damage;
            this.GetComponent<PhotonView>().RPC("View_HP", RpcTarget.All, Hp);
            return false;
        }
        return true;
    }
    [PunRPC]
    public void View_HP(float HP)
    {
        HP_Slider.value = HP;
        if(HP<=0 && this.GetComponent<PhotonView>().IsMine) 
        {
            tag = "Dead";
            StopAllCoroutines();
            StartCoroutine(Is_Die());
        }
    }
    void Start()
    {
        
        HP_Slider.maxValue = InGameManager.instance.stage*10.0f;
        Hp= InGameManager.instance.stage * 10.0f;
        HP_Slider.value = Hp;
        animator =this.GetComponent<Animator>();
        animator.SetInteger("status", 1);
        if (photonView.IsMine)
        {
            transform.SetParent(GameObject.Find("MyMobs").transform);
            field = GameObject.Find(PhotonNetwork.LocalPlayer.ActorNumber + "Field").transform;
            transform.position = field.GetChild(3).position;
            StartCoroutine(MobMove());
        }
        else
        {
            transform.SetParent(GameObject.Find("Mobs").transform);
        }
            
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
        yield return new WaitUntil(()=>field !=null);
        transform.position = Vector3.MoveTowards(transform.position, field.GetChild(corner).transform.position, speed);
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(IsArrive());
    }

    IEnumerator IsArrive()
    {
        if (transform.position == field.GetChild(corner).transform.position)
        {
            animator.SetInteger("status", 2);
            yield return new WaitUntil(() => animStateInfo.IsName("Left turn") && animStateInfo.normalizedTime >= 1.0f);
            animator.SetInteger("status",1);
            if (corner==3)
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
        PhotonNetwork.Destroy(this.gameObject);
    }

}
