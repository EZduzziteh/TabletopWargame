using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ArmyConstructor : MonoBehaviour
{
    // Start is called before the first frame update
    public int totalpointsallowed;
    public int totalpointsspent;
    public Text pointText;
    public Text errorText;

        public List<GameObject> unitprefabs = new List<GameObject>();
        public List<string> units = new List<string>();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
        }
    }

    public void SpawnArmy()
    {
        foreach(string unit in units)
        {
            
            if(unit=="Empire Levy")
            {
                GameObject temp = Instantiate(unitprefabs[0]);
                temp.transform.position = new Vector3(-100, -100, -100);
            }
            else if (unit == "Empire Legionnaires")
            {

                GameObject temp = Instantiate(unitprefabs[1]);
                temp.transform.position = new Vector3(-100, -100, -100);
            }
            else if (unit == "Empire Scouts")
            {
                
                GameObject temp=Instantiate(unitprefabs[2]);
                temp.transform.position = new Vector3(-100, -100, -100);
                
            }
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu_Main");
        Destroy(this.gameObject);
    }
    public void ConfirmArmy()
    {
        if (totalpointsspent <= totalpointsallowed && totalpointsspent > 0)
        {
           
            SceneManager.LoadScene("Map_01");
        }
    }

    public void UpdateArmyCost()
    {
        units.Clear();
        totalpointsspent = 0;
        foreach(Dropdown drop in FindObjectsOfType<Dropdown>())
        {
            if(drop.captionText.text=="Empire Levy (12)")
            {
                units.Add("Empire Levy");
                totalpointsspent += 12;
            }else if(drop.captionText.text == "Empire Legionnaires (22)")
            {
                units.Add("Empire Legionnaires");
                totalpointsspent += 22;
            }
            else if (drop.captionText.text == "Empire Scouts (16)")
            {
                units.Add("Empire Scouts");
                totalpointsspent += 16;
            }
           
        }

        if (totalpointsspent > totalpointsallowed)
        {
            pointText.color = Color.red;
        }
        else
        {
            pointText.color = Color.green;
        }

        pointText.text = "Army total: " + totalpointsspent + " / " + totalpointsallowed;
    }
}
