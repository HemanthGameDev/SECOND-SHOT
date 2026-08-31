using System.Collections.Generic;
using UnityEngine;

public class ImpactVFXPool : MonoBehaviour
{
    public static ImpactVFXPool instance;

    [Header("Player Impact")]
    [SerializeField] private ImpactVFX playerImpactPrefab;
    [SerializeField] private int playerImpactCount = 10;

    [Header("Shadow Impact")]
    [SerializeField] private ImpactVFX shadowImpactPrefab;
    [SerializeField] private int shadowImpactCount = 20;

    private List<ImpactVFX> playerImpacts = new List<ImpactVFX>();
    private List<ImpactVFX> shadowImpacts = new List<ImpactVFX>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        CreatePool(
            playerImpactPrefab,
            playerImpactCount,
            playerImpacts
        );

        CreatePool(
            shadowImpactPrefab,
            shadowImpactCount,
            shadowImpacts
        );
    }

    private void CreatePool(
        ImpactVFX prefab,
        int count,
        List<ImpactVFX> pool)
    {
        if (prefab == null)
        {
            Debug.LogError("Impact VFX prefab is missing!", this);
            return;
        }

        for (int i = 0; i < count; i++)
        {
            ImpactVFX impact = Instantiate(prefab, transform);

            impact.gameObject.SetActive(false);

            pool.Add(impact);
        }
    }

    public ImpactVFX GetPlayerImpact()
    {
        return GetAvailableImpact(playerImpacts);
    }

    public ImpactVFX GetShadowImpact()
    {
        return GetAvailableImpact(shadowImpacts);
    }

    private ImpactVFX GetAvailableImpact(List<ImpactVFX> pool)
    {
        foreach (ImpactVFX impact in pool)
        {
            if (!impact.gameObject.activeInHierarchy)
            {
                return impact;
            }
        }

        return null;
    }

    public void PlayPlayerImpact(Vector3 position, Vector3 normal)
    {
        ImpactVFX impact = GetPlayerImpact();

        if (impact != null)
        {
            impact.Play(position, normal);
        }
    }

    public void PlayShadowImpact(Vector3 position, Vector3 normal)
    {
        ImpactVFX impact = GetShadowImpact();

        if (impact != null)
        {
            impact.Play(position, normal);
        }
    }
}