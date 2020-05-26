﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFirePos_Gizmos : MonoBehaviour
{
    public Color color1 = Color.yellow;
    public float Gizmos_Radius = 0.08f;

    public void OnDrawGizmos()
    {
        Gizmos.color = color1;
        Gizmos.DrawSphere(transform.position, Gizmos_Radius);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
