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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setNewPlayer() {
        PlayerInfo.PlayerTroops[0] = miner;
        PlayerInfo.PlayerTroops[1] = fighter;
        PlayerInfo.PlayerTroops[2] = spear;
        PlayerInfo.PlayerTroops[3] = null;
        PlayerInfo.PlayerTroops[4] = null;
        PlayerPrefs.SetInt("CompletedLevels", 0);
        PlayerPrefs.SetInt("UnlockedTroops", 2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void setFinishedPlayer()
    {
        PlayerInfo.PlayerTroops[0] = miner;
        PlayerInfo.PlayerTroops[1] = soldier;
        PlayerInfo.PlayerTroops[2] = archer;
        PlayerInfo.PlayerTroops[3] = rage;
        PlayerInfo.PlayerTroops[4] = kill;
        PlayerPrefs.SetInt("CompletedLevels", 10);
        PlayerPrefs.SetInt("UnlockedTroops", 10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
