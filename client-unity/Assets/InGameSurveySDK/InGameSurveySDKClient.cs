using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace InGameSurveySDK
{
    public class InGameSurveySDKClient : MonoBehaviour
    {
        public string SurveysUrl;
        public string PostSurveyResponseUrl;
        public static InGameSurveySDKClient Instance;


        void Awake()
        {
            Instance = this;
            Utilities.ValidateForNull(SurveysUrl);
        }




        public void GetSurveys(Action<CallbackResponse<Survey[]>> callback)
        {
            Utilities.ValidateForNull(callback);
            StartCoroutine(GetSurveys(SurveysUrl, callback));
        }

        public void PostSurveyResponse(SurveyResponse surveyResponse, Action<CallbackResponse> callback)
        {
            Utilities.ValidateForNull(surveyResponse,callback);
            StartCoroutine(PostSurveyResponseInternal(surveyResponse,callback));
        }



        private IEnumerator GetSurveys(string url, Action<CallbackResponse<Survey[]>> callback)
        {
            using (UnityWebRequest www = Utilities.BuildInGameSurveyAPIWebRequest
                (SurveysUrl, HttpMethod.Get.ToString(), null))
            {
                yield return www.Send();
                if (Globals.DebugFlag) Debug.Log(www.responseCode);
                var response = new CallbackResponse<Survey[]>();
                if (Utilities.IsWWWError(www))
                {
                    if (Globals.DebugFlag) Debug.Log(www.error);
                    Utilities.BuildResponseObjectOnFailure(response, www);
                }
                else
                {
                    try
                    {
                        Survey[] surveys = JsonHelper.GetJsonArray<Survey>(www.downloadHandler.text);
                        foreach (var survey in surveys)
                        {
                            survey.questions = JsonHelper.GetJsonArray<Question>(survey.data);
                        }
                        response.Result = surveys;
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

        private IEnumerator PostSurveyResponseInternal(SurveyResponse surveyResponse, Action<CallbackResponse> completed)
        {
            string json = JsonUtility.ToJson(surveyResponse);
            //Debug.Log(json);
            using (UnityWebRequest www = Utilities.BuildInGameSurveyAPIWebRequest(PostSurveyResponseUrl,
                HttpMethod.Post.ToString(), json))
            {
                yield return www.Send();
                if (Globals.DebugFlag) Debug.Log(www.responseCode);

                var response = new CallbackResponse();

                if (Utilities.IsWWWError(www))
                {
                    if (Globals.DebugFlag) Debug.Log(www.error);
                    Utilities.BuildResponseObjectOnFailure(response, www);
                }

               
                completed(response);
            }
        }
    }
}

