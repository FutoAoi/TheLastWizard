using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform firePoint;

    public float searchRadius = 5f;

    public float fireInterval = 1f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius);

        Transform target = null;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                target = hit.transform;
                break;
            }
        }

        if (target != null && timer >= fireInterval)
        {
            timer = 0f;

            Vector3 direction = target.position - firePoint.position;

            firePoint.rotation = Quaternion.LookRotation(direction);
            MagicBehavior magic = MagicObjectPool.Instance.GetMagic(0);
            magic.AddStatus(1, 5, 80, MagicType.Projectile);
            magic.transform.position = firePoint.position;
            magic.transform.rotation = firePoint.rotation;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}