public class Frieza1 : Enemy
{
	public override void idle()
	{
		base.idle();
		if (is_die || !GameManager.is_gameStart)
		{
			return;
		}
		for (int i = 0; i < arrows.Length; i++)
		{
			if (arrows[i] != null)
			{
				arrows[i].fireEff.SetActive(value: true);
			}
		}
	}
}
