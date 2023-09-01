using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List <TextMeshProUGUI> names;
    [SerializeField]
    private List <TextMeshProUGUI> scores;

    private string publicLeaderboardKey = 
        "4fb8ba0d9d21c2cb2b2621baa47be693137e5e78f68e696828581e26d339aef6";
        // secret key is c941d819f846d60f644b01777e7abb5fc129bf55897888a3ca433c8b2a0fb5493c40c05e15bc964d65536f58251698df8aea2264e129dbd007dd5be4f9ba1159436994958fd2091a03e2568504d869eb61aa443f76cfdf12a09131fa0ceb601f4616977dc9a57da7f812a575d792d6e93f51b0b0f6d85df6fdd912b1809003b4

    private void Start() 
    {
        GetLeaderboard();
    }

    public void GetLeaderboard() {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => {
            int LoopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < LoopLength; ++i) {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string Username, int score, string extra) {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, Username, 
        score, extra, ((msg) => {
            GetLeaderboard();
        }));
    }
}
