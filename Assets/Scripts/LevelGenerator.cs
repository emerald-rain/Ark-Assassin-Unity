using UnityEngine;
using System.Linq;

[System.Serializable]
public class ObstacleChance
{
    public Obstacles obstacle;
    public float chance;
}

public class LevelGenerator : MonoBehaviour
{
    public MissionData missionData; // Misson data file
    public string[] enemyNames; // List of possible enemy names
    public ObstacleChance[] obstacleChances; // List of Obstacle and their chances to spawn

    void Start()
    {
        Mission selectedMission = missionData.missions[Random.Range(0, missionData.missions.Length)];

        foreach (EnemyInfo enemyInfo in selectedMission.enemyInfos)
        {
            enemyInfo._name = enemyNames[Random.Range(0, enemyNames.Length)];
            enemyInfo.level = 0;
            enemyInfo.obstacles = GetRandomObstacle();
        }
    }

    private Obstacles GetRandomObstacle()
    {
        float randomValue = Random.value * obstacleChances.Sum(chance => chance.chance);

        foreach (ObstacleChance obstacleChance in obstacleChances)
        {
            if (randomValue < obstacleChance.chance)
            {
                return obstacleChance.obstacle;
            }
            randomValue -= obstacleChance.chance;
        }

        return obstacleChances[0].obstacle; // Default option
    }
}
