using UnityEngine;

public class BlackHoleBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform blackHoleVisual;

    [Header("Absorption Variables")]
    [SerializeField] float absorptionRadius = 5f;
    [SerializeField] float growthFactor = 0.1f;
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
        Collider[] objectsToAbsorb = Physics.OverlapSphere(transform.position, absorptionRadius, absorbableLayer);

        foreach (Collider obj in objectsToAbsorb)
        {
            Destroy(obj.gameObject);
            absorptionRadius += growthFactor;
            UpdateBlackHoleSize();
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
