using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [Header("Item Undo")]
    [SerializeField] private Button btnUndo;

    [Header("Item Merge")]
    [SerializeField] private Button btnMerge;

    [Header("Item Shuffle")]
    [SerializeField] private Button btnShuffle;

    [Header("Item Tile Return")]
    [SerializeField] private Button btnTileReturn;
    [SerializeField] public List<BrickBase> L_ContainsItemTileReturn;

    [Header("Item Add Slot")]
    [SerializeField] private Button btnAddSlot;

    public int CountRemaining = 0;

    public bool isCanUseItem;

    private void OnEnable()
    {

    }

    private void Start()
    {

    }

    IEnumerator IE_DelayCheckUse()
    {
        yield return null;
        CheckCanUseItem();
    }

    public void CheckCanUseItem()
    {
        // Check
    }

    private void InitButon()
    {

    }

    public void InitCanBwcOrAds()
    {

    }

    public void DeActiveAll()
    {

    }

    private void InitUnlockUndo(List<BrickBase> list)
    {

    }

    private void InitUnlockTileReturn(List<BrickBase> list)
    {

    }

    private void InitUnlockItemMerge()
    {

    }

    private void OnProcessItemUndo()
    {

    }

    private void ProcessItemUndo()
    {

    }

    private void OnProcessItemMerge()
    {

    }

    private void OnProcessItemShuffle()
    {
    }

    private void OnProcessItemTileReturn()
    {
    }
    private void ProcessItemTileReturn()
    {
    }
    private void OnProcessItemAddSlot()
    {
    }

    private void ProcessItemAddSlot()
    {
    }

    private void ShuffleList(List<BrickBase> list)
    {
    }

    #region Process Item Merge
    public void InitItemOnLevelMerge()
    {

    }

    public void OnProcessMerge()
    {

    }
    #endregion

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
