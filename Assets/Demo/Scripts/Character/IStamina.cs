public interface IStamina
{
    public float Stamina { get; }
    public float MaximumStamina { get; }

    public void DecreaseStamina(float value);

    public void IncreaseStamina(float value);

    public bool GetIsStaminaAvailable(int value);
}
