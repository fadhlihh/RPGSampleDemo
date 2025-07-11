using System.Collections;
using UnityEngine;

public class SpellWeapon : Weapon
{
    [SerializeField]
    private float _distance;
    [SerializeField]
    private GameObject _spellProjectilePrefabs;
    [SerializeField]
    private Transform _spellProjectileSpawner;

    public float Distance { get => _distance; }
    public override EWeaponType Type => EWeaponType.Spell;

    public override void HeavyAttack()
    {
        StartCoroutine(OnFireSpell());
    }

    public IEnumerator OnFireSpell()
    {
        yield return new WaitForSeconds(.28f);
        if (_spellProjectilePrefabs)
        {
            GameObject spellObject = Instantiate(_spellProjectilePrefabs, _spellProjectileSpawner.position, _spellProjectileSpawner.rotation);
            ProjectileSpell projectileSpell = spellObject.GetComponent<ProjectileSpell>();
            projectileSpell.Launch(spellObject.transform.forward, _distance, 30);
        }
    }
}
