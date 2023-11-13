using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
// helps parse Json data into C# classes

public class Score{
    public string displayName;
    public int score;
    public string dateTimeSet;
    public int scoreId;
}
// a class to store the score data

public static class ApiManager : object{
    [SerializeField] static string serverBaseURL;
    public static string token;

    [SerializeField] static List<Score> tempScores = new List<Score>();

    public delegate void ScoresCallback(Score[] scores);
    public delegate void Callback();

    public static IEnumerator Login(string username, string password, Callback callback){
        WWWForm form = new WWWForm();

        form.AddField("userName", username);
        form.AddField("password", password);

        using(UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/api/auth/login", form)){
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else{
                Debug.Log(request.downloadHandler.text);
                Debug.Log(request.GetResponseHeader("set-cookie"));
                token = request.GetResponseHeader("set-cookie").Split(';')[0].Split('=')[1];

                callback();
            }
        }
    }
    // uses login information to obtain token to authorise webrequests

    public static IEnumerator SetScore(int score, Callback callback){
        WWWForm form = new WWWForm();

        form.AddField("score", score);

        using(UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/api/users/score", form)){
            request.SetRequestHeader("token", token);
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else{
                Debug.Log(request.downloadHandler.text);
                callback();
            }
        }
    }
    // uses post requests to send score to the server and uses its 
    // token obtained from login information to authorise the post

    public static IEnumerator GetScores(int limit, int offset, ScoresCallback callback){
        string url = "http://localhost:3000/api/users/score";

        if(limit != 0){
            url += "/limit/" + limit.ToString();

            if(offset != 0)
                url += "/offset/" + offset.ToString();
        }

        using(UnityWebRequest request = UnityWebRequest.Get(url)){
            request.SetRequestHeader("token", token);
            yield return request.SendWebRequest();
            // sends web request

            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                // handles errors
                Debug.Log(request.error);
            else{
                Debug.Log(request.downloadHandler.text);
                string responseData = request.downloadHandler.text;
                Score[] scores = JsonHelper.FromJson<Score>(responseData);
                // converts Json to list of score classes

                callback(scores);
            }
        }
    }
}
