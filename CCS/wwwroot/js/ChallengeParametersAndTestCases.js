parameterListItem = document.getElementById('hiddenParameter').getElementsByTagName('li')[0];
testCaseListItem = document.getElementById('hiddenTestCase').getElementsByTagName('li')[0];
let testCaseParameterList = document.querySelectorAll(".tc-params-row");
let testCaseList = document.querySelectorAll("#testCaseList li");
let allTestCases = document.getElementById("testCaseList");
let allParameters = document.getElementById("parameter_template");

function getNextId(item) {
    return parseInt(item.id.substring(item.id.length - 1)) + 1;
}

function getNextParameterId() {
    if (allParameters.children.length === 0) {
        return 0;
    } else {
        return parseInt((allParameters.children[allParameters.children.length - 1].id).charAt((allParameters.children[allParameters.children.length - 1].id).length - 1))+1;
    }
}

function getNextTestCaseId() {
    return parseInt(allTestCases.children[allTestCases.children.length - 1].children[0].children[0].textContent) + 1;
}

function addSingleParametersToTestCase() {
    let inputNode = document.createElement("input");

    inputNode.type = "text";
    inputNode.placeholder = "string"
    inputNode.className = "form-control col-2 tc-param";

    return inputNode;
}

function addParametersToAllTestCases(nextParamInt) {
    let newParam = addSingleParametersToTestCase();

    for (const li in allTestCases.children) {
        if (allTestCases.children[li].nodeName === "LI") {
            newParam.name = `TestCases[${li}].ParameterValues[${nextParamInt}]`;
            allTestCases.children[li].children[2].appendChild(newParam.cloneNode(true));
        }        
    }
}

function addParameter() {
    let nextParamInt = getNextParameterId();
    parameterListItem.id = `parameter${nextParamInt}`;
    parameterListItem.querySelector('.parameter_dataTypeId').name = `Parameters[${nextParamInt}].DataTypeId`;
    parameterListItem.querySelector('.parameter_name').name = `Parameters[${nextParamInt}].ParameterName`;
    parameterListItem.querySelector('.parameter_defaultValue').name = `Parameters[${nextParamInt}].DefaultValue`;
    parameterListItem.querySelector('.parameter_positon').name = `Parameters[${nextParamInt}].Position`;
    allParameters.appendChild(parameterListItem.cloneNode(true));
    addParametersToAllTestCases(nextParamInt);
}

function removeParameterFromTestCases() {
    for (const li in allTestCases.children) {
        if (allTestCases.children[li].nodeName === "LI") {
            allTestCases.children[li].children[2].children[allTestCases.children[li].children[2].children.length - 1].remove();
        }
    }
}

function addTestCase() {
    let newListItemnode;
    let newId;

    if (allTestCases.children[0] !== undefined) {
        newListItemnode = allTestCases.children[0].cloneNode(true);
        newId = getNextTestCaseId();
    } else {
        newListItemnode = testCaseListItem.cloneNode(true);
        newId = 1

        for (let i = 0; i < allParameters.children.length - 1; i++) {
            let newParam = addSingleParametersToTestCase();
            newParam.name = `TestCases[${i}].ParameterValues[${i + 1}]`;
            newListItemnode.querySelector('.tc-params-row').appendChild(newParam.cloneNode(true))
        }
    }

    newListItemnode.id = `testCase${newId}`;

    newListItemnode.querySelector('#testCaseId').textContent = newId;

    newListItemnode.querySelector('.tc-expected-field').name = `TestCases[${newId - 1}].ExpectedResult`;

    [...newListItemnode.children[2].children].forEach((el, index) => {
        el.name = `TestCases[${newId-1}].TestCaseParameter[${index}].Value`;
    })

    allTestCases.appendChild(newListItemnode.cloneNode(true));
}

function checkbox_changed() {
    var hiddenInput = event.target.parentNode.querySelectorAll("input[type='hidden']")[0];
    hiddenInput.value = event.target.checked;
}

function updateParamters() {
    [...allParameters.children].forEach((param, index) => {
        param.id = `parameter${index}`;
        param.querySelector('.parameter_dataTypeId').name = `Parameters[${index}].DataTypeId`;
        param.querySelector('.parameter_name').name = `Parameters[${index}].ParameterName`;
        param.querySelector('.parameter_defaultValue').name = `Parameters[${index}].DefaultValue`;
    })

}

function removeParamItem() {
    event.target.parentNode.parentNode.parentNode.parentNode.remove();
    updateParamters();
    removeParameterFromTestCases();
}


function updateTestCaseNumber() {
    [...allTestCases.children].forEach((el, index) => {
        el.id = `testCase${index + 1}`;
        el.children[0].children[0].textContent = index + 1;
        el.children[1].name = `TestCases[${index}].ExpectedResult`;

        [...el.children[2].children].forEach((testParam, y) => {
            let replacedText = testParam.name.replace(testParam.name.substring(10, 11), index)
            testParam.name = replacedText;
        })

    })
}

function removeTestCase() {
    event.target.parentNode.remove();
    updateTestCaseNumber();
}




/**
 * The following JS is a better way to do it, but we don't have enough time to merge it with the main branch.
 * The CSS changes that go with the below JS are in the "Nahom_Integrate_Param_TestCase_Frontent_To_Create_Edit_Challenges"
 * /

//const addParameterBtn = document.querySelector("#addParameterBtn")
//const parameterDiv = document.querySelector(".param-div-scroll")
//const addTestCaseBtn = document.querySelector("#addTestCaseBtn")
//const testCaseDiv = document.querySelector(".tc-div-scroll")

//class ParameterList {
//    constructor() {
//        /**
//         *  Getting the first element that's hardcoded as "_TestCase.cshtml" is needed
//         *  becuase if all testCases get deleted, at least there's a copy stored here
//         *  */
//        this.firstElement = document.querySelector(".parameter_item")
//        this.listItems = []
//    }

