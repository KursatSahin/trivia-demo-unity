using System;
using System.Collections.Generic;

public class LeaderboardEntry: IComparable<LeaderboardEntry>
{
    public int rank { get; set; }
    public string nickname { get; set; }
    public int score { get; set; }

    public LeaderboardEntry(int rank, string nickname, int score)
    {
        this.rank = rank;
        this.nickname = nickname;
        this.score = score;
    }

    public void UpdateRank(int newRank)
    {
        rank = newRank;
    }
    
    public void UpdateUsername(string newUsername)
    {
        nickname = newUsername;
    }

    public void UpdateHighscore(int newHighscore)
    {
        score = newHighscore;
    }

    public int CompareTo(LeaderboardEntry other)
    {
        return this.rank.CompareTo(other.rank);
    }
}

public class LeaderboardResponseDTO
{
    public int page;
    public bool is_last;
    public List<LeaderboardEntry> data;
}