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

    public GameObject archerUnit;   //Will spam player with archers, constantly
    [SerializeField] private float lastArcherSpawnTime;
    private Vector3 archerSpawnPoint;
    private Soldier lastArcher;
    [SerializeField] private float timeBetweenArchers;
    

    void Start()
    {
        controlledTeam.gold = 10000;    //For now I want them to just be spawning the exact waves at the exact times
        lastWaveSendTime = Time.time;
        lastArcherSpawnTime = Time.time;
        controlledTeam.SetRallyPoint(controlledTeam.barracks.transform.position.x + (10 * controlledTeam.Team));

        TrainWave(enemyWaves[currentWaveIndex].Wave);

        archerSpawnPoint = new Vector3(controlledTeam.barracks.transform.position.x + (5 * controlledTeam.Team), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.gameState != GameState.InGame) {
            lastWaveSendTime += Time.deltaTime;
            lastArcherSpawnTime += Time.deltaTime;
            return;
        }
        SpawnWaves();

        ArcherBarrage();


        
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

    private void ArcherBarrage() {
        if (lastArcherSpawnTime + timeBetweenArchers < Time.time) {


            Unit archerSpam = Instantiate(archerUnit, archerSpawnPoint, Quaternion.identity).GetComponent<Unit>();
            
            controlledTeam.troopCount += archerSpam.TroopSpaces;
            controlledTeam.troopCategory[archerSpam.unitClassification]++;

            if (controlledTeam.Team < 0)
            {
                archerSpam.transform.localScale = new Vector3(-1, 1, 1);
                //archerSpam.transform.Rotate(new Vector3(0, 180, 0)); //Perhaps redundant now given changes to Unit class
            }

           
            archerSpam.name = "Spam Archer";
            archerSpam.General = controlledTeam;
            archerSpam.Team = controlledTeam.Team;

            //Appies buffs/debuffs
            archerSpam.HP *= controlledTeam.advantage;
            archerSpam.Damage *= controlledTeam.advantage;

            lastArcher = archerSpam.GetComponent<Soldier>();
            Invoke(nameof(OrderArcher), .15f);

            lastArcherSpawnTime = Time.time;
        }
    }

    private void OrderArcher() {
        lastArcher.ReceiveGeneralOrders();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
