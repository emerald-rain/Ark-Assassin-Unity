public class ObjTimeLife : E_MonoBehaviour
{
	public float timeLife;

	private void OnEnable()
	{
		delayFunction(timeLife, delegate
		{
			effHealthDisable();
		});
	}

	private void effHealthDisable()
	{
		ObjectPooling.ins.addEffHealt(base.gameObject);
		base.gameObject.SetActive(value: false);
	}
}