//    displayParameters() {
//        this.listItems.forEach(item => {
//            parameterDiv.append(item.element)
//        })
//    }

//    addFirstParameter() {
//        this.listItems.push(new Parameter(this.firstElement))
//    }

//    addParameter() {
//        /**
//         * This checks if all parameters have been deleted, if so, it
//         * appends the "firstElement" one to the array
//         *  */
//        if (this.listItems.length <= 0) {
//            this.addFirstParameter()
//        } else {
//            let cloneOfLastParam = this.listItems[this.listItems.length - 1].element.cloneNode(true)
//            cloneOfLastParam.querySelector(".parameter_dataType").name = `Parameters[${this.listItems.length}].DataTypeId`;
//            cloneOfLastParam.querySelector(".parameter_name").name = `Parameters[${this.listItems.length}].ParameterName`;
//            cloneOfLastParam.querySelector(".default-value").name = `Parameters[${this.listItems.length}].DefaultValue`;
//            this.listItems.push(new Parameter(cloneOfLastParam))
//        }
//        this.displayParameters()
//    }

//    removeParameter() {
//        this.listItems.pop()
//    }
//}

//class Parameter {
//    constructor(elem) {
//        this.element = elem
//        this.deleteButton = this.element.querySelector(".remove_parameter_btn")
//        this.parameterName = this.element.querySelector(".parameter_name")
//        this.defaultValue = this.element.querySelector(".default-value")

//        this.deleteButton.addEventListener("click", () => removeParameter(elem))
//    }
//}

//class TestCaseList {
//    constructor() {
//        /**
//         *  Getting the first element that's hardcoded as "_Parameter.cshtml" is needed
//         *  becuase if all parameters get deleted, at least there's a copy stored here
//         *  */
//        this.firstElement = document.querySelector(".test_case_item")
//        this.listItems = []
//    }

//    displayTestCases() {
//        this.listItems.forEach(item => {
//            testCaseDiv.append(item.element)
//        })
//    }

//    addFirstTestCase() {
//        this.listItems.push(new TestCase(this.firstElement))
//    }

//    addTestCase() {
//        /**
//         * This checks if all parameters have been deleted, if so, it
//         * appends the "firstElement" one to the array
//         *  */
//        if (this.listItems.length <= 0) {
//            this.addFirstTestCase()
//        } else {
//            let cloneOfLastTestCase = this.listItems[this.listItems.length - 1].element.cloneNode(true);
//            cloneOfLastTestCase.querySelector(".tc-expected-field").name = `TestCases[${this.listItems.length}].ExpectedResult`;
//            this.listItems.push(new TestCase(cloneOfLastTestCase));
//        }
//        this.displayTestCases()
//    }

//    refreshTestCases() {
//        this.listItems.forEach(item => {
//            item.resetTestCaseParams()
//        })
//    }

//    removeTestCase() {
//        this.listItems.pop()
//        this.listItems.forEach(item => {
//            item.removeTestCaseParam()
//        })
//    }
//}

//class TestCase {
//    constructor(elem) {
//        this.element = elem
//        this.param = elem.querySelector(".tc-param")
//        this.deleteButton = this.element.querySelector(".remove_testCase_btn")
//        this.expected = this.element.querySelector(".tc-expected-field")
//        this.testCaseParamsCount = this.getNumParams()
//        this.testCaseParams = []

//        this.deleteButton.addEventListener("click", () => removeTestCase(elem))
//    }

//    getNumParams() {
//        return document.querySelector(".param-div-scroll").querySelectorAll(".parameter_item").length
//    }

//    resetTestCaseParams() {
//        // Reset the params so they don't keep adding.
//        // Pretty sketchy, don't like it.
//        this.element.querySelectorAll(".tc-param").forEach(item => item.remove())
//        if (this.getNumParams() > 0) {
//            this.element.append(this.param)
//            this.testCaseParams = [this.param]

//            for (let i = 0; i < this.getNumParams() - 1; i++) {
//                let clonedTestCaseParam = this.testCaseParams[this.testCaseParams.length - 1].cloneNode(true)
//                this.testCaseParams.push(clonedTestCaseParam)
//                this.element.append(clonedTestCaseParam)
//            }

//        }


//        // let numberOfParams = document.querySelector(".param-div-scroll").querySelectorAll(".parameter_item").length
//        // if (numberOfParams <= this.testCaseParams.length) {
//        //     this.removeTestCaseParam()
//        // } else {
//        //     for (let i = 0; i < numberOfParams; i++) {
//        //         let clonedTestCaseParam = this.testCaseParams[this.testCaseParams.length - 1].cloneNode(true)
//        //         this.testCaseParams.push(clonedTestCaseParam)
//        //         this.element.append(clonedTestCaseParam)
//        //     }
//        // }
//    }

//    removeTestCaseParam() {
//        this.testCaseParams.pop()
//    }

//}

//const pageParams = new ParameterList()
//const pageTestCases = new TestCaseList()


//const removeTestCase = (elem) => {
//    elem.remove()
//    pageTestCases.removeTestCase()
//}

//const removeParameter = (elem) => {
//    elem.remove()
//    pageParams.removeParameter()
//    pageTestCases.refreshTestCases()
//}

//// Event listeners

//document.addEventListener("DOMContentLoaded", () => {
//    pageParams.addFirstParameter()
//    pageTestCases.addFirstTestCase()
//})

//addTestCaseBtn.addEventListener("click", () => {
//    pageTestCases.addTestCase()
//})

//addParameterBtn.addEventListener("click", () => {
//    pageParams.addParameter()
//    pageTestCases.refreshTestCases()
//})
