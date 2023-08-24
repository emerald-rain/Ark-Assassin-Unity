using System;
using System.Collections;
using UnityEngine;

public class Enemy : Person
{
	private bool isArcheryUp;

	private float angleMax = 35f;

	private float angleFire;

	private float angleCurrent;

	public bool addFireArrowSkill;

	public override void init()
	{
		base.init();
		isArcheryUp = false;
	}

	public override void idle()
	{
		base.idle();
		if (addFireArrowSkill)
		{
			setFireArrowEnemy();
		}
	}

	public override void die()
	{
		base.die();
		int num = UnityEngine.Random.Range(2, 8);
		for (int i = 0; i < num; i++)
		{
			Coin coin = ObjectPooling.ins.getCoin();
			coin.transform.position = base.transform.position;
			coin.objTarget = gameManager.hero.gameObject;
			coin.gameManager = gameManager;
			coin.gameObject.SetActive(value: true);
			coin.onFire();
		}
		gameManager.hero.onDisableActionHero();
		delayFunction(0.5f, delegate
		{
			gameManager.nextRound();
		});
		if (is_Boss)
		{
			gameManager.ui_Controller.fillHpBoss.transform.parent.gameObject.SetActive(value: false);
		}
		delayFunction(4f, delegate
		{
			disableObj();
		});
		gameManager.dataHolder.gameData.numEnemyKill++;
		if (is_Boss)
		{
			gameManager.dataHolder.gameData.numKillBoss++;
		}
	}

	public override void hit(int dame, BodyParts bodyParts, Arrow arrow)
	{
		base.hit(dame, bodyParts, arrow);
		if (GameManager.is_gameStart)
		{
			switch (bodyParts)
			{
			case BodyParts.Foot:
				dame = (int)((float)dame * 0.7f);
				playAnimBody("hitFoot");
				break;
			case BodyParts.other:
				dame = (int)((float)dame * 0.5f);
				playAnimBody("hitHead");
				break;
			case BodyParts.Head:
				dame = (int)((float)dame * 2f);
				gameManager.camVibrate();
				playAnimBody("hitHead");
				break;
			default:
				playAnimBody("hitBody");
				break;
			}
			NumberSpr effNum = ObjectPooling.ins.getEffNum();
			effNum.onShow(dame, base.transform.position + new Vector3(0f, 1.5f, 0f));
			delayFunction(1.2f, delegate
			{
				gameManager.addScore(dame);
			});
		}
	}

	private void playAnimBody(string nameAnim)
	{
		if (body_animator.enabled && !is_die)
		{
			body_animator.Play(nameAnim, 0, 0f);
		}
	}

	public void disableObj()
	{
		arrowParrent.transform.SetParent(handLeft_rotate.transform);
		rigBody.bodyType = RigidbodyType2D.Static;
		for (int i = 0; i < numArrow; i++)
		{
			if (arrows[i] != null)
			{
				ObjectPooling.ins.resetArrow(arrows[i]);
				arrows[i] = null;
			}
		}
		arrowParrent.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		if (platformSetCollider != null)
		{
			platformSetCollider.setCollider(act: false);
		}
		ObjectPooling.ins.addEnemy(this);
		base.gameObject.SetActive(value: false);
	}

	public override void attack()
	{
		base.attack();
	}

