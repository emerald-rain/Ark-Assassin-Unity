using System;
using System.Collections;
using UnityEngine;

public class E_MonoBehaviour : MonoBehaviour
{
	public void delayFunction(float time, Action action)
	{
		StartCoroutine(ieDelayFunction(time, action));
	}

	private IEnumerator ieDelayFunction(float time, Action action)
	{
		yield return new WaitForSeconds(time);
		action();
	}
}
