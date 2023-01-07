        $(document).ready(function () {

        var $SIDEBAR_MENU = $("#sidebar-menu");

        $SIDEBAR_MENU.find('a').on('click', function (ev) {
        var $li = $(this).parent();
        var child_menu = $('ul:first', $li);
        var other_list_items = $li.parent().find('li').not($li);
        var other_level_menus = $li.parents('.child_menu').find('ul.child_menu').not(child_menu);
        if ($li.is('.active')) {
            $li.removeClass('active active-sm');
            $('ul:first', $li).slideUp(function () {
                setContentHeight();
                if ($('.left_col.menu_fixed > .mCustomScrollBox > .mCSB_container ').length) {
                    $('.left_col.menu_fixed > .mCustomScrollBox > .mCSB_container ').css('min-height', '0');
                }
            });
        } else {
            // prevent closing menu if we are on child menu
            if (!$li.parent().is('.child_menu')) {
                $SIDEBAR_MENU.find('li').removeClass('active active-sm');
                $SIDEBAR_MENU.find('li ul').slideUp();

            } else {
                if ($BODY.is(".nav-sm")) {
                    if (!$li.parent().is('.child_menu')) {
                        $SIDEBAR_MENU.find('li').removeClass('active active-sm');
                        $SIDEBAR_MENU.find('li ul').slideUp();
                    }
                }
            }

            $li.addClass('active');

            child_menu.slideDown(function () {
                setContentHeight();
                // fix for fixed sidebar menu
                if ($('.left_col.menu_fixed > .mCustomScrollBox > .mCSB_container ').length) {
                    $('.left_col.menu_fixed > .mCustomScrollBox > .mCSB_container ').css('min-height', $(this).outerHeight() + $('.left_col.menu_fixed .left_col').outerHeight());
                }
                // fix for fixed footer
                if ($('body.footer_fixed').length) {
                    $('.right_col_wrapper .right_col').css('min-height', $(this).outerHeight() + $('.right_col_wrapper .right_col').outerHeight());
                }
            });
            if (other_list_items.length) {
                other_list_items.removeClass('active');
            }
            if (other_level_menus.length) {
                other_level_menus.slideUp();
            }
        }
    });


    // toggle small or large menu
    var $MENU_TOGGLE = $("#menu_toggle");
    $MENU_TOGGLE.on('click', function () {
        if ($BODY.hasClass('nav-md')) {
            $SIDEBAR_MENU.find('li.active ul').hide();
            $SIDEBAR_MENU.find('li.active').addClass('active-sm').removeClass('active');

            $('.main_container > .row > .left_col').removeClass('col-md-2 col-lg-2').addClass('col-md-1 col-lg-1 col-2');
            if ($('.main_container > .row > .left_col.menu_fixed').length) {
                if ($(window).width() < 992) {
                    $('.main_container > .row > .right_col_wrapper').addClass('offset-2 offset-md-1');
                } else {
                    $('.main_container > .row > .right_col_wrapper').removeClass('offset-md-2 offset-lg-2').addClass('offset-md-1');
                }

            }
            $('.main_container > .row > .right_col_wrapper').removeClass('col-lg-10 col-md-12').addClass('col-lg-11 col-md-11 col-10');
        } else {
            $SIDEBAR_MENU.find('li.active-sm ul').show();
            $SIDEBAR_MENU.find('li.active-sm').addClass('active').removeClass('active-sm');
            $('.main_container > .row > .left_col').removeClass('col-lg-1 col-2 col-md-1').addClass('col-lg-2 col-md-2');
            if ($('.main_container > .row > .left_col.menu_fixed').length) {
                if ($(window).width() < 992) {
                    $('.main_container > .row > .right_col_wrapper').removeClass('offset-2 offset-md-1');
                } else {
                    $('.main_container > .row > .right_col_wrapper').removeClass('offset-1').addClass('offset-md-2');
                }

            }
            $('.main_container > .row > .right_col_wrapper').removeClass('col-lg-11 col-10 col-md-11').addClass('col-lg-10 col-md-12');
        }

        $BODY.toggleClass('nav-md nav-sm');

        $('.dataTable').each(function () {
            $(this).dataTable().fnDraw();
        });

        setContentHeight();
    });

    // check active menu

    var $cur_menu = $SIDEBAR_MENU.find('a').filter(function () { // find nav element with exact match
        return this.href == CURRENT_URL;
    });

    if ($cur_menu.length == 0) { // if no exact match, try to find best match
        var $cur_menu = $SIDEBAR_MENU.find('a').filter(function () {
            return CURRENT_URL.startsWith(this.href) && this.href != '';
        });

        if ($cur_menu.length > 1) { // get ONLY one with longest href as best match
            var l = 0;
            for (var i = 0; i < $cur_menu.length; i++) {
                if ($cur_menu.eq(l).attr('href').length < $cur_menu.eq(i).attr('href').length) l = i;
            }
            $cur_menu = $cur_menu.eq(l);
        }
    }

    // original code below, but executed for $cur_menu
    $cur_menu.parent('li').addClass('current-page').parents('ul').slideDown(function () {
        setContentHeight();
    }).parent().addClass('active');

    // recompute content when resizing
    $(window).smartresize(function () {
        setContentHeight();
    });

    setContentHeight();

    // fixed sidebar
    if ($.fn.mCustomScrollbar) {
        $('.menu_fixed').mCustomScrollbar({
            autoHideScrollbar: true,
            theme: 'minimal',
            scrollInertia: 600,
            mouseWheel: {
                preventDefault: true,
                scrollAmount: 100
            }
        });
    }
});