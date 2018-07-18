module.exports = {
    validateDeleteSurveyResponsesData: function (req) {
        if (req && req.params && req.params.surveyName)
            return true;
        else return false;
    },
    validateResponseData: function (body) {
        if (body && body.surveyName && body.responseData) {
            const data = body.responseData;

            if (!Array.isArray(data))
                return false;

            if (data.length < 1)
                return false;

            for (let i = 0; i < data.length; i++) {
                const d = data[i];

                if (!d.questionID)
                    return false;

                if (!d.answer)
                    return false;
            }


            return true;
        }
        else {
            return false;
        }
    },
    validateSurveyData: function (body) {
        if (body && body.surveyName && body.surveyData) {
            const data = body.surveyData;

            if (!Array.isArray(data))
                return false;

            if (data.length < 1)
                return false;

            for (let i = 0; i < data.length; i++) {
                const d = data[i];

                if (!d.questionID) {
                    return false;
                }

                if (!d.questionText) {
                    return false;
                }

                if (d.questionType !== 'multiplechoice' &&
                    d.questionType !== 'freetext')
                    return false;

                if (d.questionType == 'multiplechoice') {
                    if (!d.answers) {
                        return false;
                    }

                    if (!Array.isArray(d.answers))
                        return false;

                    if (d.answers.length < 1)
                        return false;

                    for (let j = 0; j < d.answers.length; j++) {
                        const a = d.answers[j];
                        if (!a.answerID)
                            return false;
                        if (!a.answerText)
                            return false;
                    }
                }

            }

            return true;
        }
        else
            return false;
    },
    setErrorAndCloseContext(context, errorMessage, statusCode) {
        context.log(`ERROR: ${errorMessage}`);
        context.res = {
            status: statusCode,
            body: errorMessage,
        };
        context.done();
    }
}