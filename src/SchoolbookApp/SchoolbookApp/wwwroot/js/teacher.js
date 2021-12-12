
function ShowDDContents() {
    document.getElementById("ddSubjectClass").classList.toggle("show");
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

function ddReason_Change(val) {
    var reasonText = document.getElementById("reasonText");
    if (val == "3")
        reasonText.style.display = "";
    else
        reasonText.style.display = "none";
}
