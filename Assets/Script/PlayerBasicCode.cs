using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerBasicCode : MonoBehaviour
{                                               
	private Animator anim;                                                                        
	private int state;
	private float delTime = 0;

	void Start ()
	{
		anim = GetComponent<Animator>();
		state = Animator.StringToHash("State");
	}

	void Update()
	{
		delTime += Time.deltaTime;
		if (delTime > 20)
        {
			anim.SetInteger(state, 1);
			delTime = 0;
		}
	}
}