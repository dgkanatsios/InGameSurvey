const storagehelpers = require('../shared/storagehelpers');
const utilities = require('../shared/utilities');

module.exports = function (context, req) {
   
    storagehelpers.deleteSurveyResponses(req.params.surveyName).then((res) => {
       
        context.res = {
            status: 200,
            body: res,
            
        };
        context.done();
    }).catch(error => {
        context.log(error);
        utilities.setErrorAndCloseContext(context, error, 500);
    });
};