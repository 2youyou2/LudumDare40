﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Patrol : MonoBehaviour
{
  public OfficeGenerator office;
  public Vector2 MinMaxStopTime;
  private int destPointIndex = 0;
  private NavMeshAgent agent;
  private float timeElapsed;
  private Transform currentPoint;

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    agent.autoBraking = true;
    timeElapsed = 0.0f;
    setRandomSpeed();
    GotoNextPoint();
  }

  void Update()
  {
    if ((!agent.pathPending && agent.remainingDistance < 0.01f) || agent.isStopped )
    {
      agent.isStopped = true;
      if (timeElapsed > Random.Range(MinMaxStopTime.x, MinMaxStopTime.y))
      {
        timeElapsed = 0.0f;
        GotoNextPoint();
      }
      else
      {
        timeElapsed += Time.deltaTime;
      }
    }
  }

  private void GotoNextPoint()
  {
    if (office.DeskTransformList.Count == 0)
    {
      return;
    }

    agent.isStopped = false;
    setRandomSpeed();


    destPointIndex = Random.Range(0, office.DeskTransformList.Count);
    agent.destination = office.DeskTransformList[destPointIndex].position;
  }

  private void setRandomSpeed()
  {
    agent.speed = Random.Range(0.5f, 3f);
  }
}