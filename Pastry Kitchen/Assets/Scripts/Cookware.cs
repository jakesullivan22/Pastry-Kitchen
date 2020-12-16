using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cookware : MonoBehaviour
{
    Slider myslider;
    public float timeUntilBurn;
    float timer;
    public Image fillImage;
    public enum eCookware { pancakePan, deepFryer, soupPot, steamer}
    public eCookware eCookwareType;

    // Start is called before the first frame update
    void Start()
    {
        myslider = GetComponentInChildren<Slider>();
        RectTransform sliderPos = GetComponentInChildren<RectTransform>();
        sliderPos.rotation = Quaternion.LookRotation(CameraController.manager.position3rdPerson.position - sliderPos.position, Vector3.up);
        StartCoroutine(Cook());
    }

    IEnumerator Cook()
    {
        while (timer < timeUntilBurn)
        {
            timer += Time.deltaTime;
            SliderColorUpdate();
            myslider.value = Mathf.Min(1f, timer / timeUntilBurn);
            yield return null;
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    void SliderColorUpdate()
    {
        fillImage.color = new Color(timer / timeUntilBurn, 1f - timer / timeUntilBurn, 0f);
    }
}
