using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : CharacterAnimation
{
    public UnityEvent OnBeginRollingAnimation;
    public UnityEvent OnEndRollingAnimation;

    public void OnCharacterBeginRoll()
    {
        _animator.applyRootMotion = true;
        _animator.SetBool("IsRolling", true);
        OnBeginRollingAnimation?.Invoke();
    }

    public void OnCharacterEndRoll()
    {
        _animator.SetBool("IsRolling", false);
        _animator.applyRootMotion = false;
        OnEndRollingAnimation?.Invoke();
    }
}
