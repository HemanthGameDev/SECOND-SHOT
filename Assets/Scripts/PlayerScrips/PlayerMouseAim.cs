using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseAim : MonoBehaviour
{
    public Vector3 AimDirection { get; private set; }

    private void Update()
    {
        Vector2 mousePosition = Pointer.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y;

            Vector3 aimDirection = targetPosition - transform.position;
            aimDirection.y = 0f;

            if (aimDirection.sqrMagnitude > 0.001f)
            {
                AimDirection = aimDirection.normalized;
                transform.forward = AimDirection;
            }
        }
    }

}