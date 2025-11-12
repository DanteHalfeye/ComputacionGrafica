using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private UnityEvent OnAction;

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject impactEffect;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.left * speed; // use 'velocity', not 'linearVelocity'
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Call event (if any)
        OnAction?.Invoke();

        // Spawn visual effects at impact
        SpawnEffects(collision);
        CameraShake.Instance.Shake(0.3f, 0.2f);

        // Destroy projectile
        Destroy(gameObject);
    }

    private void SpawnEffects(Collision collision)
    {
        // Get the contact point (where collision happened)
        ContactPoint contact = collision.contacts[0];

        // Compute a rotation that looks in the direction of the normal
        Quaternion normalRotation = Quaternion.LookRotation(contact.normal);

        // Spawn both effects at the impact point, facing the normal
        if (explosionEffect)
            Instantiate(explosionEffect, contact.point, normalRotation);

        if (impactEffect)
            Instantiate(impactEffect, contact.point, normalRotation);
    }
}