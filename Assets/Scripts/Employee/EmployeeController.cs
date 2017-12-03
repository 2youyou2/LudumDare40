﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmployeeController : MonoBehaviour {

	public EmployeeStateController EmployeeStateController
	{
		get;
		private set;
	}

	public EmployeeMovementController EmployeeMovementController
	{
		get;
		private set;
	}

	public EmployeeUIController EmployeeUIController
	{
		get;
		private set;
	}

	public RequestType NextRequest
	{
		get;
		set;
	}

		public int RequestValue
	{
		get;
		set;
	}
	
	public EmployeeStats employeeStats
	{
		get {return GameMetaManager.Employee.EmployeeStats;}
	}


	public void Init(OfficeDesk officeDesk, float happiness)
	{
		EmployeeStateController = GetComponent<EmployeeStateController>();
		EmployeeStateController.Happiness = happiness;
		EmployeeMovementController = GetComponent<EmployeeMovementController>();
		EmployeeUIController = GetComponentInChildren<EmployeeUIController>();

		EmployeeMovementController.Init(officeDesk, GetComponent<NavMeshController>(), GetComponent<EmployeeAnimationController>());
		EmployeeUIController.DisableAll();

		EmployeeUIController.OnRequestAnswered += OnRequestAnswered;
		EmployeeUIController.OnForceWork += OnForceWork;

		SetNextRequest();
	}

	void Update()
	{
		if (holidayEnd == 0)
		{
			EmployeeStateController.UpdateState(this);
		}
	}

	public void GenerateMoney()
	{
		timeSinceLastGeneration += Time.deltaTime;
		if (timeSinceLastGeneration > 1)
		{
			GameMetaManager.Money.AddMoney(employeeStats.MoneyGenerated);
			timeSinceLastGeneration = 0;
			EmployeeUIController.EnableMoneyChange(employeeStats.MoneyGenerated);
		}

	}

	public void OnForceWork()
	{
		EmployeeStateController.ForceWorkAgain();
		EmployeeUIController.DisableAll();
	}

	public void OnRequestAnswered(bool accepted)
	{
		if (accepted)
		{
			EmployeeStateController.AcceptRequest();
			ApplyRequest();
		}
		else
		{
			EmployeeStateController.ForceWorkAgain();
		}
		EmployeeUIController.CloseRequest();
	}

	public void ApplyRequest()
	{
		Debug.Log("REQUEST " + NextRequest);
		if (NextRequest == RequestType.PayRaise)
		{
			GameMetaManager.Money.RemoveMoney(RequestValue);
			EmployeeUIController.EnableMoneyChange(-RequestValue);
		}
		else if (NextRequest == RequestType.Holidays)
		{
			TakeHolidays();
		}
		SetNextRequest();
	}
	public void TakeHolidays()
	{
		GetComponentInChildren<Renderer>().enabled = false;
		holidayEnd = RequestValue;
		GameMetaManager.Time.OnDayPassed += CheckEndOfHolidays;
	}

	public void CheckEndOfHolidays()
	{
		if (holidayEnd != 0)
		{
			holidayEnd--;
		}
		if (holidayEnd == 0)
		{
			GetComponentInChildren<Renderer>().enabled = true;
			GameMetaManager.Time.OnDayPassed -= CheckEndOfHolidays;
		}
	}

	public void SetNextRequest()
	{
		NextRequest = (RequestType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RequestType)).Length);
		if (NextRequest == RequestType.PayRaise)	
		{
			RequestValue = UnityEngine.Random.Range(employeeStats.MinPayRaise, employeeStats.MaxPayRaise);
		}
		else if (NextRequest == RequestType.Holidays)
		{
			RequestValue = employeeStats.HolidayTime;
		}
	}

	public void ReleaseEmployee()
	{
		EmployeeMovementController.MoveToCrazyTarget();
		EmployeeUIController.OnRequestAnswered -= OnRequestAnswered;
	}

	public enum RequestType
	{
		PayRaise,
		Holidays
	}

	private float timeSinceLastGeneration;
	private int holidayEnd;
}
