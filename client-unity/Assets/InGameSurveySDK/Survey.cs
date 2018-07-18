using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGameSurveySDK
{

    //survey
    //[{"surveyName":"mysurvey","data":"[{\"questionID\":\"1\",\"questionText\":\"Sample Question 1\",\"questionType\":\"multiplechoice\",\"answers\":[{\"answerID\":\"1-1\",\"answerText\":\"Answer 1\"},{\"answerID\":\"1-2\",\"answerText\":\"Answer 2\"},{\"answerID\":\"1-3\",\"answerText\":\"Answer 3\"}]},{\"questionID\":\"2\",\"questionText\":\"Sample Question 2\",\"questionType\":\"freetext\"},{\"questionID\":\"3\",\"questionText\":\"Sample Question 3\",\"questionType\":\"multiplechoice\",\"answers\":[{\"answerID\":\"3-1\",\"answerText\":\"Answer 1\"},{\"answerID\":\"3-2\",\"answerText\":\"Answer 2\"},{\"answerID\":\"3-3\",\"answerText\":\"Answer 3\"}]}]"}]

    [Serializable()]
    public class Survey
    {
        public string surveyName;
        public string data;
        public Question[] questions;
    }


    [Serializable()]
    public class Question
    {
        public string questionID;
        public string questionText;
        public string questionType;
        public Answer[] answers;
    }

    [Serializable()]
    public class Answer
    {
        public string answerID;
        public string answerText;
    }

}
