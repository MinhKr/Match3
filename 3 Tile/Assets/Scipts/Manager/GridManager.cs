using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [Header("References")]
    public GameObject brickPrefab;
    public Grid grid;

    [Header("Settings")]
    public int width = 5;
    public int height = 5;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        if (grid == null || brickPrefab == null)
        {
            Debug.LogError("Chưa gán Grid hoặc BrickPrefab!");
            return;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = grid.GetNearestPoint(new Vector3(x, y, 0));
                GameObject brickObj = Instantiate(brickPrefab, worldPos, Quaternion.identity, transform);

                BrickBase brick = brickObj.GetComponent<BrickBase>();
                brick.Init(new Vector2Int(x, y), null); // chưa cần sprite
            }
        }

        Debug.Log($"Đã sinh {width * height} viên gạch.");
    }

}
