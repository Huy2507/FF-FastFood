/*$(document).ready(function () {
    const sidebar = document.querySelector('.sidebar');
    const toggleBtn = document.querySelector('.toggle-btn');

    $(".toggler-btn").on('click', function () {
        $(".sidebar").addClass('active');
    });
});
*/
document.addEventListener('DOMContentLoaded', function () {
    var tgBtn = document.querySelector('#toggleBtn');
    tgBtn.addEventListener("click", () => {
        var navElement = document.querySelector('nav.sidebar.aside');
        // Kiểm tra xem phần tử có tồn tại không
        if (navElement.classList.contains("active")) {
            // Loại bỏ lớp "active" từ phần tử nav
            navElement.classList.remove('active');
        } else {
           
            navElement.classList.add('active');
        }
    });
});



//Thay đổi thẻ default của thẻ select theo ID
document.addEventListener("DOMContentLoaded", () => {
        let select = document.getElementById("CategorySelect");
        let default_option="";
        if (select.getAttribute("value") == 1) {
            default_option = document.querySelector("option").innerHTML = "Fried Chicken";
            default_option.setAttribute("selected");
        } else if (select.getAttribute("value") == 2) {
            default_option = document.querySelector("option").innerHTML = "Burger";
            default_option.setAttribute("selected");
        } else if (select.getAttribute("value") == 3) {
            default_option = document.querySelector("option").innerHTML = "Pizza";
            default_option.setAttribute("selected");
    }
});

//Gán value cho categoryID khi chọn option lúc edit dữ liệu hoặc thêm dữ liệu
/*document.addEventListener("Click", () => {
    const listSelectOption = document.querySelectorAll("#CategorySelect option");
    listSelectOption.forEach(selectoption => {
        if (selectoption.getAttribute("selected") == true) {
            console.log(selectoption.innerHTML)
        }
    });
});*/

//Thay đổi choice box TYPE sẽ khiến table trong index thay đổi theo option được chọn

function loadSelect() {
    let selectType = document.querySelectorAll("#SelectType option");
    selectType.forEach((typeOption) => {
        if (typeOption.selected) {
            window.location.href = '/Admin/Index?type=' + typeOption.getAttribute("value");
            typeOption.setAttribute("selected");
        }
    });
};


//Thay đổi choice box PRICE sẽ khiến table trong index thay đổi theo option được chọn
function loadSelectPrice() {
    let selectPrice = document.querySelectorAll("#SelectPrice option");
    selectPrice.forEach((priceOption) => {
        if (priceOption.selected) {
            window.location.href = '/Admin/Index?cost=' + priceOption.getAttribute("value");

        }
    });
};
document.addEventListener("DOMContentLoaded", () => {
    let selectPriceValue = document.getElementById("SelectPrice");
    let default_optionPrice = "";
    if (selectPriceValue.getAttribute("value") == 0) {
        default_optionPrice = document.querySelector("#SelectPrice option").innerHTML = "--All price--";
        default_optionPrice.setAttribute("selected");

    } else if (selectPriceValue.getAttribute("value") == 1) {
        default_optionType = document.querySelector("#SelectPrice option").innerHTML = "Less than 30.000VND";
        default_optionType.setAttribute("selected");

    } else if (selectPriceValue.getAttribute("value") == 2) {
        default_optionType = document.querySelector("#SelectPrice option").innerHTML = "30.000-70.000VND";
        default_optionType.setAttribute("selected");

    } else if (selectPriceValue.getAttribute("value") == 3) {
        default_optionType = document.querySelector("#SelectPrice option").innerHTML = "Higher than 70.000VND";
        default_optionType.setAttribute("selected");
    }
});

//Thay đổi link ảnh sẽ hiển thị lên thanh textbox
/*function choosefile(fileinput) {
    if (fileinput.files && fileinput.files[0]) {
        var reader = new Filereader();
        reader.onload = function (e) {
            $('#image').attr('src',e.target)
        }
    }
}*/

//Khi click delete sẽ hiện thông báo confirm.
var deleteButton = document.getElementById("Delete");
deleteButton.addEventListener("click", () => {
    var a = confirm("Xác nhận xóa!")
    if (a) {
        document.getElementById("formFoodDetail").setAttribute("action", "/Admin/DeleteFood");
    }
});
//Khi click sẽ tải file excel đã export xuống.
function exportExcel() {
    let btnExport = document.querySelector("#btnExportExcel");
    btnExport.addEventListener("Click", () => {
        $.ajax{
            datatype: 'json';
            type: 'GET';
            url: '';
        }
    });
}
