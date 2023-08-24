using UnityEngine;

public class SetSpriteHero : MonoBehaviour
{
	public Sprite[] sprs;

	private SpriteRenderer spriteRenderer;

	public void onsetSprite(string _nameHero)
	{
		int num = (!_nameHero.ToLower().Equals("songoku")) ? 1 : 0;
		if (spriteRenderer == null)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		spriteRenderer.sprite = sprs[num];
	}
}
