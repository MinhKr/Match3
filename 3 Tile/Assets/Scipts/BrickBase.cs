using System.Collections;
using UnityEngine;

public class BrickBase : MonoBehaviour
{
    public Vector2Int gridPosition;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool isSelected = false;
    private static int brickCountInSlot = 0;
    private static float slotY = -3f;
    private static float slotSpacing = 1.2f;

    private static int maxSlot = 7;

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

    //----When clicked on the brick----
    private void OnMouseDown()
    {
        if (isSelected) return;

        if(brickCountInSlot >= maxSlot)
        {
            Debug.Log("Slot is full!");
            return;
        }

        isSelected = true;
        MoveToSlot();
    }

    private void MoveToSlot()
    {
        //calcu the position in the slot
        Vector3 targetPosition = new Vector3(brickCountInSlot * slotSpacing, slotY, 0);
        brickCountInSlot++;

        StartCoroutine(MoveSmooth(targetPosition));
    }

    private IEnumerator MoveSmooth(Vector3 target)
    {
        Vector3 startPosition = transform.position;
        float duration = 0;

        while (duration < 1f)
        {
            duration += Time.deltaTime * 2f;
            transform.position = Vector3.Lerp(startPosition, target, duration);
            yield return null;
        }

        transform.position = target;
    }
}
