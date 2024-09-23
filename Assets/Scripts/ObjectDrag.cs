using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        Debug.Log("Mouse down on object: " + gameObject.name); // Log when the object is clicked
        offset = transform.position - TileSystem.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Debug.Log("Dragging object: " + gameObject.name); // Log during dragging
        Vector3 pos = TileSystem.GetMouseWorldPosition() + offset;
        transform.position = TileSystem.currentTileSystem.SnapCoordinateToGrid(pos);
    }
}
