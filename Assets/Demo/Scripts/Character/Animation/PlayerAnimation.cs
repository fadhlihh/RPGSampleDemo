using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : CombatAnimation
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

    public void OnBeginLockTarget()
    {
        _animator.SetBool("IsLockTarget", true);
    }

    public void OnEndLockTarget()
    {
        _animator.SetBool("IsLockTarget", false);
    }
}
