import { attemptChallenge } from "./compiler.js";
/*
 * Use http://uxdev.cegep-heritage.qc.ca:2358/submissions if using uxdev
 * Use http://localhost:2358/submissions if running on local maching -> must have Judge0 installed
*/

const url = "http://uxdev.cegep-heritage.qc.ca:2358/submissions";
const btnAttempt = document.querySelector("#solution-attempt");
const btnReset = document.querySelector("#solution-reset");
const dropdownLanguages = document.querySelector("#ChallengeLanguages");
const output = document.querySelector("#output");
const codeArea = document.querySelector("#code-area");

document.getElementById("defaultOpen").addEventListener('click', (e) => {
    openTab(e, 'Info')
});
document.getElementById("outputTab").addEventListener('click', (e) => {
    openTab(e, 'Output')
});

btnReset.addEventListener("click", () => {
    codeArea.innerHTML = "";
});
document.getElementById("defaultOpen").click();

function openTab(evt, tabName) {
    let i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tabName).style.display = "block";
    evt.currentTarget.className += " active";
}

$(document).ready(function () {
    let code = $(".codemirror-textarea")[0];
    let editor = CodeMirror.fromTextArea(code, {
        lineNumbers: true,
        theme: "darcula"
    });
    let editorVal = editor.getValue();

    btnReset.addEventListener('click', async () => {
        editor.setValue(editorVal);
    });

    btnAttempt.addEventListener('click', async () => {
        editorVal = editor.getValue();
        document.getElementById("outputTab").click();

        // TODO: The last parameter is hardcoded to 71 (python) for the langugageId. 
        // This will have to be changes to a value from the dropdown above
        output.innerHTML = "<span class='text-light'>></span>   " + await attemptChallenge(editorVal, url, 71);
    });
});

