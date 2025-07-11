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
    public UnityEvent OnBeginDeathAnimation;
    public UnityEvent OnEndDeathAnimation;
    public UnityEvent OnBeginBlockAnimation;
    public UnityEvent OnEndBlockAnimation;

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

    public void OnCharacterBeginHit()
    {
        _animator.Play("Hit_In_Place");
    }

    public void OnStartCharacterDeath()
    {
        _animator.Play("Death");
        OnBeginDeathAnimation?.Invoke();
    }

    public void OnEndCharacterDeath()
    {
        OnEndDeathAnimation?.Invoke();
    }

    public void OnCharacterBounceBack()
    {
        _animator.Play("Bounce Back");
        OnCharacterEndAttack();
        OnCharacterEndHeavyAttack();
    }

    public void OnCharacterBeginBlock()
    {
        _animator.SetLayerWeight(1, 1);
        OnBeginBlockAnimation?.Invoke();
    }

    public void OnCharacterStopBlock()
    {
        _animator.SetLayerWeight(1, 0);
        OnEndBlockAnimation?.Invoke();
    }
}
