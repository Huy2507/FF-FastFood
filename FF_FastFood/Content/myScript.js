$(document).ready(function () {
    $('.my-toggler-btn').on('click', function () {
        // Lấy thẻ div cần di chuyển
        var $div_icon = $('#div_icon');

        // Lấy container chứa các div
        var $container = $('#div_container');

        // Lấy thẻ div tại vị trí thứ 3
        var $div3 = $container.children().eq(3);

        // Chèn div4 vào vị trí trước div3
        $div_icon.insertBefore($div3);
    });
});