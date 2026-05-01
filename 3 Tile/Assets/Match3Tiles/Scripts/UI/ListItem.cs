using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ListItem : MonoBehaviour
{
    [Header("Item Undo")]
    [SerializeField] private Button btnUndo;
    [SerializeField] private Image IconUndo_img;
    [SerializeField] private Image IconLockUndo_img;
    [SerializeField] List<Image> L_BwcOrAdsUndo;

    [Header("Item Merge")]
    [SerializeField] private Button btnMerge;
    [SerializeField] private List<E_TypeBrick> L_typeCanMerge;
    [SerializeField] private List<E_TypeBrick> L_AllTypeInLevel;
    [SerializeField] private Image IconMerge_img;
    [SerializeField] private Image IconLockMerge_img;
    [SerializeField] List<Image> L_BwcOrAdsMerge;

    [Header("Item Shuffle")]
    [SerializeField] private Button btnShuffle;
    [SerializeField] private Image IconShuffle_img;
    [SerializeField] private Image IconLockShuffle_img;
    [SerializeField] List<Image> L_BwcOrAdsShuffle;

    [Header("Item Tile Return")]
    [SerializeField] private Button btnTileReturn;
    [SerializeField] public List<Image> L_posMoveItem;
    [SerializeField] public List<BrickBase> L_ContainsItemTileReturn;
    [SerializeField] private Image IconTileReturn_img;
    [SerializeField] private Image IconLockReturn_img;
    [SerializeField] List<Image> L_BwcOrAdsTileReturn;

    [Header("Item Add Slot")]
    [SerializeField] private Button btnAddSlot;
    [SerializeField] private Image IconItemAddSlot_img;
    [SerializeField] private Image IconLockAddSlot_img;
    [SerializeField] List<Image> L_BwcOrAdsAddSlot;

    public int CountRemaining = 0;

    public bool isCanUseItem;

    private void OnEnable()
    {
        InitButton();
    }

    private void Start()
    {
        IconLockAddSlot_img.gameObject.SetActive(false);
        IconLockMerge_img.gameObject.SetActive(false);
        IconLockReturn_img.gameObject.SetActive(false);
        IconLockShuffle_img.gameObject.SetActive(false);
        IconLockUndo_img.gameObject.SetActive(false);

        isCanUseItem = true;
        StartCoroutine(IE_DelayCheckUse());

        InitCanBwcOrAds();
    }

    IEnumerator IE_DelayCheckUse()
    {
        yield return null;
        CheckCanUseItem();
    }

    public void CheckCanUseItem()
    {
        ListItemPicked _listItemPicked = GameManager.instance.UiController.UiGamePlay.listItemPicked;

        InitUnlockUndo(_listItemPicked.L_Stored);
        InitUnlockTileReturn(_listItemPicked.L_Stored);
        InitUnlockItemMerge();

        if (PlayerDataManager.GetCurrentLevel() >= 3)
        {
            btnShuffle.interactable = true;
            IconShuffle_img.color = Color.white;
        }
        else
        {
            for (int i = 0; i < L_BwcOrAdsShuffle.Count; i++)
            {
                L_BwcOrAdsShuffle[i].gameObject.SetActive(false);
            }

            IconLockShuffle_img.gameObject.SetActive(true);
            btnShuffle.interactable = false;
            IconShuffle_img.color = new Color(0.7f, 0.7f, 0.7f);
        }

        if (PlayerDataManager.GetCurrentLevel() >= 6)
        {
            btnAddSlot.interactable = true;
            IconItemAddSlot_img.color = Color.white;
        }
        else
        {
            for (int i = 0; i < L_BwcOrAdsAddSlot.Count; i++)
            {
                L_BwcOrAdsAddSlot[i].gameObject.SetActive(false);
            }

            IconLockAddSlot_img.gameObject.SetActive(true);

            btnAddSlot.interactable = false;
            IconItemAddSlot_img.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    private void InitButton()
    {
        btnUndo.onClick.AddListener(OnProcessItemUndo);
        btnMerge.onClick.AddListener(OnProcessItemMerge);
        btnTileReturn.onClick.AddListener(OnProcessItemTileReturn);
        btnShuffle.onClick.AddListener(OnProcessItemShuffle);
        btnAddSlot.onClick.AddListener(OnProcessItemAddSlot);
    }

    public void InitCanBwcOrAds()
    {
        DeActiveAll();

        int CurrentCoin = PlayerDataManager.GetCoin();

        // undo will be unlocked at level 2
        if (PlayerDataManager.GetCurrentLevel() < 2)
        {
            L_BwcOrAdsUndo[0].gameObject.SetActive(false);
            L_BwcOrAdsUndo[1].gameObject.SetActive(false);
        }
        else
        {
            if (CurrentCoin >= 50)
            {
                L_BwcOrAdsUndo[0].gameObject.SetActive(true);
            }
            else
            {
                L_BwcOrAdsUndo[1].gameObject.SetActive(true);
            }
        }

        //shuffle will be unlocked at level 3
        if (PlayerDataManager.GetCurrentLevel() < 3)
        {
            L_BwcOrAdsShuffle[0].gameObject.SetActive(false);
            L_BwcOrAdsShuffle[1].gameObject.SetActive(false);
        }
        else
        {
            if (CurrentCoin >= 100)
            {
                L_BwcOrAdsShuffle[0].gameObject.SetActive(true);
            }
            else
            {
                L_BwcOrAdsShuffle[1].gameObject.SetActive(true);
            }
        }

        //tile return will be unlocked at level 4
        if (PlayerDataManager.GetCurrentLevel() < 4)
        {
            L_BwcOrAdsTileReturn[0].gameObject.SetActive(false);
            L_BwcOrAdsTileReturn[1].gameObject.SetActive(false);
        }
        else
        {
            if (CurrentCoin >= 150)
            {
                L_BwcOrAdsTileReturn[0].gameObject.SetActive(true);
            }
            else
            {
                L_BwcOrAdsTileReturn[1].gameObject.SetActive(true);
            }
        }

        //merge will be unlocked at level 5
        if (PlayerDataManager.GetCurrentLevel() < 5)
        {
            L_BwcOrAdsMerge[0].gameObject.SetActive(false);
            L_BwcOrAdsMerge[1].gameObject.SetActive(false);
        }
        else
        {
            if (CurrentCoin >= 200)
            {
                L_BwcOrAdsMerge[0].gameObject.SetActive(true);
            }
            else
            {
                L_BwcOrAdsMerge[1].gameObject.SetActive(true);
            }
        }

        //add slot will be unlocked at level 6
        if (PlayerDataManager.GetCurrentLevel() < 6)
        {
            L_BwcOrAdsAddSlot[0].gameObject.SetActive(false);
            L_BwcOrAdsAddSlot[1].gameObject.SetActive(false);
        }
        else
        {
            if (CurrentCoin >= 250)
            {
                L_BwcOrAdsAddSlot[0].gameObject.SetActive(true);
            }
            else
            {
                L_BwcOrAdsAddSlot[1].gameObject.SetActive(true);
            }
        }
    }

    public void DeActiveAll()
    {
        for (int i = 0; i < L_BwcOrAdsUndo.Count; i++)
        {
            L_BwcOrAdsUndo[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < L_BwcOrAdsMerge.Count; i++)
        {
            L_BwcOrAdsMerge[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < L_BwcOrAdsShuffle.Count; i++)
        {
            L_BwcOrAdsShuffle[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < L_BwcOrAdsTileReturn.Count; i++)
        {
            L_BwcOrAdsTileReturn[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < L_BwcOrAdsAddSlot.Count; i++)
        {
            L_BwcOrAdsAddSlot[i].gameObject.SetActive(false);
        }
    }

    private void InitUnlockUndo(List<BrickBase> list)
    {
        if (PlayerDataManager.GetCurrentLevel() < 2)
        {
            for (int i = 0; i < L_BwcOrAdsUndo.Count; i++)
            {
                L_BwcOrAdsUndo[i].gameObject.SetActive(false);
            }

            IconLockUndo_img.gameObject.SetActive(true);
            btnUndo.interactable = false;
            IconUndo_img.color = new Color(0.7f, 0.7f, 0.7f);
            return;
        }

        if (list.Count > 0)
        {
            btnUndo.interactable = true;
            IconUndo_img.color = Color.white;
        }
        else
        {
            btnUndo.interactable = false;
            IconUndo_img.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    private void InitUnlockTileReturn(List<BrickBase> list)
    {
        if (PlayerDataManager.GetCurrentLevel() < 4)
        {
            for (int i = 0; i < L_BwcOrAdsTileReturn.Count; i++)
            {
                L_BwcOrAdsTileReturn[i].gameObject.SetActive(false);
            }

            IconLockReturn_img.gameObject.SetActive(true);

            btnTileReturn.interactable = false;
            IconTileReturn_img.color = new Color(0.7f, 0.7f, 0.7f);

            return;
        }

        if (L_ContainsItemTileReturn.Count == 0 && list.Count >= 3)
        {
            btnTileReturn.interactable = true;
            IconTileReturn_img.color = Color.white;
        }
        else
        {
            btnTileReturn.interactable = false;
            IconTileReturn_img.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    private void InitUnlockItemMerge()
    {
        if (PlayerDataManager.GetCurrentLevel() < 5)
        {
            for (int i = 0; i < L_BwcOrAdsMerge.Count; i++)
            {
                L_BwcOrAdsMerge[i].gameObject.SetActive(false);
            }

            IconLockMerge_img.gameObject.SetActive(true);
            btnMerge.interactable = false;
            IconMerge_img.color = new Color(0.7f, 0.7f, 0.7f);

            return;
        }

        bool IsCheckCanMerge = false;

        for (int i = 0; i < GridManager.instance.listCubeInLevel.Count; i++)
        {
            int Count = GameManager.instance.UiController.UiGamePlay.listItemPicked.GetAmoutBrickInListItem(GridManager.instance.listCubeInLevel[i]._typeBrick);
            Count += GridManager.instance.GetAmountBrickInLevel(GridManager.instance.listCubeInLevel[i]._typeBrick);

            if (Count >= 3)
            {
                btnMerge.interactable = true;
                IconMerge_img.color = Color.white;
                IsCheckCanMerge = true;
            }
        }

        if (!IsCheckCanMerge)
        {
            btnMerge.interactable = false;
            IconMerge_img.color = new Color(0.7f, 0.7f, 0.7f);
        }
    }

    private void OnProcessItemUndo()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        if (!isCanUseItem) return;

        int Current = PlayerDataManager.GetCoin();

        if (Current >= 50)
        {
            int newCoin = Current - 50;
            PlayerDataManager.SetCoin(newCoin);
            GameManager.instance.UiController.UiGamePlay.InitCoinAndLevel();
            ProcessItemUndo();
            InitCanBwcOrAds();
        }
        else
        {
            //AdManager.instance.ShowReward(() =>
            //{
            //    ProcessItemUndo();
            //}, () =>
            //{
            //    GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
            //}, "ShowReward");
        }
    }

    private void ProcessItemUndo()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.Booster);
        ListItemPicked _listItemPicked = GameManager.instance.UiController.UiGamePlay.listItemPicked;

        if (_listItemPicked.L_Stored.Count <= 0)
        {
            return;
        }


        GameManager.instance.isInGame = false;

        BrickBase _brickObject = _listItemPicked.L_Stored[_listItemPicked.L_Stored.Count - 1];
        _brickObject._SpriteRenderer.sortingOrder = 2;
        _brickObject._SpriteRendererTile.sortingOrder = 2;
        _brickObject.UndoThisItem(_brickObject.thisVec, _brickObject.depth);
        _brickObject.transform.DOMove(_brickObject.StartPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            GridManager.instance.CheckLayerGrid();
            _brickObject._SpriteRenderer.sortingOrder = 1;
            _brickObject._SpriteRendererTile.sortingOrder = 1;
            GameManager.instance.isInGame = true;
        }).SetAutoKill();

        _brickObject.isMoved = false;
        _listItemPicked.L_Stored.Remove(_brickObject);
        _listItemPicked.Dic_TypeCheck[_brickObject._typeBrick].Remove(_brickObject);

        if (_listItemPicked.Dic_TypeCheck[_brickObject._typeBrick].Count <= 0)
        {
            _listItemPicked.Dic_TypeCheck.Remove(_brickObject._typeBrick);

        }

        CheckCanUseItem();
    }

    private void OnProcessItemMerge()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        if (!isCanUseItem) return;

        int Current = PlayerDataManager.GetCoin();

        if (Current >= 200)
        {
            int newCoin = Current - 200;
            PlayerDataManager.SetCoin(newCoin);
            GameManager.instance.UiController.UiGamePlay.InitCoinAndLevel();
            InitItemOnLevelMerge();
            InitCanBwcOrAds();
        }
        else
        {
            //AdManager.instance.ShowReward(() =>
            //{
            //    InitItemOnLevelMerge();
            //}, () =>
            //{
            //    GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
            //}, "ShowReward");
        }
    }

    private void OnProcessItemShuffle()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        if (!isCanUseItem) return;

        int Current = PlayerDataManager.GetCoin();

        if (Current >= 100)
        {
            int newCoin = Current - 100;
            PlayerDataManager.SetCoin(newCoin);
            GameManager.instance.UiController.UiGamePlay.InitCoinAndLevel();

            //SoundManager.Instance.PlayFxSound(SoundManager.Instance.Booster);
            ShuffleList(GridManager.instance.listCubeInLevel);
            InitCanBwcOrAds();
        }
        else
        {
            //AdManager.instance.ShowReward(() =>
            //{
            //    SoundManager.Instance.PlayFxSound(SoundManager.Instance.Booster);
            //    ShuffleList(GridManager.instance.ListCubeInLevel);
            //}, () =>
            //{
            //    GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
            //}, "ShowReward");
        }
    }

    private void OnProcessItemTileReturn()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        if (!isCanUseItem) return;

        int Current = PlayerDataManager.GetCoin();

        if (Current >= 150)
        {
            int newCoin = Current - 150;
            PlayerDataManager.SetCoin(newCoin);
            GameManager.instance.UiController.UiGamePlay.InitCoinAndLevel();
            ProcessItemTileReturn();
            InitCanBwcOrAds();
        }
        else
        {
            //AdManager.instance.ShowReward(() =>
            //{
            //    ProcessItemTileReturn();
            //}, () =>
            //{
            //    GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
            //}, "ShowReward");
        }
    }
    private void ProcessItemTileReturn()
    {
        ListItemPicked _listItemPicked = GameManager.instance.UiController.UiGamePlay.listItemPicked;
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.Booster);

        btnTileReturn.interactable = false;
        IconTileReturn_img.color = new Color(0.7f, 0.7f, 0.7f);
        GameManager.instance.isInGame = false;

        int count = 2;

        for (int i = _listItemPicked.L_Stored.Count - 1; i >= _listItemPicked.L_Stored.Count - 3; i--)
        {
            L_ContainsItemTileReturn.Add(_listItemPicked.L_Stored[i]);
            _listItemPicked.ProcessElementTileReturn(_listItemPicked.L_Stored[i], i, L_posMoveItem[count].transform.position);
            count--;
        }
    }
    private void OnProcessItemAddSlot()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.buttonclick);
        if (!isCanUseItem) return;

        int Current = PlayerDataManager.GetCoin();

        if (Current >= 250)
        {
            int newCoin = Current - 250;
            PlayerDataManager.SetCoin(newCoin);
            GameManager.instance.UiController.UiGamePlay.InitCoinAndLevel();
            ProcessItemAddSlot();
            InitCanBwcOrAds();
        }
        else
        {
            //AdManager.instance.ShowReward(() =>
            //{
            //    ProcessAddSlot();
            //}, () =>
            //{
            //    GameManager.instance.UiController.PopupNoInternet.gameObject.SetActive(true);
            //}, "ShowReward");
        }
    }

    private void ProcessItemAddSlot()
    {
        //SoundManager.Instance.PlayFxSound(SoundManager.Instance.Booster);
        GameManager.instance.UiController.UiGamePlay.listItemPicked.Add1Slot();
        btnAddSlot.interactable = false;
        IconItemAddSlot_img.color = new Color(0.7f, 0.7f, 0.7f);
    }

    private void ShuffleList(List<BrickBase> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        if (n <= 1)
        {
            return;
        }
        while (n > 0)
        {
            n--;
            int k = random.Next(n);
            E_TypeBrick value = list[k]._typeBrick;
            list[k]._typeBrick = list[n]._typeBrick;
            list[n]._typeBrick = value;
            list[n].initDataTypeStartGame(GridManager.instance.ConceptCurrentLevel[(int)list[n]._typeBrick], list[n]._typeBrick);
        }
    }

    #region Process Item Merge
    public void InitItemOnLevelMerge()
    {
        int AmoutRemainingItem = GameManager.instance.UiController.UiGamePlay.listItemPicked.GetAmoutRemainingItem();


        if (AmoutRemainingItem == 2)
        {
            for (int i = 0; i < GridManager.instance.listCubeInLevel.Count; i++)
            {
                var AmoutInlistItem = GameManager.instance.UiController.UiGamePlay.listItemPicked.GetAmoutBrickInListItem(GridManager.instance.listCubeInLevel[i]._typeBrick);

                if (!L_typeCanMerge.Contains(GridManager.instance.listCubeInLevel[i]._typeBrick) && AmoutInlistItem == 1 && AmoutInlistItem > 0)
                {
                    L_typeCanMerge.Add(GridManager.instance.listCubeInLevel[i]._typeBrick);
                }
            }
        }
        else if (AmoutRemainingItem == 1)
        {
            for (int i = 0; i < GridManager.instance.listCubeInLevel.Count; i++)
            {
                if (!L_typeCanMerge.Contains(GridManager.instance.listCubeInLevel[i]._typeBrick) &&
                    GameManager.instance.UiController.UiGamePlay.listItemPicked.GetAmoutBrickInListItem(GridManager.instance.listCubeInLevel[i]._typeBrick) == 2)
                {
                    L_typeCanMerge.Add(GridManager.instance.listCubeInLevel[i]._typeBrick);
                }
            }
        }
        else
        {
            for (int i = 0; i < GridManager.instance.listCubeInLevel.Count; i++)
            {
                int Count = GameManager.instance.UiController.UiGamePlay.listItemPicked.GetAmoutBrickInListItem(GridManager.instance.listCubeInLevel[i]._typeBrick);
                Count += GridManager.instance.GetAmountBrickInLevel(GridManager.instance.listCubeInLevel[i]._typeBrick);

                if (!L_typeCanMerge.Contains(GridManager.instance.listCubeInLevel[i]._typeBrick) && Count >= 3)
                {
                    L_typeCanMerge.Add(GridManager.instance.listCubeInLevel[i]._typeBrick);
                }
            }
        }
        OnProcessMerge();
    }

    public void OnProcessMerge()
    {
        if (L_typeCanMerge.Count == 0) return;

        E_TypeBrick RandomType = L_typeCanMerge[new Random().Next(L_typeCanMerge.Count)];

        CountRemaining += GameManager.instance.UiController.UiGamePlay.listItemPicked.GetAmoutBrickInListItem(RandomType);



        for (int i = 0; i < GridManager.instance.listCubeInLevel.Count; i++)
        {
            if (GridManager.instance.listCubeInLevel[i]._typeBrick == RandomType)
            {
                CountRemaining++;


                GridManager.instance.listCubeInLevel[i]._SpriteRenderer.sortingOrder = 2;
                GridManager.instance.listCubeInLevel[i]._SpriteRendererTile.sortingOrder = 2;

                GridManager.instance.listCubeInLevel[i].KillThisCube();
                GridManager.instance.listCubeInLevel[i].TurnWhite();

                GameManager.instance.UiController.UiGamePlay.listItemPicked.ProcessItemMerge(GridManager.instance.listCubeInLevel[i], RandomType);
            }

            if (CountRemaining == 3)
            {
                CountRemaining = 0;
                L_typeCanMerge.Clear();
                break;
            }
        }
        CheckCanUseItem();
    }
    #endregion

    private void OnDisable()
    {
        btnUndo.onClick.RemoveListener(OnProcessItemUndo);
        btnMerge.onClick.RemoveListener(OnProcessItemMerge);
        btnTileReturn.onClick.RemoveListener(OnProcessItemTileReturn);
        btnShuffle.onClick.RemoveListener(OnProcessItemShuffle);

        StopAllCoroutines();
    }

}
