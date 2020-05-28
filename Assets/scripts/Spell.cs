using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public int wounds;
    public int rend;
    public int damage;
   
    public enum SpellType{Area, Single, Buff, Debuff};
    public SpellType spelltype=SpellType.Area;
    public string spellName;
    public int cost=1;
    public string spellDescription;
    public float castdist=10f;
    public int miscastChance=50;
    public GameObject attacktool;
    public GameObject distancetool;
    public float rad;
   // public List<Unit> affectedUnits=new List<Unit>();
    public GameManager gman;
    public Vector3 targetpos;
    public bool iscasting=false;

    private void Start()
    {
        
        gman = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (iscasting)
        {
           Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                targetpos = hit.point;
                attacktool.transform.localScale= new Vector3(rad+2.5f, attacktool.transform.localScale.y, rad+2.5f);
                attacktool.transform.position = new Vector3(hit.point.x, attacktool.transform.position.y, hit.point.z);
                distancetool.transform.position = new Vector3(transform.position.x, attacktool.transform.position.y, transform.position.z);
                distancetool.transform.localScale = new Vector3(castdist+rad, attacktool.transform.localScale.y, castdist+rad);

            }
        }
    }

    public void Cast()
    {
        switch (spelltype)
        {
            case SpellType.Area:
                AreaOfEffect();
                break;
            case SpellType.Single:
                gman.GetClosestEnemyModelToPoint(targetpos, GetComponent<Unit>().player);
                if (gman.closestmodel.GetComponent<Unit>()) {
                    SingleTarget(gman.closestmodel.GetComponent<Unit>());
                }
                else
                {
                    SingleTarget(gman.closestmodel.GetComponentInParent<Unit>());
                }
                break;
            case SpellType.Buff:
                break;
            case SpellType.Debuff:
                break;
        }
    }
    public void AreaOfEffect()
    {
        //affectedUnits.Clear();
        //get all affected units
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if(Vector3.Distance(unit.transform.position, targetpos)<=rad)
            {
                //affectedUnits.Add(unit);
               
                unit.TakeDamage(wounds, damage, rend);
            }
        }
        
        GetComponent<Unit>().hasActivated = true;
        iscasting = false;
        attacktool.transform.position = new Vector3(1000f, attacktool.transform.position.y, 1000f);
        gman.EndTurn();
    }
    public void SingleTarget(Unit targetunit)
    {
        if (Vector3.Distance(targetunit.transform.position, transform.position) <= castdist)
        {
            targetunit.TakeDamage(wounds, damage, rend);
            GetComponent<Unit>().hasActivated = true;
            iscasting = false;
            attacktool.transform.position = new Vector3(1000f, attacktool.transform.position.y, 1000f);
            gman.EndTurn();
        }
        else
        {
            gman.errorText.text = "That target is too far away! pass turn if no other targets.";
        }
    }
}
