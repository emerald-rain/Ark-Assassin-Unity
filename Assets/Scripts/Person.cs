using UnityEngine;

public class Person : E_MonoBehaviour
{
	public bool isHero;

	public int id;

	public int idArchery;

	public int defeseArchery;

	public SpriteRenderer archerySpr;

	public string _name;

	public int HP_before;

	[HideInInspector]
	public int HP;

	public float HP_current;

	public int dame;

	public int defense;

	public float betweenTurnAtackMin;

	public float betweenTurnAtackMax;

	public bool is_die;

	public int rateHit;

	public bool is_Boss;

	public ObjBase handLeft_rotate;

	public LineRenderer lineArchery;

	[HideInInspector]
	public Vector3[] points_idle = new Vector3[3]
	{
		new Vector3(-0.35f, 1.35f, 0f),
		new Vector3(-0.35f, 0f, 0f),
		new Vector3(-0.35f, -1.35f, 0f)
	};

	private Vector3[] points = new Vector3[3];

	public ObjBase leftHand;

	public GameObject pointArchery;

	public GameObject pointHandLeft;

	public Arrow[] arrows;

	public int numArrow = 1;

	public GameObject arrowParrent;

	[HideInInspector]
	public float force;

	public bool canAttack = true;

	public bool canMove = true;

	public Animator body_animator;

	public Platform platformIdle;

	public GameManager gameManager;

	public Animator imgHPfade;

	public GameObject imgHPfill;

	[HideInInspector]
	public Rigidbody2D rigBody;

	public HingeJoint2D[] hinges;

	public Rigidbody2D[] rigids;

	public Rigidbody2D rid1;

	public Rigidbody2D rid2;

	[HideInInspector]
	public Platform platformSetCollider;

	[HideInInspector]
	public Vector3[] arrowsRotation = new Vector3[1]
	{
		Vector3.zero
	};

	public GameObject eff_light;

	public GameObject eff_hp;

	[HideInInspector]
	public bool is_idle;

	public GameObject fireEffectBody;

	public Animator[] anim_add;

	[Header("_____________ sound ____________")]
	public AudioSource audioSource;

	public AudioClip hit_1;

	public AudioClip hit_2;

	public AudioClip audio_die;

	public virtual void init()
	{
		rid1.mass = 20f;
		rid2.mass = 20f;
		numArrow = 1;
		HP = ((!is_Boss) ? HP_before : (HP_before * 2));
		if (arrows.Length == 0)
		{
			arrows = new Arrow[numArrow];
		}
		else
		{
			for (int i = 0; i < arrows.Length; i++)
			{
				if (arrows[i] != null)
				{
					arrows[i].destroyArrowNow();
				}
			}
			arrows = new Arrow[numArrow];
		}
		for (int j = 0; j < rigids.Length; j++)
		{
			rigids[j].bodyType = RigidbodyType2D.Static;
		}
		for (int k = 0; k < hinges.Length; k++)
		{
			hinges[k].enabled = false;
		}
		pointArchery.SetActive(value: true);
		pointHandLeft.SetActive(value: true);
		arrowParrent.GetComponent<ObjBase>().onReset();
		if (rigBody == null)
		{
			rigBody = body_animator.gameObject.GetComponent<Rigidbody2D>();
		}
		HP_current = HP;
		updatePercent();
		is_die = false;
		canMove = true;
		body_animator.gameObject.transform.localPosition = new Vector3(0f, 3f, 0f);
		body_animator.transform.localEulerAngles = Vector3.zero;
		archeryDown();
		idle();
		if (anim_add.Length > 0)
		{
			for (int l = 0; l < anim_add.Length; l++)
			{
				if (anim_add[l] != null)
				{
					anim_add[l].enabled = true;
				}
			}
		}
		if (!isHero)
		{
			archerySpr.sprite = gameManager.dataHolder.getArcherySpr(idArchery);
		}
		updateDefenseArchery();
	}

