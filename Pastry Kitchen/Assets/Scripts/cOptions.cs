using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cOptions : MonoBehaviour
{
    public Button bBack;

    public void OnBackClicked()
    {
        Destroy(gameObject);
    }
}
