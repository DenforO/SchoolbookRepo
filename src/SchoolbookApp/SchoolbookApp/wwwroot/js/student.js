// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ShowClassField() {
    var roleValue = document.getElementById("roles").value;
    var roles = document.getElementById("pedal");;
    if (roleValue == "Student") {
        var labelForInput = document.createElement("label");
        labelForInput.setAttribute("for", "class");
        labelForInput.setAttribute("id", "labelForClass")
        labelForInput.setAttribute("name", "Input.Class")
        labelForInput.innerHTML = "Клас:";
        var input = document.createElement("input");
        input.setAttribute("id", "class");
        input.setAttribute("name","Input.Class")
        roles.appendChild(labelForInput);
        roles.appendChild(input);
        
    }
    else {
        var input = document.getElementById("class");
        var label = document.getElementById("labelForClass");
        roles.removeChild(input);
        roles.removeChild(label);
    }

}
