using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform position3rdPerson;
    public static CameraController manager;
    Vector3 shift1stPerson = new Vector3(0f, 0.57f, -0.84f);
    Vector3 rotation1stPerson = new Vector3(27f, 0f, 0f);
    public static bool isZoomedIn;
    GameObject canvas;
    public Button addFood;
    public Button plateFood;

    // Start is called before the first frame update
    void Awake()
    {
        manager = this;
        canvas = GetComponentInChildren<RectTransform>().gameObject;
        canvas.SetActive(false);
    }

    void ZoomToggle()
    {
        if (isZoomedIn)
        {
            transform.position = position3rdPerson.position;
            transform.rotation = position3rdPerson.rotation;

            ToggleSliders(true);
        }
        else if (!ZoomInCheck())
        {
            return;
        }

        isZoomedIn = !isZoomedIn;
        canvas.SetActive(isZoomedIn);
    }

    bool ZoomInCheck()
    {
        Vector3 playerPos = PlayerController.player.transform.position;
        float nearestBurnerDistance = float.PositiveInfinity;
        GameObject nearestBurner = null;

        for (int i = 0; i < LevelManager.manager.burners.Length; i++)
        {
            GameObject burner = LevelManager.manager.burners[i].gameObject;
            float burnerDistance = Vector3.Distance(playerPos, burner.transform.position);
            if (burnerDistance < nearestBurnerDistance && burnerDistance < PlayerController.player.minDistanceToBurner)
            {
                nearestBurnerDistance = burnerDistance;
                nearestBurner = burner.gameObject;
                PlayerController.position = i;
            }
        }

        if (nearestBurner != null)
        {
            PlayerController player = PlayerController.player;
            player.transform.position = nearestBurner.transform.position - nearestBurner.transform.forward * PlayerController.distanceBehindBurner;
            player.StopAllCoroutines();
            player.rb.velocity = Vector3.zero;
            player.transform.rotation = Quaternion.LookRotation(nearestBurner.transform.position - player.transform.position, transform.up);
            player.StartCoroutine(player.SlowDown());
            Vector3 shift = nearestBurner.transform.forward * shift1stPerson.z + Vector3.up * shift1stPerson.y;
            transform.position = nearestBurner.transform.position + shift;
            transform.rotation = Quaternion.Euler(rotation1stPerson);
            PlayerController.cookwareUsing = nearestBurner.GetComponentInChildren<Cookware>().eCookwareType;

            ToggleSliders(false);
            Food mostCritical = LevelManager.cookware[PlayerController.position].MostCriticalFood();
            if (mostCritical == null)
            {
                plateFood.gameObject.SetActive(false);
                return true;
            }
            if (mostCritical.isReadyToPlate)
            {
                plateFood.gameObject.SetActive(true);
            }
            else
            {
                plateFood.gameObject.SetActive(false);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    void ToggleSliders(bool _3rdPerson)
    {
        foreach (Cookware cookware in LevelManager.cookware)
        {
            foreach (Food food in cookware.food)
            {
                if (food == null)
                {
                    continue;
                }

                foreach (Slider slider in food.mySliders)
                {
                    slider.gameObject.SetActive(!_3rdPerson);
                }
            }
        }

        foreach (Slider slider in LevelManager.cookwareSliders)
        {
            slider.gameObject.SetActive(_3rdPerson);
        }
    }

    public void DoTheThing()
    {
        LevelManager.cookware[PlayerController.position].ResetTimer();
    }

    public void AddFood(GameObject _food)
    {
        Cookware curCookware = LevelManager.cookware[PlayerController.position];
        int foodSlot = curCookware.GetEmptyFoodSlot();
        curCookware.food[foodSlot] = Instantiate(_food, curCookware.foodSlots[foodSlot]).GetComponent<Food>();
        if (curCookware.GetNumberOfFoodSlotsTaken() == 1)
        {
            curCookware.cook = curCookware.StartCoroutine(curCookware.Cook());
        }
        if (curCookware.GetEmptyFoodSlot() == -1)
        {
            addFood.gameObject.SetActive(false);
        }
    }

    public void PlateFood()
    {
        Cookware curCookware = LevelManager.cookware[PlayerController.position];
        Destroy(curCookware.MostCriticalFood().gameObject);
        addFood.gameObject.SetActive(true);
        Food mostCritical = curCookware.MostCriticalFood();
        if (mostCritical == null)
        {
            plateFood.gameObject.SetActive(false);
            curCookware.StopCoroutine(curCookware.cook);
            return;
        }
        if (mostCritical.isReadyToPlate)
        {
            plateFood.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ZoomToggle();
        }
    }
}
