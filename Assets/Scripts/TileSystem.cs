using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileSystem : MonoBehaviour
{
    public static TileSystem currentTileSystem;
    public Button addPlantButton; 

    public GridLayout gridLayout; 
    public Grid grid; 
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1; 

    private PlaceableObject objectToPlace;
    private bool objectPlaced = false;

    #region Unity Methods
    private void Awake()
    {
        currentTileSystem = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
        addPlantButton.onClick.AddListener(OnButtonClick);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (objectToPlace && !objectPlaced)
            {
                Debug.Log(objectToPlace.name + " has not been placed, cannot spawn another.");
                return;
            }
            else
            {
                InitializeWithObject(prefab1);
                objectPlaced = false;
            }
        }
        
        
    }
    #endregion

    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            // Debug.Log("Raycast hit at: " + raycastHit.point); // Log the hit point
            return raycastHit.point;
        }
        else
        {
            // Debug.Log("Raycast missed."); // Log when nothing is hit
            return Vector3.zero;
        }
    }
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x,v.y,z:0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }
    #endregion 
    
    #region Plant Placement

    private void OnButtonClick()
    {
        Debug.Log("Add Button Clicked!");
        if (objectToPlace && !objectPlaced)
            {
                Debug.Log(objectToPlace.name + " has not been placed, cannot spawn another.");
                return;
            }
            else
            {
                InitializeWithObject(prefab1);
                objectPlaced = false;
            }
    }

    public void PlaceOnTile()
    {
        if (CanBePlaced(objectToPlace))
        {
            objectToPlace.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            TakeArea(start, objectToPlace.Size);
            objectPlaced = true;
        }
        else
        {
            Destroy(objectToPlace.gameObject);
        }
    }

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
        Debug.Log("Added ObjectDrag component to: " + obj.name);
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach(var b in baseArray)
        {
            if(b == whiteTile)
            {
                return false;
            }
            
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
    }

    #endregion 
}
