
function ShowDDContents(id) {
    var ddList = document.getElementById("ddSubjectClass");
    var newDDListId = document.getElementById("ddSubjectClass" + id);
    if (newDDListId != null)
        ddList = newDDListId;
    ddList.setAttribute('id', 'ddSubjectClass' + id);
    var newId = ddList.getAttribute('id');
    document.getElementById(newId).classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

// TO DO: pass id
function ShowClassStudents() {
    window.location.href = "/Profiles/TeacherClass";
}

// TO DO: pass id
function ShowMainClassStudents() {
    window.location.href = "/Profiles/TeacherClass";
}

// TO DO: pass id
function ShowTeacherProgram() {
}

function ddReason_Change(val) {
    var reasonText = document.getElementById("reasonText");
    if (val == "3")
        reasonText.style.display = "";
    else
        reasonText.style.display = "none";
}
