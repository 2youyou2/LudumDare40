﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeMovementController : MonoBehaviour {

  public Transform Target
  {
    get;
    private set;
  }

  public float Speed
  {
    get;
    set;
  }

  public void Init(OfficeDesk officeDesk, NavMeshController navMeshController, EmployeeAnimationController animController, EmployeeParticlesController particlesController)
  {
    personalOfficeDesk = officeDesk;
    personalOfficeDesk.Filled = true;

    this.navMeshController = navMeshController;
    this.animController = animController;
    this.particlesController = particlesController;

    Speed = 1;
  }

  public void MoveToProcrastinationTarget()
  {
    if (Target == personalOfficeDesk.Transform)
    {
      selectNewTarget();
      navigate(GoToRandom);
    }
  }

  public void MoveToCrazyTarget()
  {
    GoToDestroy();
    // DESPUÉS DE LA ANIMACIÓN RECORDAR EL DESTROY Y personalOfficeDesk.Filled = false!
  }

  public void GoToRandom()
  {
    selectNewTarget();
    navigate(GoToRandom);
  }

  public void MoveToDesk()
  {
    if (Target != personalOfficeDesk.Transform)
    {
      Target = personalOfficeDesk.Transform;
      navigate(Work);
    }
  }

  public void GoToDestroy()
  {
    if (Target == personalOfficeDesk.Transform)
    {
      navigate(DestroyAnim);
      selectNewTarget();      
    }
  }

  public void Work()
  {
    animController.WorkAnim();
  }

  public void Idle()
  {
    animController.IdleAnim();
  }

  public void DestroyAnim()
  {
    animController.DestroyAnim();
    particlesController.PlayDestroyComputerParticles();
  }

  private void selectNewTarget()
  {
    Target = GameMetaManager.Office.GetRandomIdle().transform;
  }

  private void navigate(NavMeshController.OnNavigateFinished del)
  {
    navMeshController.NavigateTo(Target, Speed, del);
    animController.WalkAnim();
  }

  private OfficeDesk personalOfficeDesk;
  private NavMeshController navMeshController;
  private EmployeeAnimationController animController;

  private EmployeeParticlesController particlesController;

}
