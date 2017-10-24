using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : EnemyBase {

    public Transform ParticlesTransform;
    public override float HP {
        get; set;
    }

    public override void Die() {

        ParticlesTransform.SetParent(null, true);
        ParticlesTransform.gameObject.SetActive(true);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
        ParticlesTransform.GetComponent<ParticleSystem>().GetParticles(particles);
        particles[0].velocity = GetComponent<Rigidbody>().velocity;
        particles = null;
        Destroy(ParticlesTransform.gameObject, 10.0f);

        Destroy(gameObject);
    }

    private void DropLoot() {
        for (int i = 0; i < DropTable.Length; i++) {
            if (UnityEngine.Random.value < DropTable[i].DropChancePercent / 100.0f) {
                var drop = Instantiate(DropTable[i].DropPrefab, transform.position, Quaternion.identity);
                drop.GetComponent<Rigidbody>().velocity = UnityEngine.Random.insideUnitSphere * 5.0f;
                DropChance.ResetMultiplier();
            }
        }
    }

    public override void Init(int points) {
        Destroy(gameObject, 35.0f);
    }

    public override void TakeDamage(float damage) {
        DropLoot();
        Die();
    }

    private void OnCollisionEnter(Collision collision) {
        var player = collision.gameObject.GetComponent<Player>();
        if (player) {
            player.TakeDamage(1);
            player.GetComponent<Rigidbody>().AddForce(transform.up * 20.0f, ForceMode.VelocityChange);
        }
        Die();
    }
}
