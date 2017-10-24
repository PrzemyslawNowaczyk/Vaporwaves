using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {

    public DropChance[] DropTable;

    public abstract float HP {
        get; set;
    }

    public abstract void Init(int points);

    public abstract void TakeDamage(float damage);

    public abstract void Die();

}