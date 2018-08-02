using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(BoxCollider2D))]

public class Controller2D : MonoBehaviour {

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float horizontalRaySpacing, verticalRaySpacing;

    public LayerMask collisionMask;
	public bool gameOver;

    public BoxCollider2D colliderBox;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisionInfo;

    // Use this for initialization
    void Start () {

        colliderBox = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
		gameOver = false;

    }

    public void Move(Vector2 velocity) {
        UpdateRaycastOrigins();
		collisionInfo.Reset();
        if (velocity.x != 0) {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0){
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

	public RaycastHit2D TouchHit(int maxDistance, LayerMask collisionMask)
	{
		Vector2 touch = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, touch - (Vector2)transform.position, maxDistance, collisionMask);
		return hit;

	}

	public RaycastHit2D MouseClickHit(int maxDistance, LayerMask collisionMask) {

		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, mouse - (Vector2)transform.position, maxDistance, collisionMask);
		return hit;

	}

    void VerticalCollisions(ref Vector2 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.green);

			if (hit){
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
				//sets them true
				collisionInfo.below = (directionY == -1);
				collisionInfo.above = (directionY == 1);

				if(hit.transform.gameObject.layer == LayerMask.NameToLayer("GameOver")) {
					gameOver = true;
				}
			}

		}

    }

    void HorizontalCollisions(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);

            if (hit) {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
				//sets them true                
				collisionInfo.left = (directionX == -1);
				collisionInfo.right = (directionX == 1);
            }
        }
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = colliderBox.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

    void CalculateRaySpacing() {
        Bounds bounds = colliderBox.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

    }

    struct RaycastOrigins {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below, left, right;

        public void Reset(){
            above = below = left = right = false;
        }
    }
}
