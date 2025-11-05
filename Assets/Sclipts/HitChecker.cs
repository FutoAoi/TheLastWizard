using UnityEngine;

public class HitChecker : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        IDamageable hittarget = other.gameObject.GetComponent<IDamageable>();
        hittarget.Hit(1);
    }
}
