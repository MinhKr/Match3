using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage instance { get; private set; }

    [Header("Brick Sprites")]
    public Sprite[] brickSprites;

    public BrickBase BrickBase;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public Sprite GetBrickSprite()
    {
        if (brickSprites == null || brickSprites.Length <= 0)
        {
            Debug.LogWarning("Prefab Storage don't have any sprites");
            return null;
        }

        int index = Random.Range(0, brickSprites.Length);
        return brickSprites[index];
    }
}
