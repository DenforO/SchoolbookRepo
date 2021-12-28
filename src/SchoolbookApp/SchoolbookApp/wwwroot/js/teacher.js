
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

function ShowClassStudents(id) {
    window.location.href = "/Profiles/TeacherClass?id="+id;
}

// TO DO: pass id
function ShowTeacherProgram() {
    window.location.href = "/Profiles/TeacherProgram";
}

$(function () {
    $("#dialog").dialog({
        autoOpen: false, modal: true, show: "blind", hide: "blind"
    });
});

function SaveStudentChanges() {
    var title = 'Промените бяха записани успешно!';
    $("#dlTitle").html(title);
    $("#dialog").dialog("open");
    return false;
}

//TO DO
function AddAbsence(studentId, subjId) {
    window.location.href = "/Absences/Create?studentid=" + studentId + "&subjid=" + subjId;
}