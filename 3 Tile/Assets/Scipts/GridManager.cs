using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
