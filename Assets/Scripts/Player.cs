using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      transform.position = new Vector3 (0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
     transform.Translate(new Vector3(Input.GetAxis("Horizontal"),
     Input.GetAxis("Vertical"), 0) * 5 * Time.deltaTime); 
    }
}