using UnityEngine;

public class TowerData : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip shootSound;

    [Header("Tower Stats")]
    public float range = 5f;
    public float shootInterval = 0.5f;
    public float projectileSpeed = 15f;
    public float projectileDuration = 2f;
    public float damage = 10f;

    [Header("Build Settings")]
    public int price = 100;

    [Header("References")]
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Projectile Sprite")]
    public Sprite projectileSprite;

    [HideInInspector] public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}