export const attemptChallenge = async (code, url, languageId = 71) => {
    const submissionToken = await createToken(code, url, languageId);
    const submissionDetails = await getSubmission(submissionToken.token, url);
    return submissionDetails.stdout
};

const createToken = async (code, url, langaugeId) => {
    console.log(JSON.stringify(code))
    const response = await fetch(url, {
        "method": "POST",
        "headers": {
            "content-type": "application/json"
        },
        "body": JSON.stringify({
            "language_id": langaugeId,
            "source_code": code
        })
    });
    return response.json();
};

const getSubmission = async (token, url) => {
    let response = await fetch(`${url}/${token}`);
    let parsedResponse = await response.json();

    while (parsedResponse.status.id === 1 || parsedResponse.status.id === 2) {
        response = await fetch(`${url}/${token}`);
        parsedResponse = await response.json();
    }
    return parsedResponse;
};