using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    //unit stats
    public int move=5;
    public int wounds = 1;
    public int save = 3;
    public int bravery = 5;
    public int unitMaxSize = 10;


    //melee stats
    public int melee = 2;
    public int meleeattacks=1;
    public int meleetohit = 1;
    public int meleetowound = 1;
    public int meleerend = 1;
    public int meleedamage = 1;

    //Ranged Stats
    public int ranged =10;
    public int rangedattacks = 1;
    public int rangedtohit = 1;
    public int rangedtowound = 1;
    public int rangedrend = 1;
    public int rangeddamage = 1;
    

    
}
