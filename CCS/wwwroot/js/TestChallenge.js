import { attemptChallenge } from "./compiler.js";
/*
 * Use http://uxdev.cegep-heritage.qc.ca:2358/submissions if using uxdev
 * Use http://localhost:2358/submissions if running on local maching -> must have Judge0 installed
*/

const url = "http://uxdev.cegep-heritage.qc.ca:2358/submissions";
const btnAttempt = document.querySelector("#solution-attempt");
const btnReset = document.querySelector("#solution-reset");
const btnTest = document.querySelector("#solution-test");
const output = document.querySelector("#output");
const codeArea = document.querySelector("#code-area");

let editorVal;

$(document).ready(function () {
    let code = $(".codemirror-textarea")[0];
    
    let editor = CodeMirror.fromTextArea(code, {
        lineNumbers: true,
        matchBrackets: true,
        theme: "darcula",
        indentUnit: 4,
    });

    let editorVal = editor.getValue();

    btnReset.addEventListener('click', async () => {
        editor.setValue('');
    });

    btnAttempt.addEventListener('click', async () => {
        editorVal = editor.getValue();
        output.innerHTML = "<span class='text-light'>></span>   " + await attemptChallenge(editorVal, url, 71);
    });

    setTimeout(function () {
        editor.refresh();
    }, 1);
});