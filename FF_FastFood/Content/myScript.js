$(document).ready(function () {

    /* Navbar */

    var $div_icon = $('#div_icon');
    var $original_position = $div_icon.prev();
    var $collapseContent = $('#collapsibleNavbar');
    $('.my-toggler-btn').click(function () {
        // Lấy thẻ div cần di chuyển
            

        // Lấy container chứa các div
        var $container = $('#div_container');

        // Lấy thẻ div tại vị trí thứ 3
        var $div3 = $container.children().eq(3);

        // Chèn div4 vào vị trí trước div3
        $div_icon.insertBefore($div3);
    });
        
    function checkScreenSize() {
        if ($(window).width() >= 768) {
            // Trả lại div_icon về vị trí ban đầu khi màn hình rộng hơn 768px
            if (!$div_icon.prev().is($original_position)) {
                $div_icon.insertAfter($original_position);
                $collapseContent.removeClass('show');
            }
        }
    }
    checkScreenSize();
    $(window).resize(checkScreenSize);

    /* Navbar End */

    /* Food */

    $('#scroll-left').on('click', function () {
        $('#category-list').animate({
            scrollLeft: '-=200px'
        }, 10);
    });

    $('#scroll-right').on('click', function () {
        $('#category-list').animate({
            scrollLeft: '+=200px'
        }, 10);
    });

    $('.navbar-nav li a').on('click', function () {
        // Xóa lớp active khỏi tất cả các mục
        $('.navbar-nav li').removeClass('active');

        // Thêm lớp active vào mục đã click
        $(this).closest('li').addClass('active');
    });


    $(window).on('scroll', function () {
        var scrollPosition = $(window).scrollTop();
        $('#category-list a').each(function () {
            var target = $(this).attr('href');
            if (target === '#') return;

            var sectionOffset = $(target).offset().top - $('#category-nav').outerHeight() - 20;
            if (scrollPosition >= sectionOffset) {
                $('#category-list a').removeClass('active');
                $(this).addClass('active');
            }
        });
    });


    var navbarHeight = $('.navbar').outerHeight();
    $('a[href^="#"]').on('click', function (e) {

        // Lấy id của phần tử mục tiêu (category_name)
        var target = this.hash;

        // Cuộn đến vị trí của phần tử mục tiêu trên trang
        $('html, body').stop().animate({
            'scrollTop': $(target).offset().top - (navbarHeight*2)
        }, 10, 'swing');

        // Cập nhật địa chỉ URL của trang
        history.pushState(null, null, target);
    });

    /* Food End */


    /* Login & Sign Up */

    $('.btn-login .btn').on('click', function (e) {
        var $this = $(this);
        $this.addClass('active');
        setTimeout(function () {
            $this.removeClass('active');
        }, 500);
    });

    /* Login Sign Up End */
});