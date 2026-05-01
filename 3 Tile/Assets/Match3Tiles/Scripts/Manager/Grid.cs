using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;
        int xCount = Mathf.RoundToInt(position.x / cellSize);
        int yCount = Mathf.RoundToInt(position.y / cellSize);
        int zCount = Mathf.RoundToInt(position.z / cellSize);

        Vector3 result = new Vector3(xCount * cellSize, yCount * cellSize, zCount * cellSize);

        result += transform.position;
        return result;
    }

    public Vector3 GetNearestPointOnGrid(float x, float y, float z)
    {
        Vector3 position = new Vector3(x, y, z) - transform.localPosition;

        int xCount = Mathf.RoundToInt(position.x / cellSize);
        int yCount = Mathf.RoundToInt(position.y / cellSize);
        int zCount = Mathf.RoundToInt(position.z / cellSize);

        Vector3 result = new Vector3(xCount * cellSize, yCount * cellSize, zCount * cellSize);
        result += transform.localPosition;

        return result;
    }
}
