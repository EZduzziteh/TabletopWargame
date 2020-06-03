using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isvsAI=false;
    public List<Unit> player1Units = new List<Unit>();
    public List<Unit> player2Units = new List<Unit>();
    public List<Unit> player1ActivatedUnits = new List<Unit>();
    public List<Unit> player2ActivatedUnits = new List<Unit>();
    
    public PlayerIndex priorityplayer;

    public GameObject UnitCardWindow;
    public GameObject SelectedUnitWindow;
    public Text spelltext;
    public Text unitnametex;
    public Text meleedist;
   
    public Text meleetohit;
    public Text meleetowound;
    public Text meleeattacks;
    public Text meleerend;
    public Text meleedamage;
    public Text rangedattacks;
    public Text rangeddist;
    public Text rangedtohit;
    public Text rangedtowound;
    public Text rangedrend;
    public Text rangeddamage;
    public Text health;
    public Text save;

   

    public int P1UnitCount=0;
    public int P2UnitCount=0;

    public Camera tacticalCamera;
    public Camera cinematicCamera;
    public Text turnText;
    public Text selectedText;
    public Text errorText;
    public Text phasetext;
    public  float closestdist;
    public Unit closetunit;
    public Model closestmodel;

    public List<SpawnPoint> p1spawnpoints=new List<SpawnPoint>();
    public List<SpawnPoint> p2spawnpoints = new List<SpawnPoint>();

    public enum BattlePhase { Order, Casting, Move, Shoot, Charge, Combat, Rally };

    public enum PlayerIndex { P1,P2/*,P3,P4*/};

    public Unit SelectedUnit;
    public Unit SelectedTarget;

    public ArmyConstructor armycon;
    public BattlePhase currentPhase = BattlePhase.Casting;
    public PlayerIndex pTurn = PlayerIndex.P1;

    // Start is called before the first frame update
    void Start()
    {
        armycon = FindObjectOfType<ArmyConstructor>();
        armycon.SpawnArmy();

        foreach( Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.player == PlayerIndex.P1)
            {
                unit.transform.position = p1spawnpoints[P1UnitCount].transform.position;
                P1UnitCount++;
                
             
                
            }
            else
            {
                unit.transform.position = p2spawnpoints[P2UnitCount].transform.position;
                P2UnitCount++;
            }
        }
        CreateActivationPool();
        StartTurn();
        RefreshUI();
    }

    public void RemoveFromActivations(Unit unit)
    {
        if (unit.player == PlayerIndex.P1)
        {
            player1ActivatedUnits.Remove(unit);
            player1Units.Remove(unit);
        }
        else
        {
            player2ActivatedUnits.Remove(unit);
            player2Units.Remove(unit);
        }
    }
    private void Update()
    {

     
      
    }

    public void ShowSelectedAttackRange()
    {

        if (SelectedUnit)
        {
            if (SelectedUnit.canMelee)
            {
                SelectedUnit.SetAttackTool();
            }
            else
            {
                SelectedUnit.SetShootTool();
            }
        }
    }

    public float GetTargetedToSelected()
    {
        Debug.Log(Vector3.Distance(SelectedTarget.transform.position, SelectedUnit.transform.position));

        return Vector3.Distance(SelectedTarget.transform.position, SelectedUnit.transform.position);
    }
    public float GetPointToSelected(Vector3 pos)
    {
        Debug.Log(Vector3.Distance(pos, SelectedUnit.transform.position));

        return Vector3.Distance(pos, SelectedUnit.transform.position);
    }

    public void ToggleUnitCardWindow()
    {
        if (UnitCardWindow.activeInHierarchy)
        {
            UnitCardWindow.SetActive(false);
        }
        else
        {
            UnitCardWindow.SetActive(true);
        }
    }
    public void ToggleSelectedWindow()
    {
        if (SelectedUnitWindow.activeInHierarchy)
        {

            SelectedUnitWindow.SetActive(false);
        }
        else
        {
            SelectedUnitWindow.SetActive(true);
        }
    }


    public void GetClosestEnemyModelToPoint(Vector3 point, PlayerIndex turn)
    {
        closestmodel = null;
        closestdist = Mathf.Infinity;
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.player != turn)
            {
                if (unit.GetComponent<Model>())
                {
                    Debug.LogWarning(Vector3.Distance(unit.GetComponent<Model>().transform.position, point));
                    if (Mathf.Abs(Vector3.Distance(unit.GetComponent<Model>().transform.position, point)) < closestdist)
                    {
                        closestdist = Mathf.Abs(Vector3.Distance(unit.GetComponent<Model>().transform.position, point));
                        closestmodel = unit.GetComponent<Model>();


                    }
                }
                else {
                    foreach (Model model in unit.gameObject.GetComponentsInChildren<Model>())
                    {
                        Debug.LogWarning(Vector3.Distance(model.transform.position, point));
                        if (Mathf.Abs(Vector3.Distance(model.transform.position, point)) < closestdist)
                        {
                            closestdist = Mathf.Abs(Vector3.Distance(model.transform.position, point));
                            closestmodel = model;


                        }
                    }
                }
            }
        }
        /*if (turn == PlayerIndex.P1)
      //  {
            

            
            //old
            foreach (Unit unit in player2ActivatedUnits)
            {
                
                    foreach (Model model in unit.gameObject.GetComponentsInChildren<Model>())
                    {
                    Debug.LogWarning(Vector3.Distance(model.transform.position, point));
                        if (Mathf.Abs(Vector3.Distance(model.transform.position, point)) < closestdist)
                        {
                            closestdist = Mathf.Abs(Vector3.Distance(model.transform.position, point));
                            closestmodel = model;
                        
                          
                        }
                    }
            }
            foreach (Unit unit in player2Units)
            {
                foreach (Model model in unit.gameObject.GetComponentsInChildren<Model>())
                {
                    if (Mathf.Abs(Vector3.Distance(model.transform.position, point)) < closestdist)
                    {
                        closestdist = Mathf.Abs(Vector3.Distance(model.transform.position, point));
                        closestmodel = model;

                       
                    }
                }
            }
        }
        else
        {
            foreach (Unit unit in player1ActivatedUnits)
            {
                foreach (Model model in unit.gameObject.GetComponentsInChildren<Model>())
                {
                    if (Mathf.Abs(Vector3.Distance(model.transform.position, point)) < closestdist)
                    {
                        closestdist = Mathf.Abs(Vector3.Distance(model.transform.position, point));
                        closestmodel = model;

                      
                    }
                }
            }
            foreach (Unit unit in player1Units)
            {
                foreach (Model model in unit.gameObject.GetComponentsInChildren<Model>())
                {
                    if (Mathf.Abs(Vector3.Distance(model.transform.position, point)) < closestdist)
                    {
                        closestdist = Mathf.Abs(Vector3.Distance(model.transform.position, point));
                        closestmodel = model;

                       
                    }
                }
            }
        }*/

    }
    public void GetClosestEnemyUnitToPoint(Vector3 point, PlayerIndex turn)
    {

        closestdist = Mathf.Infinity;
        closetunit = null;
        
        if (turn == PlayerIndex.P1)
        {
            foreach (Unit unit in player2ActivatedUnits)
            {
                if(Mathf.Abs(Vector3.Distance(unit.transform.position, point))< closestdist)
                {
                    closestdist = Mathf.Abs(Vector3.Distance(unit.transform.position, point));
                    closetunit = unit;
                }
            }
            foreach (Unit unit in player2Units)
            {
                if (Mathf.Abs(Vector3.Distance(unit.transform.position, point)) < closestdist)
                {
                    closestdist = Mathf.Abs(Vector3.Distance(unit.transform.position, point));
                    closetunit = unit;
                }
            }
        }
        else
        {
            foreach (Unit unit in player1ActivatedUnits)
            {
                if (Mathf.Abs(Vector3.Distance(unit.transform.position, point)) < closestdist)
                {
                    closestdist = Mathf.Abs(Vector3.Distance(unit.transform.position, point));
                    closetunit = unit;
                }
            }
            foreach (Unit unit in player1Units)
            {
                if (Mathf.Abs(Vector3.Distance(unit.transform.position, point)) < closestdist)
                {
                    closestdist = Mathf.Abs(Vector3.Distance(unit.transform.position, point));
                    closetunit = unit;
                }
            }
        }

        Debug.Log(closetunit);
        Debug.Log(closestdist);
    }
    public void CreateActivationPool()
    {
       
        player1ActivatedUnits.Clear();
        player2ActivatedUnits.Clear();
        

        switch (currentPhase) 
        {
            
            case BattlePhase.Casting:
                errorText.text = "Casting phase, Left click a highlighted unit to cast a spell or ability.";
                foreach (Unit unit in FindObjectsOfType<Unit>())
                {
                    if (!unit.isDead){
                        if (unit.canCast)
                        {
                            unit.hasActivated = false;

                            if (unit.player == PlayerIndex.P1)
                            {
                                player1Units.Add(unit);

                            }
                            else
                            {
                                player2Units.Add(unit);
                            }
                            unit.SelectEffect.SetActive(true);
                        } }
                }
                if (player1Units.Count <= 0 && player2Units.Count <= 0)
                {
                    EndTurn();
                }
                break;
            case BattlePhase.Move:
                foreach (Unit unit in FindObjectsOfType<Unit>())
                {
                    unit.hasActivated = false;
                    unit.hasMoved = false;
                    if (!unit.isDead)
                    {
                        if (unit.player == PlayerIndex.P1)
                        {
                            player1Units.Add(unit);
                        }
                        else
                        {
                            player2Units.Add(unit);
                        }
                        unit.SelectEffect.SetActive(true);
                    }
                }
                errorText.text = "Move phase, Left click a highlighted unit to move.";
                break;
            case BattlePhase.Shoot:
                foreach (Unit unit in FindObjectsOfType<Unit>())
                {
                    unit.hasshot = false;
                    unit.hasActivated = false;
                    if (!unit.isDead)
                    {
                        if (unit.canShoot)
                        {

                            unit.GetClosestEnemyUnit();

                            if (unit.closestenemyunitdistance <= unit.stats.ranged)
                            {

                                if (unit.player == PlayerIndex.P1)
                                {
                                    player1Units.Add(unit);
                                }
                                else
                                {
                                    player2Units.Add(unit);
                                }
                                unit.SelectEffect.SetActive(true);
                            }
                        }
                    }
                    
                    errorText.text = "shoot phase, left click a highlighed unit to shoot.";
                }
                if (player1Units.Count <= 0 && player2Units.Count <= 0)
                {
                    EndTurn();
                }
                break;
            case BattlePhase.Charge:
                foreach (Unit unit in FindObjectsOfType<Unit>())
                {
                    unit.hasActivated = false;
                   // unit.canCharge = true;

                   


                    //dont add to charge pool if shot this turn
                    if (!unit.isDead)
                    {
                        if (!unit.hasshot)
                        {



                            if (unit.canCharge)
                            {
                                unit.GetClosestEnemyUnit();

                                if (unit.closestenemyunitdistance <= 12.0f && unit.closestenemyunitdistance >= 3.0f)//charge distance
                                {
                                    if (unit.player == PlayerIndex.P1)
                                    {


                                        player1Units.Add(unit);
                                    }
                                    else
                                    {


                                        player2Units.Add(unit);
                                    }
                                    unit.SelectEffect.SetActive(true);
                                }
                            }
                        }
                        
                    }

                    
                    closestdist = Mathf.Infinity;
                }
                errorText.text = "Charge phase, left click a highlighted unit to charge.";
                break;
            case BattlePhase.Combat:
                foreach (Unit unit in FindObjectsOfType<Unit>())
                {

                    unit.hasActivated = false;
                    if (!unit.isDead)
                    {
                        if (unit.canMelee)
                        {

                            unit.GetClosestEnemyUnit();

                            if (unit.closestenemyunitdistance <= unit.stats.melee)//meelee range
                            {
                                if (unit.player == PlayerIndex.P1)
                                {
                                    player1Units.Add(unit);
                                }
                                else
                                {
                                    player2Units.Add(unit);
                                }
                                unit.SelectEffect.SetActive(true);
                            }
                        }
                        }
                    
                }
                errorText.text = "Combat phase, Select a highlighted unit to fight.";
                break;
            case BattlePhase.Rally:
                foreach (Unit unit in FindObjectsOfType<Unit>())
                {
                    unit.hasActivated = false;

                    if (!unit.isDead)
                    {
                        if (unit.hasbattleshock)
                        {
                            if (unit.player == PlayerIndex.P1)
                            {
                                player1Units.Add(unit);
                            }
                            else
                            {
                                player2Units.Add(unit);
                            }
                            unit.SelectEffect.SetActive(true);
                        }
                    }
                }
                errorText.text = "Rally phase: Select a unit.";
                break;


     
          }
    }

    public void PassTurn()
    {
        if (SelectedUnit)
        {
            
            if (SelectedUnit.GetComponent<Spell>())
            {
                SelectedUnit.GetComponent<Spell>().iscasting = false;
                SelectedUnit.SelectEffect.SetActive(false);
                EndTurn();
            }
            else
            {
                errorText.text = "Finish piling in around your leader!";
            }
           
        }
        else
        {
            if (pTurn == PlayerIndex.P1)
            {
                SelectedUnit = player1Units[0];
            }
            else
            {
                SelectedUnit = player2Units[0];
            }
            EndTurn();
        }


      


        //  errorText.text = "Select a unit before passing its turn!";

    }
    public void EndPhase()
    {
        foreach(Unit unit in player1ActivatedUnits)
        {
            unit.AnimDeactivate();
            unit.ResetTransform();

        }
        foreach(Unit unit in player1Units)
        {
            unit.AnimDeactivate();
            unit.ResetTransform();
        }
        foreach(Unit unit in player2ActivatedUnits)
        {
            unit.AnimDeactivate();
            unit.ResetTransform();
        }
        foreach (Unit unit in player2Units)
        {
            unit.AnimDeactivate();
            unit.ResetTransform();
        }
        player1ActivatedUnits.Clear();
        player1Units.Clear();
        player2ActivatedUnits.Clear();
        player2Units.Clear();

        //  Debug.Log("End Phase");
        switch (currentPhase)
        {
            case BattlePhase.Casting:
               // Debug.Log("commencing move phase");
                currentPhase = BattlePhase.Move;
                CreateActivationPool();
                StartTurn();
                break;

            case BattlePhase.Move:
               // Debug.Log("commencing shoot phase");
                currentPhase = BattlePhase.Shoot;
                CreateActivationPool();
                StartTurn();
                break;

            case BattlePhase.Shoot:
                // Debug.Log("commencing charge phase");
                //currentPhase = BattlePhase.Charge;
                currentPhase = BattlePhase.Combat;
                CreateActivationPool();
                StartTurn();
                break;

            case BattlePhase.Charge:
             //   Debug.Log("commencing combat phase");
                currentPhase = BattlePhase.Combat;
                CreateActivationPool();
                StartTurn();
                break;

            case BattlePhase.Combat:
             //   Debug.Log("commencing battleshock phase");
                currentPhase = BattlePhase.Rally;
                CreateActivationPool();
                StartTurn();
                break;

            case BattlePhase.Rally:
                currentPhase = BattlePhase.Casting;
                CreateActivationPool();
                StartTurn();

                break;



        }
    }
    public void EndTurn()
    {
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.isDead)
            {
                RemoveFromActivations(unit);
            }
        }
       
        if (SelectedUnit)
            
        {
            SelectedUnit.SelectEffect.SetActive(false);
            //reset tools

            SelectedUnit.moveTool.transform.position = new Vector3(-100f, SelectedUnit.moveTool.transform.position.y, -100f);
            SelectedUnit.chargetool.transform.position = new Vector3(-100f, SelectedUnit.chargetool.transform.position.y, -100f);
            SelectedUnit.attacktool.transform.position = new Vector3(-100f, SelectedUnit.attacktool.transform.position.y, -100f);
            SelectedUnit.shoottool.transform.position = new Vector3(-100f, SelectedUnit.shoottool.transform.position.y, -100f);
            if (SelectedUnit.GetComponent<Spell>())
            {
                SelectedUnit.GetComponent<Spell>().distancetool.transform.position = new Vector3(-100f, SelectedUnit.GetComponent<Spell>().distancetool.transform.position.y, -100f);
                SelectedUnit.GetComponent<Spell>().attacktool.transform.position = new Vector3(-100f, SelectedUnit.GetComponent<Spell>().attacktool.transform.position.y, -100f);
            }

            if (SelectedUnit.player == PlayerIndex.P1)
            {
                player1ActivatedUnits.Add(SelectedUnit);
                player1Units.Remove(SelectedUnit);
                SelectedUnit = null;
            }
            else
            {
                player2ActivatedUnits.Add(SelectedUnit);
                player2Units.Remove(SelectedUnit);
                SelectedUnit = null;
            }
        }
        switch (pTurn)
        {
            case PlayerIndex.P1:
                pTurn = PlayerIndex.P2;
                StartTurn();
                break;
            case PlayerIndex.P2:
                pTurn = PlayerIndex.P1;
                StartTurn();
                break;
            /*case PlayerIndex.P3:
                break;
            case PlayerIndex.P4:
                break;*/
        }
        

    }

    public void GetRandomActivation()
    {
        if (pTurn == PlayerIndex.P1)
        {
            int randomnum = Random.Range(0, player1Units.Count);
            SelectedUnit = player1Units[randomnum];
        }
        else
        {
            int randomnum = Random.Range(0, player2Units.Count);
            SelectedUnit = player2Units[randomnum];
        }
    }
    public void AITakeTurn()
    {
        //PassTurn();
        GetRandomActivation();
        switch (currentPhase)
        {
            case BattlePhase.Casting:
              
                break;
            case BattlePhase.Move:
                SelectedUnit.GetClosestEnemyUnit();
                if (SelectedUnit.canShoot)
                {
                    //if cna shoot
                    if (Vector3.Distance(SelectedUnit.unitLeader.transform.position, SelectedUnit.closestenemyunit.transform.position) < SelectedUnit.stats.ranged)
                    {//if in shooting range
                        EndTurn();
                    }
                    else
                    {
                        if (Vector3.Distance(SelectedUnit.unitLeader.transform.position, SelectedUnit.closestenemyunit.transform.position) > SelectedUnit.stats.melee)
                        {//if outside of melee range, 
                            Vector3 movevector = Vector3.Normalize((SelectedUnit.unitLeader.transform.position - SelectedUnit.closestenemyunit.transform.position));

                            if (Vector3.Distance(SelectedUnit.unitLeader.transform.position, SelectedUnit.closestenemyunit.transform.position) >= SelectedUnit.stats.move)
                            { //if farther than max move distance,
                              //moves the max distance towards closest enemy

                                //Debug.DrawLine(SelectedUnit.unitLeader.transform.position, SelectedUnit.unitLeader.transform.position+ movevector*-SelectedUnit.stats.move,Color.red,5f);
                                SelectedUnit.MoveUnitLeader(SelectedUnit.unitLeader.transform.position + movevector * -(SelectedUnit.stats.move - 0.5f));
                            }
                            else
                            {
                                //move to melee range
                                SelectedUnit.MoveUnitLeader(SelectedUnit.unitLeader.transform.position + movevector * (-Vector3.Distance(SelectedUnit.unitLeader.transform.position, SelectedUnit.closestenemyunit.transform.position) - SelectedUnit.stats.ranged));

                            }
                        }
                        else
                        {
                            EndTurn();
                        }
                    }
                }
                else
                {





                    if (Vector3.Distance(SelectedUnit.unitLeader.transform.position, SelectedUnit.closestenemyunit.transform.position) > SelectedUnit.stats.melee)
                    {//if outside of melee range, 
                        Vector3 movevector = Vector3.Normalize((SelectedUnit.unitLeader.transform.position - SelectedUnit.closestenemyunit.transform.position));

                        if (Vector3.Distance(SelectedUnit.unitLeader.transform.position, SelectedUnit.closestenemyunit.transform.position) >= SelectedUnit.stats.move)
                        { //if farther than max move distance,
                          //moves the max distance towards closest enemy

                            //Debug.DrawLine(SelectedUnit.unitLeader.transform.position, SelectedUnit.unitLeader.transform.position+ movevector*-SelectedUnit.stats.move,Color.red,5f);
                            SelectedUnit.MoveUnitLeader(SelectedUnit.unitLeader.transform.position + movevector * -(SelectedUnit.stats.move-0.5f));
                        }
                        else
                        {
                            //move to melee range
                            SelectedUnit.MoveUnitLeader(SelectedUnit.unitLeader.transform.position + movevector * (-Vector3.Distance(SelectedUnit.unitLeader.transform.position, SelectedUnit.closestenemyunit.transform.position) - SelectedUnit.stats.melee));

                        }
                    }
                    else
                    {
                        EndTurn();
                    }
                }
            
                break;
            case BattlePhase.Shoot:
                SelectedUnit.GetClosestEnemyUnit();

                SelectedUnit.closestenemyunit.TakeDamage(SelectedUnit.DealRangedDamage(), SelectedUnit.stats.rangeddamage, SelectedUnit.stats.rangedrend);
                EndTurn();
                break;
            case BattlePhase.Combat:
                SelectedUnit.GetClosestEnemyUnit();
                SelectedUnit.closestenemyunit.TakeDamage(SelectedUnit.DealMeleeDamage(), SelectedUnit.stats.meleedamage, SelectedUnit.stats.meleerend);
                EndTurn();
                break;
            
        }
    }
    public void StartTurn()
    {
        RefreshUI();
        //Debug.Log("StartTurn");
        //Check if any units available for phase

        if (player1Units.Count <= 0 && player2Units.Count <= 0)
        {
            pTurn = priorityplayer;
            EndPhase();
            
        }
        else
        {

            if (pTurn == PlayerIndex.P1)
            {
                if (player1Units.Count <= 0)
                {
                    EndTurn();
                }
            }
            else if (pTurn == PlayerIndex.P2)
            {
                if (player2Units.Count <= 0)
                {
                    EndTurn();
                }else if (isvsAI)
                {
                    AITakeTurn();
                }
            }


           

        }

    }
    public void RefreshUI()
    {
        //  errorText.text = "";
        phasetext.text = "Phase: "+currentPhase.ToString();
        turnText.text = "Turn: "+pTurn;
        if (SelectedUnit)
        {
            selectedText.text = "Selected: " + SelectedUnit.name;
        }
        else
        {
            selectedText.text = "Selected: none";
        }

    }

   
}
