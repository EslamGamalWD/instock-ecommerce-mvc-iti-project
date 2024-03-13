function showSuccessAlert(message = "Saved SuccessfulLy!") {
    Swal.fire({
        title: "Operation completed!",
        text: message,
        icon: "success",
        confirmButtonColor: "#32BDEA"
    });
}

function showErrorAlert() {
    Swal.fire({
        icon: "error",
        title: "Operation failed!",
        text: "Something went wrong!",
        confirmButtonColor: "#32BDEA"
    });
}

$(document).ready(function () {
    $('#datatable').on('click', '.js-toggle-status', function () {
        let btn = $(this);

        Swal.fire({
            title: "Are you sure?",
            text: `You will toggle the state of Subcategory#${btn.data("id")}!`,
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#32BDEA",
            cancelButtonColor: "#FF9770",
            confirmButtonText: "Yes, proceed!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.post({
                    url: `/Subcategory/ToggleStatus/${btn.data("id")}`,
                    data: {
                        "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
                    },
                    success: function (modifiedAt) {
                        let row = btn.parents("tr");
                        let status = row.find(".js-status");
                        let newStatus = status.text().trim() === "Deleted" ? "Active" : "Deleted";
                        if (newStatus === "Deleted") {
                            row.find(".js-toggle-icon").removeClass("ri-delete-bin-line");
                            row.find(".js-toggle-icon").addClass("ri-toggle-line");
                        } else {
                            row.find(".js-toggle-icon").removeClass("ri-toggle-line");
                            row.find(".js-toggle-icon").addClass("ri-delete-bin-line");
                        }

                        status.text(newStatus).toggleClass("badge-success badge-warning");
                        row.find(".js-modified-at").html(modifiedAt);
                        
                        row.addClass("animate__animated animate__flash");
                        setTimeout(() => {
                            $(row).removeClass("animate__animated animate__flash")
                        }, 1000);
                        let message = newStatus === "Active"
                            ? `Subcategory#${btn.data("id")} has been activated successfully`
                            : `Subcategory#${btn.data("id")} has been deleted successfully`;
                        showSuccessAlert(message);
                    },
                    error: function () {
                        showErrorAlert();
                    }
                });
            }
        });
    });
});

$(document).ready(function () {
    let message = $("#tr-msg").text();
    if (message !== '')
        showSuccessAlert(message);
});