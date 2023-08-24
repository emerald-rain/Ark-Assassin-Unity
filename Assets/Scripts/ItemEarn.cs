using UnityEngine;

public class ItemEarn : E_MonoBehaviour
{
	public SpriteRenderer spr;

	public CircleCollider2D circle;

	public GameObject eff;

	public Sprite[] items;

	public ToolControl[] targets;

	public Transform parrent;

	private int id;

	public GameManager gameManager;

	private bool isActive;

	public static ItemEarn ins;

	public Animator anim;

	public AudioSource audioSource;

	private void Awake()
	{
		ins = this;
	}

	public void onShow(int id, bool hadAnim)
	{
		if (!isActive)
		{
			isActive = true;
			this.id = id;
			anim.enabled = hadAnim;
			spr.sprite = items[id];
			spr.enabled = true;
			circle.enabled = true;
			eff.SetActive(value: true);
			delayFunction(10f, delegate
			{
				onHide();
			});
		}
	}

	public void onHide()
	{
		spr.enabled = false;
		circle.enabled = false;
		eff.SetActive(value: false);
		isActive = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (GameManager.is_gameStart && collision.gameObject.tag.Equals("Arrow"))
		{
			Arrow component = collision.GetComponent<Arrow>();
			if (component.fromHero)
			{
				anim.enabled = false;
				base.transform.localPosition = Vector3.zero;
				iTween.MoveTo(parrent.gameObject, targets[id].transform.position, 1.5f);
				delayFunction(1f, delegate
				{
					getGift();
				});
				audioSource.volume = gameManager.dataHolder.gameData.soundVolume;
				audioSource.Play();
			}
		}
	}

	private void getGift()
	{
		int num = 0;
		switch (id)
		{
		case 0:
			gameManager.dataHolder.gameData.numHealing++;
			num = gameManager.dataHolder.gameData.numHealing;
			break;
		case 1:
			gameManager.dataHolder.gameData.numFireArrow++;
			num = gameManager.dataHolder.gameData.numFireArrow;
			break;
		case 2:
			gameManager.dataHolder.gameData.numProtect++;
			num = gameManager.dataHolder.gameData.numProtect;
			break;
		case 3:
			gameManager.dataHolder.gameData.numX3++;
			num = gameManager.dataHolder.gameData.numX3;
			break;
		}
		targets[id].onShow(num);
		gameManager.saveData();
	}
}
