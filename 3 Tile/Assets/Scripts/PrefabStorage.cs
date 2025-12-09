using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage instance { get; private set; }

    public BrickBase BrickBase;

    public ParticleSystem FxMerge;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