	public virtual void hit(int dame, BodyParts bodyParts, Arrow arrow)
	{
		if (!is_die && GameManager.is_gameStart)
		{
			switch (bodyParts)
			{
			case BodyParts.Foot:
				dame = (int)((float)dame * 0.7f);
				break;
			case BodyParts.other:
				dame = (int)((float)dame * 0.5f);
				break;
			case BodyParts.Head:
			{
				dame = (int)((float)dame * 2f);
				GameObject effectHeadshot = ObjectPooling.ins.getEffectHeadshot();
				Vector3 localScale = base.transform.localScale;
				Vector3 b = (!(localScale.x > 0f)) ? new Vector3(-0.5f, 1.5f, 0f) : new Vector3(0.5f, 1.5f, 0f);
				effectHeadshot.transform.position = base.transform.position + b;
				effectHeadshot.SetActive(value: true);
				break;
			}
			}
			HP_current -= dame - defense - defeseArchery;
			if (arrow.fireEff.activeSelf)
			{
				arrow.fireEff.SetActive(value: false);
				fireEffectBody.SetActive(value: true);
				delayFunction(1f, delegate
				{
					fireEffectBody.SetActive(value: false);
				});
			}
			if (HP_current <= 0f)
			{
				die();
				playAudio(audio_die);
				return;
			}
			updatePercent();
			int num = Random.Range(0, 10);
			AudioClip audioClip = (num >= 5) ? hit_2 : hit_1;
			playAudio(audioClip);
		}
	}

	public virtual void attack()
	{
		ArcheryD archeryD = gameManager.dataHolder.archeryData.listArchery[idArchery];
		int num = archeryD.dame + archeryD.level * 2;
		if (force > 0.35f)
		{
			canAttack = false;
			for (int i = 0; i < numArrow; i++)
			{
				arrows[i].fire(force, isHero);
				arrows[i].dame = ((!arrows[i].fireEff.activeSelf) ? dame : ((int)((float)dame * 1.2f)));
				arrows[i].dame += num;
				arrows[i] = null;
			}
			if (numArrow != 1)
			{
				numArrow = 1;
				arrowsRotation = new Vector3[1]
				{
					Vector3.zero
				};
				arrows = new Arrow[1];
			}
			lineArchery.SetPositions(points_idle);
			idle();
		}
		else
		{
			idle();
		}
	}

	public virtual void idle()
	{
		is_idle = true;
		canAttack = true;
		body_animator.enabled = true;
		pointArchery.transform.localPosition = new Vector3(-0.36f, 0f, 0f);
		lineArchery.SetPositions(points_idle);
		force = 0f;
		handLeft_rotate.onReset();
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
	}

