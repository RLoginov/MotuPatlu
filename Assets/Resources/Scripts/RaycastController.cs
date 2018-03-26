using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    // lines to detect collisions
    public LayerMask collisionMask;
    public const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    // spacing in betweeen each line
    [HideInInspector]
    public float horizontalRaySpacing;
    public float verticalRaySpacing;

    // collider and raycast component
    public new BoxCollider2D collider;  
    public RaycastOrigins raycastOrigins;

    public virtual void Start()
    {
        // get collider component and calculate spacing between raycast lines
        collider = GetComponent<BoxCollider2D>();
        calculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        // set bounds between edges of object
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        // raycast lines on all sides of object
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    public void calculateRaySpacing()
    {
        // set bounds between edges of object
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        // make all lines appear between bounds
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        // space out each line
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        // vectors for the directions of each raycasted line
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

}
