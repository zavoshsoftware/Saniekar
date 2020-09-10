var selectedServiceDetails = new Array();
var serviceDetails = new Array();
var services = new Array();
var serviceId;
var gender;
var addressId;
var baseAmount;
var baseDuration;
var amount;
var duration;
var date;
var time;
var femalePrice;
var maxHourAvailable;
var number;

$(document).ready(function () {
    localStorage.setItem('token', null);
    $("#datetimepicker").kendoDateTimePicker({
        value: new Date(),
        dateInput: true
    });
});

$("#btnMobile").click(function () {
    var mobile = $("#txtMobile").val();

    if (mobile !== "") {
        $.ajax({
            url: '/Orders/CheckUserMobile',
            data: { mobile: mobile },
            dataType: "json",
            type: "get",
            contentType: "application/json; charset=utf-8",
            success: function(data) {
                if (data.includes("سیستم")) {
                    alert(data);
                } else {
                    $("#section1").css('display', 'none');
                    Login(mobile, data);
                }
            },
            error: function(response) {
                alert(response.responseText);
            },
            failure: function(response) {
                alert(response.responseText);
            }
        });
    } else 
        alert("شماره موبایل مشتری را وارد نمایید");
});

function Login(mobile, pass) {
    var xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (xhttp.readyState === 4 && xhttp.status === 200) {
            var t = xhttp.responseText;
            var mydata = JSON.parse(t);
            localStorage.setItem('token', mydata.result.tokenId);
            GetService();
            $("#section2").css('display', 'block');
        }

    }

    xhttp.open("POST", "https://mobile.3pita.com/account/login", true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    var input = JSON.stringify({
        "cellNumber": mobile,
        "password": pass
    });

    xhttp.send(input);
}

function GetService() {
    var token = localStorage.getItem('token');
    if (token != null) {
        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", "https://mobile.3pita.com/service/get", false);
        xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhttp.setRequestHeader("Authorization", token);
        xhttp.send();
        var t = xhttp.responseText;
        var mydata = JSON.parse(t);

        for (var i = 0; i < mydata.result.length; i++) {
            $("#ddlService").append($('<option></option>').attr('value', mydata.result[i].id).text(mydata.result[i].title));
            services[mydata.result[i].id] = mydata.result[i];
        }
    }
    else {
        alert('کاربر وارد شده یافت نشد')
    }
}

$("#btnService").click(function () {
    serviceId = $('#ddlService').val();
    var token = localStorage.getItem('token');
    $("#section2").css('display', 'none');
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://mobile.3pita.com/servicedetail_new/get?serviceid=" + serviceId, false);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.setRequestHeader("Authorization", token);
    xhttp.send();

    var t = xhttp.responseText;
    var mydata = JSON.parse(t);


    for (var i = 0; i < mydata.result.serviceDetails.length; i++) {
        //$("#serviceDetail").append(' <select id="ddlServiceDetail_' + mydata.result.serviceDetails[i].id + '" name="locality"><option value="' + mydata.result.serviceDetails[i].id + '">' + mydata.result.serviceDetails[i].minValue+ '</option></select>');
        $("#serviceDetail").append('<div class="form-group"><div class="control-label col-md-3">' + mydata.result.serviceDetails[i].title +
            '</div><div class="col-md-5"><input class="form-control" type="text" id="detail_' + mydata.result.serviceDetails[i].id + '"value="' + mydata.result.serviceDetails[i].minValue + '"/></div></div>');
        //serviceDetailInfoes[mydata.result.serviceDetails[i].id] = mydata.result.serviceDetails[i].serviceDetailInfo;
        serviceDetails[i] = mydata.result.serviceDetails[i];
    }
    baseAmount = mydata.result.baseAmount;
    baseDuration = mydata.result.baseDuration;
    document.getElementById('lblAmount').innerHTML = baseAmount;
    document.getElementById('lblDuration').innerHTML = baseDuration;

    $("#section3").css('display', 'block');
})

