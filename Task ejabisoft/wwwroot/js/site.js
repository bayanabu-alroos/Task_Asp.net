
$(document).ready(function () {

    $('.activateEJBtn, .deactivateEJBtn').click(function () {
        var jobEmployeeId = $(this).data('employee-job-id');
        var actionUrl = $(this).hasClass('activateEJBtn') ? '/ActivateJobEmployee' : '/DeactivateJobEmployee';

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: { jobEmployeeId: jobEmployeeId }, // Corrected parameter name
            success: function (response) {
                response
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });

    $('.activateBtn, .deactivateBtn').click(function () {
        var jobId = $(this).data('job-id');
        var actionUrl = $(this).hasClass('activateBtn') ? '/Job/ActivateJob' : '/Job/DeactivateJob';

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: { jobId: jobId },
            success: function (response) {
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });
    $('.deleteBtn').click(function () {
        var employeeId = $(this).data('job-id');
        var actionUrl = $(this).hasClass('deleteBtn') ? '/DeletedEmployee' : '';

        $.ajax({
            url: actionUrl,
            type: 'POST',
            data: { employeeId: employeeId }, // Corrected key name to match the parameter name in the controller action
            success: function (response) {
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('Error:', error); // Log the error message
            }
        });
    });

});
$(function () {
    $('#updateEmployeeModal').on('show.bs.modal', function (event) {
        console.log('Modal shown');

        var button = $(event.relatedTarget);
        var employeeId = button.data('job-id');
        var imageUrl = button.data('image');
        var employmentDate = button.data('employment_date');
        var name = button.data('name');
        var birthdate = button.data('birthdate');
        var governorate = button.data('governorate');
        var phone = button.data('phone');
        var underProbation = button.data('under_probation');

        console.log("Employee ID: " + employeeId);
        console.log("Image URL: " + imageUrl);
        console.log("Employment Date: " + employmentDate);
        console.log("Name: " + name);
        console.log("Birthdate: " + birthdate);
        console.log("Governorate: " + governorate);
        console.log("Phone: " + phone);
        console.log("Under Probation: " + underProbation);

        // Update modal fields with job details
        var modal = $(this);
        modal.find('#employeeId').val(employeeId);
        modal.find('#employeeName').val(name);
        modal.find('#governorate').val(governorate);
        modal.find('#employmentDate').val(employmentDate);
        modal.find('#birthdate').val(birthdate);
        modal.find('#phone').val(phone);
        modal.find('#underProbation').prop('checked', underProbation);

        // Display the image URL
        $('#imageUrlDisplay').attr('src', imageUrl); // Assuming imageUrlDisplay is an image element where you want to display the image
    });

    $('#updateEmployeeForm').submit(function (event) {
        event.preventDefault();
        var formData = new FormData(this); // Use FormData to handle file uploads
        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                $('#updateEmployeeModal').modal('hide');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});

/*
$(function () {
    $('#updateEmployeeModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var employeeId = button.data('job-id');
        var imageUrl = button.data('image');
        var employmentDate = button.data('employment_date');
        var name = button.data('name');
        var birthdate = button.data('birthdate');
        var governorate = button.data('governorate');
        var phone = button.data('phone');
        var underProbation = button.data('under_probation');

        // Update modal fields with job details
        var modal = $(this);
        modal.find('#employeeId').val(employeeId);
        modal.find('#employeeName').val(name);
        modal.find('#governorate').val(governorate);
        modal.find('#employmentDate').val(employmentDate);
        modal.find('#birthdate').val(birthdate);
        modal.find('#phone').val(phone);
        modal.find('#underProbation').prop('checked', underProbation);
    });

    $('#updateEmployeeForm').submit(function (event) {
        event.preventDefault();
        var formData = new FormData($(this)[0]); // Use FormData to handle file upload
        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: formData,
            processData: false, // Prevent jQuery from processing the data
            contentType: false, // Prevent jQuery from setting the content type
            success: function (response) {
                $('#updateEmployeeModal').modal('hide');
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});

*/

$(function () {
    $('#updateModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); 
        var jobId = button.data('job-id');
        var jobName = button.data('name');
        var jobCategory = button.data('category');

        var modal = $(this);
        modal.find('#jobId').val(jobId);
        modal.find('#jobName').val(jobName);
        modal.find('#jobCategory').val(jobCategory);
    });

    $('#updateJobForm').submit(function (event) {
        event.preventDefault();
        var formData = $(this).serialize(); 
        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: formData,
            success: function (response) {
                $('#updateModal').modal('hide'); 
                window.location.reload();
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});
