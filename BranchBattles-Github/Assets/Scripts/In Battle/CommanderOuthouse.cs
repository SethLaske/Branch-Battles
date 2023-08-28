using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderOuthouse : Building
{
    [SerializeField] private Commander resident;
    [SerializeField] private Building nextBarracks;

    [SerializeField] private Magic spawnMagic;
    [SerializeField] private float magicSpawnPosition;

    [SerializeField] private Unit[] finalEnemies;
    [SerializeField] private float spaceToFirstEnemy;
    [SerializeField] private float spaceBetweenEnemies;

    [SerializeField] private float timeBeforeCommanderSpawns;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnMagic != null)
        {
            spawnMagic = Instantiate(spawnMagic, Vector3.zero, Quaternion.identity);
            spawnMagic.SetTeamInfo(General);
            spawnMagic.gameObject.SetActive(false);
        }

        Team = General.Team;
        resident.gameObject.SetActive(false);
    }

    public override void Die()
    {
        StartCoroutine(CommanderSpawnRoutine());
    }

    IEnumerator CommanderSpawnRoutine() {
        //Throw down two lightning spells
        if (spawnMagic != null) {
            //spawnMagic.TriggerMagic();
            spawnMagic.ActivateMagic(magicSpawnPosition);

            spawnMagic = Instantiate(spawnMagic, Vector3.zero, Quaternion.identity);
            spawnMagic.SetTeamInfo(General);
            spawnMagic.gameObject.SetActive(false);

            spawnMagic.ActivateMagic(magicSpawnPosition);
            spawnMagic = Instantiate(spawnMagic, Vector3.zero, Quaternion.identity);
            spawnMagic.SetTeamInfo(General);
            spawnMagic.gameObject.SetActive(false);

            spawnMagic.ActivateMagic(magicSpawnPosition);
        }
        yield return new WaitForSeconds(5);

        //Spawn a wave of enemies 
        float spawnPosition = transform.position.x + (-1 * Team * spaceToFirstEnemy);
        foreach (Unit unit in finalEnemies) {
            General.ForceSpawnUnit(unit, new Vector3(spawnPosition, transform.position.y, transform.position.z));
            spawnPosition += (-1 * Team * spaceBetweenEnemies);
        }

        //Activate the commander
        yield return new WaitForSeconds(timeBeforeCommanderSpawns);
        resident.enabled = true;
        resident.gameObject.SetActive(true);
        General.maxTroopCount = 0;

        General.barracks = nextBarracks;

        base.Die();
    }


}
