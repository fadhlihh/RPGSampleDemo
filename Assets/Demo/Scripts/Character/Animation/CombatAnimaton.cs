using UnityEngine;
using UnityEngine.Events;

public class CombatAnimation : CharacterAnimation
{
    public UnityEvent OnBeginAttackingAnimation;
    public UnityEvent OnEndAttackingAnimation;
    public UnityEvent OnBeginHeavyAttackingAnimation;
    public UnityEvent OnEndHeavyAttackingAnimation;
    public UnityEvent OnBeginTraceHitAnimation;
    public UnityEvent OnEndTraceHitAnimation;

    public void OnCharacterBeginAttack(int combo)
    {
        _animator.applyRootMotion = true;
        _animator.SetBool("IsAttacking", true);
        _animator.SetInteger("Combo", combo);
        OnBeginAttackingAnimation?.Invoke();
    }

    public void OnCharacterEndAttack()
    {
        _animator.SetBool("IsAttacking", false);
        _animator.applyRootMotion = false;
        OnEndAttackingAnimation?.Invoke();
    }

    public void OnCharacterBeginHeavyAttack()
    {
        _animator.applyRootMotion = true;
        _animator.SetBool("IsHeavyAttack", true);
        _animator.SetBool("IsAttacking", true);
        OnBeginHeavyAttackingAnimation?.Invoke();
    }

    public void OnCharacterEndHeavyAttack()
    {
        _animator.SetBool("IsAttacking", false);
        _animator.SetBool("IsHeavyAttack", false);
        _animator.applyRootMotion = false;
        OnEndHeavyAttackingAnimation?.Invoke();
    }

    public void OnStartTracingHit()
    {
        OnBeginTraceHitAnimation?.Invoke();
    }

    public void OnEndTracingHit()
    {
        OnEndTraceHitAnimation?.Invoke();
    }
}
