using UnityEngine;

public class Arrow : E_MonoBehaviour
{
	public Rigidbody2D rigid;

	public GameObject arrowHead;

	public int dame;

	public ArrowFromBy fromBy;

	public Collider2D cir2d;

	public SpriteRenderer spr;

	public Animator anim;

	private int sub;

	public bool isFire;

	public GameObject trailObj;

	public bool fromHero;

	public GameObject fireEff;

	public GameObject[] trailEff;

	public AudioSource audioSource;

	public int id;

	public void onSetup(int id)
	{
		this.id = id;
		cir2d.enabled = false;
		rigid.bodyType = RigidbodyType2D.Static;
		rigid.simulated = true;
		spr.sortingOrder = 50;
		isFire = false;
		trailObj.SetActive(value: false);
		fireEff.SetActive(value: false);
		anim.Play("arrowIdle");
	}

	public void fire(float force, bool fromHero)
	{
		this.fromHero = fromHero;
		isFire = true;
		cir2d.enabled = true;
		if (trailObj != null)
		{
			trailObj.SetActive(value: false);
		}
		trailObj = getEffect(id);
		trailObj.SetActive(value: true);
		Vector2 a = arrowHead.transform.position - base.transform.position;
		rigid.bodyType = RigidbodyType2D.Dynamic;
		base.transform.parent = null;
		Vector3 localScale = base.transform.localScale;
		if (localScale.x > 0f)
		{
			sub = 0;
		}
		else
		{
			sub = 180;
		}
		rigid.AddForce(a * 700f * force);
		audioSource.volume = SoundManager.ins.volumeSound;
		audioSource.Play();
	}

	private void Update()
	{
		if (!(rigid.velocity != Vector2.zero) || !isFire)
		{
			return;
		}
		Vector2 velocity = rigid.velocity;
		float num = Mathf.Atan2(velocity.y, velocity.x) * 57.29578f;
		base.transform.eulerAngles = new Vector3(0f, 0f, num + (float)sub);
		Vector3 position = base.transform.position;
		if (!(position.y < -6f))
		{
			Vector3 position2 = base.transform.position;
			if (!(position2.x < -6f))
			{
				Vector3 position3 = base.transform.position;
				if (!(position3.x > 6f))
				{
					return;
				}
			}
		}
		ObjectPooling.ins.resetArrow(this);
	}

	public void destroyArrow()
	{
		anim.Play("arrowFade");
		delayFunction(1.5f, delegate
		{
			ObjectPooling.ins.resetArrow(this);
		});
	}

	public void destroyArrowNow()
	{
		ObjectPooling.ins.resetArrow(this);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!fromHero)
		{
			if (isFire && collision.gameObject.tag.Equals("Protect"))
			{
				disableFromProtect();
			}
		}
		else if (isFire && collision.gameObject.tag.Equals("EnemyProtect"))
		{
			disableFromProtect();
		}
		if (isFire && collision.gameObject.tag.Equals("shaw"))
		{
			collision.gameObject.GetComponent<ShawController>().playAudio();
			disableFromProtect();
		}
	}

	private void disableFromProtect()
	{
		isFire = false;
		cir2d.enabled = false;
		trailObj.SetActive(value: false);
		Vector2 velocity = rigid.velocity;
		if (velocity.x > 0f)
		{
			rigid.AddForce(new Vector2(Random.Range(-700, -610), Random.Range(250, 350)));
			rigid.AddTorque(Random.Range(120, 200), ForceMode2D.Force);
		}
		else
		{
			rigid.AddForce(new Vector2(Random.Range(610, 700), Random.Range(250, 350)));
			rigid.AddTorque(Random.Range(-200, -120), ForceMode2D.Force);
		}
	}

	private GameObject getEffect(int id)
	{
		switch (id)
		{
		case 0:
		case 13:
			return trailEff[0];
		case 11:
		case 18:
			return trailEff[1];
		case 2:
		case 3:
		case 15:
		case 21:
			return trailEff[2];
		case 1:
		case 5:
		case 20:
			return trailEff[3];
		case 8:
		case 16:
		case 17:
		case 19:
			return trailEff[4];
		default:
			switch (id)
			{
			case 21:
				break;
			case 6:
			case 10:
			case 14:
				return trailEff[7];
			default:
				return trailEff[6];
			}
			break;
		case 4:
		case 7:
		case 9:
		case 12:
		case 14:
			break;
		}
		return trailEff[5];
	}
}