	public void autoSetPosition(bool isBoss)
	{
		if (platformIdle != null)
		{
			Vector3 localScale = platformIdle.transform.localScale;
			Vector3 vector;
			if (localScale.x >= 0f)
			{
				Transform transform = base.transform;
				Vector2 posIdle = platformIdle.getPosIdle();
				transform.position = new Vector3(-6f, posIdle.y, 0f);
				Transform transform2 = base.transform;
				Vector3 localScale2 = base.transform.localScale;
				float x = Math.Abs(localScale2.x);
				Vector3 localScale3 = base.transform.localScale;
				float y = localScale3.y;
				Vector3 localScale4 = base.transform.localScale;
				transform2.localScale = new Vector3(x, y, localScale4.z);
				vector = platformIdle.getPosIdle() + new Vector2(-0.4f, 0f);
			}
			else
			{
				Transform transform3 = base.transform;
				Vector2 posIdle2 = platformIdle.getPosIdle();
				transform3.position = new Vector3(6f, posIdle2.y, 0f);
				Transform transform4 = base.transform;
				Vector3 localScale5 = base.transform.localScale;
				float x2 = 0f - Math.Abs(localScale5.x);
				Vector3 localScale6 = base.transform.localScale;
				float y2 = localScale6.y;
				Vector3 localScale7 = base.transform.localScale;
				transform4.localScale = new Vector3(x2, y2, localScale7.z);
				vector = platformIdle.getPosIdle() + new Vector2(0.4f, 0f);
			}
			float num = 0f;
			if (!isBoss)
			{
				num = Vector2.Distance(vector, base.transform.position) * 0.3f;
				iTween.MoveTo(base.gameObject, iTween.Hash("position", vector, "time", num, "easetype", iTween.EaseType.linear));
				playAnim("Run");
			}
			else
			{
				base.transform.position = vector + new Vector3(0f, 5f, 0f);
				num = 1f;
				iTween.MoveTo(base.gameObject, iTween.Hash("position", vector, "time", 0.3f, "easetype", iTween.EaseType.linear));
				delayFunction(0.3f, delegate
				{
					playAnim("Jump3");
				});
			}
			delayFunction(num, delegate
			{
				playAnim("Idle");
				StartCoroutine(delayAttact(UnityEngine.Random.Range(betweenTurnAtackMin, betweenTurnAtackMax)));
			});
		}
	}

	public void reAttack()
	{
		StartCoroutine(delayAttact(UnityEngine.Random.Range(betweenTurnAtackMin, betweenTurnAtackMax)));
	}

	private IEnumerator delayAttact(float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		if (!is_die && GameManager.is_gameStart)
		{
			playAnimBody("Idle");
			yield return new WaitForSeconds(0.05f);
			archeryDown();
			angleCurrent = 120f;
			isArcheryUp = true;
			float angle = Vector2.Angle(-gameManager.hero.transform.position + base.transform.position, Vector2.up);
			float distance = Vector2.Distance(base.transform.position, gameManager.hero.transform.position);
			int rd = UnityEngine.Random.Range(0, 100);
			if (rd <= rateHit)
			{
				angleFire = angle + UnityEngine.Random.Range(0f - distance, distance * 1.8f);
				force = 10f;
			}
			else
			{
				angleFire = UnityEngine.Random.Range(angleCurrent, angleMax);
				force = UnityEngine.Random.Range(0.5f, 2.2f);
			}
			float rdSub = UnityEngine.Random.Range(-5f, 10f);
			setValueTo(angleCurrent, angleFire + rdSub, 0.5f, 0f);
			setValueTo(angleFire + rdSub, angleFire, 0.5f, 0.5f);
		}
		yield return new WaitForSeconds(1f);
		isArcheryUp = false;
		if (!is_die && GameManager.is_gameStart)
		{
			archeryUp(force, angleFire);
		}
		yield return new WaitForSeconds(0.2f);
		if (!is_die && GameManager.is_gameStart)
		{
			attack();
			StartCoroutine(delayAttact(UnityEngine.Random.Range(betweenTurnAtackMin, betweenTurnAtackMax)));
		}
	}

	private void Update()
	{
		if (!is_die && GameManager.is_gameStart && isArcheryUp)
		{
			archeryUp(10f, angleCurrent);
		}
	}

	private void setValueTo(float _from, float _to, float _time, float timeDelay)
	{
		iTween.ValueTo(base.gameObject, iTween.Hash("from", _from, "to", _to, "time", _time, "delay", timeDelay, "onupdatetarget", base.gameObject, "onupdate", "tweenOnUpdateCallBack", "easetype", iTween.EaseType.easeOutQuad));
	}

	private void tweenOnUpdateCallBack(int newValue)
	{
		angleCurrent = newValue;
	}

	public void setFireArrowEnemy()
	{
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
