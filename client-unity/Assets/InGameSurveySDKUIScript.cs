using InGameSurveySDK;
using UnityEngine;
using UnityEngine.UI;


public class InGameSurveySDKUIScript : MonoBehaviour
{
    private Text statusText;

    public void Start()
    {
        Globals.DebugFlag = true;

        statusText = GameObject.Find("StatusText").GetComponent<Text>();

        //check here for more information regarding authentication and authorization in Azure App Service
        //https://azure.microsoft.com/en-us/documentation/articles/app-service-authentication-overview/
    }

    public void ClearOutput()
    {
        statusText.text = string.Empty;
    }



    public void GetSurveys()
    {
        InGameSurveySDKClient.Instance.GetSurveys(response =>
        {
            if (response.Status == CallBackResult.Success)
            {
                string result = "Get surveys completed";
                if (Globals.DebugFlag)
                {
                    WriteLine("Survey name:"+response.Result.surveyName);
                    foreach (var item in response.Result.questions)
                    {
                        WriteLine(string.Format("question ID is {0}, text is {1}, type is {2}", item.questionID, item.questionText, item.questionType));
                        
                        if(item.questionType == "multiplechoice")
                        {
                            foreach(var answer in item.answers)
                            {
                                WriteLine(string.Format("answer ID is {0}, answer text is {1}", answer.answerID, answer.answerText));
                            }
                        }
                    }
                    WriteLine(result);
                }
            }
            else
            {
                WriteLine(response.Exception.Message);
            }
        });
        WriteLine("Loading...");
    }




    public void WriteLine(string s)
    {
        if (statusText.text.Length > 20000)
            statusText.text = string.Empty + "-- TEXT OVERFLOW --";

        statusText.text += s + "\r\n";
    }

}




