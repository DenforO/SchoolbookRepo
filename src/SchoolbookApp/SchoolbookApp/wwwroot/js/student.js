// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ShowClassField() {
    var roleValue = document.getElementById("roles").value;
    var roles = document.getElementById("rolesDiv");;
    if (roleValue == "Student") {
        var arrayOfClassNums = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
        var arrayOfClassLetters = ["А", "Б", "В"];

        var labelForClassNum = document.createElement("label");
        labelForClassNum.setAttribute("for", "classNum");
        labelForClassNum.setAttribute("id", "labelForClassNum")
        labelForClassNum.setAttribute("name", "Input.ClassNum")
        labelForClassNum.innerHTML = "Номер на класа:";
        var selectListNum = document.createElement("select");
        selectListNum.setAttribute("id", "classNum");
        selectListNum.setAttribute("name", "Input.ClassNum")
        for (let i = 0; i < arrayOfClassNums.length; i++) {
            var option = document.createElement("option");
            option.value = arrayOfClassNums[i];
            option.text = arrayOfClassNums[i];
            selectListNum.appendChild(option);
        }

        var labelForClassLetter = document.createElement("label");
        labelForClassLetter.setAttribute("for", "classLetter");
        labelForClassLetter.setAttribute("id", "labelForClassLetter");
        labelForClassLetter.setAttribute("name", "Input.ClassLetter");
        labelForClassLetter.innerHTML = "Буква:";
        var selectListLetter = document.createElement("select");
        selectListLetter.setAttribute("id", "classLetter");
        selectListLetter.setAttribute("name", "Input.ClassLetter");
        for (let j = 0; j < arrayOfClassLetters.length; j++) {
            var optionLetter = document.createElement("option")
            optionLetter.value = arrayOfClassLetters[j];
            optionLetter.text = arrayOfClassLetters[j];
            selectListLetter.appendChild(optionLetter);
        }

        roles.appendChild(labelForClassNum);
        roles.appendChild(selectListNum);
        roles.appendChild(labelForClassLetter);
        roles.appendChild(selectListLetter);
        
    }
    else {

        var selectNum = document.getElementById("classNum");
        var labelNum = document.getElementById("labelForClassNum");
        var selectLetter = document.getElementById("classLetter");
        var labelLetter = document.getElementById("labelForClassLetter");
        roles.removeChild(selectNum);
        roles.removeChild(labelNum);
        roles.removeChild(selectLetter);
        roles.removeChild(labelLetter);
    }

}
