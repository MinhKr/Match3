using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBase : MonoBehaviour
{
    // =====================================================================
    //  Instance Variables
    // =====================================================================
    public Vector2Int gridPosition;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _isSelected = false;
    public Sprite typeOfBrick;

    public System.Action<BrickBase> onMoveFinished;

    // =====================================================================
    //  Slot System (STATIC)
    // =====================================================================
    private static List<BrickBase> fallenBricks = new List<BrickBase>();

    private static int brickCountInSlot = 0;
    private static int maxSlot = 7;

    private static float slotY = -3f;
    private static float slotSpacing = 0.7f;
    private static float slotXOffset = -2f;

    // =====================================================================
    //  INIT
    // =====================================================================
    public void Init(Vector2Int pos, Sprite icon)
    {
        gridPosition = pos;

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        // Set sprite
        if (icon == null)
            icon = PrefabStorage.instance.GetBrickIcon();

        typeOfBrick = icon;
        _spriteRenderer.sprite = icon;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.9f);
    }
#endif

    // =====================================================================
    //  CLICK
    // =====================================================================
    private void OnMouseDown()
    {
        if (_isSelected) return;

        if (brickCountInSlot >= maxSlot)
        {
            Debug.Log("Slot is full!");
            return;
        }

        _isSelected = true;
        MoveToSlot();
    }

    // =====================================================================
    //  MOVE TO SLOT
    // =====================================================================
    private void MoveToSlot()
    {
        Vector3 targetPos = new Vector3((brickCountInSlot * slotSpacing) + slotXOffset, slotY, 0f);

        brickCountInSlot++;
        fallenBricks.Add(this);

        onMoveFinished = OnBrickArrivedSlot;  // register callback
        StartCoroutine(MoveSmooth(targetPos));
    }

    private IEnumerator MoveSmooth(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;

        // fire event
        onMoveFinished?.Invoke(this);
        onMoveFinished = null;   // reset callback!
    }

    // =====================================================================
    //  ARRIVED SLOT → CHECK MATCH
    // =====================================================================
    private void OnBrickArrivedSlot(BrickBase brick)
    {
        CheckMatch3();
    }

    private void CheckMatch3()
    {
        if (fallenBricks.Count < 3)
            return;

        // ---- Group bricks by type (Sprite) ----
        Dictionary<Sprite, List<BrickBase>> groups = new Dictionary<Sprite, List<BrickBase>>();

        foreach (var brick in fallenBricks)
        {
            Sprite type = brick.typeOfBrick;

            // Create a new list if this type doesn't exist yet
            if (!groups.ContainsKey(type))
                groups[type] = new List<BrickBase>();

            groups[type].Add(brick);
        }

        // ---- Find any type that has 3 or more bricks ----
        foreach (var group in groups)
        {
            if (group.Value.Count >= 3)
            {
                Debug.Log("Match 3 found! Type = " + group.Key.name);

                // Remove only the first 3 bricks of this group
                List<BrickBase> matched = group.Value.GetRange(0, 3);

                RemoveMatchedBricks(matched);
                return;
            }
        }
    }



    // =====================================================================
    //  REMOVE MATCHED
    // =====================================================================
    private static void RemoveMatchedBricks(List<BrickBase> list)
    {
        foreach (var brick in list)
        {
            fallenBricks.Remove(brick);

            if (brick != null)
                Destroy(brick.gameObject);
        }

        brickCountInSlot = fallenBricks.Count;
        RearrangeSlot();
    }

    // =====================================================================
    //  REARRANGE SLOT
    // =====================================================================
    private static void RearrangeSlot()
    {
        for (int i = 0; i < fallenBricks.Count; i++)
        {
            var brick = fallenBricks[i];
            if (brick == null) continue;

            Vector3 target = new Vector3((i * slotSpacing) + slotXOffset, slotY, 0f);

            brick.StartCoroutine(brick.MoveSmooth(target));
        }
    }

    // =====================================================================
    //  RESET SLOT
    // =====================================================================
    public static void ResetAllSlot()
    {
        foreach (var brick in fallenBricks)
        {
            if (brick != null)
                Destroy(brick.gameObject);
        }

        fallenBricks.Clear();
        brickCountInSlot = 0;
    }
}
