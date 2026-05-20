using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Lastlight.Interfaces;

namespace Lastlight.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private WeaponData weaponData;

        [Header("References")]
        [SerializeField] private Transform muzzlePoint;
        [SerializeField] private Camera fpsCamera;

        // Input Actions (Project-wide Actions)
        private InputAction attackAction;
        private InputAction reloadAction;

        // Components
        private AudioSource audioSource;

        // Fire rate tracking
        private float lastFireTime;

        // Public state (UI 연동용)
        public int  CurrentAmmo { get; private set; }
        public bool IsReloading { get; private set; }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            if (fpsCamera == null)
                fpsCamera = Camera.main;

            attackAction = InputSystem.actions.FindAction("Attack");
            reloadAction = InputSystem.actions.FindAction("Reload");

            if (attackAction == null)
                Debug.LogWarning("[WeaponController] 'Attack' 액션을 Input Actions에서 찾을 수 없습니다.");
            if (reloadAction == null)
                Debug.LogWarning("[WeaponController] 'Reload' 액션을 Input Actions에서 찾을 수 없습니다.");
        }

        private void Start()
        {
            if (weaponData != null)
                CurrentAmmo = weaponData.MaxAmmo;
        }

        private void OnEnable()
        {
            if (attackAction != null)
                attackAction.performed += OnAttackPerformed;
            if (reloadAction != null)
                reloadAction.performed += OnReloadPerformed;
        }

        private void OnDisable()
        {
            if (attackAction != null)
                attackAction.performed -= OnAttackPerformed;
            if (reloadAction != null)
                reloadAction.performed -= OnReloadPerformed;
        }

        private void Update()
        {
            if (weaponData == null || IsReloading) return;

            // 자동 발사: 버튼을 누르고 있는 동안 연속 발사
            if (weaponData.IsAutomatic && attackAction != null && attackAction.IsPressed())
                TryFire();
        }

        // ────────────── Input Callbacks ──────────────

        private void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            // 단발 모드만 이벤트로 처리 (자동 발사는 Update에서 처리)
            if (weaponData != null && !weaponData.IsAutomatic)
                TryFire();
        }

        private void OnReloadPerformed(InputAction.CallbackContext ctx) => TryReload();

        // ────────────── Weapon Logic ──────────────

        private void TryFire()
        {
            if (weaponData == null || IsReloading) return;

            if (CurrentAmmo <= 0)
            {
                PlaySound(weaponData.EmptySound);
                return;
            }

            if (Time.time - lastFireTime < 1f / weaponData.FireRate) return;

            lastFireTime = Time.time;
            CurrentAmmo--;

            FireRaycast();
            SpawnMuzzleFlash();
            PlaySound(weaponData.FireSound);
        }

        private void FireRaycast()
        {
            if (fpsCamera == null) return;

            Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hit, weaponData.Range)) return;

            SpawnImpactEffect(hit);

            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(weaponData.Damage, hit.point, hit.normal);
        }

        private void TryReload()
        {
            if (weaponData == null || IsReloading || CurrentAmmo == weaponData.MaxAmmo) return;
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            IsReloading = true;
            PlaySound(weaponData.ReloadSound);
            yield return new WaitForSeconds(weaponData.ReloadTime);
            CurrentAmmo = weaponData.MaxAmmo;
            IsReloading = false;
        }

        private void SpawnMuzzleFlash()
        {
            if (weaponData.MuzzleFlashPrefab == null || muzzlePoint == null) return;

            GameObject flash = Instantiate(
                weaponData.MuzzleFlashPrefab,
                muzzlePoint.position,
                muzzlePoint.rotation
            );
            Destroy(flash, 0.1f);
        }

        private void SpawnImpactEffect(RaycastHit hit)
        {
            if (weaponData.ImpactEffectPrefab == null) return;

            GameObject impact = Instantiate(
                weaponData.ImpactEffectPrefab,
                hit.point,
                Quaternion.LookRotation(hit.normal)
            );
            Destroy(impact, 2f);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip == null) return;
            audioSource.PlayOneShot(clip);
        }
    }
}
