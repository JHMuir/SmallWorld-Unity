using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSystem : MonoBehaviour
{
    public static TileSystem current;

    public GridLayout gridLayout; 
    public Grid grid; 
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject prefab1; 

    private PlaceableObject objectToPlace;

    #region Unity Methods
    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();

    }
    #endregion

    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 SnapCoordianteToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }
    #endregion
}
