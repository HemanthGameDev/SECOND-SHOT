using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;

    [Header("Player Bullets")]
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private int playerBulletCount = 20;

    [Header("Shadow Bullets")]
    [SerializeField] private GameObject shadowBulletPrefab;
    [SerializeField] private int shadowBulletCount = 20;

    private List<GameObject> playerBullets = new List<GameObject>();
    private List<GameObject> shadowBullets = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        CreateBulletPool(
            playerBulletPrefab,
            playerBulletCount,
            playerBullets
        );

        CreateBulletPool(
            shadowBulletPrefab,
            shadowBulletCount,
            shadowBullets
        );
    }

    private void CreateBulletPool(
        GameObject bulletPrefab,
        int bulletCount,
        List<GameObject> bulletList)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is missing!", this);
            return;
        }

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);

            bullet.SetActive(false);

            bulletList.Add(bullet);
        }
    }

    public GameObject GetPlayerBullet()
    {
        return GetInactiveBullet(playerBullets);
    }

    public GameObject GetShadowBullet()
    {
        return GetInactiveBullet(shadowBullets);
    }

    private GameObject GetInactiveBullet(List<GameObject> bulletList)
    {
        foreach (GameObject bullet in bulletList)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        return null;
    }

    public void DeactivateAllBullets()
    {
        DeactivateBullets(playerBullets);
        DeactivateBullets(shadowBullets);
    }

    private void DeactivateBullets(List<GameObject> bulletList)
    {
        foreach (GameObject bullet in bulletList)
        {
            bullet.SetActive(false);
        }
    }
}