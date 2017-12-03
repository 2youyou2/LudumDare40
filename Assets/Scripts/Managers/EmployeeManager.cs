﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager {

	public List<EmployeeController> EmployeeList
	{
		get;
		private set;
	}

	public int CurrentEmployees
	{
		get
		{
			return EmployeeList.Count;
		}
	}

	public EmployeeManager(EmployeeGenerator employeeGenerator)
	{
		this.employeeGenerator = employeeGenerator;
    EmployeeList = new List<EmployeeController>();
		//TO-DO ESTO ES UNA INSTANCIACIÓN DE PRUEBA!
	}
	
	public void CreateNewEmployee(OfficeDesk officeDesk)
	{
		EmployeeController employeeController = employeeGenerator.InstantiateEmployeePrefab(officeDesk.transform);
		employeeController.Init(officeDesk);
    EmployeeList.Add(employeeController);
  }

	public bool TryCreateNewEmployee()
	{
    if (GameMetaManager.Office.GetEmptyDeskCount() > 0)
    {
      CreateNewEmployee(GameMetaManager.Office.GetEmptyDesk());
			return true;
    }
		else
		{
			return false;
		}
	}


	private EmployeeGenerator employeeGenerator;

}
