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
                    foreach (var survey in response.Result)
                    {
                        WriteLine("Survey name:" + survey.surveyName);
                        foreach (var question in survey.questions)
                        {
                            WriteLine(string.Format("question ID is {0}, text is {1}, type is {2}", question.questionID, question.questionText, question.questionType));

                            if (question.questionType == "multiplechoice")
                            {
                                foreach (var answer in question.answers)
                                {
                                    WriteLine(string.Format("answer ID is {0}, answer text is {1}", answer.answerID, answer.answerText));
                                }
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

    public void PostSurveyResponse()
    {

        var surveyResponse = new SurveyResponse()
        {
            surveyName = "mysurvey",
            responseData = new Response[]
            {
                new Response() { questionID = "1", answer = "1-2"},
                new Response() { questionID = "2", answer = "2-3"},
                new Response() { questionID = "3", answer = "free text"}
            }
        };

        InGameSurveySDKClient.Instance.PostSurveyResponse(surveyResponse, response =>
        {
            if (response.Status == CallBackResult.Success)
            {
                string result = "Post survey response completed";
                if (Globals.DebugFlag)
                {
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




