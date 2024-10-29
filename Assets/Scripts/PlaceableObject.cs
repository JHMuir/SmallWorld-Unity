using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableObject : MonoBehaviour
{
    // public bool Placed{ get; private set; }
    public Vector3Int Size { get; private set; }
    private Vector3[] Vertices;
    private bool planted = false;

    private GameObject popupPrefab;
    private GameObject popupInstance;
    public string objectInfo = "<Enter Information Here";

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;

    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = TileSystem.currentTileSystem.gridLayout.WorldToCell(worldPos);;
        }

        Size = new Vector3Int(x:Math.Abs((vertices[0] - vertices[1]).x), 
                              y:Math.Abs((vertices[0] - vertices[3]).y), 
                              z:1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
        popupPrefab = Resources.Load<GameObject>("Window");

    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);
        planted = true;

        //Create placement event, if applicable
    }

    private void OnMouseDown()
    {
        if(planted)
        {
            ShowPopup();
        }
    }

    private void ShowPopup()
    {
        if (popupPrefab != null && popupInstance == null)
        {
            popupInstance = Instantiate(popupPrefab, transform.position, Quaternion.identity);
            popupInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

            Text body = popupInstance.GetComponent<Text>();
            if(body != null)
            {
                body.text = objectInfo;
            }
        }
    }
    
}
