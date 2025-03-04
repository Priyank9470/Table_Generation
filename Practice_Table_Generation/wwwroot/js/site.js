function showInPopup(url, title) {
    debugger
    $(".preloader").show();
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            if (url.indexOf('/Task/Create') > -1) {
                $('#form-modals .modal-body').html(res);
                $('#form-modals .modal-title').html(title + "<span  style=\"font-size: 12px;margin-left: 0px;\" class=\"header_info\"><i class=\"fas fa-info-circle\" title=\"This screen will allow the user to view " + title + " \" style=\"position: absolute; margin-left: 4px; margin-top: -6px;\"></i></span>");
                $('#form-modals').modal('show');
                $(".preloader").hide();
                // document.getElementById("ParentsMeetingId").value = url.split('back/')[1];
            }
            else {
                $('#form-modal .modal-body').html(res);
                $('#form-modal .modal-title').html(title + "<span  style=\"font-size: 12px;margin-left: 0px;\" class=\"header_info\"><i class=\"fas fa-info-circle\" title=\"This screen will allow the user " + title + " \" style=\"position: absolute; margin-left: 4px; margin-top: -6px;\"></i></span>");
                $('#form-modal').modal('show');
                $(".preloader").hide();
            }

            $('#form-modal').on('hidden.bs.modal', function () {
                $('#form-modal .modal-body').html('');
            })
            jQuery('.datepicker').datepicker({
                autoclose: true,
                todayHighlight: true,
                format: 'dd-M-yyyy',
                //endDate: '0'
            }).attr('readonly', false);
        },
        failure: function (response) {
            $(".preloader").hide();
            console.log(response.responseText)
        },
        error: function (response) {
            if (url.indexOf('/Task/create') > -1) {
                ShowNotifications(response);
            }
            $(".preloader").hide();
            console.log(response.responseText);
        }
    })
}