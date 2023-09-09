using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Not a great class, but handles game overs for now, and will be fine unless a team can have multiple barracks
public class TeamBase : Building
{
    private CameraControls playercamera;
    private LevelManager levelmanager;

    [SerializeField] private List<Unit> reinforcements;
    [SerializeField] private float distanceToSpawnReinforcements;

    void Start()
    {
        Team = General.Team;
        maxHealth = HP;
        playercamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControls>();
        levelmanager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
    }

    
    //Could also call it result screen and condense it to one screen per base
    public override void Die()
    {
        base.Die(); 
        General.Defeat();
        General.Opponent.Victory();
        levelmanager.GameOver(Team);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (Team == 1) {
            float amount = (.3f * ((maxHealth - HP) / maxHealth)) + .1f;
            playercamera.CallShake(amount, amount);
        }

        if (HP <= maxHealth / 2) {

            while (reinforcements.Count > 0) {
                General.ForceSpawnUnit(reinforcements[0], new Vector3(transform.position.x - (Team * distanceToSpawnReinforcements), transform.position.y));
                distanceToSpawnReinforcements += 2;
                reinforcements.RemoveAt(0);
            }
        }
        
    }


}
