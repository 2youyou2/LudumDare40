﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeed : MonoBehaviour {

	void Awake () 
	{
		animator = GetComponent<Animator>();
	}
	
	void Update()
	{
		float ratio = (float)GameMetaManager.Money.CurrentMoney / (float)GameMetaManager.Office.GetExpandTarget();
		animator.speed = Mathf.Lerp(2.5f, 0.5f, ratio);
		Debug.Log(animator.speed);
	}

	private Animator animator;
}
