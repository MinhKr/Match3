using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage instance { get; private set; }

    public BrickBase BrickBase;
}
