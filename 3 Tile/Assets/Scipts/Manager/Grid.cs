using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;

    public Vector3 GetNearestPoint(Vector3 position)
    {
        position -= transform.position;
        int xCount = Mathf.RoundToInt(position.x / cellSize);
        int yCount = Mathf.RoundToInt(position.y / cellSize);
        int zCount = Mathf.RoundToInt(position.z / cellSize);

        Vector3 result = new Vector3(xCount * cellSize, yCount * cellSize, zCount * cellSize);

        result += transform.position;
        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        // Số ô hiển thị quanh gốc
        int gridExtent = 5;

        for (int x = -gridExtent; x <= gridExtent; x++)
        {
            for (int y = -gridExtent; y <= gridExtent; y++)
            {
                Vector3 point = GetNearestPoint(new Vector3(x, y));
                Gizmos.DrawWireCube(point, Vector3.one * 0.95f);
            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 1.2f);
    }
#endif

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 snapped = GetNearestPoint(mousePos);

            Debug.Log($"Click: {mousePos} → Snap: {snapped}");

            // Tạo tạm một hình tròn đỏ tại vị trí snap
            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dot.transform.position = snapped;
            dot.transform.localScale = Vector3.one * 0.2f;
            dot.GetComponent<SpriteRenderer>();
        }
    }

}
