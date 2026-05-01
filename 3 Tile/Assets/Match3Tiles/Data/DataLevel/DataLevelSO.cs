using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Random = System.Random;

[CreateAssetMenu(fileName = "DataLevel", menuName = "Data/DataLevel", order = 1)]
public class DataLevelSO : SerializedScriptableObject
{
    public List<List<Sprite>> listDataImage = new List<List<Sprite>>();
    public List<Level> listLevel;

    public Level GetLevel(int i)
    {
        if (i > listLevel.Count)
        {
            int start = listLevel.Count - 5;

            if (start < 0)
            {
                start = 0;
            }

            int effectiveIndex = start + ((i - listLevel.Count - 1) % 5);

            if (effectiveIndex < 0)
            {
                effectiveIndex = 0;
            }
            Debug.Log(effectiveIndex + 1 + "level");

            return GetLevel(effectiveIndex + 1);
        }

        for (int x = 0; x < listLevel.Count; x++)
        {
            if (listLevel[x].level == i)
            {
                return listLevel[x];
            }
        }

        return null;
    }

    public int GetAmountTypeInLevel(int level)
    {
        if (level > listLevel.Count)
        {
            int start = listLevel.Count - 5;

            if (start < 0)
            {
                start = 0;
            }

            int effectiveIndex = start + ((level - listLevel.Count - 1) % 5);

            if (effectiveIndex < 0)
            {
                effectiveIndex = 0;
            }
            Debug.Log(effectiveIndex + "Amount Type");

            return listLevel[effectiveIndex].AmountTypeInGame;
        }

        for (int i = 0; i < listLevel.Count; i++)
        {
            if (listLevel[i].level == level)
            {
                return listLevel[i].AmountTypeInGame;
            }
        }
        return 0;
    }
    public List<Sprite> GetListTypeRandomInGame(int ConceptIndex)
    {
        Random random = new Random();
        int amoutInlevel = GetAmountTypeInLevel(PlayerDataManager.GetCurrentLevel());

        List<Sprite> ListTypeInGame = listDataImage[ConceptIndex].OrderBy(x => random.Next()).Take(amoutInlevel).ToList();

        return ListTypeInGame;
    }
}

[Serializable]
public class Level
{
    public int level;
    public int AmountTypeInGame;
    public int CoinCompleteLevel;
    public List<Vector3> listGridPlace = new List<Vector3>();
    public List<GROUP> listGrp = new List<GROUP>();
}

[Serializable]

public class GROUP
{
    public E_GroupType groupType;
    public Vector2 place;
}
