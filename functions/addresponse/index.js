const storagehelpers = require('../shared/storagehelpers');
const utilities = require('../shared/utilities');

module.exports = function (context, req) {
    if (utilities.validateResponseData(context.body)) {
        storagehelpers.insertResponse(context.body).catch(error => {
            utilities.setErrorAndCloseContext(context, error, 500);
        }).then((res) => {
            context.res = {
                body: res
            };
            context.done();
        });
    } else {
        utilities.setErrorAndCloseContext(context, `Need POST Data with something like the following: ${JSON.stringify(sampleResponse)}`, 400);
    }

};

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
    },

    ]
};