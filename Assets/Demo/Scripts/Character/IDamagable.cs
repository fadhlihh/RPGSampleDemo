public interface IDamagable
{
    public int HealthPoint { get; }
    public int MaximumHealthPoint { get; }
    public bool IsDead { get; }

    public void Damage(DamageData damageData);
    public void Heal(int value);

    public void Death();
}
