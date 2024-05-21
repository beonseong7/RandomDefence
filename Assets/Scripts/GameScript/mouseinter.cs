using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseinter : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 previousPosition;

    [SerializeField]
    private float minDistance = 0.1f;
    [SerializeField]
    private float width = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        line.startWidth = line.endWidth = width;
        previousPosition = transform.position;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) { 
            previousPosition= Input.mousePosition;
            previousPosition.z = 0;
        }
        if (Input.GetMouseButton(0)) { this.DragOn(); }
        if(Input.GetMouseButtonUp(0)) { line.positionCount = 0; }
    }

    void DragOn()
    {
        Vector3 currentPosition =Input.mousePosition;
        currentPosition.z = 0f;
        line.positionCount = 5;
        if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
        {
            line.SetPosition(0, previousPosition);
            line.SetPosition(1, new Vector3(currentPosition.x,previousPosition.y,0));
            line.SetPosition(2, currentPosition);
            line.SetPosition(3, new Vector3(previousPosition.x, currentPosition.y, 0));
            line.SetPosition(4, previousPosition);
        }
    }
    // Update is called once per frame
}
