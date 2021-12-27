
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
    window.location.href = "/Subjects/StudentsProgram";
}

function ddReason_Change(val) {
    var reasonText = document.getElementById("reasonText");
    if (val == "3")
        reasonText.style.display = "";
    else
        reasonText.style.display = "none";
}

$(function () {
    $("#dialog").dialog({
        autoOpen: false, modal: true, show: "blind", hide: "blind"
    });
});

function SaveClassChanges() {

    //var grade = $('#ddGrades').text();
    //var reason = $('#ddReason').val();
    //var reasonText = $('#ddReason').text();
    //if (reason == 3)
    //    reasonText = $('#reasonText').text();
    //var note = $('#noteText').text();
    //var noteData = $('#dataNoteText').text();
    //var absence = $('#absenceText').text();
    //var absenceData = $('#dataAbsenceText').text();

    //var params = {
    //    grade: grade,
    //    reason: reasonText,
    //    note: note,
    //    noteData: noteData,
    //    absence: absence,
    //    absenceData: absenceData
    //}

    //$.ajax(
    //    {
    //        type: "POST",
    //        url: "Profiles/SaveChanges",
    //        data: params,
    //        dataType: "json",
    //        success: function (result) {
    //            //dialogbox
    //            //презареди с нови данни
    //        }
    //    }
    //)

    var title = 'Промените бяха записани успешно!';
    $("#dlTitle").html(title);
    $("#dialog").dialog("open");
    return false;
}