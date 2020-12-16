using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform position3rdPerson;
    public static CameraController manager;
    Vector3 shift1stPerson = new Vector3(0f, .87f, -1.62f);
    public static bool isZoomedIn;
    GameObject canvas;

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
        }
        else
        {
            Vector3 playerPos = LevelManager.manager.burners[PlayerController.position].position;
            transform.position = playerPos + shift1stPerson;
            transform.rotation = Quaternion.Euler(20f, 0f, 0f);
        }

        isZoomedIn = !isZoomedIn;
        canvas.SetActive(isZoomedIn);
    }

    public void DoTheThing()
    {
        LevelManager.manager.burners[PlayerController.position].GetComponentInChildren<Cookware>().ResetTimer();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ZoomToggle();
        }
    }
}
