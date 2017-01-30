jQuery(document).ready(function ($) {

    /* ======= Scrollspy ======= */
    $('body').scrollspy({ target: '#top', offset: 400 });

    /* ======= ScrollTo ======= */
    $('a.scrollto').on('click', function (e) {

        //store hash
        var target = this.hash;

        e.preventDefault();

        $('body').scrollTo(target, 800, { offset: -80, 'axis': 'y', easing: 'easeOutQuad' });
        //Collapse mobile menu after clicking
        if ($('.navbar-collapse').hasClass('in')) {
            $('.navbar-collapse').removeClass('in').addClass('collapse');
        }

    });

    /* ======= Flexslider ======= */
    $('.flexslider').flexslider({
        animation: "fade",
        touch: true,
        directionNav: false
    });

    /* ======= jQuery Placeholder ======= */
    $('input, textarea').placeholder();

    /* ======= jQuery FitVids - Responsive Video ======= */
    $("#video-container").fitVids();

    /* ======= Style Switcher ======= */
    $('#config-trigger').on('click', function (e) {
        var $panel = $('#config-panel');
        var panelVisible = $('#config-panel').is(':visible');
        if (panelVisible) {
            $panel.hide();
        } else {
            $panel.show();
        }
        e.preventDefault();
    });

    $('#config-close').on('click', function (e) {
        e.preventDefault();
        $('#config-panel').hide();
    });


    $('#color-options a').on('click', function (e) {
        var $styleSheet = $(this).attr('data-style');
        var $logoImage = $(this).attr('data-logo');
        $('#theme-style').attr('href', $styleSheet);
        $('#logo-image').attr('src', $logoImage);

        var $listItem = $(this).closest('li');
        $listItem.addClass('active');
        $listItem.siblings().removeClass('active');

        e.preventDefault();

    });

    $('#device-options a').on('click', function (e) {
        var $cssClass = $(this).attr('data-class');
        var $deviceType = $(this).attr('data-type');

        var $iphone = $('#promo').find('.iphone');
        var $android = $('#promo').find('.android');
        var $ipad = $('#promo').find('.ipad');

        if ($deviceType === "iphone") {
            $ipad.hide();
            $android.hide();
            $iphone.show();
        }

        if ($deviceType === "ipad") {
            $iphone.hide();
            $android.hide();
            $ipad.show();
        }

        if ($deviceType === "android") {
            $iphone.hide();
            $ipad.hide();
            $android.show();
        }

        if ($cssClass === "iphone-white") {
            $iphone.removeClass('iphone-black').addClass('iphone-white');
        }

        if ($cssClass === "iphone-black") {
            $iphone.removeClass('iphone-white').addClass('iphone-black');
        }

        if ($cssClass === "ipad-black") {
            $ipad.removeClass('ipad-white').addClass('ipad-black');
        }

        if ($cssClass === "ipad-white") {
            $ipad.removeClass('ipad-black').addClass('ipad-white');
        }

        var $listItem = $(this).closest('li');
        $listItem.addClass('active');
        $listItem.siblings().removeClass('active');

        e.preventDefault();

    });

    function setOS() {
        var ua = navigator.userAgent;
        var type = 'unknown';

        if (ua.match(/Windows Phone/i) != null) {
            type = 'windows';
        } else if (ua.match(/Android/i) != null) {
            type = 'android';
        } else if (ua.match(/iPhone/i) != null) {
            type = 'ios';
        }

        $('body').addClass('os-' + type);
    };

    setOS();

    $('#ajax-contact-form button[type=submit]').on('click', function (e) {
        e.stopPropagation();
        e.preventDefault();

        if (false === $('#ajax-contact-form').parsley().validate('block1')) {
            if (window.appInsights) {
                window.appInsights.trackPageView('send-feedback-validation-failed');
            }
            return false;
        }
        if (window.appInsights) {
            window.appInsights.trackPageView('send-feedback');
        }
        var name = $('#contact-name').val();
        var email = $('#contact-email').val();
        var message = $('#contact-message').val();
        $.post('api/contact', {
            name: name,
            email: email,
            message: message,
        }, function (result) {
            if (result == 'success') {
                $.notify("Vielen Dank für die Kontaktaufnahme. Wir melden uns umgehend.", { position: "bottom right", className: "success" });
            } else {
                $.notify("Uppps. Das hätte nicht passieren dürfen. Ihre Nachricht konnte nicht verschickt werden.", { position: "bottom right", className: "error" });
            }
        });

        return false;
    });

    $('#promo-download-general').on('click', function (e) {
        if (window.appInsights) {
            window.appInsights.trackPageView('download-general-promo');
        }
    });
    $('#promo-download-ios').on('click', function (e) {
        if (window.appInsights) {
            window.appInsights.trackPageView('download-ios-promo');
        }
    });
    $('#promo-download-android').on('click', function (e) {
        if (window.appInsights) {
            window.appInsights.trackPageView('download-android-promo');
        }
    });
    $('#promo-download-wp').on('click', function (e) {
        if (window.appInsights) {
            window.appInsights.trackPageView('download-wp-promo');
        }
    });
    $('#download-andoid').on('click', function (e) {
        if (window.appInsights) {
            window.appInsights.trackPageView('download-andoid');
        }
    });
    $('#download-ios').on('click', function (e) {
        if (window.appInsights) {
            window.appInsights.trackPageView('download-ios');
        }
    });
    $('#download-wp').on('click', function (e) {
        if (window.appInsights) {
            window.appInsights.trackPageView('download-wp');
        }
    });
});