using System;
using System.Collections;
using UnityEngine;

public class Hero : Person
{
	public GameObject t_head;

	public GameObject t_body;

	public GameObject t_foot;

	public GameObject protect;

	public float timeLifeProtect;

	public SetSpriteHero[] setSpriteHeroes;

	public GameObject tocSave;

	public GameObject[] toc_songoku;

	public GameObject[] toc_vegeta;

	public HingeJoint2D hinge1;

	public HingeJoint2D hinge2;

	public AudioClip audio_start;

	public GameObject effectUpgradeArchery;

	public GameObject effectContinue;

	public override void init()
	{
		setHandRight(isActive: false);
		base.init();
		idArchery = gameManager.dataHolder.gameData.ArcheryId;
		requestDataHero();
		requestSpriteHero();
		if (platformSetCollider != null)
		{
			platformSetCollider.setCollider(act: false);
		}
	}

	public override void idle()
	{
		setHandRight(isActive: false);
		base.idle();
	}

	public override void archeryDown()
	{
		base.archeryDown();
		setHandRight(isActive: true);
	}

	private void setHandRight(bool isActive)
	{
		if (isActive)
		{
			rid1.bodyType = RigidbodyType2D.Dynamic;
			rid2.bodyType = RigidbodyType2D.Dynamic;
			hinge1.enabled = true;
			hinge2.enabled = true;
			rid1.simulated = true;
			rid2.simulated = true;
		}
		else
		{
			rid1.bodyType = RigidbodyType2D.Static;
			rid2.bodyType = RigidbodyType2D.Static;
			hinge1.enabled = false;
			hinge2.enabled = false;
			rid1.simulated = false;
			rid2.simulated = false;
		}
	}

	public void onShowEffContinue()
	{
		effectContinue.SetActive(value: true);
		delayFunction(2f, delegate
		{
			effectContinue.SetActive(value: false);
		});
	}

	public void requestDataHero()
	{
		HeroPoints heroPoints = GetHeroPoints();
		HP_before = heroPoints.points[heroPoints.level].hp;
		HP = HP_before;
		HP_current = HP;
		updatePercent();
		dame = heroPoints.points[heroPoints.level].dame;
		defense = heroPoints.points[heroPoints.level].defense;
	}

	public void requestSpriteHero()
	{
		for (int i = 0; i < setSpriteHeroes.Length; i++)
		{
			setSpriteHeroes[i].onsetSprite(gameManager.dataHolder.gameData.heroEquiped);
		}
		GameObject[] array = (!gameManager.dataHolder.gameData.heroEquiped.ToLower().Equals("songoku")) ? toc_vegeta : toc_songoku;
		HeroPoints heroPoints = (!gameManager.dataHolder.gameData.heroEquiped.ToLower().Equals("songoku")) ? gameManager.dataHolder.heroData.heroPoints[1] : gameManager.dataHolder.heroData.heroPoints[0];
		if (tocSave != null)
		{
			tocSave.SetActive(value: false);
		}
		tocSave = array[heroPoints.level];
		tocSave.SetActive(value: true);
	}

