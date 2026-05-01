using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BrickBase : MonoBehaviour
{
    [SerializeField] public Vector2 thisVec;
    [SerializeField] public int depth = 0;
    [SerializeField] public E_TypeBrick _typeBrick;
    [SerializeField] public SpriteRenderer _SpriteRenderer;
    [SerializeField] public SpriteRenderer _SpriteRendererTile;
    [SerializeField] protected Collider2D _circleCollider2D;
    public Vector3 StartPosition;

    public Color colorGray = new Color(0.7f, 0.7f, 0.7f);
    public Color colorWhite = new Color(1f, 1f, 1f);

    private bool isSorted;
    public bool isAddCompleted;
    public bool isMoved;

    Tweener T_Move;

    private void Start()
    {
        StartPosition = transform.position;

        //StartCheckGrid();
    }

    public void InitData(Vector2 vec, int _depth)
    {
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y, _depth));
        GridManager.instance.listGridPlace.Add(new Vector3(vec.x, vec.y, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y - 1, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y - 1, _depth));
        transform.position = new Vector3(transform.position.x, transform.position.y, _depth * -0.05f);
        thisVec = vec;
        depth = _depth;

        if (_depth < -1)
        {
            TurnBlack();
        }
        else
        {
            TurnWhite();
        }
    }

    public void initDataTypeStartGame(Sprite Icon, E_TypeBrick Type)
    {
        _SpriteRenderer.sprite = Icon;
        _typeBrick = Type;
    }

    public void CheckBrickTileReturned()
    {
        ListItem _listItem = GameManager.instance.UiController.UiGamePlay.listItem;

        if (_listItem.L_ContainsItemTileReturn.Contains(this))
        {
            _listItem.L_ContainsItemTileReturn.Remove(this);
        }
    }

    // ======== MOVE TO UI TARGET ========
    public void MoveToTarget(Transform target, System.Action complete = null)
    {
        T_Move = transform.DOMove(new Vector3(target.position.x, target.position.y, transform.position.z), 0.3f).SetEase(Ease.Linear);

        T_Move.OnComplete(() =>
        {
            //GameManager.instance.UiController.UiGamePlay.listItem.CheckCanUseItem();
            complete?.Invoke();
            GameManager.instance.UiController.UiGamePlay.listItemPicked.SortElement();
            isAddCompleted = true;
            GameManager.instance.UiController.UiGamePlay.listItemPicked.CheckDic(this);
            GridManager.instance.listCubeInLevel.Remove(this);

        });

        isMoved = true;
        transform.SetParent(null);
    }

    private void OnMouseDown()
    {
        if (!GameManager.instance.isInGame) return;
        isSorted = false;
        if (GameManager.instance.UiController.UiGamePlay.listItemPicked.CheckIsFullItem()) return;
        if (isMoved) return;

        if (!CheckGridOverlap()) return;


        DeleteBox(thisVec);
        GridManager.instance.CheckLayerGrid();
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.Tile);

        if (GameManager.instance.UiController.UiGamePlay.listItemPicked.L_Stored.Count > 0)
        {
            for (int i = 0; i < GameManager.instance.UiController.UiGamePlay.listItemPicked.L_Stored.Count; i++)
            {
                if (_typeBrick == GameManager.instance.UiController.UiGamePlay.listItemPicked.L_Stored[i]._typeBrick)
                {
                    //CheckBrickTileReturned();
                    GameManager.instance.UiController.UiGamePlay.listItemPicked.SortAndMoveElement(this);
                    GameManager.instance.UiController.UiGamePlay.listItemPicked.AddDic(_typeBrick, this);
                    isSorted = true;
                    break;
                }
            }
        }
        else
        {
            //CheckBrickTileReturned();
            GameManager.instance.UiController.UiGamePlay.listItemPicked.AddDic(_typeBrick, this);
            MoveToTarget(GameManager.instance.UiController.UiGamePlay.listItemPicked.L_Items[0].transform);
            isSorted = true;
        }

        if (!isSorted && GameManager.instance.UiController.UiGamePlay.listItemPicked.L_Stored.Count > 0)
        {
            //CheckBrickTileReturned();
            GameManager.instance.UiController.UiGamePlay.listItemPicked.AddDic(_typeBrick, this);
            int index = GameManager.instance.UiController.UiGamePlay.listItemPicked.L_Stored.Count - 1;
            MoveToTarget(GameManager.instance.UiController.UiGamePlay.listItemPicked.L_Items[index].transform);
        }
    }

    public void DeleteBox(Vector2 pos)
    {
        Vector3 grisPos = GridManager.instance.grid.GetNearestPointOnGrid(pos);
        DeleteTile(grisPos);
    }

    public void DeleteTile(Vector2 vec)
    {
        float depth = int.MinValue;
        for (int i = 0; i < GridManager.instance.listGrid.Count; i++)
        {
            if (GridManager.instance.listGrid[i].x == vec.x && GridManager.instance.listGrid[i].y == vec.y)
            {
                if (GridManager.instance.listGrid[i].z > depth)
                {
                    depth = GridManager.instance.listGrid[i].z;
                }
            }
        }
        CheckDeleteGrid(new Vector3(vec.x, vec.y, depth));
    }

    #region CheckDeleteGrid
    public void CheckDeleteGrid(Vector3 vec)
    {
        /*if (depth == vec.z)
        {
            if (CheckGridOverlap())
            {
                if (vec.x == thisVec.x && vec.y == thisVec.y)
                {
                    KillThisCube();
                    return;
                }

                if (vec.x == thisVec.x && vec.y == thisVec.y - 1)
                {

                    KillThisCube();
                    return;
                }

                if (vec.x == thisVec.x - 1 && vec.y == thisVec.y)
                {
                    KillThisCube();
                    return;
                }

                if (vec.x == thisVec.x - 1 && vec.y == thisVec.y - 1)
                {
                    KillThisCube();
                    return;
                }
            }
        }*/
    }

    private void StartCheckGrid()
    {
        StartCoroutine(CheckGrid());
    }

    private IEnumerator CheckGrid()
    {
        yield return null;
        CheckGridOverlap();
    }
    #endregion

    public bool CheckGridOverlap()
    {
        for (int i = 0; i < GridManager.instance.listGrid.Count; i++)
        {
            if (GridManager.instance.listGrid[i].z > depth)
            {
                if (GridManager.instance.listGrid[i].x == thisVec.x && GridManager.instance.listGrid[i].y == thisVec.y)
                {
                    TurnBlack();
                    return false;
                }

                if (GridManager.instance.listGrid[i].x == thisVec.x - 1 && GridManager.instance.listGrid[i].y == thisVec.y)
                {
                    TurnBlack();
                    return false;
                }

                if (GridManager.instance.listGrid[i].x == thisVec.x && GridManager.instance.listGrid[i].y == thisVec.y - 1)
                {
                    TurnBlack();
                    return false;
                }

                if (GridManager.instance.listGrid[i].x == thisVec.x - 1 && GridManager.instance.listGrid[i].y == thisVec.y - 1)
                {
                    TurnBlack();
                    return false;
                }
            }
        }

        TurnWhite();
        return true;
    }

    public void UndoThisItem(Vector2 vec, int _depth)
    {
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y, _depth));
        GridManager.instance.listGridPlace.Add(new Vector3(vec.x, vec.y, _depth));
        GridManager.instance.listCubeInLevel.Add(this);
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x, vec.y - 1, _depth));
        GridManager.instance.listGrid.Add(new Vector3(vec.x - 1, vec.y - 1, _depth));
    }

    public void KillThisCube()
    {
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x, thisVec.y, depth));
        GridManager.instance.listGridPlace.Remove(new Vector3(thisVec.x, thisVec.y, depth));
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x, thisVec.y - 1, depth));
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x - 1, thisVec.y, depth));
        GridManager.instance.listGrid.Remove(new Vector3(thisVec.x - 1, thisVec.y - 1, depth));
    }

    public void KillTween()
    {
        T_Move?.Kill();
    }

    public void TurnBlack()
    {
        _SpriteRendererTile.color = colorGray;
        _SpriteRenderer.color = colorGray;
    }

    public void TurnWhite()
    {
        _SpriteRendererTile.color = colorWhite;
        _SpriteRenderer.color = colorWhite;
    }

    private void OnDisable()
    {
        KillTween();
    }
}
