using UnityEngine;

public class NumberSpr : E_MonoBehaviour
{
	public Sprite[] numSprs;

	public SpriteRenderer[] sprs;

	private Animator animator;

	public Color colorDefault;

	public void onShow(int num, Vector3 pos)
	{
		onShow(num, pos, colorDefault);
	}

	public void onShow(int num, Vector3 pos, Color color)
	{
		for (int i = 0; i < sprs.Length; i++)
		{
			sprs[i].color = color;
		}
		base.transform.position = pos;
		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
		animator.Play("numAnim", 0, 0f);
		if (num < 100)
		{
			sprs[0].transform.localPosition = new Vector3(-0.125f, 0f, 0f);
			sprs[1].transform.localPosition = new Vector3(0.125f, 0f, 0f);
			sprs[2].gameObject.SetActive(value: false);
			sprs[0].sprite = numSprs[num / 10];
			sprs[1].sprite = numSprs[num % 10];
		}
		else
		{
			sprs[0].transform.localPosition = new Vector3(-0.25f, 0f, 0f);
			sprs[1].transform.localPosition = new Vector3(0f, 0f, 0f);
			sprs[2].transform.localPosition = new Vector3(0.25f, 0f, 0f);
			sprs[2].gameObject.SetActive(value: true);
			sprs[0].sprite = numSprs[num / 100];
			sprs[1].sprite = numSprs[num % 100 / 10];
			sprs[2].sprite = numSprs[num % 10];
		}
		delayFunction(1.5f, delegate
		{
			ObjectPooling.ins.addEffNum(this);
		});
	}
}
