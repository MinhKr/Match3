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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BrickBase.ResetAllSlot();
        }
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
                brick.Init(new Vector2Int(x, y), PrefabStorage.instance.GetBrickIcon());//Random icon when init
            }
        }

        // Center the grid
        Vector3 offset = new Vector3(-width / 2f + 0.5f, -height / 4f + 0.5f, 0);
        transform.position = offset;


        Debug.Log($"Đã sinh {width * height} viên gạch.");
    }

}
