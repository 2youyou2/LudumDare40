﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmployeeController : MonoBehaviour 
{
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

	public System.Action<bool> OnHolidayTaked;

	public void Init(OfficeDesk officeDesk, float happiness)
	{
		EmployeeStateController = GetComponent<EmployeeStateController>();
		EmployeeStateController.Happiness = happiness;
		EmployeeMovementController = GetComponent<EmployeeMovementController>();
		EmployeeUIController = Instantiate(employeeUIControllerPrefab);
		// EmployeeUIController.transform.SetParent(transform, false);
		// EmployeeUIController.GetComponent<Canvas>().worldCamera = GameMetaManager.Camera.Camera;
		
		EmployeeMovementController.Init(officeDesk, GetComponent<NavMeshController>(), GetComponent<EmployeeAnimationController>(), GetComponent<EmployeeParticlesController>());
		EmployeeUIController.DisableAll();

		EmployeeUIController.OnRequestAnswered += OnRequestAnswered;
		EmployeeUIController.OnForceWork 			 += OnForceWork;
		EmployeeUIController.OnFired 					 += OnFired;

		SetNextRequest();
	}

	void Update()
	{
		if(EmployeeUIController)
		{
			EmployeeUIController.transform.localPosition = transform.localPosition;
			if (holidayEnd == 0)
			{
				EmployeeStateController.UpdateState(this);
			}
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

	public void UngenerateMoney()
	{
		if (raging)
		{
			timeSinceLastGeneration += Time.deltaTime;
			if (timeSinceLastGeneration > 1)
			{
				GameMetaManager.Money.RemoveMoney(employeeStats.MoneyLostRage);
				timeSinceLastGeneration = 0;
				EmployeeUIController.EnableMoneyChange(-employeeStats.MoneyLostRage);
			}
		}
		
	}

	public void OnForceWork()
	{
		GameMetaManager.Employee.OnBackToWork();
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
			EmployeeStateController.DenyRequest();
		}
		EmployeeUIController.CloseRequest();
	}

	public void OnFired()
	{
		GameMetaManager.Employee.OnFired();
		EmployeeMovementController.Fire();
		DestroyEmployee(false);
	}

	public void DestroyEmployee(bool destroy)
	{
		GameMetaManager.Employee.ReleaseEmployee(this);
		Destroy(EmployeeUIController.gameObject);
		if(destroy)
		{
			Destroy(gameObject);
		}
	}

	public void ApplyRequest()
	{
		if (NextRequest == RequestType.PayRaise)
		{
			GameMetaManager.Money.RemoveMoney(RequestValue);
			EmployeeUIController.EnablePayRaise(RequestValue);
			GameMetaManager.Employee.OnPayMoney();
		}
		else if (NextRequest == RequestType.Holidays)
		{
			TakeHolidays();
		}
		SetNextRequest();
	}
	public void TakeHolidays()
	{
		EmployeeMovementController.Holidays();
		GameMetaManager.Employee.OnPlane();
		OnHolidayTaked(true);
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
			GameMetaManager.Employee.OnPlane();
			OnHolidayTaked(false);
			GetComponentInChildren<Renderer>().enabled = true;
			GetComponent<NavMeshController>().enabled = true;
			//GetComponentInChildren<CapsuleCollider>().enabled = true;
			GameMetaManager.Employee.OnDoorFired();
			GameMetaManager.Time.OnDayPassed -= CheckEndOfHolidays;
		}
	}

	public void SetNextRequest()
	{
		if(UnityEngine.Random.Range(0.0f, 1.0f) < employeeStats.payRaiseProbability)
    {
    	NextRequest = RequestType.PayRaise;
			RequestValue = UnityEngine.Random.Range(employeeStats.MinPayRaise, employeeStats.MaxPayRaise);
    }
    else
    {
    	NextRequest = RequestType.Holidays;
			RequestValue = employeeStats.HolidayTime;
		}
	}

	public void ReleaseEmployee()
	{
		if (!releasing)
		{
			if(UnityEngine.Random.Range(0.0f, 1.0f) > 0.6f)
			{
				EmployeeMovementController.MoveToCrazyTarget(true);
				EmployeeUIController.EnableFire();
				raging = true;
			}
			else
			{
				EmployeeMovementController.MoveToCrazyTarget(false);
				GameMetaManager.Money.RemoveMoney(employeeStats.MoneyLostFly);
				EmployeeUIController.EnableMoneyChange(-employeeStats.MoneyLostRage);
			}
			releasing = true;
			EmployeeUIController.OnRequestAnswered -= OnRequestAnswered;
		}
	}

	public enum RequestType
	{
		PayRaise,
		Holidays
	}

	private float timeSinceLastGeneration;
	public int holidayEnd;
	private bool releasing = false;

	private bool raging = false;

	[SerializeField]
	private EmployeeUIController employeeUIControllerPrefab;
}
