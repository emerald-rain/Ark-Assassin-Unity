using UnityEngine;

public class ShawController : ObstaclesBase
{
	public AudioSource audioSource;

	public GameObject impack;

	public SpriteRenderer spr;

	public Collider2D pcollider;

	public Animator animator;

	public Sprite[] sprite_shaws;

	private void OnEnable()
	{
		impack.SetActive(value: false);
		spr.enabled = true;
		pcollider.enabled = true;
		animator.enabled = true;
		spr.sprite = sprite_shaws[Random.Range(0, sprite_shaws.Length)];
	}

	public void playAudio()
	{
		audioSource.volume = SoundManager.ins.volumeSound;
		audioSource.Play();
	}

	public override void onDisbleObj()
	{
		animator.enabled = false;
		impack.SetActive(value: true);
		spr.enabled = false;
		pcollider.enabled = false;
		delayFunction(1f, delegate
		{
			base.transform.parent.gameObject.SetActive(value: false);
		});
	}
}
