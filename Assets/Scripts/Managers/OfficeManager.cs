﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager
{
	public int CurrentSize
	{
		get
		{
			return officeGenerator.CurrentSize;
		}
	}

	public List<OfficeDesk> DeskList
	{
		get
		{
			return officeGenerator.DeskList;
		}
	}

  public List<IdlePoint> IdleList
  {
    get
    {
      return officeGenerator.IdleList;
    }
  }

  public List<WindowPoint> WindowList
  {
    get
    {
      return officeGenerator.WindowList;
    }
  }

	public Transform DoorPoint
	{
		get {return officeGenerator.DoorPoint;}
	}

	public int PreviousExpandCost
	{
		get;
		private set;
	}

	public event System.Action OnExpansion;

  public OfficeManager(int initialSize, OfficeGenerator officeGenerator, OfficeStats officeStats)
	{
		this.officeGenerator = officeGenerator;
		this.officeGenerator.InitOffice(initialSize);
		this.officeStats = officeStats;
	}

	public int GetExpandCost()
	{
		if(CurrentSize < officeStats.officeExpansionPrices.Count - 1)
		{
			return officeStats.officeExpansionPrices[CurrentSize + 1];
		}
		else
		{
			return officeStats.officeExpansionPrices[officeStats.officeExpansionPrices.Count - 1];
		}
	}

	public int GetExpandTarget()
	{
		if(CurrentSize < officeStats.officeExpansionMoneyTarget.Count - 1)
		{
			return officeStats.officeExpansionMoneyTarget[CurrentSize + 1];
		}
		else
		{
			return officeStats.officeExpansionMoneyTarget[officeStats.officeExpansionMoneyTarget.Count - 1];
		}
	}

	public int GetDailyCost()
	{
		if(CurrentSize < officeStats.officeDailyCost.Count)
		{
			return officeStats.officeDailyCost[CurrentSize];
		}
		else
		{
			return officeStats.officeDailyCost[officeStats.officeDailyCost.Count - 1];
		}
	}

	public bool CanPayExpandCost()
	{
		return GameMetaManager.Money.CanPay(GetExpandCost());
	}

	public bool TryExpand()
	{
		if(CanPayExpandCost())
		{
			PayExpand();
			ExpandOffice();
			return true;
		}
		return false;
	}

	public int GetEmptyDeskCount()
	{
		return DeskList.Count - GameMetaManager.Employee.EmployeeList.Count;
	}

	public OfficeDesk GetEmptyDesk()
	{
		if(GetEmptyDeskCount() <= 0)
		{
			return null;
		}
		foreach(OfficeDesk desk in DeskList)
		{
			if(!desk.Filled)
			{
				return desk;
			}
		}
		return null;
	}

	public IdlePoint GetRandomIdle()
	{
		return IdleList[Random.Range(0, IdleList.Count)];
	}

	public WindowPoint GetRandomWindow()
	{
		return WindowList[Random.Range(0, WindowList.Count)];
	}

	private void PayExpand()
	{
		GameMetaManager.Money.RemoveMoney(GetExpandCost());
		PreviousExpandCost = GetExpandCost();
	}

	private void ExpandOffice()
	{
		officeGenerator.ExpandOffice();
		NotifyOnExpansion();
	}

	private void NotifyOnExpansion()
	{
		if(OnExpansion != null)
		{
			OnExpansion();
		}
	}

	private OfficeGenerator officeGenerator = null;
	private OfficeStats officeStats = null;
}

