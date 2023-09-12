using UnityEngine;

public class Taopaipai : Enemy
{
	public GameObject protect;

	private bool theFirst;

	public override void init()
	{
		base.init();
		theFirst = true;
	}

	public override void hit(int dame, BodyParts bodyParts, Arrow arrow)
	{
		base.hit(dame, bodyParts, arrow);
		if (theFirst && HP_current < 150f)
		{
			protect.SetActive(value: true);
			theFirst = false;
			Invoke("disableProtect", 3f);
		}
	}

	private void disableProtect()
	{
		protect.SetActive(value: false);
	}

	public override void die()
	{
		base.die();
		CancelInvoke("disableProtect");
		disableProtect();
	}
}
