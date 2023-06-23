using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCaster : MonoBehaviour
{
    public GameManager gman;
    public bool disabled=false;


    

   
    // Start is called before the first frame update

    private void Start()
    {
        gman = FindObjectOfType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        
        //left click
        if (Input.GetMouseButtonDown(0))
        {
            if (!disabled)
            {
                switch (gman.currentPhase)
                {
                    case GameManager.BattlePhase.Casting:
                        HandleCasting();
                        break;

                    case GameManager.BattlePhase.Move:
                        HandleMovement();
                        break;

                    case GameManager.BattlePhase.Shoot:
                        HandleShooting();
                        break;
                    
                    case GameManager.BattlePhase.Combat:
                        HandleCombat();
                        break;
                   
                }
            }


        }
        //right click
        else if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<Unit>())
                {
                    //display unit information.

                    if (hit.collider.GetComponent<Spell>())
                    {
                        gman.spelltext.text = "Spell: "+hit.collider.GetComponent<Spell>().spellName +", Type: "+ hit.collider.GetComponent<Spell>().spelltype + ", Info: "+ hit.collider.GetComponent<Spell>().spellDescription;
                    }
                    else
                    {
                        gman.spelltext.text = "This unit has no spells to cast.";
                    }
                    if (hit.collider.GetComponent<Unit>().canShoot)
                    {
                        gman.rangeddist.text = "distance: " + hit.collider.GetComponent<Unit>().stats.ranged;
                        gman.rangedattacks.text = "attacks: " + hit.collider.GetComponent<Unit>().stats.rangedattacks;
                       // 
                        gman.rangeddamage.text = "damage: " + hit.collider.GetComponent<Unit>().stats.rangeddamage;
                        gman.rangedrend.text = "rend: " + hit.collider.GetComponent<Unit>().stats.rangedrend;
                        gman.rangedtohit.text = "to hit: " + hit.collider.GetComponent<Unit>().stats.rangedtohit;
                        gman.rangedtowound.text = "to wound: " + hit.collider.GetComponent<Unit>().stats.rangedtowound;
                    }
                    else
                    {
                        gman.rangeddist.text = "This unit can ";
                        gman.rangedattacks.text = "not shoot";
                     
                        gman.rangeddamage.text = " ";
                        gman.rangedrend.text = "";
                        gman.rangedtohit.text = "" ;
                        gman.rangedtowound.text = "" ;
                    }
                    if (hit.collider.GetComponent<Unit>().canMelee) 
                    {
                        gman.meleeattacks.text = "attacks: " + hit.collider.GetComponent<Unit>().stats.meleeattacks;
                        gman.meleedamage.text = "damage: " + hit.collider.GetComponent<Unit>().stats.meleedamage;
                        gman.meleedist.text = "distance: " + hit.collider.GetComponent<Unit>().stats.melee;
                        gman.meleerend.text = "rend: " + hit.collider.GetComponent<Unit>().stats.meleerend;
                        gman.meleetohit.text = "to hit: " + hit.collider.GetComponent<Unit>().stats.meleetohit;
                        gman.meleetowound.text = "to wound: " + hit.collider.GetComponent<Unit>().stats.meleetowound;
                    }
                    else
                    {
                        gman.meleeattacks.text = "";
                        gman.meleedamage.text = "not melee";
                        gman.meleedist.text = "This unit can ";
                        gman.meleerend.text = "";
                        gman.meleetohit.text = "";
                        gman.meleetowound.text = "" ;
                    }
                    gman.unitnametex.text = hit.collider.GetComponent<Unit>().name;
                    gman.save.text = "save: " + hit.collider.GetComponent<Unit>().stats.save;
                    gman.health.text = "health: " + hit.collider.GetComponent<Unit>().stats.wounds;
                   

                }
                else if (hit.collider.GetComponentInParent<Unit>())
                {
                    //display unit information.




                    if (hit.collider.GetComponentInParent<Spell>())
                    {
                        gman.spelltext.text = hit.collider.GetComponentInParent<Spell>().spellName + hit.collider.GetComponentInParent<Spell>().spelltype + hit.collider.GetComponentInParent<Spell>().spellDescription;
                    }
                    else
                    {
                        gman.spelltext.text = "This unit has no spells to cast.";
                    }
                    if (hit.collider.GetComponentInParent<Unit>().canShoot)
                    {
                        gman.rangeddist.text = "distance: " + hit.collider.GetComponentInParent<Unit>().stats.ranged;
                        gman.rangedattacks.text = "attacks: " + hit.collider.GetComponentInParent<Unit>().stats.rangedattacks;
                       
                        gman.rangeddamage.text = "damage: " + hit.collider.GetComponentInParent<Unit>().stats.rangeddamage;
                        gman.rangedrend.text = "rend: " + hit.collider.GetComponentInParent<Unit>().stats.rangedrend;
                        gman.rangedtohit.text = "to hit: " + hit.collider.GetComponentInParent<Unit>().stats.rangedtohit;
                        gman.rangedtowound.text = "to wound: " + hit.collider.GetComponentInParent<Unit>().stats.rangedtowound;
                    }
                    else
                    {
                        gman.rangeddist.text = "This unit can ";
                        gman.rangedattacks.text = "not shoot";
                      
                        gman.rangeddamage.text = " ";
                        gman.rangedrend.text = "";
                        gman.rangedtohit.text = "";
                        gman.rangedtowound.text = "";
                    }
                    if (hit.collider.GetComponentInParent<Unit>().canMelee)
                    {
                        gman.meleedamage.text = "damage: " + hit.collider.GetComponentInParent<Unit>().stats.meleedamage;
                        gman.meleeattacks.text = "Attacks: " + hit.collider.GetComponentInParent<Unit>().stats.meleeattacks;
                        gman.meleedist.text = "distance: " + hit.collider.GetComponentInParent<Unit>().stats.melee;
                        gman.meleerend.text = "rend: " + hit.collider.GetComponentInParent<Unit>().stats.meleerend;
                        gman.meleetohit.text = "to hit: " + hit.collider.GetComponentInParent<Unit>().stats.meleetohit;
                        gman.meleetowound.text = "to wound: " + hit.collider.GetComponentInParent<Unit>().stats.meleetowound;
                    }
                    else
                    {
                        gman.meleedamage.text = "";
                        gman.meleedist.text = "This unit can";
                        gman.meleeattacks.text = "not melee ";
                        gman.meleerend.text = "";
                        gman.meleetohit.text = "";
                        gman.meleetowound.text = "";
                    }
                    gman.save.text = "save: " + hit.collider.GetComponentInParent<Unit>().stats.save;
                    gman.health.text = "health: " + hit.collider.GetComponentInParent<Unit>().stats.wounds;
                    gman.unitnametex.text = hit.collider.GetComponentInParent<Unit>().name;






                }
            }
        }
    }


    void HandleCasting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag != "NotClickable")
            {
                if (hit.collider.gameObject.GetComponent<Unit>())
                {
                    if (hit.collider.gameObject.GetComponent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    else if (hit.collider.gameObject.GetComponent<Unit>().player == gman.pTurn)
                    {


                        gman.SelectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                        hit.collider.gameObject.GetComponent<Unit>().isSelected = true;
                        gman.SelectedUnit.GetComponent<Spell>().iscasting = true;
                        gman.errorText.text = "Casting: " +gman.SelectedUnit.GetComponent<Spell>().spellName;
                        gman.RefreshUI();

                    }
                }
                else if (hit.collider.gameObject.GetComponentInParent<Unit>())
                {
                    if (hit.collider.gameObject.GetComponentInParent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    else if (hit.collider.gameObject.GetComponentInParent<Unit>().player == gman.pTurn)
                    {


                        gman.SelectedUnit = hit.collider.gameObject.GetComponentInParent<Unit>();
                        hit.collider.gameObject.GetComponentInParent<Unit>().isSelected = true;
                        gman.SelectedUnit.GetComponent<Spell>().iscasting = true;
                        gman.RefreshUI();

                    }
                }
                else
                {
                    if (Vector3.Distance(gman.SelectedUnit.transform.position, hit.point)<=gman.SelectedUnit.GetComponent<Spell>().castdist){


                        gman.SelectedUnit.GetComponent<Spell>().Cast();
                        
                    }
                    else
                    {
                        gman.errorText.text = "Cant cast that far!";
                    }
                }
               

            }
        }
    
}
    void HandleMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag != "NotClickable")
            {
                
                //if we hit a unit
                if (hit.collider.gameObject.GetComponent<Unit>())
                {
                    //if our unit has already activated
                    if (hit.collider.gameObject.GetComponent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    //if we hit an unactivated unit that belongs to the player whose turn is taking place
                    else if (hit.collider.gameObject.GetComponent<Unit>().player == gman.pTurn)
                    {
                        //if we dont already have a currently selected unit, set this one as the selected unit.
                        if (!gman.SelectedUnit)
                        {
                            gman.SelectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                            hit.collider.gameObject.GetComponent<Unit>().isSelected = true;
                            gman.SelectedUnit.SetMoveTool();
                            gman.SelectedUnit.AnimMove(0.0f);
                            gman.RefreshUI();
                        }

                    }
                }//also check to see if we clicked on a model from a unit
                else if (hit.collider.gameObject.GetComponentInParent<Unit>())
                {
                    if (hit.collider.gameObject.GetComponentInParent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    else if (hit.collider.gameObject.GetComponentInParent<Unit>().player == gman.pTurn)
                    {

                        if (!gman.SelectedUnit)
                        {
                            gman.SelectedUnit = hit.collider.gameObject.GetComponentInParent<Unit>();
                            hit.collider.gameObject.GetComponentInParent<Unit>().isSelected = true;
                            gman.SelectedUnit.SetMoveTool();
                            gman.SelectedUnit.AnimMove(0.0f);
                            gman.RefreshUI();
                        }

                    }
                }
                //if leader hasnt moved, move leader.
                else if (!gman.SelectedUnit.hasMoved)
                {
                    gman.SelectedUnit.MoveUnitLeader(hit.point);
                }
                else
                {
                    //gman.SelectedUnit.MoveModel(hit.point);
                }

            }
        }
    }
    void HandleShooting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.gameObject.GetComponent<Unit>())
            {
                if (gman.SelectedUnit && hit.collider.gameObject.GetComponent<Unit>().player != gman.pTurn)
                {
                    gman.SelectedTarget = hit.collider.gameObject.GetComponent<Unit>();


                    if (gman.GetTargetedToSelected() <= gman.SelectedUnit.stats.ranged)
                    {

                        //#TODO
                        //Debug.Log("#TODO Handle Ranged Damage");
                        gman.SelectedTarget.TakeDamage(gman.SelectedUnit.DealRangedDamage(), gman.SelectedUnit.stats.rangeddamage, gman.SelectedUnit.stats.rangedrend);
                        gman.EndTurn();
                    }
                    
                    else
                    {
                        gman.errorText.text = "That unit is not in shooting range";
                    }
                   

                }
                else if (hit.collider.gameObject.GetComponent<Unit>().canShoot)
                {
                    if (hit.collider.gameObject.GetComponent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    else if (hit.collider.gameObject.GetComponent<Unit>().player == gman.pTurn)
                    {


                        gman.SelectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                        hit.collider.gameObject.GetComponent<Unit>().isSelected = true;
                        gman.SelectedUnit.SetShootTool();
                        gman.RefreshUI();

                    }
                }
                else
                {
                    gman.errorText.text = "That Unit can not shoot!";
                }
            }
            else if (hit.collider.gameObject.GetComponentInParent<Unit>())
            {
                if (gman.SelectedUnit && hit.collider.gameObject.GetComponentInParent<Unit>().player != gman.pTurn)
                {
                    gman.SelectedTarget = hit.collider.gameObject.GetComponentInParent<Unit>();


                    if (gman.GetTargetedToSelected() <= gman.SelectedUnit.stats.ranged)
                    {

                        //#TODO
                        //Debug.Log("#TODO Handle Ranged Damage");
                        gman.SelectedTarget.TakeDamage(gman.SelectedUnit.DealRangedDamage(), gman.SelectedUnit.stats.rangeddamage, gman.SelectedUnit.stats.rangedrend);
                        gman.EndTurn();
                    }

                    else
                    {
                        gman.errorText.text = "That unit is not in shooting range";
                    }

                   
                
                }
                else if (hit.collider.gameObject.GetComponentInParent<Unit>().canShoot)
                {
                    if (hit.collider.gameObject.GetComponentInParent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    else if (hit.collider.gameObject.GetComponentInParent<Unit>().player == gman.pTurn)
                    {


                        gman.SelectedUnit = hit.collider.gameObject.GetComponentInParent<Unit>();
                        hit.collider.gameObject.GetComponentInParent<Unit>().isSelected = true;
                        gman.SelectedUnit.SetShootTool();
                        gman.RefreshUI();

                    }
                }
                else
                {
                    gman.errorText.text = "That Unit can not shoot!";
                }
            }
          

        }
    }
   

  
    void HandleCombat()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.gameObject.GetComponent<Unit>())
            {
                if (gman.SelectedUnit && hit.collider.gameObject.GetComponent<Unit>().player != gman.pTurn)
                {
                    gman.SelectedTarget = hit.collider.gameObject.GetComponent<Unit>();


                    if (gman.GetTargetedToSelected() <= gman.SelectedUnit.stats.melee)
                    {
                        //#TODO
                        //Debug.Log("#TODO Handle melee Damage");
                        gman.SelectedTarget.TakeDamage(gman.SelectedUnit.DealMeleeDamage(), gman.SelectedUnit.stats.meleedamage, gman.SelectedUnit.stats.meleerend);
                        gman.EndTurn();
                    }
                    else
                    {
                        gman.errorText.text = "That unit is not in melee range";
                    }
                    

                }
                else if (hit.collider.gameObject.GetComponent<Unit>().canMelee)
                {
                    if (hit.collider.gameObject.GetComponent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    else if (hit.collider.gameObject.GetComponent<Unit>().player == gman.pTurn)
                    {


                        gman.SelectedUnit = hit.collider.gameObject.GetComponent<Unit>();
                        hit.collider.gameObject.GetComponent<Unit>().isSelected = true;
                        gman.SelectedUnit.SetAttackTool();
                        gman.RefreshUI();

                    }
                }
                else
                {
                    gman.errorText.text = "That Unit can not fight!";
                }
            }
            else if (hit.collider.gameObject.GetComponentInParent<Unit>())
            {
                if (gman.SelectedUnit && hit.collider.gameObject.GetComponentInParent<Unit>().player != gman.pTurn)
                {
                    gman.SelectedTarget = hit.collider.gameObject.GetComponentInParent<Unit>();

                    if (gman.GetTargetedToSelected() <= gman.SelectedUnit.stats.melee)
                    {
                        //#TODO
                        // Debug.Log("#TODO Handle melee Damage");

                        gman.SelectedTarget.TakeDamage(gman.SelectedUnit.DealMeleeDamage(), gman.SelectedUnit.stats.meleedamage, gman.SelectedUnit.stats.meleerend);
                        gman.EndTurn();
                    }
                    
                    else
                    {
                        gman.errorText.text = "That unit is not in melee range";
                    }

                

                }
                else if (hit.collider.gameObject.GetComponentInParent<Unit>().canMelee)
                {
                    if (hit.collider.gameObject.GetComponentInParent<Unit>().hasActivated)
                    {
                        gman.errorText.text = "That Unit has already activated this phase!";
                    }
                    else if (hit.collider.gameObject.GetComponentInParent<Unit>().player == gman.pTurn)
                    {


                        gman.SelectedUnit = hit.collider.gameObject.GetComponentInParent<Unit>();
                        hit.collider.gameObject.GetComponentInParent<Unit>().isSelected = true;
                        gman.SelectedUnit.SetAttackTool();
                        gman.RefreshUI();

                    }
                }
                else
                {
                    gman.errorText.text = "That Unit can not fight!";
                }
            }


        }
    }
   
  
    void HandleRally()
    {

    }
   
   
}
