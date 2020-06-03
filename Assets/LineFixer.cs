using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFixer : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();
    public LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = points.Count;
      
          for (int i = 0; i <= points.Count-1;i++)
        {
            line.SetPosition(i, new Vector3(points[i].transform.position.x, points[i].transform.position.y+1, points[i].transform.position.z));
        }
        
    }

    public void UpdateLine()
    {
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
