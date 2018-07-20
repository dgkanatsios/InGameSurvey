const storagehelpers = require('../shared/storagehelpers');
const utilities = require('../shared/utilities');

module.exports = function (context, req) {

    let promises = [];
    promises.push(storagehelpers.insertSurvey(sampleSurvey));
    promises.push(storagehelpers.insertResponse(sampleResponse1));
    promises.push(storagehelpers.insertResponse(sampleResponse2));

    Promise.all(promises).then((res) => {
        context.res = {
            body: res
        };
        context.done();
    }).catch(error => {
        utilities.setErrorAndCloseContext(context, error, 500);
    });
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
            questionType: "freetext",
        },
        {
            questionID: "3",
            questionText: "Sample Question 3",
            questionType: "multiplechoice",
            answers: [
                {
                    answerID: "3-1",
                    answerText: "Answer 1"
                },
                {
                    answerID: "3-2",
                    answerText: "Answer 2"
                },
                {
                    answerID: "3-3",
                    answerText: "Answer 3"
                }
            ]

        }
    ]
};

const sampleResponse1 = {
    surveyName: 'mysurvey',
    responseData: [{
        "questionID": "1",
        "answer": "1-3"
    },
    {
        "questionID": "2",
        "answer": "freetextanswer"
    },
    {
        "questionID": "3",
        "answer": "3-2"
    },

    ]
};

const sampleResponse2 = {
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
    },

    ]
};