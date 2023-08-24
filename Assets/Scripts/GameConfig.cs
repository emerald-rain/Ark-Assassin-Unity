public class GameConfig
{
	public const float heightPlatform = 0.4f;

	public const float witdhPlatform = 0.5f;

	public const float x1Platform = 0.55f;

	public const float x2Platform = 1.5f;

	public const float percentBody = 1f;

	public const float percentHead = 2f;

	public const float percentFoot = 0.7f;

	public const float percentColor = 0.8f;

	public const float percentOther = 0.5f;

	public const float posYHeroTheFirst = -1f;

	public const float distanceCameraWithHero = 1f;

	public const float arrowForceMax = 4.5f;

	public static string formatCoin(string coin)
	{
		string str = string.Empty;
		while (coin.Length > 3)
		{
			str = coin.Substring(coin.Length - 3) + "." + str;
			coin = coin.Remove(coin.Length - 3);
		}
		str = coin + "." + str;
		return str.Remove(str.Length - 1);
	}
}
