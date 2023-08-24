using UnityEngine;

public class RopeController : ObstaclesBase
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
		if (component.isFire)
		{
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
				rigBody.AddForce(new Vector2(Random.Range(-2000, -1000), 50f));
			}
			else
			{
				rigBody.AddForce(new Vector2(Random.Range(1000, 2000), 50f));
			}
			audioSource.volume = SoundManager.ins.volumeSound;
			audioSource.Play();
		}
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
