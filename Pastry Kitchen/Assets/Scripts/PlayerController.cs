using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static int position;
    public static PlayerController player;
    public static float distanceBehindBurner = 2f;
    public float minDistanceToBurner = 2f;
    public Rigidbody rb;
    public float maxSpeed = 7f;

    public float timeToTurn = 1f;
    public float timeToSlowDown = .1f;
  
    // Start is called before the first frame update
    void Awake()
    {
        player = this;
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator Turn()
    {
        float timer = 0f;
        while (timer < timeToTurn)
        {
            timer += Time.deltaTime;
            if (rb.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity, Vector3.up), timer / timeToTurn);
            }
            yield return null;
        }
    }

    public IEnumerator SlowDown()
    {
        float timer = 0f;
        Vector3 initialVelocity = rb.velocity;
        while (timer < timeToSlowDown)
        {
            timer += Time.deltaTime;
            rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, timer / timeToSlowDown);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraController.isZoomedIn)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(x) + Mathf.Abs(z) > 0)
            {
                rb.AddForce((x * Vector3.right + z * Vector3.forward) * maxSpeed);
                Vector3 rightQuickTurn = Vector3.right * Mathf.Clamp(rb.velocity.x, Mathf.Min(x * maxSpeed, 0), Mathf.Max(x * maxSpeed, 0));
                Vector3 forwardQuickTurn = Vector3.forward * Mathf.Clamp(rb.velocity.z, Mathf.Min(z * maxSpeed, 0), Mathf.Max(z * maxSpeed, 0));
                rb.velocity = rightQuickTurn + forwardQuickTurn;
                rb.velocity = rb.velocity.normalized * Mathf.Min(rb.velocity.magnitude, maxSpeed);

            }

            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            {
                StopAllCoroutines();
                StartCoroutine(Turn());
            }

            if ((Input.GetButtonUp("Horizontal") && !Input.GetButton("Vertical")) || (Input.GetButtonUp("Vertical") && !Input.GetButton("Horizontal")))
            {
                StartCoroutine(SlowDown());
            }
        }
    }
}
