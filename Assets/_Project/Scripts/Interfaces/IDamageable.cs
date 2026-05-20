using UnityEngine;

namespace Lastlight.Interfaces
{
    public interface IDamageable
    {
        bool IsAlive { get; }
        void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
    }
}
