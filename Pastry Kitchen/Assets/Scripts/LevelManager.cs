using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;
    public Transform[] burners;
    public int startPosition;

    void Awake()
    {
        manager = this;
    }
}
