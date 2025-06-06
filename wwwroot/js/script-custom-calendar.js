var routeURL = location.protocol + "//" + location.host;

$(document).ready(function () {
    $("#startdate").kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });

    console.log("Checking for calendar:", document.getElementById('calendar'));

    if (document.getElementById('calendar')) {
        console.log("Calendar found. Initializing...");
        InitializeCalendar();
    } else {
        console.log("Calendar not found. Initializing...");
        // Try again after slight delay
        setTimeout(InitializeCalendar, 500);
    }
});

//function InitializeCalendar() {
//    try {
//        $('#calendar').fullCalendar({
//            timezone: false,
//            header: {
//                left: 'prev,next today',
//                center: 'title',
//                right: 'month,agendaWeek,agendaDay'
//            },
//            selectable: true,
//            editable: false,
//            select: function (event) {
//                onShowModal(event, null);
//    }
//    });
//    }
//    catch (e) {
//        alert(e);
//    }
//}
// function OnCloseModal(){
//  $("#appointmentInput").modal("hide");
//}
//function onShowModal(obj, isEventDetail) {
//    $("#appointmentInput").modal("show");
//}


function InitializeCalendar() {
    try {
        var calendarEl = document.getElementById('calendar');

        if (calendarEl) {
                calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay:'block',
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: routeURL + '/api/Appointment/GetCalendarData?doctorId=' + $("#DoctorId").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {
                            var events = [];
                            if (response.status == 1) {
                                $.each(response.dataenum, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startDate,
                                        end: data.endDate,
                                        backgroundColor: data.isDoctorApproved ? "#28a745" : "#dc3545",
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: data.id

                                    });
                                })
                            }
                            successCallback(events);
                        },
                        error: function (xhr) {
                            $.notify("Error", "error")
                        }

                    })
                },
                eventClick: function (info) {
                    getEventDetailsByEventId(info.event);
                }

            });

            calendar.render();
        }

    }
    catch (e) {
        console.error("Error initializing calendar:", e);
    }
}
$("#DoctorId").change(function () {

    $("#calendar").html("");
    InitializeCalendar();
});
    
function OnCloseModal() {
    $("#appointmentForm")[0].reset();
    $("#id").val(0);
    $("#title").val(''),
    $("#description").val(''),
    $("#startdate").val(''),
    $("#Duration").val(''),
    $("#appointmentInput").find("#PatientId").val(''),
    $("#appointmentInput").modal("hide");
}

function onShowModal(obj, isEventDetail) {
    if (isEventDetail != null) {
            $("#title").val(obj.title),
            $("#description").val(obj.description),
            $("#startdate").val(obj.startDate),
            $("#Duration").val(obj.duration),
            $("#DoctorId").val(obj.doctorId),
            $("#appointmentInput").find("#PatientId").val(obj.patientId),
            $("#id").val(obj.id),
            $("#lbdoctor").html(obj.doctorName),
            $("#lbpatient").html(obj.patientName)
        if (obj.isDoctorApproved) {
            $("#lbstatus").html('Approved');
            $("#btnConfirm").addClass("d-none");
            $("#btnSubmit").addClass("d-none");
        }
        else {
            $("#lbstatus").html('Pending');
            $("#btnConfirm").removeClass("d-none");
            $("#btnSubmit").removeClass("d-none");
        }
        $("#btnDelete").removeClass("d-none");
    }
    else {
        $("#startdate").val(obj.startStr + " " + new moment().format("hh:mm A"));
        $("#id").val(0);
        $("#btnDelete").addClass("d-none");
        $("#btnSubmit").removeClass("d-none");
    }
    $("#appointmentInput").modal("show");
}
var calendar;
function onSubmitForm(event) {
    event.preventDefault(); 
    if (CheckValidation()) {
        console.log("🔹 Submit started");
        let rawId = $("#id").val();
        var requestData = {
            Id: rawId ? parseFloat(rawId) : null,
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartDate: $("#startdate").val(),
            Duration: parseInt($("#Duration").val()),
            DoctorId: $("#DoctorId").val(),
            PatientId: $("#appointmentInput").find("#PatientId").val(),
            IsDoctorApproved: false,
            IsForClient: false
        };
        console.log("Request Data:", requestData);
        $.ajax({
            url: routeURL + '/api/Appointment/SaveCalendarData',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status == 1 || response.status == 2) {
                    $.notify(response.message, "success")
                    console.log("✅ Success:", response);
                    OnCloseModal();
                    calendar.refetchEvents();
                }
                else {
                    $.notify(response.message, "error")
                }
            },
            error: function (xhr) {
                $.notify("Error", "error")
            }

        })
    }
}

function CheckValidation() {
    var isValid = true;
    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }
    if ($("#startdate").val() === undefined || $("#startdate").val() === "") {
        isValid = false;
        $("#startdate").addClass('error');
    }
    else {
        $("#startdate").removeClass('error');
    }
    return isValid;
}

function getEventDetailsByEventId(info) {
    console.log("ajax started")
    $.ajax({
        url: routeURL + '/api/Appointment/GetCalendardataById/' +info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status == 1 && response.dataenum !== undefined) {
                onShowModal(response.dataenum, true);
            }
            //successCallback(events);
        },
        error: function (xhr) {
            $.notify("Error", "error")
        }

    })
}

function onDeleteAppointment() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeURL + '/api/Appointment/DeleteAppointment/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {

            if (response.status === 1) {
                $.notify(response.message, "success");
                calendar.refetchEvents();
                OnCloseModal();
            }
            else {

                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onConfirm() {
    var id = parseInt($("#id").val());
    $.ajax({
        url: routeURL + '/api/Appointment/ConfirmAppointment/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {

            if (response.status === 1) {
                $.notify(response.message, "success");
                calendar.refetchEvents();
                OnCloseModal();
            }
            else {

                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}