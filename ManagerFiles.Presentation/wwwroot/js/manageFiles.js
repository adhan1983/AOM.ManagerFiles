$(document).ready(function () {    

    var connection = new signalR.HubConnectionBuilder().withUrl("/broadcast").build();
    connection.on("feedBack", function (model) {
        $("#progress_bar").css("width", `${model.currentPercent}%`);
        $("#nameFile").html(model.nameFile);
        $("#uploadCount").html(model.currentCount);
    });
    connection.start().then(function () {
        console.log("Sinalr Connected");
    }).catch(function (err) {
        console.log(err.toString());
    });    
    $("#btnUpload").click(function (e) {
        e.preventDefault();
        uploadFile();

    });
    $("#btnCopyAndMove").click(function (e) {
        e.preventDefault();
        copyOrMoveFilesAsync();
    });
    defaultTableContent();   
    
});

function defaultTableContent() {
    var empltyTableTemplate = $("<tr></tr>");
    var placeholder = "<td colSpan='7'><p>No Data</p></td>";
    empltyTableTemplate.html(placeholder);
    $("#tableBody").html(empltyTableTemplate);
}
function fileToUploadOnchange() {
    $this = document.getElementById("fileToUpload");
    fileSelected($this);
}
function fileSelected(input) {    

    var file = document.getElementById('fileToUpload').files[0];

    document.getElementById("uploadFile").value = file.name;    
}

function uploadFile() {
    var file = document.getElementById('fileToUpload').files[0];
    if (file) {
        $("#progressModal").modal("show");
        var url = 'Upload/UploadFile';
        var fd = new FormData();
        fd.append("fileToUpload", file);
        var xhr = new XMLHttpRequest();
        xhr.addEventListener("load", uploadComplete, false);
        xhr.addEventListener("error", uploadFailed, false);
        xhr.addEventListener("abort", uploadCanceled, true);
        xhr.open("POST", url);
        xhr.send(fd);    

    } else {
        alert("You are can not upload an empty file");
    }

}

function copyOrMoveFilesAsync() {    

    $("#progressModal").modal("show");
    var justCopy = $('#justCopy').is(":checked");
    var files = [];
    $("#tbodySourceFiles input[type=checkbox]:checked").each(function () {
        var row = $(this).closest("tr")[0];
        files.push(row.cells[1].innerHTML)               
    });    
    var data = { justCopy: justCopy, files: files };

    var xhr = $.ajax({
        url: '/Upload/CopyOrMoveFiles',
        type: 'POST',
        data: data,        
        success: function (result, status, xhr) {            
            $("#progressModal").modal("hide");
            if (status && result) {
                populateTableSource("tbodyDestinyFiles", result.data.destiny.files, true);
                populateTableSource("tbodySourceFiles", result.data.origin.files, false);
            }            
        },        
        error: function (xhr, status, error) {            
            xhr.abort();
            uploadCanceled();
        }        
    });

    $("#btnCancelMovingFiles").click(function () {
        xhr.abort();
    });

}

function uploadComplete(evt) {

    var response = JSON.parse(evt.target.response);   
    
    if (response.error) {
        alert(response.message);
        return;
    }
    populateTableSource("tbodySourceFiles", response.data.origin.files, false);

    $("#progressModal").modal("hide");

    ResetUploadModal();

}


function populateTableSource(element, data, isTableDestiny) {
    
    if (data.length <= 0) {
        $("#" + element).html('');
        return;
    }

    $("#" + element).html('');    

    $.each(data, function (i, val) {
        

        var trChecked = isTableDestiny ?
            '<input type="checkbox"' + 'checked="checked"' + "name='" + val.name + '"' + "value='" + val.name + '"' + '>' :
            '<input type="checkbox"' + "name='" + val.name + '"' + "value='" + val.name + '"' + '>'

        var tableRow = $("<tr></tr>");
        var tablecells = '<td class="tdCopyOrMovingFile">' +
            trChecked +
            '</td>' +
            '<td class="namesFile">' + val.name + '</td>' +
            '<td>' + val.typeFile + '</td>'

        tableRow.html(tablecells);

        $("#" + element).append(tableRow[0]);
    });
}

function uploadFailed(evt) {
    $("#progressModal").modal("hide");
    ResetUploadModal();
    alert("There was an error attempting to upload the file");
}

function uploadCanceled(evt) {    
    $("#progressModal").modal("hide");
    ResetUploadModal();
}

function ResetUploadModal() {
    document.getElementById("uploadFile").value = '';
    document.getElementById("fileToUpload").value = null;
    $("#progressContainer").hide();
    $("#progress_bar").css("width", "-1%");
}