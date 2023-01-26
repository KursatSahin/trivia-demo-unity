using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardItemView : MonoBehaviour
{
    [SerializeField] private Color lightBackgroundColor;
    [SerializeField] private Color darkBackgroundColor;
    
    [SerializeField] private Image background;
    [SerializeField] private Image glowImage;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text rankText; 
    [SerializeField] private TMP_Text highScoreText;

    private LeaderboardEntry _data;

    public void SetLeaderboardItemData(LeaderboardEntry leaderboardEntry)
    {
        _data = leaderboardEntry;
        UpdateView();
    }

    private void UpdateView()
    {
        // Set visibility of the shining frame
        glowImage.gameObject.SetActive(_data.rank == 3);
        
        // Set the username text
        usernameText.text = _data.nickname;

        // Set the rank text
        rankText.text = _data.rank.ToString();

        // Set the high score text
        highScoreText.text = _data.score.ToString();
    }
}