using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public List<Vector3> listGrid = new List<Vector3>();
    public List<Vector3> listGridPlace = new List<Vector3>();
    public List<BrickBase> listCubeInLevel = new List<BrickBase>();

    public List<Sprite> ConceptCurrentLevel;

    public Grid grid;
    [SerializeField] private Transform left, right;

    public bool Playable = false;
    private bool reroll = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GetCurrentLevel();
    }

    public void GetCurrentLevel()
    {
        CreateBrick();
        CreateDataConceptAndAmoutTypeInLevel();
        ShuffleList(listCubeInLevel);

    }
    private void CreateBrick()
    {
        Level currentLevel = PrefabStorage.instance.DataLevel.GetLevel(PlayerDataManager.GetCurrentLevel());

        for (int i = 0; i < currentLevel.listGrp.Count; i++)
        {
            PlaceGroup(currentLevel.listGrp[i].place.x, currentLevel.listGrp[i].place.y, currentLevel.listGrp[i].groupType);
        }

        for (int i = 0; i < currentLevel.listGridPlace.Count; i++)
        {
            PlaceBox(currentLevel.listGridPlace[i].x, currentLevel.listGridPlace[i].y, currentLevel.listGridPlace[i].z);
        }
    }

    private void CreateDataConceptAndAmoutTypeInLevel()
    {
        int concept = UnityEngine.Random.Range(0, PrefabStorage.instance.DataLevel.listDataImage.Count);
        ConceptCurrentLevel = PrefabStorage.instance.DataLevel.GetListTypeRandomInGame(concept);
        var enumValues = Enum.GetValues(typeof(E_TypeBrick)).Cast<E_TypeBrick>();
        var selectedElements = enumValues.Take(PrefabStorage.instance.DataLevel.GetAmountTypeInLevel(PlayerDataManager.GetCurrentLevel()));// trả về n phần tử đầu tiên trong danh sách enum của Type Brick

        int elementsPerGroup = 3;
        int numberOfFullGroups = listCubeInLevel.Count / elementsPerGroup;
        int remainingElements = listCubeInLevel.Count % elementsPerGroup;

        for (int i = 0; i < numberOfFullGroups; i++)
        {
            int startIndex = i * elementsPerGroup;
            int endIndex = startIndex + elementsPerGroup;

            for (int j = startIndex; j < endIndex; j++)
            {
                int conceptIndex = i % selectedElements.Count();
                listCubeInLevel[j].initDataTypeStartGame(ConceptCurrentLevel[conceptIndex], selectedElements.ElementAt(conceptIndex));
            }
        }

        if (remainingElements > 0)
        {
            int startIndex = numberOfFullGroups * elementsPerGroup;
            var remainingConcepts = ConceptCurrentLevel.Skip(numberOfFullGroups % selectedElements.Count()).Take(remainingElements).ToList();

            for (int i = startIndex; i < listCubeInLevel.Count; i++)
            {
                int randomConceptIndex = UnityEngine.Random.Range(0, remainingConcepts.Count);
                listCubeInLevel[i].initDataTypeStartGame(remainingConcepts[randomConceptIndex], selectedElements.ElementAt(randomConceptIndex));
            }
        }
    }

    [Button]
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
            list[n].initDataTypeStartGame(ConceptCurrentLevel[(int)list[n]._typeBrick], list[n]._typeBrick);
        }
    }

    public void PlaceBox(float x, float y, float depth)
    {
        Vector3 grisPos = grid.GetNearestPointOnGrid(x, y, 0);


        BrickBase brick = Instantiate(PrefabStorage.instance.BrickBase);
        brick.transform.position = grisPos;
        brick.transform.parent = transform;
        brick.InitData(grisPos, Mathf.RoundToInt(depth - 1));

        listCubeInLevel.Add(brick);
        //Cube cube = _go.GetComponent<Cube>();
        //cube.InitCube(grisPos, Mathf.RoundToInt(depth - 1));

        //_go.SetActive(true);
    }

    // sinh ra theo group
    public void PlaceGroup(float x, float y, E_GroupType type)
    {
        Vector3 grisPos = grid.GetNearestPointOnGrid(x, y, 0);
        GridGroup _go = Instantiate(PrefabStorage.instance.GridGroup);
        _go.gameObject.SetActive(true);
        _go.transform.position = grisPos;
        _go.transform.parent = transform;
        _go.CreateGroup(type);
    }


    // lay ra doi tuong sau nhat de sinh ra o duoi listGroup
    public int CheckGrid(Vector2 vec)
    {
        float depth = 0;
        for (int i = 0; i < listGrid.Count; i++)
        {
            if (listGrid[i].x == vec.x && listGrid[i].y == vec.y)
            {
                if (depth > listGrid[i].z)
                {
                    depth = listGrid[i].z;
                }
            }

            if (listGrid[i].x == vec.x - 1 && listGrid[i].y == vec.y)
            {
                if (depth > listGrid[i].z)
                {
                    depth = listGrid[i].z;
                }
            }

            if (listGrid[i].x == vec.x && listGrid[i].y == vec.y - 1)
            {
                if (depth > listGrid[i].z)
                {
                    depth = listGrid[i].z;
                }
            }

            if (listGrid[i].x == vec.x - 1 && listGrid[i].y == vec.y - 1)
            {
                if (depth > listGrid[i].z)
                {
                    depth = listGrid[i].z;
                }
            }
        }

        return Mathf.RoundToInt(depth);
    }

    public void CheckLayerGrid()
    {
        for (int i = 0; i < listCubeInLevel.Count; i++)
        {
            if (listCubeInLevel[i].CheckGridOverlap())
            {
                listCubeInLevel[i].TurnWhite();
            }
            else
            {
                listCubeInLevel[i].TurnBlack();
            }
        }
    }
    public int GetAmountBrickInLevel(E_TypeBrick _type)
    {
        int count = 0;

        for (int i = 0; i < listCubeInLevel.Count; i++)
        {
            if (listCubeInLevel[i]._typeBrick == _type)
            {
                count++;
            }
        }
        return count;
    }

    public void InitStartPosBrick()
    {
        for (int i = 0; i < listCubeInLevel.Count; i++)
        {
            listCubeInLevel[i].StartPosition = listCubeInLevel[i].transform.position;
        }
    }


}
