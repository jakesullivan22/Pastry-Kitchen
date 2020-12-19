using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;
    public Transform[] burners;
    public static Cookware[] cookware;
    public static Slider[] cookwareSliders;
    public int startPosition;

    void Awake()
    {
        manager = this;
        cookware = new Cookware[burners.Length];
        cookwareSliders = new Slider[burners.Length];
        for (int i = 0; i < burners.Length; i++)
        {
            cookware[i] = burners[i].GetComponentInChildren<Cookware>();
            cookwareSliders[i] = cookware[i].GetComponentInChildren<Slider>();
        }
    }
}
