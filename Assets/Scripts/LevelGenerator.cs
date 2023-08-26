using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public MissionData missionData;

    public string[] enemyNames; // List of possible enemy names

    void Start()
    {
        Mission selectedMission = missionData.missions[Random.Range(0, missionData.missions.Length)];

        foreach (EnemyInfo enemyInfo in selectedMission.enemyInfos)
        {
            // enemyInfo.id = /* Set id */;
            enemyInfo._name = enemyNames[Random.Range(0, enemyNames.Length)];
            enemyInfo.level = 0;
            // enemyInfo.obstacles = Obstacles.None;
        }
    }
}
