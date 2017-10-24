using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : EnemyBase {

    public float FollowingSpeed;
    [Range(0.05f, 20.0f)]
    public float TimeBetweenShots;
    public float BulletSpeed;
    public GameObject BulletPrefab;
    public Transform Pivot;
    public Transform GunPoint;

    private Transform target;
    private float hp = 5.0f;
    private float timeSpent;

    public override float HP {
        get {
            return hp;
        }

        set {
            hp = value;
        }
    }

    private void Awake() {
        target = FindObjectOfType<Player>().transform;
        StartCoroutine(ShotCycle());
    }

    public override void Init(int points) {
        FollowingSpeed = 1.5f + points * 1.0f;
        BulletSpeed = 30 + points * 3;
        TimeBetweenShots = Mathf.Max(3.0f - points * 0.2f, 1.0f);
        transform.localScale = Vector3.one * (1.0f + points / 3.0f);
        timeSpent = UnityEngine.Random.Range(TimeBetweenShots/2.0f, TimeBetweenShots);
    }

    public override void TakeDamage(float damage) {
        hp -= damage;

        if(hp <= 0.0f) {
            Die();
        }
    }

    public override void Die() {
        for (int i = 0; i < DropTable.Length; i++) {
            if (UnityEngine.Random.value < DropTable[i].DropChancePercent / 100.0f) {
                var drop = Instantiate(DropTable[i].DropPrefab, Pivot.transform.position, Quaternion.identity);
                drop.GetComponent<Rigidbody>().velocity = UnityEngine.Random.insideUnitSphere * 5.0f;
                DropChance.ResetMultiplier();
            }
        }

        var particle = GetComponentInChildren<ParticleSystem>();
        if (particle) {
            particle.transform.SetParent(null, true);
            particle.transform.position = Pivot.transform.position;
            particle.transform.rotation = Pivot.transform.rotation;
            particle.transform.localScale = Pivot.transform.lossyScale;
            particle.Emit(100);
            GameObject.Destroy(particle.gameObject, 2.0f);
        }
        Destroy(gameObject, 0.1f);
    }

    IEnumerator ShotCycle() {
        while (true) {
            while (timeSpent < TimeBetweenShots) {
                timeSpent += Time.deltaTime;
                FollowTarget();
                yield return null;
            }
            Shot();
            timeSpent = 0.0f;
        }
        
    }

    private void Shot() {
        var bullet = Instantiate(BulletPrefab, GunPoint.transform.position, GunPoint.transform.rotation);
        bullet.transform.localScale = transform.localScale;
        bullet.GetComponent<Rigidbody>().velocity = Pivot.transform.forward * BulletSpeed;
    }

    private void FollowTarget() {
        var lookAtTarget = Quaternion.LookRotation(((target.transform.position + Vector3.up) - Pivot.transform.position).normalized, Vector3.up);
        var currentRotation = Pivot.rotation;
        Pivot.rotation = Quaternion.RotateTowards(currentRotation, lookAtTarget, FollowingSpeed * Time.deltaTime);
    }
}