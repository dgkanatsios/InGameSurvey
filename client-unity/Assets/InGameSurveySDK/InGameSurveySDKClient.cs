using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace InGameSurveySDK
{
    public class InGameSurveySDKClient : MonoBehaviour
    {
        public string GetSurveysUrl;
        public static InGameSurveySDKClient Instance;


        void Awake()
        {
            Instance = this;
            Utilities.ValidateForNull(GetSurveysUrl);
        }




        public void GetSurveys(Action<CallbackResponse<Survey>> callback)
        {
            Utilities.ValidateForNull(callback);
            StartCoroutine(GetSurveys(GetSurveysUrl, callback));
        }



        private IEnumerator GetSurveys(string url, Action<CallbackResponse<Survey>> callback)
        {
            using (UnityWebRequest www = Utilities.BuildInGameSurveyAPIWebRequest
                (GetSurveysUrl, HttpMethod.Get.ToString(), null))
            {
                yield return www.Send();
                if (Globals.DebugFlag) Debug.Log(www.responseCode);
                CallbackResponse<Survey> response = new CallbackResponse<Survey>();
                if (Utilities.IsWWWError(www))
                {
                    if (Globals.DebugFlag) Debug.Log(www.error);
                    Utilities.BuildResponseObjectOnFailure(response, www);
                }
                else
                {
                    try
                    {
                        Survey survey = JsonUtility.FromJson<Survey>(www.downloadHandler.text);
                        survey.questions = JsonHelper.GetJsonArray<Question>(survey.data);
                        response.Result = survey;
                        response.Status = CallBackResult.Success;
                    }
                    catch (Exception ex)
                    {
                        response.Status = CallBackResult.DeserializationFailure;
                        response.Exception = ex;
                    }
                }
                callback(response);
            }
        }

        //private IEnumerator PostScoreInternal(Score instance, Action<CallbackResponse<User>> onInsertCompleted)
        //{
        //    string json = JsonUtility.ToJson(instance);

        //    using (UnityWebRequest www = Utilities.BuildScoresAPIWebRequest(GetLeaderboardsAPIURL() + "scores",
        //        HttpMethod.Post.ToString(), json, userID, username))
        //    {
        //        yield return www.Send();
        //        if (Globals.DebugFlag) Debug.Log(www.responseCode);

        //        CallbackResponse<User> response = new CallbackResponse<User>();

        //        if (Utilities.IsWWWError(www))
        //        {
        //            if (Globals.DebugFlag) Debug.Log(www.error);
        //            Utilities.BuildResponseObjectOnFailure(response, www);
        //        }

        //        else if (www.downloadHandler != null)  //all OK
        //        {
        //            //let's get the new object that was created
        //            try
        //            {
        //                User newObject = JsonUtility.FromJson<User>(www.downloadHandler.text);
        //                if (Globals.DebugFlag) Debug.Log("new object is " + newObject.ToString());
        //                response.Status = CallBackResult.Success;
        //                response.Result = newObject;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Status = CallBackResult.DeserializationFailure;
        //                response.Exception = ex;
        //            }
        //        }
        //        onInsertCompleted(response);
        //    }
        //}
    }
}

