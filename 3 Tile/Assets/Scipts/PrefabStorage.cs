using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage instance { get; private set; }

    [Header("Brick Sprites")]
    public Sprite[] brickIcons;

    public BrickBase BrickBase;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public Sprite GetBrickIcon()
    {
        if (brickIcons == null || brickIcons.Length <= 0)
        {
            Debug.LogWarning("Prefab Storage don't have any sprites");
            return null;
        }

        int index = Random.Range(0, brickIcons.Length);
        return brickIcons[index];
    }
}
