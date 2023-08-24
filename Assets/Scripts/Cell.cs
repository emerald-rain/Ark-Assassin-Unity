using UnityEngine;

public class Cell : Enemy
{
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
}
