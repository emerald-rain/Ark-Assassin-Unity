using UnityEngine;

public class Coin : E_MonoBehaviour
{
	public Rigidbody2D rigidbody2d;

	public GameObject objTarget;

	public bool isMove;

	public Collider2D collider2d;

	public GameManager gameManager;

	private const float speed = 200f;

	public void onFire()
	{
		collider2d.enabled = true;
		rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
		Vector3 position = base.transform.position;
		if (position.x < 0f)
		{
			rigidbody2d.AddForce(new Vector2(Random.Range(50, 200), Random.Range(100, 300)));
		}
		else
		{
			rigidbody2d.AddForce(new Vector2(Random.Range(-200, -50), Random.Range(100, 300)));
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		delayFunction(1f, delegate
		{
			isMove = true;
			collider2d.enabled = false;
			rigidbody2d.bodyType = RigidbodyType2D.Static;
		});
	}

	private void Update()
	{
		if (isMove)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, gameManager.targetCoinPosition.transform.position, 200f * Time.deltaTime);
			if (Vector3.Distance(base.transform.position, gameManager.targetCoinPosition.transform.position) < 0.1f)
			{
				isMove = false;
				gameManager.addCoin(1);
				base.gameObject.SetActive(value: false);
			}
		}
	}
}
