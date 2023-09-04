using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemySpawnWaves
{
    public float timeToSendWave;
    public List<Unit> Wave = new List<Unit>();
}

public class EnemyWaveAI : MonoBehaviour
{
    public TeamInfo controlledTeam;

    public EnemySpawnWaves[] enemyWaves;
    [SerializeField] private float lastWaveSendTime;
    private int currentWaveIndex = 0;

    public EnemySpawnWaves spamUnits;   //Will spam player with archers, constantly
    private float lastSpamSpawnTime;
    private Vector3 spamSpawnPoint;
    private List<Unit> lastSpamWave = new List<Unit>();
    //private float timeToChargeSpam;
    //[SerializeField] private float timeBetweenSpam;
    

    void Start()
    {
        controlledTeam.gold = 10000;    //For now I want them to just be spawning the exact waves at the exact times
        lastWaveSendTime = Time.time;
        lastSpamSpawnTime = Time.time;
        controlledTeam.SetRallyPoint(controlledTeam.barracks.transform.position.x + (10 * controlledTeam.Team));

        TrainWave(enemyWaves[currentWaveIndex].Wave);

        spamSpawnPoint = new Vector3(controlledTeam.barracks.transform.position.x + (5 * controlledTeam.Team), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.gameState != GameState.InGame) {
            lastWaveSendTime += Time.deltaTime;
            lastSpamSpawnTime += Time.deltaTime;
            return;
        }
        SpawnWaves();

        if (lastSpamSpawnTime + spamUnits.timeToSendWave < Time.time)
        {
            StartCoroutine(Barrage());
        }

        
    }

    private void SpawnWaves() {
        if (lastWaveSendTime + enemyWaves[currentWaveIndex].timeToSendWave < Time.time) {
            //Send the wave then spawn a new one
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 15f);
            foreach (Collider2D collider in colliders)
            {
                Soldier soldier = collider.GetComponent<Soldier>();
                if (soldier != null && soldier.Team == controlledTeam.Team)
                {

                    soldier.ReceiveGeneralOrders();
                }

            }

            if (currentWaveIndex < enemyWaves.Length - 1) {
                currentWaveIndex++;
                TrainWave(enemyWaves[currentWaveIndex].Wave);
            }

            lastWaveSendTime = Time.time;
        }
    }

    private void TrainWave(List<Unit> Wave) {
        foreach (Unit unit in Wave) {
            controlledTeam.TrainUnit(unit);
        }
    }

    IEnumerator Barrage() {
        
        lastSpamSpawnTime = Time.time;
        //timeToChargeSpam = .15f;

        foreach (Unit unit in spamUnits.Wave) { 
            Unit spamUnit = Instantiate(unit, spamSpawnPoint, Quaternion.identity).GetComponent<Unit>();
            
            controlledTeam.troopCount += spamUnit.TroopSpaces;
            controlledTeam.troopCategory[spamUnit.unitClassification]++;

            if (controlledTeam.Team < 0)
            {
                spamUnit.transform.localScale = new Vector3(-1, 1, 1);
                //archerSpam.transform.Rotate(new Vector3(0, 180, 0)); //Perhaps redundant now given changes to Unit class
            }


            spamUnit.name = "Spam " + spamUnit.name;
            spamUnit.General = controlledTeam;
            spamUnit.Team = controlledTeam.Team;

            //Appies buffs/debuffs
            spamUnit.HP *= controlledTeam.advantage;
            spamUnit.Damage *= controlledTeam.advantage;

            //timeToChargeSpam += spamUnit.SpawnTime;

            yield return new WaitForSeconds(1f);
            lastSpamWave.Add(spamUnit);
        }

        OrderSpam();
        
    }

    private void OrderSpam() {
        foreach (Soldier soldier in lastSpamWave) {
            Debug.Log("Ordering spam forward");
            soldier.ReceiveGeneralOrders();
        }

        lastSpamWave.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
