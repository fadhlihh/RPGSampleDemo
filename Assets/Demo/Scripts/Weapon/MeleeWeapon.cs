using UnityEngine;

public class MeleeWeapon : Weapon
{
    public override EWeaponType Type => EWeaponType.Melee;

    public override void HeavyAttack()
    {
        _damageModifier = 10;
    }
}
