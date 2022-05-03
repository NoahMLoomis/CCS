$('#testing-suite-tab').on('click', () => {
    let testCases = getTestCases();
    insertTestCases(testCases);
});

function getTestCases() {
    let ul = document.getElementById('testCaseList');
    let listItems = ul.getElementsByTagName('li');
    return listItems;
};

function insertTestCases(listItems) {
    let resultsTable = document.getElementById("resultsTableBody");
    resultsTable.innerHTML = "";
    for (let i = 0; i < listItems.length; i++) {
        testCaseNumber = listItems[i].getElementsByTagName('span')[0];
        expectedResult = listItems[i].getElementsByTagName('input')[0];
        parameters = listItems[i].getElementsByTagName('input')[1];
        testCaseNumberValue = testCaseNumber.textContent || testCaseNumber.innerText || testCaseNumber.value;
        expectedResultValue = expectedResult.textContent || expectedResult.innerText || expectedResult.value;
        parametersValue = parameters.textContent || parameters.innerText || parameters.value;
        let thisRow = resultsTable.insertRow(i);
        let cellTestCaseNumber = thisRow.insertCell(0);
        let cellParametersValue = thisRow.insertCell(1);
        let cellExpectedResult = thisRow.insertCell(2);
        let cellTestResult = thisRow.insertCell(3);
        let cellPassed = thisRow.insertCell(4);
        cellTestCaseNumber.innerHTML = "Test Case " + testCaseNumberValue;
        cellParametersValue.innerHTML = parametersValue;
        cellExpectedResult.innerHTML = expectedResultValue;
        cellTestResult.innerHTML = "Not Test";
        cellPassed.innerHTML = "";
    }
}
