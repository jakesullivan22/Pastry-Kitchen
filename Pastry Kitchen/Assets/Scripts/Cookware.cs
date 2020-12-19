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
    public Food[] food;
    public Transform[] foodSlots;
    public Coroutine cook;

    // Start is called before the first frame update
    void Start()
    {
        myslider = GetComponentInChildren<Slider>();
        RectTransform sliderPos = GetComponentInChildren<RectTransform>();
        sliderPos.rotation = Quaternion.LookRotation(CameraController.manager.position3rdPerson.position - sliderPos.position, Vector3.up);
        
        switch (eCookwareType)
        {
            case eCookware.pancakePan:
                food = new Food[foodSlots.Length];
                break;
            case eCookware.deepFryer:
                food = new Food[6];
                break;
            case eCookware.soupPot:
                food = new Food[5];
                break;
            case eCookware.steamer:
                food = new Food[3];
                break;
            default:
                break;
        }
    }

    public IEnumerator Cook()
    {
        switch (eCookwareType)
        {
            case eCookware.pancakePan:
                while (true)
                {
                    Food mostCriticalFood = MostCriticalFoodBottom();
                    if (mostCriticalFood != null)
                    {
                        timer = mostCriticalFood.mySliders[0].value * mostCriticalFood.timeUntilBurn;
                        timeUntilBurn = mostCriticalFood.timeUntilBurn;
                        SliderColorUpdate();
                        myslider.value = Mathf.Min(1f, timer / timeUntilBurn);
                    }
                    yield return null;
                }
            case eCookware.deepFryer:
            case eCookware.soupPot:
            case eCookware.steamer:
                while (timer < timeUntilBurn)
                {
                    timer += Time.deltaTime;
                    SliderColorUpdate();
                    myslider.value = Mathf.Min(1f, timer / timeUntilBurn);
                    yield return null;
                }
                break;
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    public Food MostCriticalFood()
    {
        Food mostCriticalFood = null;
        float percentCooked = float.NegativeInfinity;
        foreach (Food _food in food)
        {
            if (_food == null)
            {
                continue;
            }

            float sliderTotal = 0f;

            foreach (Slider slider in _food.mySliders)
            {
                if (slider == null)
                {
                    continue;
                }
                sliderTotal += slider.value;
                
            }

            sliderTotal /= (float)_food.mySliders.Length;

            if (sliderTotal > percentCooked)
            {
                mostCriticalFood = _food;
                percentCooked = sliderTotal;
            }
        }
        return mostCriticalFood;
    }

    public Food MostCriticalFoodBottom()
    {
        Food mostCriticalFood = null;
        float percentCooked = float.NegativeInfinity;
        foreach (Food _food in food)
        {
            if (_food == null)
            {
                continue;
            }

            if (_food.mySliders[0].value > percentCooked)
            {
                mostCriticalFood = _food;
                percentCooked = _food.mySliders[0].value;
            }
        }
        return mostCriticalFood;
    }

    public int GetEmptyFoodSlot()
    {
        int emptySlot = -1;
        for (int i = 0; i < food.Length; i++)
        {
            if (emptySlot == -1 && food[i] == null)
            {
                emptySlot = i;
            }
        }
        return emptySlot;
    }

    public int GetNumberOfFoodSlotsTaken()
    {
        int takenSlots = 0;
        for (int i = 0; i < food.Length; i++)
        {
            if (food[i] == null)
            {
                takenSlots++;
            }
        }
        return takenSlots;
    }

    void SliderColorUpdate()
    {
        fillImage.color = new Color(timer / timeUntilBurn, 1f - timer / timeUntilBurn, 0f);
    }
}
