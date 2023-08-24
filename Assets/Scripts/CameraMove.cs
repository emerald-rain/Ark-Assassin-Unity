using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public GameObject hero;

	public float speed = 2f;

	public GameObject zoneLeft;

	public GameObject zoneRight;

	public static float cameraOrtho;

	private void Awake()
	{
		cameraOrtho = ((!((float)(Camera.main.pixelHeight / Camera.main.pixelWidth) > 1.8f)) ? 5f : 5.6f);
		Camera.main.orthographicSize = cameraOrtho;
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f));
		float x = vector.x;
		zoneLeft.transform.localPosition = new Vector3(x, 0f, 10f);
		zoneRight.transform.localPosition = new Vector3(0f - x, 0f, 10f);
		Camera.main.orthographicSize = 3f;
	}

	private void Update()
	{
		if (GameManager.is_camMove)
		{
			float t = speed * Time.deltaTime;
			Vector3 position = base.transform.position;
			Vector3 position2 = base.transform.position;
			float y = position2.y;
			Vector3 position3 = hero.transform.position;
			position.y = Mathf.Lerp(y, position3.y + 1f, t);
			position.x = 0f;
			base.transform.position = position;
		}
	}
}
