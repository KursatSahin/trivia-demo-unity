using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class HttpLeaderboardRequestHelper
    {
        private const string _leaderboardEndpoint = "localhost:8080/leaderboard";
        private readonly List<LeaderboardEntry> _leaderboardEntries = new List<LeaderboardEntry>();

        public async UniTask<List<LeaderboardEntry>> GetLeaderboard()
        {
            _leaderboardEntries.Clear();
            
            // In this case we need request to data from server in loop until we get last page
            int page = 0;
            UnityWebRequest leaderBoardRequest;
            LeaderboardResponseDTO leaderboardResponseDto = null;
            
            do
            {
                leaderBoardRequest = await HttpRequestHelper.Send(HttpRequestHelper.RequestType.GET, _leaderboardEndpoint, new Dictionary<string, string>(){{"page", page.ToString()}});
                
                if (leaderBoardRequest.result == UnityWebRequest.Result.Success)
                {
                    leaderboardResponseDto = JsonHelper.ReadJson<LeaderboardResponseDTO>(leaderBoardRequest.downloadHandler.text);
                    _leaderboardEntries.AddRange(leaderboardResponseDto.data);
                    
                    page++;
                }
                else
                {
                    Debug.Log(leaderBoardRequest.error);
                }
                
                 
            } while (leaderboardResponseDto != null && leaderboardResponseDto.is_last == false);

            return _leaderboardEntries;
        }
    }
}