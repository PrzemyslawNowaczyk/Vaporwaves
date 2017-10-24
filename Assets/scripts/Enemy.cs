using System;
using UnityEngine;

public class Enemy : EnemyBase {

    public int Damage = 1;
    public float damageCooldown;
    private AudioSource _audio;

    public override float HP {
        get { return _hp; }
        set { _hp = value; }
    }

    float _hp;
    PursuePlayer pursue;
    Player player;

    float damageTimer;

    bool inCollisionWithPlayer;

    private void Awake() {
        _audio = GetComponent<AudioSource>();
    }

    public override void Init(int points) {
        pursue = GetComponentInChildren<PursuePlayer>();
        player = FindObjectOfType<Player>();

        var speed = UnityEngine.Random.Range(0, points);
        var strength = points - speed;
        HP += (points / 10) + strength * 1.5f;
        pursue.Speed = Mathf.Min(22.0f, 4 + (points / 4) + 2 * speed);
        transform.localScale = Vector3.one * HP;
        _audio.pitch = 1.0f + (15.0f - HP) * 0.05f;
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            inCollisionWithPlayer = true;
        }
    }

    public override void TakeDamage(float damage) {
        HP -= damage;

        if(HP <= 0) {
            Die();
        }
    }

    public override void Die() {
        for (int i = 0; i < DropTable.Length; i++) {
            if(UnityEngine.Random.value < DropTable[i].DropChancePercent / 100.0f) {
                var drop = Instantiate(DropTable[i].DropPrefab, transform.position, Quaternion.identity);
                drop.GetComponent<Rigidbody>().velocity = UnityEngine.Random.insideUnitSphere * 5.0f;
                DropChance.ResetMultiplier();
            }
        }

        var particle = GetComponentInChildren<ParticleSystem>();
        if (particle) {
            particle.transform.SetParent(null, true);
            particle.Emit(100);
            GameObject.Destroy(particle.gameObject, 2.0f);
        }
        Destroy(gameObject, 0.1f);
    }

    private void Update() {
        if(transform.position.y < 0.0f) {
            transform.position += Vector3.up * 50.0f;
        }
    }

    private void FixedUpdate() {

        if (inCollisionWithPlayer) {
            if (damageTimer <= 0) {
                player.TakeDamage(Damage);
                damageTimer = damageCooldown;
            }
            else {
                damageTimer -= Time.fixedDeltaTime;
            }
        }
  

        inCollisionWithPlayer = false;
    }

}
