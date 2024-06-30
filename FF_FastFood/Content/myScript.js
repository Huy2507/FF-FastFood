﻿$(document).ready(function () {

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
    $('a[href^="tat-ca-cac-mon"]').on('click', function (e) {
        e.preventDefault();
        // Lấy id của phần tử mục tiêu (category_name)
        var target = this.hash;

        // Cuộn đến vị trí của phần tử mục tiêu trên trang
        $('html, body').stop().animate({
            'scrollTop': 0
        }, 10, 'swing');

        // Cập nhật địa chỉ URL của trang
        history.pushState(null, null, '/thuc-don/tat-ca-cac-mon');
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


    /* My Account */

    $('.listaction-myaccount li a').on('click', function (e) {
        e.preventDefault(); // Ngăn chặn hành động mặc định của thẻ <a>

        var href = $(this).attr('href'); // Lấy đường dẫn từ thuộc tính href của thẻ <a>

        $.ajax({
            url: href, // Đường dẫn gửi yêu cầu
            type: 'GET', // Phương thức HTTP (GET hoặc POST)
            success: function (data) {
                // Xử lý thành công, data là nội dung trả về từ server (thường là HTML của partial view)
                $('body').html(data); // Thay đổi nội dung của phần tử .textfield-login 
                history.pushState(null, null, href);
            },
            error: function () {
                // Xử lý khi có lỗi
                alert('Đã xảy ra lỗi khi gửi yêu cầu.');
            }
        });

        return false; // Ngăn chặn hành động mặc định của thẻ <a>
    });


    var deleteUrl = '@Url.Action("DeleteAddress", "YourAddresses")';
    var addressIdToDelete;

    $('.btn-delete').on('click', function () {
        addressIdToDelete = $(this).data('id');
        $('#deleteModal').modal('show');
    });

    $('.close-form-xn').on('click', function () {
        $('#deleteModal').modal('hide');
    })

    $('#confirmDelete').on('click', function () {
        $.ajax({
            url: deleteUrl,
            type: 'POST',
            data: { id: addressIdToDelete },
            success: function (response) {
                if (response.success) {
                    // Reload or update the address list after deletion
                    location.reload();
                } else {
                    alert('Có lỗi xảy ra khi xóa địa chỉ.');
                }
            }
        });
        $('#deleteModal').modal('hide');
    });

    $('.btn-view-details').on('click', function () {
        var orderId = $(this).data('id');
        var href = '/MyAccount/OrderDetails/' + orderId
        $.ajax({
            url: href,
            type: 'GET',
            success: function (data) {
                $('#orderList').hide();
                $('#orderDetails').html(data);
                $('#orderDetails').show();
            }
        });
    });

    $('#backToOrders').on('click', function (e) {
        e.preventDefault(); // Ngăn chặn hành động mặc định của thẻ <a>

        var href = $(this).attr('href');
        $.ajax({
            url: href,
            type: 'GET',
            success: function (data) {
                $('#renderBody').html(data);
                $('#orderDetails').hide();
                $('#orderList').show();
                history.pushState(null, null, href);
            }
        });
        return false;
    });

    /* My Account End */


    /* Swiper Begin */
    const scrollRevealOption = {
        distance: "50px",
        origin: "bottom",
        duration: 1000,
    };

    ScrollReveal().reveal(".header__image img", {
        ...scrollRevealOption,
        origin: "right",
    });
    ScrollReveal().reveal(".header__content h1", {
        ...scrollRevealOption,
        delay: 500,
    });
    ScrollReveal().reveal(".header__content p", {
        ...scrollRevealOption,
        delay: 1000,
    });
    ScrollReveal().reveal(".header__image__content ", {
        duration: 1000,
        delay: 1500,
    });

    ScrollReveal().reveal(".product__image img", {
        ...scrollRevealOption,
        origin: "left",
    });
    ScrollReveal().reveal(".product__card", {
        ...scrollRevealOption,
        delay: 500,
        interval: 500,
    });
    const swiper = new Swiper(".swiper", {
        loop: true,
        effect: "slide",
        grabCursor: true,
        speed: 600, // Transition speed in milliseconds
        spaceBetween: 30, // Space between slides in pixels
        slidesPerView: "auto", // Number of slides visible at the same time
        slidesPerGroup: 1,

        pagination: {
            el: ".swiper-pagination",
        },
        breakpoints: {
            576: { // For screens 576px and up
                slidesPerView: 1, // Override slidesPerView
                spaceBetween: 10, // Override spaceBetween
            },
            // Add more breakpoints if needed
            768: {
                slidesPerView: 2,
                spaceBetween: 20,
            },
            992: {
                slidesPerView: 3,
                spaceBetween: 30,
            },
            1445: {
                slidesPerView: 4,
                spaceBetween: 40,
            }
        }
    });
    /* Swiper End */


    /* Chef */

    $(window).on('load', function () {
        if (window.url == "https://localhost:44355/Chef/Foods") {
            console.log("Clicked item:1", window.url);
            var thisItem = "#menu-item-2";
            $(thisItem).addClass('active');
        };
    });

    $('.menu-link').on('click', function (e) {
        var thisItem = "#menu-item-2";
        setTimeout(function () {
            $('.menu-item').removeClass('active');
            $(thisItem).addClass('active');

        }, 100);
        // Đóng menu
        $('#layout-menu').addClass('close').removeClass('show');
        $('.layout-overlay').removeClass('show');
        
    });

    $('.layout-menu-toggle-close').on('click', function (e) {
        e.preventDefault();
        $('#layout-menu').addClass('close');
        $('#layout-menu').removeClass('show');
        $('.layout-overlay').removeClass('show');
    });

    $('.btn-open-chef-sidebar').on('click', function () {
        $('#layout-menu').removeClass('close');
        $('#layout-menu').addClass('show');
        $('.layout-overlay').addClass('show');
    });

    $('.layout-overlay').on('click', function () {
        $('#layout-menu').addClass('close');
        $('#layout-menu').removeClass('show');
        $(this).removeClass('show');
    });

    $('.order-row').on('click', function () {
        var orderId = $(this).data('order-id');
        var detailsRow = $('.order-details-row[data-order-id="' + orderId + '"]');
        var orderDetailsContainer = $('#order-details-' + orderId);

        // Đóng tất cả các hàng chi tiết khác
        $('.order-details-row').not(detailsRow).slideUp('slow', function () {
            $(this).find('.order-details-container').html('');
        });

        // Nếu hàng chi tiết đang mở, đóng lại
        if (detailsRow.is(':visible')) {
            detailsRow.stop(true, true).slideUp(500, function () {
                orderDetailsContainer.html('');
            });

        } else {
            // Nếu hàng chi tiết đang đóng, mở ra và tải dữ liệu
            detailsRow.stop(true, true).slideDown(500, function () {
                $.ajax({
                    url: '/Chef/OrderDetails/' + orderId, // Đảm bảo rằng action và controller đúng
                    type: 'GET',
                    success: function (data) {
                        orderDetailsContainer.html(data);
                    },
                    error: function (xhr, status, error) {
                        console.error('Error fetching order details:', error);
                    }
                });
            });
        }
    });

    $('.complete-order').on('click', function (e) {
        e.stopPropagation();

        var orderId = $(this).data('order-id');
        var $row = $(this).closest('tr.order-row');

        var confirmation = confirm('Bạn có chắc chắn muốn hoàn thành đơn hàng này không?');

        if (confirmation) {
            $.ajax({
                url: '/Chef/CompleteOrder/' + orderId,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $row.hide();
                    } else {
                        alert('Không thể hoàn thành đơn hàng. Vui lòng thử lại sau.');
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error completing order:', error);
                    alert('Có lỗi xảy ra. Vui lòng thử lại sau.');
                }
            });
        }
    });

    // Xử lý sự kiện khi chọn radio button
    $('input[name="categoryFilter"]').change(function () {
        var categoryId = $(this).val(); // Lấy giá trị categoryId được chọn

        // Gửi AJAX request để lọc danh sách món ăn
        $.ajax({
            url: '/Chef/FilterFoods',
            type: 'GET',
            data: { categoryId: categoryId },
            success: function (data) {
                $('.order-container').html(data); // Cập nhật danh sách món ăn trên view
            },
            error: function (xhr, status, error) {
                console.error('Error fetching filtered foods:', error);
            }
        });

    });

    $('.chef-add-food').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            url: '/Chef/Create',
            type: 'GET',
            success: function (data) {
                $('.chef-content').html(data);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching filtered foods:', error);
            }
        });
    });

    $('.chef-food-row').on('click', function (e) {
        var foodId = $(this).data('food-id');
        $.ajax({
            url: '/Chef/Edit/' + foodId,
            type: 'GET',
            success: function (data) {
                $('.chef-content').html(data);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching filtered foods:', error);
            }
        });
    })

    $('.chef-delete-food').on('click', function (e) {
        e.stopPropagation();
        var foodId = $(this).data('food-id');
        var $row = $(this).closest('tr.chef-food-row');
        var confirmation = confirm('Bạn có chắc chắn muốn xóa món này không?');

        if (confirmation) {
            $.ajax({
                url: '/Chef/DeleteFood/' + foodId,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        $row.hide();
                    } else {
                        alert('Không thể hoàn thành đơn hàng. Vui lòng thử lại sau.');
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching filtered foods:', error);
                    alert('Có lỗi xảy ra. Vui lòng thử lại sau.');
                }
            });
        }
    })

    /* Chef End */
});