﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Employee AI/Actions/Procrastinate")]
public class ProcrastinateAction : Action {

	public override void Act(EmployeeController controller)
	{
		LazeAround(controller);
	}

	private void LazeAround(EmployeeController controller)
	{
		controller.EmployeeMovementController.MoveToProcrastinationTarget();
		Debug.Log ("LAZING AROUND zZzzZ");
	}
}
