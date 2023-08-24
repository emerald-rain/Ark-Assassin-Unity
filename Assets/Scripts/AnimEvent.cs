using UnityEngine;

public class AnimEvent : MonoBehaviour
{
	public Hero hero;

	public void moveDone()
	{
		hero.moveDone();
	}

	public void disableObj()
	{
		base.gameObject.SetActive(value: false);
	}
}
