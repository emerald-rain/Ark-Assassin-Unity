using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
	public BodyParts bodyParts;

	public Person person;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals("Arrow"))
		{
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
				person.hit(component.dame, bodyParts, component);
				GameObject effHealth = ObjectPooling.ins.getEffHealth();
				effHealth.transform.SetParent(base.transform);
				effHealth.transform.localScale = new Vector3(1f, 1f, 1f);
				effHealth.transform.position = component.arrowHead.transform.position;
			}
		}
	}
}
