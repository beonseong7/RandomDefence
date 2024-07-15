using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseinter : MonoBehaviour
{
    private LineRenderer line;
    public List<GameObject> SelectedGameObject=new List<GameObject>();
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    [SerializeField]
    private float minDistance = 0f;
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
            CheckObjectClear();
            previousPosition = Input.mousePosition;
            previousPosition.z = 0;
        }
        if (Input.GetMouseButton(0)) { this.DragOn(); }
        if(Input.GetMouseButtonUp(0)) { 
            line.positionCount = 0;
            currentPosition = Input.mousePosition;
            CheckObjectsInSelection();
        }
        if(Input.GetMouseButtonDown(1)) {
            ObjectMove();
        }

    }

    void DragOn()
    {
        currentPosition =Input.mousePosition;
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
    void ObjectMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 worldPosition = hit.point;

            foreach (GameObject obj in SelectedGameObject)
            {
                obj.transform.GetComponent<Character_>().Set_destination(worldPosition);
            }
        }
    }
    Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // 두 지점으로부터 사각형을 생성
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
    private void OnGUI()
    {
        
    }
    void CheckObjectClear()
    {
        foreach(GameObject obj in SelectedGameObject)
        {
            obj.transform.Find("Canvas").transform.GetChild(1).gameObject.SetActive(false);
        }
        SelectedGameObject.Clear();
    }
    void CheckObjectsInSelection()
    {
        // 선택된 사각형 영역 안에 오브젝트가 있는지 확인
        Rect selectionRect = GetScreenRect(previousPosition,currentPosition);
        var selectableObjects = InGameManager.instance.MyCharacter.transform;
        foreach (Transform obj in selectableObjects)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
            screenPos.y = Screen.height - screenPos.y;
            if (selectionRect.Contains(screenPos) && obj.GetComponent<PhotonView>().IsMine)
            {
                SelectedGameObject.Add(obj.gameObject);
                obj.transform.Find("Canvas").transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    // Update is called once per frame
}
