using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public enum foodType {sugarOilRiceCake, PumpkinRiceCake, SweetThinPancake, FriedDoughTwist, SesameBalls, LotusCrisp, MungBeanSoup, EightIngredientCongee, SweetDumplings, SweetStewedEgg, BeanPasteBun, BowlCake }
    public foodType foodName;
    public GameObject[] sides;
    float[] timer;
    public float timeUntilBurn;
    public float requiredCookedPctToPlate;
    public Image[] fillImage;
    public Slider[] mySliders;
    public bool isFlipped;
    public bool isReadyToPlate;
    public GameObject mesh;
    public Vector3 initialPosition;
    public Vector3 flipPosition;
    public Vector3 notFlippedPosition;
    public bool isGoingUp;
    public float upAmount;
    public float targetUpAmount = .3f;
    float timeToFlip = .2f;

    // Start is called before the first frame update
    void Start()
    {
        timer = new float[sides.Length];
        StartCoroutine(Cook());
    }

    IEnumerator Cook()
    {
        while (true)
        {
            switch (foodName)
            {
                case foodType.sugarOilRiceCake:
                    break;
                case foodType.PumpkinRiceCake:
                    break;
                case foodType.SweetThinPancake:
                    if (isFlipped)
                    {
                        timer[1] += Time.deltaTime;
                        timer[0] += Time.deltaTime * .1f;
                        for (int i = 0; i < sides.Length; i++)
                        {
                            mySliders[i].value = Mathf.Min(1f, timer[sides.Length - 1 - i] / timeUntilBurn);
                        }
                    }
                    else
                    {
                        timer[1] += Time.deltaTime * .1f;
                        timer[0] += Time.deltaTime;
                        for (int i = 0; i < sides.Length; i++)
                        {
                            mySliders[i].value = Mathf.Min(1f, timer[i] / timeUntilBurn);
                        }
                    }

                    if (!isReadyToPlate && timer[0] / timeUntilBurn > requiredCookedPctToPlate && timer[1] / timeUntilBurn > requiredCookedPctToPlate)
                    {
                        isReadyToPlate = true;
                        if (CameraController.isZoomedIn)
                        {
                            CameraController.manager.plateFood.gameObject.SetActive(true);
                        }
                    }

                    break;
                case foodType.FriedDoughTwist:
                    break;
                case foodType.SesameBalls:
                    break;
                case foodType.LotusCrisp:
                    break;
                case foodType.MungBeanSoup:
                    break;
                case foodType.EightIngredientCongee:
                    break;
                case foodType.SweetDumplings:
                    break;
                case foodType.SweetStewedEgg:
                    break;
                case foodType.BeanPasteBun:
                    break;
                case foodType.BowlCake:
                    break;
                default:
                    break;
            }
            
            SliderColorUpdate();

            yield return null;
        }
    }

    public IEnumerator Flip()
    {
        float tempTimer = 0f;
        Quaternion initialQuaternion = mesh.transform.rotation;
        Vector3 startPosition = mesh.transform.localPosition;
        Vector3 landPosition = Vector3.zero;
        if (isFlipped)
        {
            landPosition = notFlippedPosition;
        }
        else
        {
            landPosition = flipPosition;
        }
        while(tempTimer < timeToFlip)
        {
            tempTimer += Time.deltaTime;
            mesh.transform.localPosition = Vector3.Lerp(startPosition, landPosition, tempTimer / timeToFlip);
            mesh.transform.rotation = Quaternion.Lerp(initialQuaternion, Quaternion.Euler(0f, 0f, initialQuaternion.eulerAngles.z + 180f), tempTimer / timeToFlip);
            yield return null;
        }
        isFlipped = !isFlipped;
        upAmount = 0f;
        StartCoroutine(Cook());
    }

    public IEnumerator Fall()
    {
        float tempTimer = 0f;
        Vector3 startPosition = mesh.transform.localPosition;
        Vector3 landPosition = Vector3.zero;
        if (isFlipped)
        {
            landPosition = flipPosition;
        }
        else
        {
            landPosition = notFlippedPosition;
        }
        while (tempTimer < timeToFlip)
        {
            tempTimer += Time.deltaTime;
            mesh.transform.localPosition = Vector3.Lerp(startPosition, landPosition, tempTimer / timeToFlip);
            yield return null;
        }
        upAmount = 0f;
        StartCoroutine(Cook());
    }

    void SliderColorUpdate()
    {
        if (isFlipped)
        {
            for (int i = 0; i < sides.Length; i++)
            {
                fillImage[sides.Length - 1 - i].color = new Color(timer[i] / timeUntilBurn, 1f - timer[i] / timeUntilBurn, 0f);
            }
        }
        else
        {
            for (int i = 0; i < sides.Length; i++)
            {
                fillImage[i].color = new Color(timer[i] / timeUntilBurn, 1f - timer[i] / timeUntilBurn, 0f);
            }
        }
    }
}
