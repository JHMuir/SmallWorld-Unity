using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSystem : MonoBehaviour
{
    public static TileSystem currentTileSystem;

    public GridLayout gridLayout; 
    public Grid grid; 
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1; 

    private PlaceableObject objectToPlace;

    #region Unity Methods
    private void Awake()
    {
        currentTileSystem = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A Pressed.");
            InitializeWithObject(prefab1);
        }
        // else if(Input.GetKeyDown(KeyCode.B))
        // {
        //     InitializeWithObject(prefab2);
        // }
    }
    #endregion

    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Debug.Log("Raycast hit at: " + raycastHit.point); // Log the hit point
            return raycastHit.point;
        }
        else
        {
            Debug.Log("Raycast missed."); // Log when nothing is hit
            return Vector3.zero;
        }
    }
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }
    #endregion 
    
    #region Plant Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
        Debug.Log("Added ObjectDrag component to: " + obj.name);
    }


    #endregion 
}