	private HeroPoints GetHeroPoints()
	{
		for (int i = 0; i < gameManager.dataHolder.heroData.heroPoints.Length; i++)
		{
			if (gameManager.dataHolder.gameData.heroEquiped.ToLower().Equals(gameManager.dataHolder.heroData.heroPoints[i].nameHero.ToLower()))
			{
				return gameManager.dataHolder.heroData.heroPoints[i];
			}
		}
		return null;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Space) && canMove)
		{
			moveNextPlatform();
		}
		if (UnityEngine.Input.GetKeyUp(KeyCode.D) && canMove)
		{
			base.die();
		}
		if (timeLifeProtect > 0f)
		{
			timeLifeProtect -= Time.deltaTime;
		}
		else if (protect.activeSelf)
		{
			protect.SetActive(value: false);
		}
	}

	public void OnEnableProtect()
	{
		timeLifeProtect = 10f;
		protect.SetActive(value: true);
	}

	public void OnDisableProtect()
	{
		timeLifeProtect = 0f;
	}

	public override void hit(int dame, BodyParts bodyParts, Arrow arrow)
	{
		base.hit(dame, bodyParts, arrow);
		if (!is_die && GameManager.is_gameStart)
		{
			switch (bodyParts)
			{
			case BodyParts.Foot:
				dame = (int)((float)dame * 0.7f);
				break;
			case BodyParts.Head:
				dame = (int)((float)dame * 2f);
				gameManager.camVibrate();
				break;
			}
			dame -= defense + defeseArchery;
			NumberSpr effNum = ObjectPooling.ins.getEffNum();
			effNum.onShow(dame, base.transform.position + new Vector3(0f, 1.5f, 0f), Color.red);
		}
	}

	public void onDisableActionHero()
	{
		canMove = false;
		canAttack = false;
	}

	public void moveNextPlatform()
	{
		setHandRight(isActive: false);
		is_idle = false;
		canMove = false;
		canAttack = false;
		Vector3 localScale = base.transform.localScale;
		float num = (!(localScale.x > 0f)) ? (platformIdle.getPosXPlatform1() + 0.27f) : (platformIdle.getPosXPlatform1() - 0.27f);
		int num2 = platformIdle.getNum();
		Vector3 position = base.transform.position;
		float num3 = Math.Abs(position.x - num) * 0.3f;
		body_animator.enabled = true;
		body_animator.Play("Run");
		lineArchery.SetPositions(points_idle);
		for (int i = 0; i < numArrow; i++)
		{
			if (arrows[i] == null)
			{
				arrows[i] = ObjectPooling.ins.getArrow(idArchery);
				arrows[i].transform.SetParent(arrowParrent.transform);
			}
			arrows[i].transform.localPosition = new Vector3(-0.5f, 0f, 0f);
			arrows[i].transform.localEulerAngles = arrowsRotation[i];
			arrows[i].transform.localScale = new Vector3(1f, 1f, 1f);
		}
		GameObject gameObject = base.gameObject;
		object[] obj = new object[6]
		{
			"position",
			null,
			null,
			null,
			null,
			null
		};
		float x = num;
		Vector3 position2 = base.transform.position;
		obj[1] = new Vector3(x, position2.y, 0f);
		obj[2] = "easetype";
		obj[3] = iTween.EaseType.linear;
		obj[4] = "time";
		obj[5] = num3;
		iTween.MoveTo(gameObject, iTween.Hash(obj));
		for (int j = 0; j < num2 - 1; j++)
		{
			delayAnim(num3 + (float)j * 0.5f, "Jump2", 0f);
		}
		delayAnim(num3 + (float)(num2 - 1) * 0.5f, "Jump3", 0.1f);
		delayFunction(num3 + (float)num2 * 0.5f + 0.2f, delegate
		{
			moveDone();
		});
		gameManager.updateListPlatform();
		gameManager.disableActiveDot();
	}

	public void moveDone()
	{
		Transform transform = base.transform;
		Vector3 localScale = base.transform.localScale;
		float x = localScale.x * -1f;
		Vector3 localScale2 = base.transform.localScale;
		float y = localScale2.y;
		Vector3 localScale3 = base.transform.localScale;
		transform.localScale = new Vector3(x, y, localScale3.z);
		platformIdle = gameManager.getNextPlatform(platformIdle);
		canMove = true;
		canAttack = true;
		is_idle = true;
		gameManager.instanceObstales();
	}

	public void delayAnim(float timeDelay, string nameAnim, float sub)
	{
		StartCoroutine(ieDelayAnim(timeDelay, nameAnim, sub));
	}

	private IEnumerator ieDelayAnim(float timeDelay, string nameAnim, float sub)
	{
		yield return new WaitForSeconds(timeDelay);
		body_animator.Play(nameAnim);
		Vector3 localScale = base.transform.localScale;
		if (localScale.x > 0f)
		{
			GameObject gameObject = base.gameObject;
			object[] obj = new object[8]
			{
				"delay",
				0.1f,
				"position",
				null,
				null,
				null,
				null,
				null
			};
			Vector3 position = base.transform.position;
			float x = position.x + 0.5f + sub;
			Vector3 position2 = base.transform.position;
			obj[3] = new Vector3(x, position2.y + 0.4f, 0f);
			obj[4] = "easetype";
			obj[5] = iTween.EaseType.linear;
			obj[6] = "time";
			obj[7] = 0.3f;
			iTween.MoveTo(gameObject, iTween.Hash(obj));
		}
		else
		{
			GameObject gameObject2 = base.gameObject;
			object[] obj2 = new object[8]
			{
				"delay",
				0.1f,
				"position",
				null,
				null,
				null,
				null,
				null
			};
			Vector3 position3 = base.transform.position;
			float x2 = position3.x - 0.5f - sub;
			Vector3 position4 = base.transform.position;
			obj2[3] = new Vector3(x2, position4.y + 0.4f, 0f);
			obj2[4] = "easetype";
			obj2[5] = iTween.EaseType.linear;
			obj2[6] = "time";
			obj2[7] = 0.3f;
			iTween.MoveTo(gameObject2, iTween.Hash(obj2));
		}
	}

	public override void die()
	{
		setHandRight(isActive: true);
		base.die();
		gameManager.gameLose();
	}

	public void updateSprArrow()
	{
		for (int i = 0; i < arrows.Length; i++)
		{
			arrows[i].spr.sprite = ObjectPooling.ins.arrow_sprs[idArchery];
			arrows[i].id = idArchery;
		}
	}

	public void updateSprArrow(int id)
	{
		for (int i = 0; i < arrows.Length; i++)
		{
			arrows[i].spr.sprite = ObjectPooling.ins.arrow_sprs[id];
		}
	}

	public void onShowEffectUpgradeArchery()
	{
		playAnim("upgradeArchery");
		effectUpgradeArchery.SetActive(value: true);
		delayFunction(2f, delegate
		{
			effectUpgradeArchery.SetActive(value: false);
		});
	}
}
