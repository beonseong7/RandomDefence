using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobScript : MonoBehaviour
{
    int corner = 0;
    Animator animator;
    [Header("Status")]
    public float speed;
    public float Hp;
    public float physicArmor;
    public float magicArmor;
    public int turn_speed;
    void Start()
    {
        animator=this.GetComponent<Animator>();
        StartCoroutine(MobMove());
    }

    // Update is called once per frame
    IEnumerator MobMove()
    {
        switch (corner % 4)
        {
            case 0:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1607, 6, -1612), speed);
                break;
            case 1:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(1597, 6, -1612), speed);
                break;
            case 2:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(1597, 6, 1421), speed);
                break;
            case 3:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1607, 6, 1421), speed);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(IsArrive());
    }

    IEnumerator IsArrive()
    {
        if (transform.position == new Vector3(-1607, 6, -1612))
        {

            for(int i = 0;i<90/ Mathf.Abs(turn_speed); i++)
            {
                transform.Rotate(new Vector3(0, turn_speed, 0));
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0, 90, 0);
            corner = 1;
        }
        else if (transform.position == new Vector3(1597, 6, -1612))
        {
            for (int i = 0; i < 90 / Mathf.Abs(turn_speed); i++)
            {
                transform.Rotate(new Vector3(0, turn_speed, 0));
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0, 0, 0);
            corner = 2;
        }
        else if (transform.position == new Vector3(1597, 6, 1421))
        {
            for (int i = 0; i < 90 / Mathf.Abs(turn_speed); i++)
            {
                transform.Rotate(new Vector3(0, turn_speed, 0));
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0, 270, 0);
            corner = 3;
        }
        else if (transform.position == new Vector3(-1607, 6, 1421))
        {
            for (int i = 0; i < 90 / Mathf.Abs(turn_speed); i++)
            {
                transform.Rotate(new Vector3(0, turn_speed * Time.deltaTime, 0));
                yield return new WaitForSeconds(0.01f);
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
            corner = 0;
    }
        StartCoroutine(MobMove());
    }
    OnAnimation

}
