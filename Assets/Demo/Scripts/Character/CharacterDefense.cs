using UnityEngine;
using UnityEngine.Events;

public class CharacterDefense : MonoBehaviour
{
    [SerializeField]
    private int _damageModifier;

    public UnityEvent OnStartBlock;
    public UnityEvent OnStopBlock;

    public bool IsBlocking { get; private set; }
    public int DamageModifier { get => _damageModifier; }

    public void StartBlock()
    {
        IsBlocking = true;
        OnStartBlock?.Invoke();
    }

    public void StopBlock()
    {
        IsBlocking = false;
        OnStopBlock?.Invoke();
    }
}
