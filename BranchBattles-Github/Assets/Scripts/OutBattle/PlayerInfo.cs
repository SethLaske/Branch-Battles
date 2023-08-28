using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo 
{
    public static Dictionary<int, bool> LevelKeys = new Dictionary<int, bool>();
    public static Dictionary<int, bool> TroopKeys = new Dictionary<int, bool>();
    public static Dictionary<string, bool> PopUpKeys = new Dictionary<string, bool>();
    public static int TroopSpaces;

    public static Unit[] PlayerTroops = new Unit[5];
    //public static Unit PlayerPacifist1;
    //public static Unit PlayerSoldier1;
    //public static Unit PlayerSoldier2;
    //public static Unit PlayerSoldier3;
    //public static Unit PlayerSoldier4;

    public static void ClearPlayerInfo()
    {
        InitPlayerInfo();
        LevelKeys.Clear();
        TroopKeys.Clear();
        PopUpKeys.Clear();
        TroopSpaces = 0;
        PlayerTroops = new Unit[5];
    }

    public static void InitPlayerInfo() {
        LevelKeys = new Dictionary<int, bool>();
        TroopKeys = new Dictionary<int, bool>();
        PopUpKeys = new Dictionary<string, bool>();
        PlayerTroops = new Unit[5];
    }
   
}
