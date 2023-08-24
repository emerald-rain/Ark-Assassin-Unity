using UnityEngine;

public class WoodController : ObstaclesBase
{
	public Rigidbody2D rigBody;

	public AudioSource audioSource;

	public GameObject impack;

	public SpriteRenderer spr;

	public Collider2D pcollider;

	private void OnEnable()
	{
		impack.SetActive(value: false);
		spr.enabled = true;
		pcollider.enabled = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.tag.Equals("Arrow"))
		{
			return;
		}
		Arrow component = collision.gameObject.GetComponent<Arrow>();
		if (!component.isFire)
		{
			return;
		}
		component.isFire = false;
		component.cir2d.enabled = false;
		component.trailObj.SetActive(value: false);
		component.spr.sortingOrder = -50;
		component.rigid.bodyType = RigidbodyType2D.Static;
		component.rigid.simulated = false;
		component.transform.SetParent(base.transform);
		component.destroyArrow();
		Vector3 position = component.gameObject.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		if (x >= position2.x)
		{
			Vector3 position3 = component.gameObject.transform.position;
			float y = position3.y;
			Vector3 position4 = base.transform.position;
			if (y >= position4.y)
			{
				rigBody.AddTorque(Random.Range(20, 50), ForceMode2D.Force);
			}
			else
			{
				rigBody.AddTorque(Random.Range(-50f, -20f), ForceMode2D.Force);
			}
		}
		else
		{
			Vector3 position5 = component.gameObject.transform.position;
			float y2 = position5.y;
			Vector3 position6 = base.transform.position;
			if (y2 >= position6.y)
			{
				rigBody.AddTorque(Random.Range(-50, -20), ForceMode2D.Force);
			}
			else
			{
				rigBody.AddTorque(Random.Range(20, 50), ForceMode2D.Force);
			}
		}
		audioSource.volume = SoundManager.ins.volumeSound;
		audioSource.Play();
	}

	public override void onDisbleObj()
	{
		impack.SetActive(value: true);
		spr.enabled = false;
		pcollider.enabled = false;
		delayFunction(1f, delegate
		{
			base.transform.parent.gameObject.SetActive(value: false);
		});
	}
}
