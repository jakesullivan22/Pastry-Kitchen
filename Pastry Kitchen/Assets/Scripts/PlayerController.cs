using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static int position;
    float distanceBehindBurner = 2f;
  
    // Start is called before the first frame update
    void Start()
    {
        MoveToPosition(LevelManager.manager.startPosition);
    }

    void MoveToPosition(int _position)
    {
        Vector3 targetBurner = LevelManager.manager.burners[_position].position;
        transform.position = targetBurner - Vector3.forward * distanceBehindBurner;
        position = _position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal") && !CameraController.isZoomedIn)
        {
            if (Input.GetAxisRaw("Horizontal") > 0f)
            {
                MoveToPosition(Mathf.Min(position + 1, LevelManager.manager.burners.Length - 1));
            } 
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                MoveToPosition(Mathf.Max(position - 1, 0));
            }
        }
    }
}
