using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Grenade Settings")]
    public GameObject grenadePrefab;
    public Transform throwPoint;
    public float throwForce = 15f;
    public float upwardForce = 5f;
    public float grenadeLifetime = 5f;

    [Header("Trajectory Line")]
    public LineRenderer lineRenderer;
    public int points = 30;
    public float pointSpacing = 0.1f;
    public LayerMask collisionMask;

    private bool isActive = false;

    void OnEnable()
    {
        isActive = true;
    }

    void OnDisable()
    {
        isActive = false;
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!isActive) return;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            DrawArc();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Throw();
        }
    }

    public void ManualThrow()
    {
        if (isActive)
        {
            Throw();
        }
    }

    public void Throw()
    {
        if (grenadePrefab == null || throwPoint == null)
        {
            Debug.LogWarning("Grenade prefab or throw point not assigned!");
            return;
        }

        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);

        if (grenade.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 velocity = throwPoint.forward * throwForce + throwPoint.up * upwardForce;
            rb.linearVelocity = velocity;
        }
        else
        {
            Debug.LogWarning("Grenade prefab has no Rigidbody!");
        }

        Destroy(grenade, grenadeLifetime);
    }

    void DrawArc()
    {
        lineRenderer.positionCount = points;
        Vector3 start = throwPoint.position;
        Vector3 velocity = throwPoint.forward * throwForce + throwPoint.up * upwardForce;

        for (int i = 0; i < points; i++)
        {
            float t = i * pointSpacing;
            Vector3 point = start + t * velocity;
            point.y += 0.5f * Physics.gravity.y * t * t;

            lineRenderer.SetPosition(i, point);

            if (Physics.OverlapSphere(point, 0.1f, collisionMask).Length > 0)
            {
                lineRenderer.positionCount = i + 1;
                break;
            }
        }
    }
}
