using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameSurveySDK

{

    /*
     const sampleResponse = {
    surveyName: 'mysurvey',
    responseData: [{
        "questionID": "1",
        "answer": "1-2"
    },
    {
        "questionID": "2",
        "answer": "freetextanswer2"
    },
    {
        "questionID": "3",
        "answer": "3-1"
    }]
};
    */

    [Serializable()]
    public class SurveyResponse
    {
        public string surveyName;
        public Response[] responseData;
    }

    [Serializable()]
    public class Response
    {
        public string questionID;
        public string answer;
    }
}
