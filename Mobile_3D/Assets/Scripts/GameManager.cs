using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameObject scoreObj;
    Text scoreTxt;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public static int mScore = 0;
    
    void Start()
    {
        scoreObj = GameObject.Find("Score");
        scoreTxt = scoreObj.GetComponent<Text>();
        scoreTxt.text = "Score : " + mScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeScore()
    {
        mScore++;
        scoreTxt.text = "Score : " + mScore.ToString();
    }
}
