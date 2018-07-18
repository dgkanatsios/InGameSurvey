using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace InGameSurveySDK
{
    public class InGameSurveySDKClient : MonoBehaviour
    {
        public string Url;
        public static InGameSurveySDKClient Instance;


        void Awake()
        {
            Instance = this;
            Utilities.ValidateForNull(Url);
        }


     

        public void GetMessagesOfTheDay(Action<CallbackResponse<Message[]>> callback)
        {
            Utilities.ValidateForNull(callback);
            StartCoroutine(GetStuffArray<Message>(Url, callback));
        }

      
        private IEnumerator GetStuffArray<T>(string url,  Action<CallbackResponse<T[]>> callback)
        {
            


            using (UnityWebRequest www = Utilities.BuildInGameSurveyAPIWebRequest
                (url, HttpMethod.Get.ToString(), null))
            {
                yield return www.Send();
                if (Globals.DebugFlag) Debug.Log(www.responseCode);
                var response = new CallbackResponse<T[]>();
                if (Utilities.IsWWWError(www))
                {
                    if (Globals.DebugFlag) Debug.Log(www.error);
                    Utilities.BuildResponseObjectOnFailure(response, www);
                }
                else
                {
                    try
                    {
                        T[] data = JsonHelper.GetJsonArray<T>(www.downloadHandler.text);
                        response.Result = data;
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

       

    }
}

