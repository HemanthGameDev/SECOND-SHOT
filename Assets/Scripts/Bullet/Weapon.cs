using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject shootPoint;
    public event Action<Vector3> OnShot;

    private InputSystem_Actions attackAction;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        attackAction = new InputSystem_Actions();
        
    }

    private void OnEnable()
    {
        attackAction.Enable();
    }

    private void OnDisable()
    {
        attackAction.Disable();
    }

    private void Update()
    {
        if (playerController.IsDead)
            return;

        if (attackAction.Player.Attack.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPooling.instance.GetPlayerBullet();

        if (bullet == null)
        {
            Debug.Log("No available bullet in pool.");
            return;
        }

        bullet.transform.position = shootPoint.transform.position;
        bullet.SetActive(true);

        Vector3 shootDirection = shootPoint.transform.forward;

        bullet.GetComponent<Bullet>().Initialize(shootDirection);

        OnShot?.Invoke(shootDirection);
    }
}