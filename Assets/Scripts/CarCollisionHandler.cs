using UnityEngine;

// Report to GameManager and destroy both vehicles when colliding with other vehicles.
public class CarCollisionHandler : MonoBehaviour
{
    // 2D explosion effects (Prefab)"
    public GameObject explosion2D;

    // Explosion sound effects
    public AudioClip explosionSFX;    // Added: Explosion audio
    private AudioSource audioSource;  // Added: Used for playing sound effects

    // Special effects offset to camera
    public float cameraOffset = 0.3f;  // Added: Distance offset towards the camera

    bool hasHandled = false;

    void Awake()
    {
        // Automatically add AudioSource (if the prefab does not exist).
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;   // 3D sound (where it can be heard)
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHandled) return;

        // Only concerned with collisions with other objects labeled "Car".
        if (collision.collider.CompareTag("Car"))
        {
            hasHandled = true;

            // Play explosion sound effect
            if (explosionSFX != null)
            {
                audioSource.PlayOneShot(explosionSFX);
            }

            // Generate 2D cartoon explosion effects 
            if (explosion2D != null)
            {
                Vector3 pos = collision.contacts[0].point;

                // Calculate the offset: move one point closer to the camera from the collision point.
                Vector3 camDir = (Camera.main.transform.position - pos).normalized;
                pos += camDir * cameraOffset;

                // Billboard facing the camera
                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                GameObject fx = Instantiate(explosion2D, pos, rot);
                Destroy(fx, 2f);
            }

            // Report the car crash
            TrafficGameController.Instance?.OnCarCrash();

            // Destroy both vehicles (slight delay to ensure sound and special effects play).
            Destroy(collision.gameObject, 0.05f);
            Destroy(gameObject, 0.05f);
        }
    }
}
