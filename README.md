[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![unofficial Google Analytics for GitHub](https://gaforgithub.azurewebsites.net/api?repo=InGameSurvey)](https://github.com/dgkanatsios/gaforgithub)
# In Game Survey

A simple In Game Survey implementation for gaming clients, using [Azure Functions](https://functions.azure.com) and [Azure Table Storage](https://azure.microsoft.com/en-us/services/storage/tables/) for the backend storage.

## One-click deployment

Click the following button to deploy the project to your Azure subscription:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fdgkanatsios%2FInGameSurvey%2Fmaster%2Fdeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>

This operation will trigger a template deployment of the [deploy.json](deploy.json) and the [deploy.function.json](deploy.function.json) ARM template files to your Azure subscription, which will create the necessary Azure resources, pull the source code from this repository (or your fork) and create the required environment variables. 

## Architecture - Technical Details

The In Game Survey API is served by [Azure Functions](https://functions.azure.com) whereas the storage mechanism is implemented using [Azure Table Storage](https://azure.microsoft.com/en-us/services/storage/tables/), thus making this solution pretty inexpensive. 

A Unity game engine SDK and sample scene (compatible with Unity 5.6 and higher) is provided in the `client-unity` folder.

### Table Storage
Regarding the storage mechanism for this project: 
- the table that contains your surveys' questions is called 'surveys'
- for each survey you create, a new table is created as the Function App creates a new table in your Table Storage account to store the responses for the newly created survey. Table name is equal to 'responses' plus a string you select, when you deploy a new survey (using the `surveyName` JSON property). So, for a survey called 'mysurvey', the table name would be 'responsesmysurvey'. For the Azure Table Storage table naming rules, check [here](https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model#tables-entities-and-properties). The value for the 'responses' prefix for your table can be changed in the `functions/shared/constants.js` file, if you want to have your own customized prefix. Obviously, each new survey you create should have a unique name.

### Functions

The Function App that gets deployed contains 5 distinct Functions, all of which are [HTTP triggered](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook#trigger---javascript-example):

- *addsamplesurvey*: This allows you to create a simple survey as well as two responses for that. Use it for demo purposes, to test that the deployment succeeded or to bootstrap your project. Feel free to delete from your production environment

- *addsurvey*: Use this to create a new survey. Survey object format is the following (POST data:
```json
{
    "surveyName": "mysurvey",
    "surveyData": [
        {
            "questionID": "1",
            "questionText": "Sample Question 1",
            "questionType": "multiplechoice",
            "answers": [
                {
                    "answerID": "1-1",
                    "answerText": "Answer 1"
                },
                {
                    "answerID": "1-2",
                    "answerText": "Answer 2"
                },
                {
                    "answerID": "1-3",
                    "answerText": "Answer 3"
                }
            ]
        },
        {
            "questionID": "2",
            "questionText": "Sample Question 2",
            "questionType": "freetext"
            
        }
    ]
}
```
A JSON object like this should be passed as POST data to the 'addSurvey' Function. As previously described, pay special attention to the 'surveyName' property, as this must be a unique string for each new survey you create.

- *getsurveys*: Use this to get the active surveys. The return value from the is similar to the following (use GET to call the Function):
```json
[
    {
        "surveyName":"mysurvey",
        "data":
        "[{\"questionID\":\"1\",\"questionText\":\"Sample Question 1\",\"questionType\":\"multiplechoice\",\"answers\":[{\"answerID\":\"1-1\",\"answerText\":\"Answer 1\"},{\"answerID\":\"1-2\",\"answerText\":\"Answer 2\"},{\"answerID\":\"1-3\",\"answerText\":\"Answer 3\"}]},{\"questionID\":\"2\",\"questionText\":\"Sample Question 2\",\"questionType\":\"freetext\"},{\"questionID\":\"3\",\"questionText\":\"Sample Question 3\",\"questionType\":\"multiplechoice\",\"answers\":[{\"answerID\":\"3-1\",\"answerText\":\"Answer 1\"},{\"answerID\":\"3-2\",\"answerText\":\"Answer 2\"},{\"answerID\":\"3-3\",\"answerText\":\"Answer 3\"}]}]"
    }
]
```
As you can see, the 'surveyName' property identifies each unique survey whereas the data property contains the questions in stringified JSON format.

- *addresponse*: This Function enables the user to post a new response to a survey. The POST data should have the format:
```json
{
    "surveyName": "mysurvey",
    "responseData": [{
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
}
```
The 'surveyName' property must correspond to the survey to which you want to post answers to whereas the 'responseData' contains an array of 'questionID'/'answer' value pairs that correspond to each survey question and the user's answer.

- *deletesurveyresponses*: When you pass the surveyName as a parameter in the query string, the Function will delete the responses. You can use this after you have finished processing the survey responses. Disabled by default, for obvious reasons.

## Resources

- [Azure Table Storage design guide](https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-design-guide)
- [Azure Functions documentation](https://docs.microsoft.com/en-us/azure/azure-functions/)
- [Azure Functions Consumption Plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale#consumption-plan)