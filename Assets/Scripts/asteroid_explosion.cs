using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid_explosion : MonoBehaviour

{
    void Start()
    {
        Destroy(this.gameObject, 3f);
    }
}