$("#btnServiceDetail").click(function() {
    var i = 1;

    $("[id*=detail]").each(function() {
        var serviceDetail = $(this).attr("id").match(/[\d]+$/);
        if (serviceDetail == null || serviceDetail == "") {
            serviceDetail = $(this).attr("id");
            var serviceDetailId = serviceDetail.split("_");
            var serviceDetailVal = $("#" + serviceDetail).val();
        } else {
            var serviceDetailId = serviceDetail.input.split("_");
            var serviceDetailVal = $("#" + serviceDetail.input).val();
        }
        if (serviceDetailVal == null || serviceDetailVal == "") {
            alert("تمامی موارد خواسته شده را تکمیل نمایید");
            return false;
        }
        serviceDetailId[2] = serviceDetailVal;
        selectedServiceDetails[i] = serviceDetailId;
        i++;

    });

    FillDateTime();
    $("#section3").css('display', 'none');
    $("#section4").css('display', 'block');

});

$("#btnDateTime").click(function () {
    //datetime = $("#datetimepicker").val();
    date = $("#ddlDate").val();
    time = $("#ddltime").val();
    gender = $("#ddlGender").val();
    if (gender != "", date != "", time != "") {
        var token = localStorage.getItem('token');
        $("#section4").css('display', 'none');
        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", "https://mobile.3pita.com/adress/getaddress", false);
        xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhttp.setRequestHeader("Authorization", token);
        xhttp.send();
        var t = xhttp.responseText;
        var mydata = JSON.parse(t);
        for (var i = 0; i < mydata.result.length; i++) {
            $("#tblAddress").append('<tr><td>' + mydata.result[i].title +
                '</td><td>' + mydata.result[i].city
                +
                '</td><td>' + mydata.result[i].cityArea
                +
                '</td><td>' + mydata.result[i].number
                +
                '</td><td>' + mydata.result[i].unitNumber
                +
                '</td><td>' + mydata.result[i].phone
                +
                '</td><td><input type="checkbox" value="انتخاب آدرس" onClick=SelectedAddress("' + mydata.result[i].id + '"); id="selectAddress_' + mydata.result[i].id
                + '"/></td></tr>');
        }
        $("#section5").css('display', 'block');
        CalCulatePrice();
        document.getElementById('lblFinalAmount').innerHTML = amount;
        document.getElementById('lblFinalDuration').innerHTML = duration;
    }
    else {
        alert("تمامی موارد خواسته شده را تکمیل نمایید");
    }
});

function SelectedAddress(id) {
    var selectedAddressId = "selectAddress_" + id;

    if (document.getElementById(selectedAddressId).checked == true) {
        addressId = id;
    }
    else {
        addressId = null;
        alert("آدرس را انتخاب نمایید");
    }

}

$("#btnPostOrder").click(function () {

    if (addressId == "" || addressId == "undefined" || addressId == null) {
        alert("آدرس را انتخاب نمایید");
    }

    var serviceDetailList = ReturnServiceDetailArray();
    var xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (xhttp.readyState == 4 && xhttp.status == 200) {
            var t = xhttp.responseText;
            var mydata = JSON.parse(t);

            var orderCode = mydata.result.code;
            $('#section5').css('display', 'none');
            $('.displaynone').css('display', 'block');
            $('#successMessage').html('سفارش کد ' + orderCode + ' با موفقیت ثبت گردید');
        }

    }
    var token = localStorage.getItem('token');
    xhttp.open("POST", "https://mobile.3pita.com/order/postorder", true);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.setRequestHeader("Authorization", token);
    var input = JSON.stringify({
        "serviceId": serviceId,
        "visitDate": date,
        "visitTime": time,
        "employeeGender": gender,
        "netAmount": amount,
        "finalAmunt": amount,
        "serviceDetails": serviceDetailList,
        "duration": duration,
        "number": $("#txtNumber").val(),
        "discountCodeId": "",
        "addressId": addressId,
        "description": $("#txtDescription").val(),
        "carwashServiceId": "",
        "serviceTypeCodeId": services[serviceId].serviceTypeCode
    });

    xhttp.send(input);
})

function ReturnServiceDetailArray() {
    var Complex = new Array();
    for (var i = 1; i < selectedServiceDetails.length; i++) {
        Complex.push({ "ServiceDetailId": selectedServiceDetails[i][1], "Quantity": selectedServiceDetails[i][2] });
    }
    return Complex;
}

