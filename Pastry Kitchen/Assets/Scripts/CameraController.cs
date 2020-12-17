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
            PlayerController.player.transform.position = nearestBurner.transform.position - nearestBurner.transform.forward * PlayerController.distanceBehindBurner;
            PlayerController.player.StopAllCoroutines();
            PlayerController.player.rb.velocity = Vector3.zero;
            PlayerController.player.transform.rotation = Quaternion.LookRotation(nearestBurner.transform.position - PlayerController.player.transform.position, transform.up);
            PlayerController.player.StartCoroutine(PlayerController.player.SlowDown());
            transform.position = nearestBurner.transform.position + shift1stPerson;
            transform.rotation = Quaternion.Euler(20f, 0f, 0f);
            return true;
        }
        else
        {
            return false;
        }
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
