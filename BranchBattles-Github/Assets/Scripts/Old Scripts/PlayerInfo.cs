using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Player info will be referenced by the various objects like checking for gold/resources, before allowing them. Enemy Info will be almost identical
public class PlayerInfo : MonoBehaviour
{
    public int Gold;
    public int TroopCount;
    public int TroopMax;

    public int AFKGoldAmount;
    public float AFKGoldTime;
    public float counter = 0;

    public Building Barracks;

    public Unit Unit1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= AFKGoldTime) {
            Gold += AFKGoldAmount;
            counter = 0;
        }
    }

    public void spawnUnit1() {
        if (Gold >= Unit1.Cost && (TroopCount + Unit1.TroopSpaces <= TroopMax))
        {
            Gold -= Unit1.Cost;
            TroopCount += TroopMax;
            Unit newEnemy = Instantiate(Unit1, new Vector3(Barracks.transform.position.x, -2.25f, 0), Quaternion.identity);

            newEnemy.Team = Barracks.Team;
        }
        
        
    }
}
