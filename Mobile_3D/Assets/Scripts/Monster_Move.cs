﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Monster_Move : MonoBehaviour
{
    public List<Transform> movePoints;
    public int nextPoint = 0;
    public int monster_Energy = 5;
    public NavMeshAgent Monster_Agent;
    public Animator animator;
    public Transform heroTr;
    private WaitForSeconds wfs;
    public bool isPatrolling;
    public Vector3 targetPosition;

    private void OnEnable()
    {
        StartCoroutine(CheckMonster());
        Debug.Log("OnEnable");
    }

    IEnumerator CheckMonster()
    {
        while(true)
        {
            yield return wfs;
            float distance = Vector3.Distance(this.transform.position, heroTr.position);

            if(distance <= 2.0f)
            {
                Monster_Agent.speed = 0.1f;
                Monster_Agent.autoBraking = false;
            }
            else if(distance > 2.0f && distance <= 8.0f)
            {
                Monster_Agent.autoBraking = false;
                Monster_Agent.speed = 1.0f;
                isPatrolling = false;

                ApproachTarget(heroTr.position);
            }
            else
            {
                //patrol Mode
                Monster_Agent.speed = 0.6f;
                isPatrolling = true;
                Monster_Agent.destination = movePoints[nextPoint].position;
            }
        }
    }

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        heroTr = player.GetComponent<Transform>();
        wfs = new WaitForSeconds(0.4f);
    }

    void Start()
    {
        Monster_Agent = GetComponent<NavMeshAgent>();
        Monster_Agent.autoBraking = false;

        animator = GetComponent<Animator>();
        var p_group = GameObject.Find("EnemyMovePos");

        p_group.GetComponentsInChildren<Transform>(movePoints);
        movePoints.RemoveAt(0);
        isPatrolling = true;
        MoveMonster();
        Monster_Agent.speed = 1f;
    }

    void MoveMonster()
    {
        if (isPatrolling)
            Monster_Agent.destination = movePoints[nextPoint].position;
        Monster_Agent.isStopped = false;
    }

    void ApproachTarget(Vector3 pos)
    {
        if (Monster_Agent.isPathStale) return;
        Monster_Agent.destination = pos;
        Monster_Agent.isStopped = false;
    }
    
    void Update()
    {
        if (!isPatrolling) return;

        if(Monster_Agent.remainingDistance <= 0.5f)
        {
            nextPoint = ++nextPoint % movePoints.Count;
            MoveMonster();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("MISSILE"))
        {
            monster_Energy--;
        }
        else return;
        if (monster_Energy < 0)
        {
            Destroy(this.gameObject, 0.1f);
            GameManager.instance.UpgradeScore();


        }
    }

    void SpawnMonster()
    {
        monster_Energy = 5;
        this.transform.position = new Vector3(Random.Range(-1.0f, 1.0f), 0.5f, Random.Range(0, 1));
        this.gameObject.SetActive(true);
    }
}
