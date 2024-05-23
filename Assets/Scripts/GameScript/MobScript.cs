using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobScript : MonoBehaviour
{
    int corner = 0;
    [Header("Status")]
    public float speed;
    public float Hp;
    public float physicArmor;
    public float magicArmor;
    void Start()
    {
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
        IsArrive();
    }

    void IsArrive()
    {
        if (transform.position == new Vector3(-1607, 6, -1612))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            corner = 1;
        }
        else if (transform.position == new Vector3(1597, 6, -1612))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            corner = 2;
        }
        else if (transform.position == new Vector3(1597, 6, 1421))
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            corner = 3;
        }
        else if (transform.position == new Vector3(-1607, 6, 1421))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            corner = 0;
    }
        StartCoroutine(MobMove());
    }

}