function CalCulatePrice() {
    if (serviceId !== "8ceb3275-cbe0-400a-a1b4-91edf67f67e8") {
        var breackValue = 0;
        for (var i = 0; i < serviceDetails.length; i++) {
            if (breackValue === 1)
                break;
            var increamentValueLimmit = serviceDetails[i].increamentValueLimmit;
            var txtServiceDetailId = "#detail_" + serviceDetails[i].id;
            var value = $(txtServiceDetailId).val();

            if (value < increamentValueLimmit) {
                for (var t = 0; t < serviceDetails[i].serviceDetailInfo.length; t++) {
                    var serviceDetailInfoVal = serviceDetails[i].serviceDetailInfo[t].total;
                    if (serviceDetailInfoVal == value) {
                        amount = serviceDetails[i].serviceDetailInfo[t].amount;
                        duration = serviceDetails[i].serviceDetailInfo[t].duration;
                        breackValue = 1;
                        break;
                    }
                }
            }
            else {
                var increamentValue = serviceDetails[i].increamentValue;
                var seedValue = serviceDetails[i].seedValue;
                var extraAmount = serviceDetails[i].amount;
                var extraValue = value - seedValue;
                var extraUnit = extraValue / increamentValue;
                extraValue = extraUnit * extraAmount;
                if (extraAmount > 0) {
                    if (amount > baseAmount) {
                        amount = amount + extraValue;
                    }
                    else {
                        amount = baseAmount + extraValue;
                    }
                }
                var durationValue = serviceDetails[i].duration;
                var extraDuration = durationValue * extraUnit;
                if (extraDuration > 0) {
                    if (duration > baseDuration) {
                        duration = duration + extraDuration;
                    }
                    else {
                        duration = baseDuration + extraDuration;
                    }
                }


            }
        }

    }
    else {
        var xhttp = new XMLHttpRequest();

        xhttp.onreadystatechange = function () {
            if (xhttp.readyState === 4 && xhttp.status === 200) {
                var t = xhttp.responseText;
                var mydata = JSON.parse(t);

                amount = mydata.result.price;
                duration = mydata.result.duration;

            }

        }
        var serviceDetailList = ReturnServiceDetailArray();
        var token = localStorage.getItem('token');
        xhttp.open("POST", "https://mobile.3pita.com/ResidentialComplex/post", true);
        xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhttp.setRequestHeader("Authorization", token);
        var input = JSON.stringify({

            "serviceDetails": serviceDetailList
        });

        xhttp.send(input);
    }

    if (gender === "women") {
        amount = amount + parseInt(femalePrice);
    }

    if (duration > maxHourAvailable) {
        if (duration % maxHourAvailable > 0)
            number = duration / maxHourAvailable + 1;

        else
            number = duration / maxHourAvailable;

    } else {
        number = 1;
    }
}

function FillDateTime() {
    var token = localStorage.getItem('token');
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "https://mobile.3pita.com/VisitDate/get?serviceId=" + serviceId, false);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.setRequestHeader("Authorization", token);
    xhttp.send();

    var t = xhttp.responseText;
    var mydata = JSON.parse(t);
    for (var i = 0; i < mydata.result.dates.length; i++) {
        $("#ddlDate").append($('<option></option>').attr('value', mydata.result.dates[i].date).text(mydata.result.dates[i].showDate));

    }
    for (var j = 0; j < mydata.result.houres.length; j++) {
        $("#ddltime").append($('<option></option>').attr('value', mydata.result.houres[j]).text(mydata.result.houres[j]));
    }

    femalePrice = mydata.result.femalePrice;
    maxHourAvailable = mydata.result.maxHourAvailable;
}

//function CalCulateDiscount()
//{
//    var code = $("#txtDiscountCode").val();
//    if (code != "" || code != null)
//    {
//        var xhttp = new XMLHttpRequest();

//        xhttp.onreadystatechange = function () {
//            if (xhttp.readyState == 4 && xhttp.status == 200) {
//                var t = xhttp.responseText;
//                var mydata = JSON.parse(t);
//                discountAmount = amount - mydata.result.amount;
//            }
//        }
//        var token = localStorage.getItem('token');
//        xhttp.open("POST", "https://mobile.3pita.com/Discount/CheckCodeValidation", true);
//        xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
//        xhttp.setRequestHeader("Authorization", token);
//        var input = JSON.stringify({
//            "code": code
//        });

//    }
//    else
//    {
//        discountAmount = amount;
//    }
//}
