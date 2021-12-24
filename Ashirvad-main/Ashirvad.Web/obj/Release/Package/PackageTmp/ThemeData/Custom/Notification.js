

function success(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'success message..!!!';
    }
    Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 5000,
        successwithDelay:5000
    }).fire({
        type: 'success',
        title: Message
    });
}

function error(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'error message..!!!';
    }
    Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 5000
    }).fire({
        type: 'error',
        title: Message
    });
}

function warning(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'warning message..!!!';
    }
    Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    }).fire({
        type: 'warning',
        title: Message
    });
}

function info(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'info message..!!!';
    }
    Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    }).fire({
        type: 'info',
        title: Message
    });
}

function question(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'question message..!!!';
    }
    Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    }).fire({
        type: 'question',
        title: Message
    });
}


function SAConfirm(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'success message..!!!';
    }
    Swal.fire({
        title: 'Continue Purchase?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: `Yes`,
        cancelButtonText: `Change Order Quantity`,
        text: Message,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        
        if (result.value == true) {
            
            return 1;
        } else if (result.value == false) {
            return 0;
        }
    })
}
//sweetalert

function SAsuccess(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'success message..!!!';
    }
    Swal.fire({
        title: 'Success',
        text: Message,
        type: 'success',
        confirmButtonText: 'OK',
        successwithDelay:1000
    });
}

function SAerror(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'error message..!!!';
    }
    Swal.fire({
        title: 'Error!',
        html: Message,
        type: 'error',
        confirmButtonText: 'OK',
        
    });
}

function SAwarning(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'warning message..!!!';
    }
    Swal.fire({
        title: 'Warning',
        text: Message,
        type: 'warning',
        confirmButtonText: 'OK'
    });
}

function SAinfo(Message) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'info message..!!!';
    }
    Swal.fire({
        title: 'Info',
        text: Message,
        type: 'info',
        confirmButtonText: 'OK'
    });
}

function SAquestion(Message) {
    if (Message === null || Message === '' || Message === "")
    {
        Message = 'question message..!!!';
    }
    Swal.fire({
        title: 'Question',
        text: Message,
        type: 'question',
        confirmButtonText: 'OK',
        cancelButtonText: 'No, keep it'
       
        
        
    });
}


function successwithDelay(Message, showtime) {
    if (Message === null || Message === '' || Message === "") {
        Message = 'success message..!!!';
    }
    Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: true,
        time: showtime
    }).fire({
        type: 'success',
        title: Message
    });
}






