using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Unit : MonoBehaviour
{




    public List<Transform> pilepoints = new List<Transform>();
    public bool canMelee = false;
    public bool canShoot=false;
    public bool canCharge = true;
    public bool canCast = false;
    public CombatText hittext;
    int pileindex = 0;
    int modelindex = 0;
    int modelcount;
    int carriedoverwounds;
    public int chargedist;
    public int modelslostthisturn=0;
    public int slainmodels=0;
    public bool isDead = false;
    public bool isSelected = false;
    public bool hasActivated = false;
    public bool hasMoved = false;
    public bool hasshot = false;
    public bool hasbattleshock;
    public Leader unitLeader;
    public List<Model> models = new List<Model>();
    public GameManager gman;
    public MovementTool moveTool;
    public AttackTool attacktool;
    public ShootTool shoottool;
    public ChargeTool chargetool;
    public ClickCaster caster;
    public GameObject SelectEffect;


    public Unit closestenemyunit;
    public Model closestenemymodel;
    public float closestenemyunitdistance=1000f;
    // Owning player
    public GameManager.PlayerIndex player = GameManager.PlayerIndex.P1;
    public BaseStats stats;

    public void GetClosestEnemyUnit()
    {
        closestenemyunitdistance = 99999.0f;
        closestenemyunit = null;

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (!unit.isDead)
            {
                if (unit.player != player)
                {


                    if (Vector3.Distance(unit.transform.position, transform.position) <= closestenemyunitdistance)
                    {
                        closestenemyunit = unit;
                        closestenemyunitdistance = Vector3.Distance(unit.transform.position, transform.position);
                        Debug.Log("closest enemy distance: " + closestenemyunitdistance + closestenemyunit);
                    }



                }
            }
        }
      
    }
    void Start()
    {
       
           
       
        
     
        caster = FindObjectOfType<ClickCaster>();
        gman = FindObjectOfType<GameManager>();
        chargetool = FindObjectOfType<ChargeTool>();
        moveTool = FindObjectOfType<MovementTool>();
        attacktool = FindObjectOfType<AttackTool>();
        shoottool = FindObjectOfType<ShootTool>();
        stats = GetComponent<BaseStats>();
        //SelectEffect.SetActive(false);
        if (GetComponent<Hero>())
        {
            unitLeader = GetComponent<Leader>();
        }
        else
        {
            foreach (Model model in GetComponentsInChildren<Model>())
            {
                if (model.gameObject.GetComponent<Leader>())
                {
                    unitLeader = model.gameObject.GetComponent<Leader>();
                }
                else
                {
                    models.Add(model);
                }
            }
        }

       foreach(PilePoint point in GetComponentsInChildren<PilePoint>())
        {
            pilepoints.Add(point.transform);
        }
    }


    private void Update()
    {
       
    }


    IEnumerator TimeDelay(float seconds, string functiontocall)
    {
        //Print the time of when the function is first called.
       // Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(seconds);

        //After we have waited 5 seconds print the time again.
       // Debug.Log("Finished Coroutine at timestamp : " + Time.time);
       // FinishMovingLeader();
        Invoke(functiontocall,0);
        caster.disabled = false;
        
    }
    public void MoveUnitLeader(Vector3 targetpos)
    {
        AnimMove(0.6f);
        if (Vector3.Distance(targetpos, unitLeader.gameObject.transform.position) <= stats.move)
        {
            float dist = Vector3.Distance(unitLeader.gameObject.transform.position, targetpos);
            unitLeader.GetComponent<Model>().LerpToPos(targetpos, dist/stats.move);
            caster.disabled = true;
            StartCoroutine(TimeDelay(dist/stats.move,"FinishMovingLeader"));


        }
        else
        {
            gman.errorText.text = "Too far!";
        }
    }

        public void FinishMovingLeader()
        {
        //make leader face closest enemy
        GetClosestEnemyUnit();
        //FinishMovingLeader();
        unitLeader.transform.LookAt(closestenemyunit.transform.position);

            Vector3 movevector = unitLeader.transform.position - transform.position;

            transform.position = unitLeader.transform.position;

            foreach (Model model in GetComponentsInChildren<Model>())
            {

                model.transform.position -= movevector;
            }

            
            hasMoved = true;
            if (GetComponent<Hero>())
            {
                moveTool.transform.position = new Vector3(1000, moveTool.transform.position.y, 1000);
                hasMoved = false;
                modelindex = 0;
                hasActivated = true;
            if (player == GameManager.PlayerIndex.P1)
            {
                gman.pTurn = GameManager.PlayerIndex.P2;
            }
            else
            {
                gman.pTurn = GameManager.PlayerIndex.P1;
            }
            Debug.Log("hero ending turn");
            SelectEffect.SetActive(false);
            gman.EndTurn();
           
           



            }
            else
            {
            gman.errorText.text = "Pile in your units around the leader you just moved!";
                //set move tool to leader position and size for cohesion
                SetMoveTool();
                moveTool.transform.localScale = new Vector3(5, moveTool.transform.localScale.y, 5);

            if (gman.isvsAI && player == GameManager.PlayerIndex.P2)
            {
                AutoPile();
            }
            }


        


        if (GetComponentsInChildren<Model>().Length<=1) 
        {
           // Debug.LogError(GetComponentsInChildren<Model>().Length);
            moveTool.transform.position = new Vector3(1000, moveTool.transform.position.y, 1000);
            hasMoved = true;
            modelindex = 0;
            hasActivated = true;
            SelectEffect.SetActive(false);
            gman.EndTurn();
           
        }



    }

   public void AutoPile()
    {
      
       
            if (pilepoints[pileindex])
            {
                MoveModel(pilepoints[pileindex].position);
                pileindex++;
            }
            else
            {
                Debug.LogWarning("No pile point assigned to " + name);
            }
        
    }
    public void MoveModel(Vector3 targetpos)
    {
     
        //-1 for leader and -1 because array.
        if (modelindex <= stats.unitMaxSize - 2 - slainmodels)
        {
            if (Vector3.Distance(targetpos, unitLeader.transform.position) <= 2.5)
            {
                float dist = Vector3.Distance(targetpos, models[modelindex].transform.position);
                models[modelindex].LerpToPos(targetpos, dist/stats.move);//transform.position = new Vector3(targetpos.x, unitLeader.transform.position.y, targetpos.z);
                caster.disabled = true;
                StartCoroutine(TimeDelay(dist/stats.move, "FinishMovingModel"));
                
            }
            else
            {
                gman.errorText.text = "That Unit is Too Far from leader";
               
            }
           
          
        }
    }
    public void FinishMovingModel()
    {
        GetClosestEnemyUnit();

        models[modelindex].transform.LookAt(closestenemyunit.transform.position);
        modelindex++;

        if (gman.isvsAI && player == GameManager.PlayerIndex.P2)
        {
            if (pileindex <= stats.unitMaxSize-2-slainmodels)
            {
                AutoPile();
            }
            else
            {
                pileindex = 0;
                 moveTool.transform.position = new Vector3(1000, moveTool.transform.position.y, 1000);
                hasMoved = false;
                modelindex = 0;
                hasActivated = true;
                SelectEffect.SetActive(false);
                gman.EndTurn();
            }

        }
        else
        {
            if (modelindex > stats.unitMaxSize - 2 - slainmodels)
            {
                moveTool.transform.position = new Vector3(1000, moveTool.transform.position.y, 1000);
                hasMoved = false;
                modelindex = 0;
                hasActivated = true;
                SelectEffect.SetActive(false);
                gman.EndTurn();


            }
        }
    }

    public void ResetTransform()
    {
        Vector3 movevector = unitLeader.transform.position - transform.position;
        transform.position = unitLeader.transform.position;
        foreach (Model model in GetComponentsInChildren<Model>())
        {
            model.transform.position -= movevector;
        }
    }
    public void ChargeUnitLeader(Vector3 targetpos)
    {
        AnimMove(1.0f);
        if (Vector3.Distance(targetpos, unitLeader.gameObject.transform.position) <= chargedist)
        {
            unitLeader.transform.position = new Vector3(targetpos.x, unitLeader.transform.position.y, targetpos.z);
            
            hasMoved = true;
            if (GetComponent<Hero>())
            {
                moveTool.transform.position = new Vector3(1000, chargetool.transform.position.y, 1000);
                hasMoved = true;
                modelindex = 0;
                hasActivated = true;
                SelectEffect.SetActive(false);
                gman.EndTurn();



            }
            else
            {
                //set move tool to leader position and size for cohesion
                SetChargeTool();
                chargetool.transform.localScale = new Vector3(5, chargetool.transform.localScale.y, 5);
            }


        }
        if (GetComponentsInChildren<Model>().Length <= 1)
        {
           // Debug.LogError(GetComponentsInChildren<Model>().Length);
            moveTool.transform.position = new Vector3(1000, moveTool.transform.position.y, 1000);
            hasMoved = true;
            modelindex = 0;
            hasActivated = true;
            SelectEffect.SetActive(false);
            gman.EndTurn();
        
        }



    }


    public bool RollDice(int requiredroll, int dicemodifier)
    {
        
            int randomnum = Random.Range(1, 7); //return 1-6 for the dice roll.

       // Debug.Log("Rolled: " + randomnum);
            if (randomnum >= requiredroll)
            {
                return true;//passed the roll
            }
            else
            {
                return false;//failed the roll
            }
        
    }
    public int DealRangedDamage()
    {
        AnimAttack();
        int attackdice=0;
        int hitdice = 0;
        int wounddice = 0;
        foreach(Model model in GetComponentsInChildren<Model>())
        {
            //Add attack for each model in unit
            attackdice++;
        }
        //multiply by amount of attacks
        attackdice *= stats.rangedattacks;

        Debug.LogWarning("attacks: " + attackdice);
        for(int i=0; i < attackdice; i++)//roll for hit on each attack
        {
           
            if (RollDice(stats.rangedtohit,0))
            {
                hitdice++;
            }
        }
        Debug.LogWarning("hits: " + hitdice);

        for (int i = 0; i < hitdice; i++)//roll for wound on each hit
        {

            if (RollDice(stats.rangedtowound,0))
            {
                wounddice++;
            }
        }


        Debug.LogWarning("wounds: " + wounddice);

        hasshot = true;
        return wounddice;

    }
    public int DealMeleeDamage()
    {
        AnimAttack();
        int attackdice = 0;
        int hitdice = 0;
        int wounddice = 0;
        foreach (Model model in GetComponentsInChildren<Model>())
        {
            //Add attack for each model in unit
            attackdice++;
        }
        //multiply by amount of attacks
        attackdice *= stats.meleeattacks;

        Debug.LogWarning("attacks: " + attackdice);
        for (int i = 0; i < attackdice; i++)//roll for hit on each attack
        {

            if (RollDice(stats.meleetohit, 0))
            {
                hitdice++;
            }
        }
        Debug.LogWarning("hits: " + hitdice);

        for (int i = 0; i < hitdice; i++)//roll for wound on each hit
        {

            if (RollDice(stats.meleetowound, 0))
            {
                wounddice++;
            }
        }


        Debug.Log("wounds: " + wounddice);

        return wounddice;
    }
    public void TakeDamage(int rawwounds, int weapondamage, int weaponrend)
    {
        Debug.LogWarning("Took " + rawwounds + " raw wounds");
        int unsavedwounds=0;

        for (int i = 0; i < rawwounds; i++)//roll for wound on each hit
        {

            if (!RollDice(stats.save, weaponrend))//if failed save roll
            {
                unsavedwounds++;//wound goes through
            }
        }

        Debug.LogWarning("failed  " + unsavedwounds + " saves");

        unsavedwounds *= weapondamage; //calculate total damage based on wounds and weapon damage

        Debug.LogWarning("Took " + unsavedwounds + " damage from wounds");


        int modelstoremove = 0;
        int assignedwounds = carriedoverwounds;

        Debug.LogWarning("Had " + carriedoverwounds + "wounds already");

        hittext.text.text = unsavedwounds.ToString();
        hittext.text.color = Color.red;
        hittext.fadetimer = Time.time + 1.5f;
        hittext.ToggleText(true);

        for (int i=unsavedwounds; i>0; i--)//calculate how many models to remove and how many wounds must be assigned to a model that is still living.
        {
            assignedwounds++;
            if (assignedwounds >= stats.wounds)
            {
                modelstoremove++;
                assignedwounds = 0;
            }

            
        }
        Debug.LogWarning("Need to remove " + modelstoremove + " models and assign "+assignedwounds+ "Wounds");
       
         carriedoverwounds = assignedwounds;

        if (modelstoremove > 0)
        {
            RemoveModels(modelstoremove);
        }

    }


    public void RemoveModels(int amounttoremove)
    {
        if (models.Count > 0&&amounttoremove<models.Count)
        {
            slainmodels+=amounttoremove;
            for (int i = 0; i < amounttoremove; i++)
            {

                models[0].GetComponentInChildren<Animator>().SetTrigger("Die");

                models[0].transform.parent = null;
                models.RemoveAt(0);
               
            }
        }
        else
        {
            isDead = true;
            SelectEffect.SetActive(false);
            //gman.RemoveFromActivations(this);

            
       
           // GetComponentInChildren<Animator>().SetTrigger("Die");
            foreach (Model model in GetComponentsInChildren<Model>())
            {
                model.GetComponentInChildren <Animator>().SetTrigger("Die");
            }
            unitLeader.transform.parent = null;
            if (player == GameManager.PlayerIndex.P1)
            {
                gman.P1UnitCount--;
            }
            else
            {
                gman.P2UnitCount--;
            }

            if (gman.P1UnitCount <= 0)
            {
                //  Debug.LogError("Wingame for p2");
                Destroy(gman.armycon.gameObject);
                SceneManager.LoadScene("Win_Undead");
            }
            else if (gman.P2UnitCount <= 0)
            {
                //  Debug.LogError("Wingame for p1");
                Destroy(gman.armycon.gameObject);
              
                SceneManager.LoadScene("Win_Empire");
            }
            //Destroy(this);

        }
    }
    public void SetMoveTool()
    {
        moveTool.gameObject.SetActive(true);
        moveTool.transform.position = new Vector3(unitLeader.transform.position.x, moveTool.transform.position.y, unitLeader.transform.position.z);
        moveTool.transform.localScale = new Vector3(stats.move*2, moveTool.transform.localScale.y, stats.move*2);
    }
    public void SetChargeTool()
    {
        
        chargetool.gameObject.SetActive(true);
        chargetool.transform.position = new Vector3(unitLeader.transform.position.x, chargetool.transform.position.y, unitLeader.transform.position.z);
        chargetool.transform.localScale = new Vector3(chargedist * 2, chargetool.transform.localScale.y, chargedist * 2);
    }
    public void SetAttackTool()
    {
        attacktool.gameObject.SetActive(true);
        attacktool.transform.position = new Vector3(unitLeader.transform.position.x, attacktool.transform.position.y, unitLeader.transform.position.z);
        attacktool.transform.localScale = new Vector3(stats.melee * 2, attacktool.transform.localScale.y, stats.melee * 2);
    }
    public void SetShootTool()
    {
        shoottool.gameObject.SetActive(true);
        shoottool.transform.position = new Vector3(unitLeader.transform.position.x, attacktool.transform.position.y, unitLeader.transform.position.z);
        shoottool.transform.localScale = new Vector3(stats.ranged * 2, shoottool.transform.localScale.y, stats.ranged * 2);
    }

    public void AnimAttack()
    {
        //set attack animation for leader
        unitLeader.gameObject.GetComponentInChildren<Animator>().SetTrigger("Attack");
        foreach (Model model in models)//set attack animation for models
        {
            model.gameObject.GetComponentInChildren<Animator>().SetTrigger("Attack");
        }
    }
    public void AnimDeactivate()
    {
        //set deactivate animation for leader
        unitLeader.gameObject.GetComponentInChildren<Animator>().SetBool("Active", false);
        foreach (Model model in models)//set deactivate animation for models
        {
            model.gameObject.GetComponentInChildren<Animator>().SetBool("Active", false);
        }
    }
    public void AnimActivate()
    {
        //set activate animation for leader
        unitLeader.gameObject.GetComponentInChildren<Animator>().SetBool("Active", true);
        foreach (Model model in models)//set activate animation for models
        {
            model.gameObject.GetComponentInChildren<Animator>().SetBool("Active", true);
        }
    }
    public void AnimTakeDamage()
    {
        //set take damage animation for leader
        unitLeader.gameObject.GetComponentInChildren<Animator>().SetTrigger("TakeDamage");
        foreach (Model model in models)//set take damage animation for models
        {
            model.gameObject.GetComponentInChildren<Animator>().SetTrigger("TakeDamage");
        }
    }

    public void AnimMove(float speed)
    {
        //set active if not already
        AnimActivate();
        //set speed in animation for leader
        unitLeader.gameObject.GetComponentInChildren<Animator>().SetFloat("Speed", speed);
        foreach (Model model in models)//set speed in animation for models
        {
            model.gameObject.GetComponentInChildren<Animator>().SetFloat("Speed", speed);
        }
    }

    





}
