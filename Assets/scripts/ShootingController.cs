using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {

    public Player player;
    public ParticleSystem leftParticles;
    public ParticleSystem rightParticles;
    public Animator animator;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform eyesCamera;
    public GameObject bulletPrefab;
    public AudioSource sourceLeft;
    public AudioSource sourceRight;
    public AudioClip[] sounds;

    private int state = 1;

    private int shots;
    private int crits;

    private void Start() {
        leftParticles.Play();
        rightParticles.Play();
    }

    public void shootLeft() {
        leftParticles.Emit(500);
        for (int i = 0; i < player.Bullets; i++) {
            CreateBullet(leftPoint);
        }
        sourceLeft.clip = sounds[Mathf.FloorToInt(Random.Range(0, sounds.Length))];
        sourceLeft.Play();
    }

    public void shootRight() {
        rightParticles.Emit(500);
        for (int i = 0; i < player.Bullets; i++) {
            CreateBullet(rightPoint);
        }
        sourceRight.clip = sounds[Mathf.FloorToInt(Random.Range(0, sounds.Length))];
        sourceRight.Play();

    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {

            if (Random.value <= player.CritChance) {
                animator.SetInteger("state", 3);
                return;
            }

            if (state == 2) {
                animator.SetInteger("state", 1);
                state = 1;
            }
            else {
                animator.SetInteger("state", 2);
                state = 2;
            }

        }
    }

    public void ComputeNextState() {
        if (Input.GetButton("Fire1")) {

            if(Random.value <= player.CritChance) {
                animator.SetInteger("state", 3);
                shots++;
                crits++;
                return;
            }

            if(state == 2 ) {
                animator.SetInteger("state", 1);
                state = 1;
            }
            else {
                animator.SetInteger("state", 2);
                state = 2;
            }

            shots++;
        }
        else {
            animator.SetInteger("state", 0);
        }
    }

    private void CreateBullet(Transform origin) {
        var bullet = Instantiate(bulletPrefab, origin.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Damage = player.Damage;

        var spread = Random.insideUnitSphere * player.Spread;
        spread.z = 0.0f;
        spread = eyesCamera.TransformDirection(spread);

        bullet.GetComponent<Rigidbody>().velocity = 
            Quaternion.Euler(spread) 
            * eyesCamera.forward * player.BulletsSpeed * Random.Range(0.8f, 1.1f);
    }

}