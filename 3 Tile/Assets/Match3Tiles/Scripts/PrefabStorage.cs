using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage instance { get; private set; }

    public BrickBase BrickBase;
    public LevelManager level_1;
    public GridGroup GridGroup;

    [Header("---DataController---")]
    public DataLevelSO DataLevel;
    //public DataBoosterTut DataBooster;

    public ParticleSystem FxMerge;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
