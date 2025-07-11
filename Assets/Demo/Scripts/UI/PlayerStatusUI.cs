using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private Image _healthBar;
    [SerializeField]
    private Image _staminaBar;

    public void SetHealthBarValue(float value, float maxHealth)
    {
        _healthBar.fillAmount = value / maxHealth;
    }

    public void SetStaminaBarValue(float value, float maximumStamina)
    {
        _staminaBar.fillAmount = value / maximumStamina;
    }
}
