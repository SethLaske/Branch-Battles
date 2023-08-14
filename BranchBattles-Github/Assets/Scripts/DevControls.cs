using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DevControls : MonoBehaviour
{
    public Unit miner;
    public Unit fighter;
    public Unit spear;
    public Unit soldier;
    public Unit archer;
    public Unit rage;
    public Unit kill;
    // Start is called before the first frame update

    public void GoToLevel(string level) {
        SceneManager.LoadScene(level);
    }

    public void SetNewPlayer() {
        PlayerInfo.ClearPlayerInfo();
        for (int i = -1; i < 11; i++)
        {
            PlayerInfo.LevelKeys.Add(i, false);
            PlayerInfo.TroopKeys.Add(i, false);
        }
        PlayerInfo.LevelKeys[0] = true;
        PlayerInfo.TroopSpaces = 2;

        PlayerInfo.PlayerTroops[0] = miner;
        PlayerInfo.PlayerTroops[1] = fighter;
        PlayerInfo.PlayerTroops[2] = spear;
        PlayerInfo.PlayerTroops[3] = null;
        PlayerInfo.PlayerTroops[4] = null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        
    }

    

    public void SetFinishedPlayer()
    {
        PlayerInfo.ClearPlayerInfo();
        for (int i = -1; i < 11; i++)
        {
            PlayerInfo.LevelKeys.Add(i, true);
            PlayerInfo.TroopKeys.Add(i, true);
        }
        //PlayerInfo.LevelKeys[-1] = true;
        PlayerInfo.TroopSpaces = 4;

        PlayerInfo.PlayerTroops[0] = miner;
        PlayerInfo.PlayerTroops[1] = fighter;
        PlayerInfo.PlayerTroops[2] = spear;
        PlayerInfo.PlayerTroops[3] = rage;
        PlayerInfo.PlayerTroops[4] = archer;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
