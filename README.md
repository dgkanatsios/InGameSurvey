[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](LICENSE.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
[![unofficial Google Analytics for GitHub](https://gaforgithub.azurewebsites.net/api?repo=InGameSurvey)](https://github.com/dgkanatsios/gaforgithub)
# In Game Survey

A simple In Game Survey implementation for gaming clients, using [Azure Functions](https://functions.azure.com)

## One-click deployment

Click the following button to deploy the project to your Azure subscription:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fdgkanatsios%2FInGameSurvey%2Fmaster%2Fdeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>

This operation will trigger a template deployment of the [deploy.json](deploy.json) ARM template file to your Azure subscription, which will create the necessary Azure resources as well as pull the source code from this repository. 

## Architecture - Technical Details

The In Game Survey API is served by [Azure Functions](https://functions.azure.com) whereas the storage is implemented using [Azure Table Storage](https://azure.microsoft.com/en-us/services/storage/tables/), thus making this solution pretty inexpensive. 

A Unity game engine SDK and sample scene (compatible with Unity 5.6) is provided in the `client-unity` folder.

The Function App that gets deployed contains 5 Functions:

- *addsamplesurvey*: This allows you to create a simple survey as well as two responses for that. Use for demo purposes
- *addsurvey*: Use this to create a new survey. Survey format must be like:
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
Pay special attention to the 'surveyName' property, as this must be a lowercase unique string. The Function App creates a new table in your Table Storage account to store the responses for this survey
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
- *getsurveys*: Use this to get the active surveys
- *deletesurveyresponses*: When you pass the surveyName as a parameter in the query string, the Function will delete the responses. You can use this after you have finished processing the data. Disabled by default (for obvious reasons)

## Resources

- [Azure Table Storage design guide](https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-design-guide)
- [Azure Functions documentation](https://docs.microsoft.com/en-us/azure/azure-functions/)
- [Azure Functions Consumption Plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale#consumption-plan)