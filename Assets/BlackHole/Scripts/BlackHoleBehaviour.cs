using System;
using UnityEngine;

public class BlackHoleBehaviour : MonoBehaviour
{
    public static Action onRadiusThreshold;
    public static Action<float> onRadiusGrowth;

    [Header("References")]
    [SerializeField] Transform blackHoleVisual;

    [Header("Absorption Variables")]
    [SerializeField] float absorptionRadius = 5f;
    [SerializeField] float growthFactor = 0.1f;
    [SerializeField] float gravitationalPull = 10f;
    [SerializeField] float gravitationalRange = 2f;
    [SerializeField] float destructionDistanceThreshold = 0.5f;
    [SerializeField] LayerMask absorbableLayer;

    [Header("Scaling Factors")]
    [SerializeField] float rangeGrowthFactor = 0.1f;

    private float initialRadius;
    private int lastThreshold = 0;

    void Start()
    {
        initialRadius = absorptionRadius;
        UpdateBlackHoleSize();
    }

    void FixedUpdate()
    {
        AbsorbObjects();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, absorptionRadius);
    }

void AbsorbObjects()
{
    float effectiveGravitationalRange = gravitationalRange + (absorptionRadius - initialRadius) * rangeGrowthFactor;
    Collider[] objectsToAbsorb = Physics.OverlapSphere(transform.position, absorptionRadius * effectiveGravitationalRange, absorbableLayer);

    foreach (Collider obj in objectsToAbsorb)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = obj.gameObject.AddComponent<Rigidbody>();
        }

        Vector3 directionToBlackHole = (transform.position - obj.transform.position).normalized;
        float distanceToBlackHole = Vector3.Distance(transform.position, obj.transform.position);

        float effectiveGravitationalPull = gravitationalPull * (absorptionRadius / initialRadius);
        float forceMagnitude = effectiveGravitationalPull / distanceToBlackHole;
        rb.AddForce(directionToBlackHole * forceMagnitude, ForceMode.Acceleration);

        if (distanceToBlackHole < destructionDistanceThreshold)
        {
            Destroy(obj.gameObject);
            absorptionRadius += growthFactor;
            UpdateBlackHoleSize();
        }
    }
}


    void UpdateBlackHoleSize()
    {
        if (blackHoleVisual != null)
        {
            blackHoleVisual.localScale = Vector3.one * absorptionRadius * 0.2f;
        }

        int currentThreshold = Mathf.FloorToInt(absorptionRadius / 10f) * 10;

        if (currentThreshold > lastThreshold)
        {
            Debug.Log($"Absorption Radius reached {currentThreshold} meters");
            onRadiusThreshold?.Invoke();
            lastThreshold = currentThreshold;
        }

        onRadiusGrowth?.Invoke(absorptionRadius);
    }
}