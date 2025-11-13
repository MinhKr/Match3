using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBase : MonoBehaviour
{
    public Vector2Int gridPosition;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool isSelected = false;

    private static int brickCountInSlot = 0;
    private static float slotY = -3f;
    private static float slotSpacing = 0.7f;
    private static int maxSlot = 7;

    private static List<BrickBase> fallenBricks = new List<BrickBase>();

    public void Init(Vector2Int pos, Sprite icon)
    {
        gridPosition = pos;

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        //Set the icon
        if (icon != null)
        {
            _spriteRenderer.sprite = icon;
        }
        else
        {
            Sprite randomIcon = PrefabStorage.instance.GetBrickIcon();
            _spriteRenderer.sprite = randomIcon;
        }
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

        if (brickCountInSlot >= maxSlot)
        {
            Debug.Log("Slot is full!");
            return;
        }

        isSelected = true;
        MoveToSlot();
    }

    private void MoveToSlot()
    {
        //calculate the position in the slot
        Vector3 targetPosition = new Vector3((brickCountInSlot * slotSpacing) - 2, slotY, 0);
        brickCountInSlot++;
        fallenBricks.Add(this);

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

    public static void ResetAllSlot()
    {
        Debug.Log("Resetting all fallen bricks in slot.");
        foreach (var brick in fallenBricks)
        {
            if (brick != null)
                Destroy(brick.gameObject);
        }

        fallenBricks.Clear();
        brickCountInSlot = 0;
    }
}
