using UnityEngine;

namespace Lastlight.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Lastlight/Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private string weaponName;
        [SerializeField] private float damage = 25f;
        [SerializeField] private float fireRate = 5f;
        [SerializeField] private float range = 50f;
        [SerializeField] private int maxAmmo = 12;
        [SerializeField] private float reloadTime = 1.5f;
        [SerializeField] private bool isAutomatic;

        [Header("Effects")]
        [SerializeField] private GameObject muzzleFlashPrefab;
        [SerializeField] private GameObject impactEffectPrefab;

        [Header("Audio")]
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioClip reloadSound;
        [SerializeField] private AudioClip emptySound;

        public string WeaponName        => weaponName;
        public float  Damage            => damage;
        public float  FireRate          => fireRate;
        public float  Range             => range;
        public int    MaxAmmo           => maxAmmo;
        public float  ReloadTime        => reloadTime;
        public bool   IsAutomatic       => isAutomatic;
        public GameObject MuzzleFlashPrefab   => muzzleFlashPrefab;
        public GameObject ImpactEffectPrefab  => impactEffectPrefab;
        public AudioClip  FireSound     => fireSound;
        public AudioClip  ReloadSound   => reloadSound;
        public AudioClip  EmptySound    => emptySound;
    }
}
