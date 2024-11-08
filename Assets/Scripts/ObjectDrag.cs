using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using System.Numerics;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - TileSystem.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 pos = TileSystem.GetMouseWorldPosition() + offset;
        transform.position = TileSystem.currentTileSystem.SnapCoordinateToGrid(pos);
    }

    private void OnMouseUp()
    {
        TileSystem.currentTileSystem.PlaceOnTile();
    }
}
