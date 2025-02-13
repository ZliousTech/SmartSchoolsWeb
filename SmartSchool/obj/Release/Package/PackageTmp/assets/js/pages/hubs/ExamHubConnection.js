const examHub = $.connection.examsHub;

function handleSignalRFailure() {
    console.error("OPPS Check your SignalR Connection: ");
}


function sendExamNotification(examsectionID, message) {
    $.connection.hub.start()
        .done(() => {
            examHub.server.sendMessageToGroup(examsectionID, message);
        })
        .fail(() => {
            handleSignalRFailure()
        });
}


function joinToExamGroup(studentsectionID) {
    $.connection.hub.start()
        .done(() => {
            examHub.server.joinExamGroup(studentsectionID);
        })
        .fail(() => {
            handleSignalRFailure()
        });
}

function getExamNotification() {
    examHub.client.receiveMessage = (message) => {
        notificationBellPlay('Exam Notification', message);
    };
}

function notificationBellPlay(header, message) {
    document.getElementById("notification").play();
    $("#bell-icon").addClass("bell");
    Swal.fire(
        header,
        message,
        'info'
    );
}
