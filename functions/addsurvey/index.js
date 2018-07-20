const storagehelpers = require('../shared/storagehelpers');
const utilities = require('../shared/utilities');

module.exports = function (context, req) {
    if (utilities.validateSurveyData(req.body)) {
        storagehelpers.insertSurvey(req.body).then((res) => {
            context.res = {
                body: res
            };
            context.done();
        }).catch(error => {
            utilities.setErrorAndCloseContext(context, error, 500);
        });
    } else {
        utilities.setErrorAndCloseContext(context, `Need POST Data with something like the following: ${JSON.stringify(sampleSurvey)}`, 400);
    }

};

const sampleSurvey = {
    surveyName: 'mysurvey',
    surveyData: [
        {
            questionID: "1",
            questionText: "Sample Question 1",
            questionType: "multiplechoice",
            answers: [
                {
                    answerID: "1-1",
                    answerText: "Answer 1"
                },
                {
                    answerID: "1-2",
                    answerText: "Answer 2"
                },
                {
                    answerID: "1-3",
                    answerText: "Answer 3"
                }
            ]
        },
        {
            questionID: "2",
            questionText: "Sample Question 2",
            questionType: "freetext"
            
        }
    ]
};