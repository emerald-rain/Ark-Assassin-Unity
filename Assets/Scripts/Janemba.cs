using UnityEngine;

public class Janemba : Enemy
{
	public GameObject protect;

	private bool theFirst;

	public override void init()
	{
		base.init();
		theFirst = true;
	}

	public override void idle()
	{
		numArrow = 3;
		arrowsRotation = new Vector3[3]
		{
			new Vector3(0f, 0f, -8f),
			Vector3.zero,
			new Vector3(0f, 0f, 8f)
		};
		Arrow arrow = arrows[0];
		arrows = new Arrow[3];
		arrows[0] = arrow;
		base.idle();
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
