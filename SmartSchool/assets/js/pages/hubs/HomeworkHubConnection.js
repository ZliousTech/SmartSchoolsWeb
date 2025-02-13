const homeworkHub = $.connection.homeworkHub;

function handleSignalRFailure() {
    console.error("OPPS Check your SignalR Connection: ");
}


function sendHomeworkNotification(homeworksectionID, message) {
    $.connection.hub.start()
        .done(() => {
            homeworkHub.server.sendMessageToHomeworkGroup(homeworksectionID, message);
        })
        .fail(() => {
            handleSignalRFailure()
        });
}


function joinToGroup(studentsectionID) {
    $.connection.hub.start()
        .done(() => {
            homeworkHub.server.joinHomeworkGroup(studentsectionID);
        })
        .fail(() => {
            handleSignalRFailure()
        });
}

function getHomeworkNotification() {
    homeworkHub.client.receiveHomeworkMessage = (message) => {
        notificationBellPlay('Homework Notification', message);
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
