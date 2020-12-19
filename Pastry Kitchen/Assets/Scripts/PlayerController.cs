using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static int position;
    public static Cookware.eCookware cookwareUsing;
    public static PlayerController player;
    public static float distanceBehindBurner = 2f;
    public float minDistanceToBurner = 2f;
    public Rigidbody rb;
    public float maxSpeed = 7f;

    public float timeToTurn = 1f;
    public float timeToSlowDown = .1f;
    float lastMouseX;
    float lastMouseY;
    bool holdingFood;
    Food heldFood;
  
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
        else
        {
            switch (cookwareUsing)
            {
                case Cookware.eCookware.pancakePan:
                    if (Input.GetButtonDown("Fire1"))
                    {
                        float radiusX = .2f;
                        float clickX = Mathf.Abs((Input.mousePosition.x - (float)Screen.width / 2f)) / (float)Screen.width;
                        float radiusY = .2f;
                        float clickY = Mathf.Abs((Input.mousePosition.y - (float)Screen.height * 2f / 5f)) / (float)Screen.height;
                        if (clickX > radiusX || clickY > radiusY)
                        {
                            return;
                        }
                        lastMouseX = Input.mousePosition.x;
                        lastMouseY = Input.mousePosition.y;

                        if ((lastMouseX - (float)Screen.width / 2f) / (float)Screen.width < 0f)
                        {
                            heldFood = LevelManager.cookware[position].food[0];
                        }
                        else
                        {
                            heldFood = LevelManager.cookware[position].food[1];
                        }
                        
                        if (heldFood != null)
                        {
                            holdingFood = true;
                            heldFood.initialPosition = heldFood.transform.position;
                            heldFood.isGoingUp = true;
                            heldFood.StopAllCoroutines();
                        }
                    }

                    if (!holdingFood)
                    {
                        return;
                    }

                    if (Input.GetButtonUp("Fire1"))
                    {
                        heldFood.StartCoroutine(heldFood.Fall());
                        holdingFood = false;
                        heldFood.isGoingUp = false;
                        heldFood = null;
                    }

                    if (Input.mousePosition.y == lastMouseY)
                    {
                        return;
                    }

                    if (Input.GetButton("Fire1"))
                    {
                        float moveY = (Input.mousePosition.y - lastMouseY) / (float)Screen.height;
                        lastMouseY = Input.mousePosition.y;
                        if (heldFood.isGoingUp && moveY + heldFood.upAmount > 0)
                        {
                            heldFood.upAmount += moveY;
                            float x = heldFood.mesh.transform.position.x;
                            float z = heldFood.mesh.transform.position.z;
                            heldFood.mesh.transform.position = Vector3.Lerp(heldFood.initialPosition, heldFood.initialPosition + Vector3.up * heldFood.targetUpAmount, heldFood.upAmount / heldFood.targetUpAmount);
                        }
                        if (heldFood.isGoingUp && heldFood.upAmount > heldFood.targetUpAmount)
                        {
                            heldFood.StartCoroutine(heldFood.Flip());
                            heldFood.isGoingUp = false;
                            holdingFood = false;
                            heldFood = null;
                        }
                    }
                    break;
                case Cookware.eCookware.deepFryer:
                    break;
                case Cookware.eCookware.soupPot:
                    break;
                case Cookware.eCookware.steamer:
                    break;
                default:
                    break;
            }
        }
    }
}
