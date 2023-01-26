using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class HttpRequestHelper {

    public enum RequestType { GET, POST }

    public static async UniTask<UnityWebRequest> Send(RequestType requestType, string endpoint, Dictionary<string, string> args = null)
    {
        UnityWebRequest request = null;

        switch (requestType)
        {
            case RequestType.GET:
                request = await Get(endpoint, args);
                break;
        }
        
        return request;
    }

    private static async UniTask<UnityWebRequest> Get(string endpoint, Dictionary<string, string> queryParams = null)
    {
        var uri = AddQueryParams(endpoint, queryParams);
        var request = UnityWebRequest.Get(uri);
        
        request.SetRequestHeader("Content-Type", "application/json");
        
        await request.SendWebRequest();
        
        return request;
    }
    
    private static string AddQueryParams(string baseEndpoint, Dictionary<string, string> queryParams = null)
    {
        string endpoint = baseEndpoint;
        
        if (queryParams != null && queryParams.Count > 0)
        {
            endpoint += "?";
            int count = 0;
            foreach (KeyValuePair<string, string> queryParam in queryParams)
            {
                if (count > 0)
                {
                    endpoint += "&";
                }
                endpoint += queryParam.Key + "=" + queryParam.Value;
                count++;
            }
        }
        return endpoint;
    }

    
}
