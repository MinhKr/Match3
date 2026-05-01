using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ListItemPicked : MonoBehaviour
{
    public List<Image> L_Items;

    [ShowInInspector]
    public Dictionary<E_TypeBrick, List<BrickBase>> Dic_TypeCheck = new Dictionary<E_TypeBrick, List<BrickBase>>();
    public List<BrickBase> L_Stored;

    [Header("Slot")]
    public int maxCapacity = 7;
    public Image addSlot_img;

    public Image fill_tiles;
    public RectTransform underFill_Tiles;
    public Sprite spriteFill_Tiles;

    Tweener T_Back;

    private void Start()
    {
        addSlot_img.gameObject.SetActive(false);
    }

    public void InitListItem(Sprite Brick)
    {
        for (int i = 0; i < L_Items.Count; i++)
            L_Items[i].sprite = Brick;
    }

    public void AddDic(E_TypeBrick type, BrickBase brick)
    {
        if (Dic_TypeCheck.ContainsKey(type))
        {
            IncreaseDic(type, brick);
        }
        else
        {
            List<BrickBase> L_Bricks = new List<BrickBase>();
            L_Bricks.Add(brick);
            Dic_TypeCheck.Add(type, L_Bricks);
        }
        L_Stored.Add(brick);
    }

    private void IncreaseDic(E_TypeBrick type, BrickBase brick)
    {
        List<BrickBase> L_Bricks = Dic_TypeCheck[type];

        L_Bricks.Add(brick);
        Dic_TypeCheck[type] = L_Bricks;
    }

    public void CheckDic(BrickBase brick)
    {
        List<BrickBase> L_Bricks = Dic_TypeCheck[brick._typeBrick];

        if (L_Bricks.Count >= 3 && brick.isAddCompleted) // animation complete
        {
            if (!L_Bricks[2].isAddCompleted) return; // check animation complete of 3rd brick
            GameManager.instance.UiController.UiGamePlay.listItem.isCanUseItem = false; // lock other item 

            List<BrickBase> L_3math = L_Bricks.GetRange(0, Mathf.Min(3, L_Bricks.Count));

            if (GridManager.instance.listCubeInLevel.Count <= 1 && GameManager.instance.UiController.UiGamePlay.listItem.L_ContainsItemTileReturn.Count <= 0)
            {
                Sequence sequenceComplete = DOTween.Sequence();

                sequenceComplete.Append(L_3math[1].transform.DOMove(L_Items[0].transform.position, 0.4f).SetEase(Ease.Linear));
                sequenceComplete.Join(L_3math[2].transform.DOMove(L_Items[1].transform.position, 0.4f).SetEase(Ease.Linear));

                L_3math[0].transform.DOMoveY(-3f, 0.4f).SetEase(Ease.Linear);
                L_3math[0].transform.DORotate(Vector3.forward * 360, 0.4f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() =>
                {
                    sequenceComplete.Play();
                });

                sequenceComplete.OnComplete(() =>
                {
                    if (L_3math[0].transform)
                    {
                        L_3math[0]._SpriteRenderer.sortingOrder = 2;
                        L_3math[0]._SpriteRendererTile.sortingOrder = 2;

                        L_3math[0].transform.DOMove(L_Items[0].transform.position, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            foreach (var item in L_3math)
                            {
                                SimplePool.Spawn(PrefabStorage.instance.FxMerge, item.transform.position, Quaternion.identity);
                                Destroy(item.gameObject);
                                L_Stored.Remove(item);
                                Dic_TypeCheck[brick._typeBrick].Remove(item);

                                if (Dic_TypeCheck[brick._typeBrick].Count <= 0)
                                {
                                    Dic_TypeCheck.Remove(brick._typeBrick);
                                }
                            }

                            //SoundManager.instance.PlayFxSound(SoundManager.instance.TileMatch);
                            GameManager.instance.UiController.UiGamePlay.listItem.CheckCanUseItem();

                            //PlayerDataManager.IncreaseLevel(PlayerDataManager.GetCurrentLevel());
                            GameManager.instance.UiController.ProcessWinLose(E_Result.Win);
                        });
                    }
                });
            }
            else
            {
                Sequence sequence = DOTween.Sequence();

                foreach (var item in L_3math)
                {
                    if (item.transform)
                    {
                        sequence.Join(item.transform.DOScale(2f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            SimplePool.Spawn(PrefabStorage.instance.FxMerge, item.transform.position, Quaternion.identity);

                            Destroy(item.gameObject);
                            L_Stored.Remove(item);
                            Dic_TypeCheck[brick._typeBrick].Remove(item);
                        }));
                    }
                }

                sequence.OnComplete(() =>
                {
                    SortElement();

                    if (Dic_TypeCheck[brick._typeBrick].Count <= 0)
                    {
                        Dic_TypeCheck.Remove(brick._typeBrick);
                    }

                    Debug.Log("Complete");
                    GameManager.instance.UiController.UiGamePlay.listItem.isCanUseItem = true;
                    //SoundManager.instance.PlayFxSound(SoundManager.instance.TileMatch);
                    GameManager.instance.UiController.UiGamePlay.listItem.CheckCanUseItem();
                });
            }
        }
        else
        {
            if (CheckIsFullItem() && brick.isAddCompleted)
            {
                List<E_TypeBrick> L_TypeInGame = new();

                for (int i = 0; i < L_Stored.Count; i++)
                {
                    if (!L_TypeInGame.Contains(L_Stored[i]._typeBrick))
                    {
                        L_TypeInGame.Add(L_Stored[i]._typeBrick);
                    }
                }

                for (int i = 0; i < L_TypeInGame.Count; i++)
                {
                    int a = GetAmoutBrickInListItem(L_TypeInGame[i]);

                    if (a >= 3)
                    {
                        GameManager.instance.UiController.UiGamePlay.listItem.CheckCanUseItem();
                        return;
                    }
                }
                GameManager.instance.UiController.ProcessWinLose(E_Result.Lose);
            }
            StartCheck();
        }
    }

    private void StartCheck()
    {
        StartCoroutine(IE_DelayCheckLayer());
    }

    IEnumerator IE_DelayCheckLayer()
    {
        yield return null;
        GridManager.instance.CheckLayerGrid();
    }

    public int GetPosMoveTo(E_TypeBrick e_TypeBrick)
    {
        if (L_Stored == null) return -1;

        if (L_Stored.Count >= maxCapacity) return -1;

        for (int i = L_Stored.Count - 1; i >= 0; i--)
        {
            if (L_Stored[i]._typeBrick == e_TypeBrick)
            {
                return i + 1;
            }
        }

        return -1;
    }

    public void SortAndMoveElement(BrickBase brick)
    {
        int posInsert = GetPosMoveTo(brick._typeBrick);
        if (posInsert < 0 || posInsert > L_Items.Count || posInsert > maxCapacity) return;
        brick.MoveToTarget(L_Items[posInsert].transform);

        for (int i = posInsert; i < L_Stored.Count; i++)
        {
            L_Stored[i].transform.position = L_Items[i + 1].transform.position;
        }

        BrickBase[] newList = new BrickBase[L_Stored.Count + 1];

        int posIndex = posInsert;
        for (int i = 0; i < posIndex; i++)
        {
            newList[i] = L_Stored[i];
        }
        newList[posInsert] = brick;

        for (int j = posIndex + 1; j < newList.Length; j++)
        {
            newList[j] = L_Stored[j - 1];
        }
        L_Stored = new List<BrickBase>(newList);

        /*L_Stored.Insert(posInsert, brick);*/
    }

    public void SortElement()
    {
        if (L_Stored.Count > L_Items.Count) return;

        for (int i = 0; i < L_Stored.Count; i++)
        {
            L_Stored[i].transform.position = L_Items[i].transform.position;
        }
    }

    public void SortElement(List<BrickBase> List)
    {
        if (List.Count > L_Items.Count) return;

        for (int i = 0; i < List.Count; i++)
        {
            List[i].transform.position = L_Items[i].transform.position;
        }
    }

    // Locate the location where the brick have to move to
    public void ProcessItemMerge(BrickBase brick, E_TypeBrick type)
    {
        bool isHaveBrickSameType = false;

        if (L_Stored.Count <= 0)
        {
            brick.MoveToTarget(L_Items[0].transform, () =>
            {
                brick._SpriteRenderer.sortingOrder = 1;
                brick._SpriteRendererTile.sortingOrder = 1;
            });
            AddDic(type, brick);
            return;
        }

        for (int i = L_Stored.Count - 1; i >= 0; i--)
        {
            if (L_Stored[i]._typeBrick == type)
            {
                isHaveBrickSameType = true;
                AddDic(type, brick);
                SortAndMoveElement(brick);
                return;
            }
        }

        if (!isHaveBrickSameType)
        {
            AddDic(type, brick);
            brick.MoveToTarget(L_Items[L_Stored.Count - 1].transform, () =>
            {
                brick._SpriteRenderer.sortingOrder = 1;
                brick._SpriteRendererTile.sortingOrder = 1;
            });
        }
    }

    public int GetAmoutBrickInListItem(E_TypeBrick type) //count same type to check match
    {
        int count = 0;
        for (int i = 0; i < L_Stored.Count; i++)
        {
            if (L_Stored[i]._typeBrick == type)
            {
                count++;
            }
        }
        return count;
    }

    public int GetAmoutRemainingItem()
    {
        return L_Items.Count - L_Stored.Count;
    }

    public bool CheckIsFullItem()
    {
        if (L_Stored.Count >= maxCapacity)
        {
            return true;
        }
        return false;
    }

    public void ProcessElementTileReturn(BrickBase brick, int pos, Vector3 position)
    {
        brick.transform.DOMove(position, 0.4f).SetEase(Ease.Linear).OnStepComplete(() =>
        {
            if (L_Stored.Count >= 0)
            {
                L_Stored.Remove(L_Stored[pos]);
            }

            if (Dic_TypeCheck.ContainsKey(brick._typeBrick))
            {
                Dic_TypeCheck[brick._typeBrick].Remove(brick);

                if (Dic_TypeCheck[brick._typeBrick].Count == 0)
                {
                    Dic_TypeCheck.Remove(brick._typeBrick);
                }
            }

            brick.isMoved = false;
            GameManager.instance.isInGame = true;
            GameManager.instance.UiController.UiGamePlay.listItem.CheckCanUseItem();
        }).SetAutoKill();
    }

    public void AddSlot()
    {
        L_Items.Add(addSlot_img);
        maxCapacity += 1;
        addSlot_img.gameObject.SetActive(true);

        fill_tiles.sprite = spriteFill_Tiles;
        transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 900);
        underFill_Tiles.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 875);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
