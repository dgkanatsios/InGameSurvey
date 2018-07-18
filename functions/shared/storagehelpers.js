const azurestorage = require('azure-storage');
const uuidv4 = require('uuid/v4');
const constants = require('./constants');

function insertSurvey(body) {
    return new Promise(function (resolve, reject) {
        const tableSvc = azurestorage.createTableService();
        tableSvc.createTableIfNotExists(constants.surveysTableName,
            function (error, result, response) {
                if (error) {
                    reject(error);
                } else {

                    const survey = {
                        PartitionKey: constants.surveysPartitionKey,
                        RowKey: body.surveyName,
                        SurveyName: body.surveyName,
                        Data: JSON.stringify(body.surveyData),
                        Active: true
                    };

                    tableSvc.insertEntity(constants.surveysTableName, survey, function (error, result, response) {
                        if (error) {
                            reject(error);
                        } else {
                            resolve(`Inserted In Game Survey with Name ${survey.RowKey}`);
                        }
                    });
                }
            });
    });
}

function insertResponse(body) {
    return new Promise(function (resolve, reject) {
        const tableSvc = azurestorage.createTableService();
        tableSvc.createTableIfNotExists(constants.responsesTableNamePrefix + body.surveyName,
            function (error, result, response) {
                if (error) {
                    reject(error);
                } else {

                    const response = {
                        PartitionKey: constants.responsesPartitionKey,
                        RowKey: uuidv4(),
                        SurveyName: body.surveyName,
                        Data: JSON.stringify(body.responseData)
                    };

                    tableSvc.insertEntity(constants.responsesTableNamePrefix + body.surveyName, response, function (error) {
                        if (error) {
                            reject(error);
                        } else {
                            resolve(`Inserted In Game Survey Response with ID ${response.RowKey}`);
                        }
                    });
                }
            });
    });
}

function getSurveys() {
    return new Promise(function (resolve, reject) {
        const tableSvc = azurestorage.createTableService();
        tableSvc.createTableIfNotExists(constants.surveysTableName,
            function (error, result, response) {
                if (error) {
                    reject(error);
                } else {
                    const query = new azurestorage.TableQuery().
                        where('Active eq ?', true);

                    tableSvc.queryEntities(constants.surveysTableName, query, null, function (error, result) {
                        if (error) {
                            reject(error);
                        } else {
                            resolve(result.entries.map(entry => {
                                const data = {
                                    SurveyName: entry.SurveyName._,
                                    Data: entry.Data._
                                };
                                return data;
                            }));
                        }
                    });
                }
            });
    });
}

function getSurveyResponses(surveyName) {
    return new Promise(function (resolve, reject) {
        const tableSvc = azurestorage.createTableService();
        tableSvc.createTableIfNotExists(constants.responsesTableNamePrefix + surveyName,
            function (error, result, response) {
                if (error) {
                    reject(error);
                } else {
                    const query = new azurestorage.TableQuery();

                    tableSvc.queryEntities(constants.responsesTableNamePrefix + surveyName, query, null, function (error, result) {
                        if (error) {
                            reject(error);
                        } else {
                            resolve(result.entries.map(entry => {
                                const data = {
                                    Data: entry.Data._
                                };
                                return data;
                            }));
                        }
                    });

                }
            });
    });
}


function deleteSurveyResponses(surveyName) {
    return new Promise(function (resolve, reject) {
        const tableSvc = azurestorage.createTableService();
        tableSvc.deleteTable(constants.responsesTableNamePrefix + surveyName, function (error, result, response) {
            if (error) {
                reject(error);
            } else {
                resolve("Delete table OK");
            }
        });
    });
}

module.exports = {
    insertSurvey,
    insertResponse,
    getSurveyResponses,
    deleteSurveyResponses,
    getSurveys
};