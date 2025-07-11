using UnityEngine;
using UnityEngine.Events;

public class EnemyCharacter : Character, IDamagable
{
    [SerializeField]
    private GameObject _impactPrefab;

    public int HealthPoint { get; private set; }
    public int MaximumHealthPoint { get; private set; } = 100;
    public bool IsDead { get; private set; } = false;

    public UnityEvent OnDamage;
    public UnityEvent OnDeath;

    protected override void Awake()
    {
        base.Awake();
        HealthPoint = MaximumHealthPoint;
    }

    public void Damage(DamageData damageData)
    {
        if (!IsDead)
        {
            Instantiate(_impactPrefab, damageData.HitImpactPosition, Quaternion.identity);
            HealthPoint -= damageData.HitPoint;
            OnDamage?.Invoke();
            if (HealthPoint <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    public void Heal(int value)
    {

    }

    public void DestroyCharacter()
    {
        Destroy(gameObject, 3);
    }
}
