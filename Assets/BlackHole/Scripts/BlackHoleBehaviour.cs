using UnityEngine;

public class BlackHoleBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform blackHoleVisual;

    [Header("Absorption Variables")]
    [SerializeField] float absorptionRadius = 5f;
    [SerializeField] float growthFactor = 0.1f;

    [SerializeField] float gravitationalPull = 10f; 
    [SerializeField] float gravitationalRangeFactor = 2f;
    [SerializeField] LayerMask absorbableLayer;
    

    void Start()
    {
        UpdateBlackHoleSize();
    }

    void Update()
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
        Collider[] objectsToAbsorb = Physics.OverlapSphere(transform.position, absorptionRadius * gravitationalRangeFactor, absorbableLayer);

        foreach (Collider obj in objectsToAbsorb)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 directionToBlackHole = (transform.position - obj.transform.position).normalized;
                float distanceToBlackHole = Vector3.Distance(transform.position, obj.transform.position);

                float forceMagnitude = gravitationalPull / distanceToBlackHole;
                rb.AddForce(directionToBlackHole * forceMagnitude, ForceMode.Acceleration);

                if (distanceToBlackHole < absorptionRadius)
                {
                    Destroy(obj.gameObject);
                    absorptionRadius += growthFactor;
                    UpdateBlackHoleSize();
                }
            }
        }
    }

    void UpdateBlackHoleSize()
    {
        if (blackHoleVisual != null)
        {
            blackHoleVisual.localScale = Vector3.one * absorptionRadius * 0.2f; 
        }
    }
}