	public void x3Arrow()
	{
		if (!is_die && GameManager.is_gameStart && body_animator.enabled && is_idle && numArrow != 3)
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
			idle();
			body_animator.Play("x3Arrow");
			eff_light.transform.position = base.transform.position;
			delayFunction(0.22f, delegate
			{
				eff_light.SetActive(value: true);
			});
			delayFunction(1f, delegate
			{
				eff_light.SetActive(value: false);
			});
		}
	}

	public void healing()
	{
		if (!is_die && GameManager.is_gameStart && body_animator.enabled && HP_current < (float)HP && is_idle)
		{
			HP_current = HP;
			body_animator.Play("HP+");
			imgHPfade.Play("sl_hp+");
			iTween.ScaleTo(imgHPfill, new Vector3(1f, 1f, 1f), 0.5f);
			iTween.ScaleFrom(imgHPfade.gameObject, iTween.Hash("scale", new Vector3(2f, 4f, 4f), "delay", 0.5f, "time", 0.3f));
			eff_hp.transform.position = base.transform.position + new Vector3(0f, 0.2f, 0f);
			eff_hp.SetActive(value: true);
			delayFunction(1f, delegate
			{
				eff_hp.SetActive(value: false);
			});
		}
	}

	public void addFireArrow()
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

	public virtual void die()
	{
		if (!GameManager.is_gameStart)
		{
			return;
		}
		is_die = true;
		body_animator.enabled = false;
		lineArchery.SetPositions(points_idle);
		pointArchery.SetActive(value: false);
		pointHandLeft.SetActive(value: false);
		arrowParrent.transform.SetParent(null);
		Rigidbody2D component = arrowParrent.GetComponent<Rigidbody2D>();
		component.bodyType = RigidbodyType2D.Dynamic;
		component.AddForce(new Vector2(Random.Range(-50, 50), Random.Range(50, 100)));
		component.AddTorque(Random.Range(-10, 10), ForceMode2D.Force);
		rid1.mass = 0.05f;
		rid2.mass = 0.05f;
		for (int i = 0; i < rigids.Length; i++)
		{
			rigids[i].bodyType = RigidbodyType2D.Dynamic;
		}
		for (int j = 0; j < hinges.Length; j++)
		{
			hinges[j].enabled = true;
		}
		for (int k = 0; k < numArrow; k++)
		{
			if (arrows[k] != null)
			{
				arrows[k].rigid.bodyType = RigidbodyType2D.Dynamic;
			}
		}
		rigBody.bodyType = RigidbodyType2D.Dynamic;
		Vector3 localScale = base.transform.localScale;
		if (localScale.x > 0f)
		{
			rigBody.AddForce(new Vector2(Random.Range(-50f, -30f), Random.Range(30f, 60f)));
			rigBody.AddTorque(Random.Range(15f, 40f), ForceMode2D.Force);
		}
		else
		{
			rigBody.AddForce(new Vector2(Random.Range(30f, 50f), Random.Range(30f, 60f)));
			rigBody.AddTorque(Random.Range(-40f, -15f), ForceMode2D.Force);
		}
		platformSetCollider = gameManager.getPreviousPlatform(platformIdle);
		platformSetCollider.setCollider(act: true);
		if (anim_add.Length > 0)
		{
			for (int l = 0; l < anim_add.Length; l++)
			{
				if (anim_add[l] != null)
				{
					anim_add[l].enabled = false;
				}
			}
		}
		iTween.Stop();
		StopAllCoroutines();
	}

	public virtual void archeryDown()
	{
		body_animator.Play("Idle", 0, 0f);
		body_animator.enabled = false;
		leftHand.onReset();
		handLeft_rotate.onReset();
		arrowParrent.transform.localEulerAngles = new Vector3(0f, 0f, -84.8f);
		arrowParrent.transform.localPosition = new Vector3(0f, -2.34f, 0f);
		for (int i = 0; i < numArrow; i++)
		{
			if (arrows[i] != null)
			{
				arrows[i].rigid.bodyType = RigidbodyType2D.Static;
				arrows[i].transform.SetParent(arrowParrent.transform);
				arrows[i].transform.localPosition = new Vector3(-0.5f, 0f, 0f);
				arrows[i].transform.localEulerAngles = arrowsRotation[i];
			}
		}
	}

	public void archeryUp(float force, float angle)
	{
		if (force > 2.1f)
		{
			force = 2.1f;
		}
		else if (force < 0.35f)
		{
			force = 0.35f;
		}
		this.force = force;
		for (int i = 0; i < numArrow; i++)
		{
			arrows[i].transform.localPosition = new Vector3(0f - force, 0f, 0f);
			arrows[i].transform.localEulerAngles = arrowsRotation[i];
		}
		Vector3 localScale = base.transform.localScale;
		if (localScale.x < 0f)
		{
			angle = 0f - angle;
		}
		Vector3 localScale2 = base.transform.localScale;
		if (localScale2.x < 0f)
		{
			if (angle < -145f)
			{
				angle = -145f;
			}
			if (angle > -40f)
			{
				angle = -40f;
			}
		}
		else
		{
			if (angle > 145f)
			{
				angle = 145f;
			}
			if (angle < 40f)
			{
				angle = 40f;
			}
		}
		handLeft_rotate.transform.eulerAngles = new Vector3(0f, 0f, angle);
		points = new Vector3[3]
		{
			new Vector3(-0.35f, 1.35f, 0f),
			new Vector3(0f - force + 0.2f, 0f, 0f),
			new Vector3(-0.35f, -1.35f, 0f)
		};
		lineArchery.SetPositions(points);
		pointArchery.transform.localPosition = new Vector3(0f - force + 0.5f, 0f, 0f);
	}

	public void updatePercent()
	{
		float num = (!(HP_current < 0f)) ? (HP_current / (float)HP) : 0f;
		if (!is_Boss)
		{
			imgHPfill.transform.localScale = new Vector3(num, 1f, 1f);
			if (num == 0f || num == 1f)
			{
				imgHPfade.Play("hpIdle");
			}
			else
			{
				imgHPfade.Play("hpHit");
			}
		}
		else
		{
			gameManager.ui_Controller.setPerHpBoss(num);
		}
	}

	public void playAnim(string nameAnim)
	{
		body_animator.Play(nameAnim);
	}

	public void playAudio(AudioClip audioClip)
	{
		if (audioSource != null)
		{
			audioSource.volume = SoundManager.ins.volumeSound;
			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}

	public void updateDefenseArchery()
	{
		ArcheryD archeryD = gameManager.dataHolder.archeryData.listArchery[idArchery];
		defeseArchery = archeryD.defense + archeryD.level;
	}
}
