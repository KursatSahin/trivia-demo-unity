using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class HttpQuestionsRequestHelper
    {
        private const string _questionsEndpoint = "localhost:8080/questions";
        private readonly List<Question> _questionList = new List<Question>();

        public async UniTask<List<Question>> GetQuestions()
        {
            _questionList.Clear();
            
            UnityWebRequest questionsRequest;
            QuestionResponseDTO questionResponseDto = null;
            
            questionsRequest = await HttpRequestHelper.Send(HttpRequestHelper.RequestType.GET, _questionsEndpoint);
                
            if (questionsRequest.result == UnityWebRequest.Result.Success)
            {
                questionResponseDto = JsonHelper.ReadJson<QuestionResponseDTO>(questionsRequest.downloadHandler.text);
                _questionList.AddRange(questionResponseDto.questions);
                    
            }
            else
            {
                Debug.Log(questionsRequest.error);
            }

            return _questionList;
        }
    }
}