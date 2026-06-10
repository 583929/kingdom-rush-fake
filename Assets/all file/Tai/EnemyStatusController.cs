using UnityEngine;

public class EnemyStatusController : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    [Header("Move Speed")]
    public float baseMoveSpeed = 2f;
    public float currentMoveSpeed = 2f;

    private int fireStack = 0;
    private float burnTimer = 0f;
    private float burnTickTimer = 0f;
    private float fireDamage = 0f;
    private float burnDamagePercent = 0.25f;

    private int iceStack = 0;
    private float frostTimer = 0f;
    private float frostTickTimer = 0f;
    private float iceDamage = 0f;
    private float slowPercent = 0.1f;
    private float frostDamagePercent = 0.2f;

    private bool isFrozen = false;
    private float freezeTimer = 0f;

    private const int FULL_STACK = 5;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        UpdateBurn();
        UpdateFrost();
        UpdateFreeze();
        UpdateMoveSpeed();
        CheckFireIceCombo();
    }

    public void ApplyBurn(
        float towerDamage,
        float duration,
        int maxStack,
        float damagePercent
    )
    {
        fireStack++;

        if (fireStack > maxStack)
            fireStack = maxStack;

        fireDamage = towerDamage;
        burnTimer = duration;
        burnDamagePercent = damagePercent;
    }

    public void ApplyFrost(
        float towerDamage,
        float slowAmount,
        float duration,
        int maxStack,
        float damagePercent,
        float freezeDuration
    )
    {
        iceStack++;

        if (iceStack > maxStack)
            iceStack = maxStack;

        iceDamage = towerDamage;
        slowPercent = slowAmount;
        frostTimer = duration;
        frostDamagePercent = damagePercent;

        if (iceStack >= maxStack)
        {
            Freeze(freezeDuration);
        }
    }

    void UpdateBurn()
    {
        if (fireStack <= 0)
            return;

        burnTimer -= Time.deltaTime;
        burnTickTimer -= Time.deltaTime;

        if (burnTickTimer <= 0f)
        {
            float damage = fireDamage * burnDamagePercent * fireStack;

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            burnTickTimer = 1f;
        }

        if (burnTimer <= 0f)
        {
            fireStack = 0;
        }
    }

    void UpdateFrost()
    {
        if (iceStack <= 0)
            return;

        frostTimer -= Time.deltaTime;
        frostTickTimer -= Time.deltaTime;

        if (frostTickTimer <= 0f)
        {
            float damage = iceDamage * frostDamagePercent * iceStack;

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            frostTickTimer = 0.5f;
        }

        if (frostTimer <= 0f)
        {
            iceStack = 0;
        }
    }

    void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
    }

    void UpdateFreeze()
    {
        if (isFrozen == false)
            return;

        freezeTimer -= Time.deltaTime;

        if (freezeTimer <= 0f)
        {
            isFrozen = false;
            iceStack = 0;
        }
    }

    void UpdateMoveSpeed()
    {
        if (isFrozen)
        {
            currentMoveSpeed = 0f;
            return;
        }

        if (iceStack > 0)
        {
            float totalSlow = slowPercent * iceStack;
            totalSlow = Mathf.Clamp(totalSlow, 0f, 0.7f);

            currentMoveSpeed = baseMoveSpeed * (1f - totalSlow);
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
        }
    }

    void CheckFireIceCombo()
    {
        if (fireStack >= FULL_STACK && iceStack >= FULL_STACK)
        {
            float effectDamage = fireDamage + iceDamage;
            float bonusMagicDamage = effectDamage * 0.1f;

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(bonusMagicDamage);
            }

            Debug.Log("Combo Lửa + Băng: gây thêm sát thương phép!");

            fireStack = 0;
            iceStack = 0;
        }
    }

    public float GetCurrentMoveSpeed()
    {
        return currentMoveSpeed;
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}