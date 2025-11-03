using UnityEngine;

public class BrickBase : MonoBehaviour
{
    public Vector2Int gridPosition;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Init(Vector2Int pos, Sprite sprite)
    {
        gridPosition = pos;
        _spriteRenderer.sprite = sprite;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.9f);
    }
#endif
}
