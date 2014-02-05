$(function () {

    // function for adding municipalities to municipality dropdown
    $('#countyDropDown').prop('selectedIndex', -1);
    $('#countyDropDown').on('change', function (parameters) {
        var countyId = $(this).val();
        $('#municipalityDropDown').empty();
        $.post('/properties/municipalities', { countyId: countyId }, function (data) {
            $.each(data, function (index, element) {
                $('#municipalityDropDown').append(
                    $('<option></option>').val(element).html(element.MunicipalityName).attr("value", element.MunicipalityId));
                $('#municipalityDropDown').prop('selectedIndex', -1);
            });
        });
    });


    //function for adding selected municipality into form input value
    $('#municipalityDropDown').on('change', function () {
        $('input[name=municipalityId]').val($(this).val());
    });


    //function for adding a datepicker into forms
    $(".date-picker").datepicker();

    $(".date-picker").on("change", function () {
        var id = $(this).attr("id");
        var val = $("label[for='" + id + "']").text();
        $("#msg").text(val + " changed");
    });

    //function for inserting paidToDate into subsequents form
    $("#subsequentsDatePicker").on("change", function() {
        var date = $(this).val();
        console.log(date);
        $(".OutLayDate").val(date);
    });

})

