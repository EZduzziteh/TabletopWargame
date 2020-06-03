using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    public TextMesh text;
    public float fadetimer;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMesh>();
        text.gameObject.SetActive(false);
    }

    public void ToggleText(bool value)
    {
     
            text.gameObject.SetActive(value);
        
    }

    public void UpdateText(string newtextdisplay)
    {
        text.text = newtextdisplay;
      
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0, Space.Self);
        if (fadetimer <= Time.time)
        {
            gameObject.SetActive(false);
        }
    }
}
