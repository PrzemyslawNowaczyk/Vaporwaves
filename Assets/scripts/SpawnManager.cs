using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public Transform[] Spawns;
    public Transform[] GunSpawns;
    public List<EnemyBase> enemies = new List<EnemyBase>();

    public GameObject EnemyPrefab1;
    public GameObject EnemyPrefab2;
    public GameObject EnemyGunPrefab;

    int difficulty = 2;

    private void Update() {

        for (int i = enemies.Count - 1; i >= 0; i--) {
            if(enemies[i] == null) {
                enemies.RemoveAt(i);
            }
        }

        var guns = Random.Range(0, difficulty);
        

        if (enemies.Count == 0) {
            var gunSlotsTaken = 0;
            difficulty++;
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            WaveChanged.Invoke(difficulty-2);
            var random01 = Random.value;
            //flying round
            if (random01 < 0.2f) {
                foreach (var spwn in Spawns) {
                    for (int i = 0; i < difficulty / 2; i++) {
                        var enem = Instantiate(EnemyPrefab1, spwn.position + Vector3.up * difficulty * 2.0f, Quaternion.identity);
                        var enemComp = enem.GetComponent<EnemyBase>();
                        enemComp.Init(difficulty);
                        enemies.Add(enemComp);
                    }
                }
            }
            //ground round
            else if (random01 > 0.5f && random01 < 0.8f) {
                foreach (var spwn in Spawns) {
                    for (int i = 0; i < difficulty / 2; i++) {
                        var enem = Instantiate(EnemyPrefab2, spwn.position, Quaternion.identity);
                        var enemComp = enem.GetComponent<EnemyBase>();
                        enemComp.Init(difficulty);
                        enemies.Add(enemComp);
                    }
                }
            }
            //mixed round
            else if (random01 >= 0.8f && random01 < 0.95f) {
                var ratio = Random.value;
                foreach (var spwn in Spawns) {
                    for (int i = 0; i < difficulty / 2; i++) {
                        GameObject enem;
                        if (Random.value < ratio) {
                            enem = Instantiate(EnemyPrefab1, spwn.position + Vector3.up * difficulty * 2.0f, Quaternion.identity);
                        }
                        else {
                            enem = Instantiate(EnemyPrefab2, spwn.position + Vector3.up * difficulty * 2.0f, Quaternion.identity);
                        }
                        var enemComp = enem.GetComponent<EnemyBase>();
                        enemComp.Init(difficulty);
                        enemies.Add(enemComp);
                    }
                }
            }
            //gun round
            else {
                for (int i = 0; i < difficulty; i++) {
                    if(GunSpawns.Length > i) {
                        var enem = Instantiate(EnemyGunPrefab, GunSpawns[i].position, GunSpawns[i].localRotation);
                        var enemComp = enem.GetComponent<EnemyBase>();
                        enemComp.Init(difficulty);
                        enemies.Add(enemComp);
                        gunSlotsTaken++;
                    }
                }
            }

            for (int i = gunSlotsTaken; i < guns && i < GunSpawns.Length; i++) {
                    var enem = Instantiate(EnemyGunPrefab, GunSpawns[i].position, GunSpawns[i].localRotation);
                    var enemComp = enem.GetComponent<EnemyBase>();
                    enemComp.Init(difficulty);
                    enemies.Add(enemComp);

            }

        }
    }

    public ValueChangeEvent WaveChanged;
}
