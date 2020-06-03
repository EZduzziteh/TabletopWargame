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
    public string scenetoload = "Village_01";

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
                temp.name = "Empire Levy";
                temp.transform.position = new Vector3(-100, -100, -100);
            }
            else if (unit == "Empire Legionnaires")
            {

                GameObject temp = Instantiate(unitprefabs[1]);
                temp.name = "Empire Legionnaires";
                temp.transform.position = new Vector3(-100, -100, -100);
            }
            else if (unit == "Empire Scouts")
            {
                
                GameObject temp=Instantiate(unitprefabs[2]);
                temp.name = "Empire Scouts";
                temp.transform.position = new Vector3(-100, -100, -100);

            }
            else if (unit == "Empire Knights")
            {

                GameObject temp = Instantiate(unitprefabs[3]);
                temp.name = "Empire Knights";
                temp.transform.position = new Vector3(-100, -100, -100);

            }
            else if (unit == "Empire Loremaster")
            {

                GameObject temp = Instantiate(unitprefabs[4]);
                temp.name = "Empire Loremaster";
                temp.transform.position = new Vector3(-100, -100, -100);

            }
        }
    }
    public void UpdateBuildPoints( int buildpoints)
    {
        totalpointsallowed = buildpoints;
        UpdateArmyCost();
    }
    public void UpdateSceneToLoad(string scenename)
    {
        scenetoload = scenename;
       
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
           
            SceneManager.LoadScene(scenetoload);
        }
    }

    public void UpdateArmyCost()
    {
        units.Clear();
        totalpointsspent = 0;
        foreach(Dropdown drop in FindObjectsOfType<Dropdown>())
        {
            if(drop.captionText.text=="Empire Levy (10)")
            {
                units.Add("Empire Levy");
                totalpointsspent += 10;
            }else if(drop.captionText.text == "Empire Legionnaires (20)")
            {
                units.Add("Empire Legionnaires");
                totalpointsspent += 20;
            }
            else if (drop.captionText.text == "Empire Scouts (20)")
            {
                units.Add("Empire Scouts");
                totalpointsspent += 20;
            }
            else if (drop.captionText.text == "Empire Knights (30)")
            {
                units.Add("Empire Knights");
                totalpointsspent += 30;
            }
            else if (drop.captionText.text == "Empire Loremaster (30)")
            {
                units.Add("Empire Loremaster");
                totalpointsspent += 30;
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
