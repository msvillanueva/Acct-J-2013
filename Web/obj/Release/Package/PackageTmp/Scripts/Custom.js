$(function () {

    //===== Hide/show sidebar =====//
    $('.fullview').click(function () {
        $("body").toggleClass("clean");
        $('#sidebar').toggleClass("hide-sidebar mobile-sidebar");
        $('#content').toggleClass("full-content");
    });



    //===== Hide/show action tabs =====//
    $('.showmenu').click(function () {
        $('.actions-wrapper').slideToggle(100);
    });

    $('.j-datepicker').datepicker();

    $('.j-numeric-only').keydown(function (event) {
        // Allow: backspace, delete, tab, escape, and enter
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        else {
            // Ensure that it is a number and stop the keypress
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    //===== Easy tabs =====//

    //$('.sidebar-tabs').easytabs({
    //    animationSpeed: 150,
    //    collapsible: false,
    //    tabActiveClass: "active"
    //});

    //$('.actions').easytabs({
    //    animationSpeed: 300,
    //    collapsible: false,
    //    tabActiveClass: "current"
    //});

    //===== Collapsible plugin for main nav =====//
    //$('.expand').collapsible({
    //    defaultOpen: 'current,third',
    //    cookieName: 'navAct',
    //    cssOpen: 'subOpened',
    //    cssClose: 'subClosed',
    //    speed: 200
    //});



});

jQuery.fn.filterByText = function (textbox) {
    return this.each(function () {
        var select = this;
        var options = [];
        $(select).find('option').each(function () {
            options.push({ value: $(this).val(), text: $(this).text() });
        });
        $(select).data('options', options);

        $(textbox).bind('change keyup', function () {
            var options = $(select).empty().data('options');
            var search = $.trim($(this).val());
            var regex = new RegExp(search, "gi");

            $.each(options, function (i) {
                var option = options[i];
                if (option.text.match(regex) !== null) {
                    $(select).append(
                        $('<option>').text(option.text).val(option.value)
                    );
                }
            });
        });
    });
};

function ToDecimalString(num) {
    var n = num.toFixed(2);
    var parts = n.toString().split(".");
    return parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + (parts[1] ? "." + parts[1] : "");
};

