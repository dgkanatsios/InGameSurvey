const storagehelpers = require('../shared/storagehelpers');
const utilities = require('../shared/utilities');

module.exports = function (context, req) {
    //called via GET
    if (utilities.validateDeleteSurveyResponsesData(req)) {
        storagehelpers.deleteSurveyResponses(req.query.surveyName).then((res) => {

            context.res = {
                status: 200,
                body: res,

            };
            context.done();
        }).catch(error => {
            context.log(error);
            utilities.setErrorAndCloseContext(context, error, 500);
        });
    }
    else {
        utilities.setErrorAndCloseContext(context, `Need GET Data for parameter surveyName`, 400);
    }
};