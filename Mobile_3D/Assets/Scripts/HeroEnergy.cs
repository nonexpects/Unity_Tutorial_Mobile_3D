using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroEnergy : MonoBehaviour
{
    public Image mEnergy;
    public float currentEnergy;
    public float fullEnergy = 100f;

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = fullEnergy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("MUMMY"))
        {
            currentEnergy -= 10f;
            mEnergy.fillAmount = currentEnergy / fullEnergy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
