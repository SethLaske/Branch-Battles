using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo 
{
    public static Dictionary<int, bool> LevelKeys = new Dictionary<int, bool>();
    public static Dictionary<int, bool> TroopKeys = new Dictionary<int, bool>();
    public static int TroopSpaces;

    public static Unit[] PlayerTroops = new Unit[5];
    //public static Unit PlayerPacifist1;
    //public static Unit PlayerSoldier1;
    //public static Unit PlayerSoldier2;
    //public static Unit PlayerSoldier3;
    //public static Unit PlayerSoldier4;

    public static void ClearPlayerInfo()
    {
        LevelKeys.Clear();
        TroopKeys.Clear();
        TroopSpaces = 0;
        PlayerTroops = new Unit[5];
    }
   
}
