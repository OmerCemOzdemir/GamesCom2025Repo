using UnityEngine;

public class Gizmo : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private Vector2 size = new Vector2(1, 1);
    [SerializeField] private Vector3 offset = new Vector2(0, 0);


    [SerializeField] private Color color = Color.green;
    [SerializeField] private Shape shape = Shape.Square;


    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        switch (shape)
        {
            case Shape.Square:
                Gizmos.DrawWireCube(transform.position + offset, size);
                break;
            case Shape.Circle:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            default:
                break;
        }

        //DrawWireCircle(transform.position, radius);
    }

}

enum Shape
{
    Square,
    Circle
}

/*
     // Helper method to draw a circle in the Scene view
    private void DrawWireCircle(Vector3 center, float radius, int segments = 32)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
 
 */