
function getProductItem(id, title, image, amount, type, colors, hasMattress, matresses) {
    var ddl = loadClorDropDown(colors, id);
    var ddlMattress = loadMattressDropDown(hasMattress, matresses, id);


    var item = "<div class='col-md-4'><div><img class='img-responsive' src='" +
        image +
        "'/></div><div>" +
        title +
        "</div><div>" +
        amount +
        "</div>" +
        ddl + "<div>" +
        "<div>" +
        ddlMattress +
        "</div><input type='button' value='انتخاب' class='btn btn-primary' onclick=addToBasket('" +
        id + "','" + type + "'); />" +
        "</div></div>";
    return item;
}


function setCookie(name, value) {
    document.cookie = name + "=" + (value || "") + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return "";
}

function addToBasket(id, type) {
    freezePage();
    var colorId = $('#ddl_' + id).val();
    if (colorId === "0") {
        alert("رنگ محصول را انتخاب کنید");
        unFreezePage();
    } else {
        addProductToCookie(id);

        $.ajax({
            type: "GET",
            url: "/InputDocument/GetBasketInfoByCookie",
            contentType: "application/json; charset=utf-8",
            data: { "type": type },
            datatype: "json",
            success: function(data) {

                var item = "";
                for (var i = 0; i < data.Products.length; i++) {
                    item = item +
                        loadBasket(data.Products[i].ParentTitle,
                            data.Products[i].ChildTitle,
                            data.Products[i].Quantity,
                            data.Products[i].Amount,
                            data.Products[i].RowAmount,
                            data.Products[i].Id,
                            type,
                            data.Products[i].MattressTitle);
                }

                $('#factor tbody').html(item);
                $('#total').html(data.Total);
                //$('#totalAmount').val(data.Total); 
                //changeTotal();
                changeTotalOrder();
            },
            error: function() {
                alert("Dynamic content load failed.");
            }
        });
    }
    unFreezePage();
}

function addToBasketWithoutAmont(id, type) {
    freezePage();
    var colorId = $('#ddl_' + id).val();
    if (colorId === "0") {
        alert("رنگ محصول را انتخاب کنید");
        unFreezePage();
    } else {
        addProductToCookie(id);

        $.ajax({
            type: "GET",
            url: "/InputDocument/GetBasketInfoByCookie",
            contentType: "application/json; charset=utf-8",
            data: { "type": type },
            datatype: "json",
            success: function(data) {

                var item = "";
                for (var i = 0; i < data.Products.length; i++) {
                    item = item +
                        loadBasketWithoutAmont(data.Products[i].ParentTitle,
                            data.Products[i].ChildTitle,
                            data.Products[i].Quantity,
                           
                            data.Products[i].Id,
                            type,
                            data.Products[i].MattressTitle);
                }

                $('#factor tbody').html(item);
                $('#total').html(data.Total);
                //$('#totalAmount').val(data.Total); 
                //changeTotal();
                changeTotalOrder();
            },
            error: function() {
                alert("Dynamic content load failed.");
            }
        });
    }
    unFreezePage();
}


function loadBasketWithoutAmont(parentTitle, childTitle, quantity, id, type, mattressTitle) {
    //var ddl = loadClorDropDown(childProducts, id);

    // var ddlMattress = loadMattressDropDown(hasMattress, matresses, id);

    var item = "<tr>" +
        "<td>" +
        parentTitle +
        "</td>" +
        "<td>" +
        childTitle +
        "</td>" +
        "<td>" +
        mattressTitle +
        "</td>" +
        "<td class='qtytable'>" + quantity +
        //"<td class='qtytable'><input class='qty' type='text' value=" + quantity + " id='qty" + id + "' onKeyUp='return changeRowTotal(this.id,3)'  />" +
        "</td>" +
       
        //"<td class='amounttable'><input type='text' value=" + amount + " id='amount" + id + "' onKeyUp='return changeRowTotal(this.id,6)'/>" +
        //"<td>" + amount +
      
        "<td><i class='fa fa-remove' onclick=removeRow('" + id + "','" + type + "'); />" +
        "</td>" + "</tr>";

    return item;
}

function changeColor(parentId) {
    var colorId = $('#ddl_' + parentId).val();


    //var newCookie='';
    //for (var i = 0; i < products.length - 1; i++) {
    //    if (products[i] === parentId) {
    //        products[i] = colorId;
    //    }

    //    newCookie = newCookie + products[i] + '/';
    //}

    //setCookie("basket", newCookie);

    $.ajax({
        type: "GET",
        url: "/Orders/GetAdditiveColorAmount",
        contentType: "application/json; charset=utf-8",
        data: { "parentId": parentId, "colorId": colorId },
        datatype: "json",
        success: function (data) {

            //OldAmount
            var oldRowAmount = $('#rowAmount' + parentId).text();
            oldRowAmount = clearAmount(oldRowAmount);
            oldRowAmount = clearAmount(oldRowAmount);
            oldRowAmount = clearAmount(oldRowAmount);
            //OldAmount

            //OldTotalAmount
            var oldTotal = $('#total').text();
            oldTotal = clearAmount(oldTotal);
            oldTotal = clearAmount(oldTotal);
            oldTotal = clearAmount(oldTotal);
            //OldTotalAmount

            data = clearAmount(data);
            data = clearAmount(data);



            var qty = $('#qty' + parentId).val();

            var newRowAmount = parseInt(data) * parseInt(qty);


            $('#amount' + parentId).val(data);
            $('#rowAmount' + parentId).html(newRowAmount);


            var newTotal = parseInt(oldTotal) + parseInt(newRowAmount) - parseInt(oldRowAmount);
            $('#total').html(newTotal);

            //$('#factor tbody').html(item);
            //$('#total').html(data.Total);
            //$('#totalAmount').val(data.Total); 
            //changeTotal();


            var coockie = getCookie('basket');

            var cookieItems = coockie.split('/');

            var newcookie = '';

            for (var i = 0; i < cookieItems.length - 1; i++) {
                if (cookieItems[i].includes(parentId)) {
                    var finalcookieItem = cookieItems[i].split('^');

                    cookieItems[i] = finalcookieItem[0] + "^" + finalcookieItem[1] + "^" + colorId + "^" + finalcookieItem[3];
                }

                newcookie = newcookie + cookieItems[i] + "/";
            }

            setCookie("basket", newcookie);


            changeTotalOrder();
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });

}

function returnRowAmountData() {
    var coockie = getCookie('basket');

    var rowdata = '';

    var products = coockie.split('/');
    var doneId = '';

    var productQty = '';
    for (var i = 0; i < products.length - 1; i++) {

        if (!doneId.includes(products[i])) {

            var rowAmount = $('#rowAmount' + products[i]).html();

            rowdata = rowdata + rowAmount + '/';
            doneId = doneId + '/' + products[i];

            productQty = productQty + products[i] + ',' + $('#qty' + products[i]).val() + "/";
        }
    }


    rowdata = clearAmount(rowdata);
    rowdata = clearAmount(rowdata);



    return rowdata + '^' + productQty;
}

function updateFactor() {
    freezePage();
    var data = returnRowAmountData().split('^');
    var rowdata = data[0];
    var qtydata = data[1];

    $.ajax({
        type: "GET",
        url: "/InputDocument/UpdateFactor",
        contentType: "application/json; charset=utf-8",
        data: { "rowData": rowdata, "qtyData": qtydata },
        datatype: "json",
        success: function (data) {

            $('#total').html(data);

            var addedAmount = $('#addedAmount').val();

            var decreasedAmount = $('#decreasedAmount').val();

            data = clearAmount(data);
            data = clearAmount(data);

            var tot = parseInt(data) + parseInt(addedAmount) - parseInt(decreasedAmount);

            $('#totalAmount').val(tot);
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });

    unFreezePage();

}

function clearAmount(amount) {
    if (amount.includes('تومان'))
        amount = amount.replace('تومان', '');

    if (amount.includes(','))
        amount = amount.replace(',', '');

    return amount;
}

function loadBasket(parentTitle, childTitle, quantity, amount, rowAmount, id, type, mattressTitle) {
    //var ddl = loadClorDropDown(childProducts, id);

   // var ddlMattress = loadMattressDropDown(hasMattress, matresses, id);

    var item = "<tr>" +
        "<td>" +
        parentTitle +
        "</td>" +
        "<td>" +
        childTitle +
        "</td>" +
        "<td>" +
        mattressTitle +
        "</td>" +
        "<td class='qtytable'>" + quantity+
        //"<td class='qtytable'><input class='qty' type='text' value=" + quantity + " id='qty" + id + "' onKeyUp='return changeRowTotal(this.id,3)'  />" +
        "</td>" +
        "<td class='amounttable'>" + amount+
        //"<td class='amounttable'><input type='text' value=" + amount + " id='amount" + id + "' onKeyUp='return changeRowTotal(this.id,6)'/>" +
        //"<td>" + amount +
        "</td>" +
        "<td id='rowAmount" + id + "'>" +
        rowAmount +
        "</td> " +
        "<td><i class='fa fa-remove' onclick=removeRow('" + id + "','" + type + "'); />" +
        "</td>" + "</tr>";

    return item;
}

function loadClorDropDown(childProducts, parentId) {
    var ddl = "";

    if (childProducts.length > 0) {
        ddl = "<select id='ddl_" + parentId + "'><option value='0'>انتخاب رنگ*</option>";
    }

    for (var i = 0; i < childProducts.length; i++) {
        if (childProducts[i].IsSelected) {
            ddl = ddl + "<option selected value='" + childProducts[i].Value + "'>" + childProducts[i].Color + "</option>";
        } else {
            ddl = ddl + "<option value='" + childProducts[i].Value + "'>" + childProducts[i].Color + "</option>";

        }
    }

    ddl = ddl + "</select>";
    return ddl;
}

function loadMattressDropDown(hasMattress, matresses, parentId) {
    var ddlMatt = "-";
    if (hasMattress === true) {
        ddlMatt = "<select id='ddlMattress_" + parentId + "')><option value='0'>انتخاب تشک*</option>";

        for (var i = 0; i < matresses.length; i++) {
            ddlMatt = ddlMatt + "<option value='" + matresses[i].Value + "'>" + matresses[i].Color + "</option>";
        }

        ddlMatt = ddlMatt + "</select>";
    }


    return ddlMatt;
}

function changeRowTotal(id, type) {
    freezePage();

    var productId = getProductIdByElementId(id, type);

    var amount = $('#amount' + productId).val();
    amount = amount.replace(',', '');
    amount = parseInt(amount);
    var qty = parseInt($('#qty' + productId).val());

    var rowAmount = qty * amount;
    $('#rowAmount' + productId).html(rowAmount);


    unFreezePage();
}

function getProductIdByElementId(id, type) {
    var startIndex = parseInt(type);
    var finishIndex = parseInt(type) + 36;
    var productId = id.substring(startIndex, finishIndex);

    return productId;
}


function loadProductListWithoutAmounts(products, type) {
    var item = "";
    for (var i = 0; i < products.length; i++) {
        item = item + getProductItemWithoutAmounts(products[i].Id, products[i].Title, products[i].ImageUrl,  type, products[i].AvailableColors, products[i].HasMattress, products[i].MattressItems);
    }
    $('#product-list').html(item);
}

function getProductItemWithoutAmounts(id, title, image, type, colors, hasMattress, matresses) {
    var ddl = loadClorDropDown(colors, id);
    var ddlMattress = loadMattressDropDown(hasMattress, matresses, id);

    var item = "<div class='col-md-4'><div><img class='img-responsive' src='" +
        image +
        "'/></div><div>" +
        title +
        "</div>" +
        ddl + "<div>" +
        "<div>" +
        ddlMattress +
        "</div><input type='button' value='انتخاب' class='btn btn-primary' onclick=addToBasketWithoutAmont('" +
        id + "','" + type + "'); />" +
        "</div></div>";
    return item;
}

function loadProductList(products, type) {
    var item = "";
    for (var i = 0; i < products.length; i++) {
        item = item + getProductItem(products[i].Id, products[i].Title, products[i].ImageUrl, products[i].Amount, type, products[i].AvailableColors, products[i].HasMattress, products[i].MattressItems);
    }
    $('#product-list').html(item);
}

function loadChildMessage(parentProductTitle) {
    $("#parent-message").css('display', 'block');
    $('#parentProductTitle').html(parentProductTitle);
}

function addProductToCookie(id) {
    var currentBasket = '';
    var currentCookie = getCookie("basket");

    var colorId = $('#ddl_' + id).val();
    if (colorId === undefined || colorId === "0") {
        colorId = 'nocolor';
    }

    var mattressId = $('#ddlMattress_' + id).val();
    if (mattressId === undefined || mattressId === "0") {
        mattressId = 'nomatterss';
    }

    if (currentCookie.includes(id)) {

        var cookieItems = currentCookie.split('/');

        var newcookie = '';

        var isSetNewPro = false;

        for (var i = 0; i < cookieItems.length - 1; i++) {
            if (cookieItems[i].includes(id)) {
                var finalcookieItem = cookieItems[i].split('^');

                if (finalcookieItem[2] === colorId && finalcookieItem[3] === mattressId) {
                    var qty = parseInt(finalcookieItem[1]) + 1;
                    cookieItems[i] = id + "^" + qty + "^" + finalcookieItem[2] + "^" + finalcookieItem[3];
                    isSetNewPro = true;
                    newcookie = newcookie + cookieItems[i] + "/";
                    //break;

                } else {
                    newcookie = newcookie+ cookieItems[i] + "/";
                    //newcookie = currentCookie + id + "^1^" + colorId + "^" + mattressId + "/";
                }
            } else {
                newcookie = newcookie + cookieItems[i] + "/";
            }
        }

        if (!isSetNewPro) {
            newcookie = currentCookie + id + "^1^" + colorId + "^" + mattressId + "/";
        }

        currentBasket = newcookie;

    } else {
        currentBasket = currentCookie + id + "^1^" + colorId + "^" + mattressId + "/";
    }
    setCookie("basket", currentBasket);
}

function freezePage() {
    $("#loading").addClass('modalWindow');
    $("#loading img").css('display', 'inline-block');
}


function unFreezePage() {
    $("#loading").removeClass('modalWindow');
    $("#loading img").css('display', 'none');
}


function removeRow(id, type) {
    freezePage();
    $.ajax({
        type: "GET",
        url: "/InputDocument/RemoveFromBasket",
        contentType: "application/json; charset=utf-8",
        data: { "id": id, "type": type },
        datatype: "json",
        success: function (data) {
            console.log(data.Products);
            var item = "";
            for (var i = 0; i < data.Products.length; i++) {
                item = item + loadBasket(data.Products[i].ParentTitle, data.Products[i].ChildTitle, data.Products[i].Quantity, data.Products[i].Amount, data.Products[i].RowAmount, data.Products[i].Id);
            }

            $('#factor tbody').html(item);
            $('#total').html("جمع کل: " + data.Total);
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });

    unFreezePage();
}

function changeTotal() {
    $('#totalAmount').val('');
    var total = $('#total').html();
    var addedAmount = $('#addedAmount').val();
    var decreasedAmount = $('#decreasedAmount').val();

    total = clearAmount(total);
    total = clearAmount(total);
    total = clearAmount(total);
    addedAmount = clearAmount(addedAmount);
    addedAmount = clearAmount(addedAmount);
    addedAmount = clearAmount(addedAmount);
    decreasedAmount = clearAmount(decreasedAmount);
    decreasedAmount = clearAmount(decreasedAmount);
    decreasedAmount = clearAmount(decreasedAmount);


    var tot = parseInt(total) + parseInt(addedAmount) - parseInt(decreasedAmount);
    $('#totalAmount').val(tot);
}

function finalizeInputDoc(type) {
    freezePage();

    var cookie = getCookie('basket');

    if (cookie) {
        var branchId = $('#BranchId').val();
        var supplierId = $('#SupplierId').val();
        var orderId = $('#orderId').val();
        var inputDate = $('#InputDate').val();
        var addedAmount = $('#addedAmount').val();
        var decreasedAmount = $('#decreasedAmount').val();
        var desc = $('#desc').val();

        if (!inputDate)
            inputDate = $('#InputDocument_InputDate').val();

        if (type === "request") {
            supplierId = $('#BranchId2').val();
            if (!inputDate)
                inputDate = $('#ProductRequest_RequestDate').val();
        }

        if (branchId && supplierId && inputDate) {

            if (type === "input") {
                $.ajax({
                    type: "Post",
                    url: "/InputDocument/PostFinalize",
                    data: {
                        "branchId": branchId,
                        "supplierId": supplierId,
                        "orderId": orderId,
                        "inputDate": inputDate,
                        "addedAmount": addedAmount,
                        "decreasedAmount": decreasedAmount,
                        "desc": desc
                    },
                    success: function (data) {
                        if (data === "true") {
                            $('#submit-succes').css('display', 'block');
                            $('#submit-error').css('display', 'none');
                            setCookie('basket', null);

                        } else {
                            $('#submit-succes').css('display', 'none');
                            $('#submit-error').css('display', 'block');
                            $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                        }
                    },
                    error: function () {
                        $('#submit-succes').css('display', 'none');
                        $('#submit-error').css('display', 'block');
                        $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                    }
                });
            }
            else if (type === "request") {
                $.ajax({
                    type: "Post",
                    url: "/ProductRequest/PostFinalize",
                    data: {
                        "requestBranchId": branchId,
                        "requestSupplierId": supplierId,
                        "orderId": orderId,
                        "inputDate": inputDate,
                        "desc": desc
                    },
                    success: function (data) {
                        if (data === "true") {
                            $('#submit-succes').css('display', 'block');
                            $('#submit-error').css('display', 'none');
                            setCookie('basket', null);

                        } else {
                            $('#submit-succes').css('display', 'none');
                            $('#submit-error').css('display', 'block');
                            $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                        }
                    },
                    error: function () {
                        $('#submit-succes').css('display', 'none');
                        $('#submit-error').css('display', 'block');
                        $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                    }
                });
            }

        } else {
            $('#submit-succes').css('display', 'none');
            $('#submit-error').css('display', 'block');
            $('#submit-error').html('فیلدهای مربوطه را تکمیل نمایید.');
        }
    } else {
        $('#submit-succes').css('display', 'none');
        $('#submit-error').css('display', 'block');
        $('#submit-error').html('محصولی انتخاب نشده است.');
    }
    unFreezePage();
}


function edit(type) {
    freezePage();

    var cookie = getCookie('basket');

    if (cookie) {
        var branchId = $('#BranchId').val();
        var supplierId = $('#SupplierId').val();
        var orderId = $('#orderId').val();
        var inputDate = $('#InputDate').val();
        var addedAmount = $('#addedAmount').val();
        var decreasedAmount = $('#decreasedAmount').val();
        var desc = $('#desc').val();

        if (!inputDate)
            inputDate = $('#InputDocument_InputDate').val();

        if (type === "request") {
            supplierId = $('#BranchId2').val();
            inputDate = $('#ProductRequest_RequestDate').val();
        }
        var url = window.location.pathname;
        var id = url.substring(url.lastIndexOf('/') + 1);
        if (branchId && supplierId && orderId && inputDate) {

            if (type === "input") {
                $.ajax({
                    type: "Post",
                    url: "/InputDocument/PostEdit",
                    data: {
                        "branchId": branchId,
                        "supplierId": supplierId,
                        "orderId": orderId,
                        "inputDate": inputDate,
                        "addedAmount": addedAmount,
                        "decreasedAmount": decreasedAmount,
                        "desc": desc,
                        "parentId": id
                    },
                    success: function (data) {
                        if (data === "true") {
                            $('#submit-succes').css('display', 'block');
                            $('#submit-error').css('display', 'none');
                        } else {
                            $('#submit-succes').css('display', 'none');
                            $('#submit-error').css('display', 'block');
                            $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                        }
                    },
                    error: function () {
                        $('#submit-succes').css('display', 'none');
                        $('#submit-error').css('display', 'block');
                        $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                    }
                });
            }
            else if (type === "request") {
                $.ajax({
                    type: "Post",
                    url: "/ProductRequest/PostEdit",
                    data: {
                        "requestBranchId": branchId,
                        "requestSupplierId": supplierId,
                        "orderId": orderId,
                        "inputDate": inputDate,
                        "desc": desc,
                        "parentId": id
                    },
                    success: function (data) {
                        if (data === "true") {
                            $('#submit-succes').css('display', 'block');
                            $('#submit-error').css('display', 'none');
                        } else {
                            $('#submit-succes').css('display', 'none');
                            $('#submit-error').css('display', 'block');
                            $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                        }
                    },
                    error: function () {
                        $('#submit-succes').css('display', 'none');
                        $('#submit-error').css('display', 'block');
                        $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                    }
                });
            }

        } else {
            $('#submit-succes').css('display', 'none');
            $('#submit-error').css('display', 'block');
            $('#submit-error').html('فیلدهای مربوطه را تکمیل نمایید.');
        }
    } else {
        $('#submit-succes').css('display', 'none');
        $('#submit-error').css('display', 'block');
        $('#submit-error').html('محصولی انتخاب نشده است.');
    }
    unFreezePage();
}


function finalizeOrder() {

    freezePage();

    var cookie = getCookie('basket');

    if (cookie) {
        var branchId = $('#BranchId').val();
        var orderDate = $('#OrderDate').val();
        var recieveDate = $('#RecieveDate').val();
        var cellNumber = $('#CellNumber').val();
        var fullName = $('#fullName').val();
        var phone = $('#Phone').val();
        var address = $('#address').val();
        var cityId = $('#CityId').val();
        var regionId = $('#RegionId').val();
        var addedAmount = $('#addedAmount').val();
        var decreasedAmount = $('#decreasedAmount').val();
        var desc = $('#desc').val();
        var shipmentTypeId = $('#ShipmentTypeId').val();
        var paymentTypeId = $('#PaymentTypeId').val();
        var paymentAmount = $('#payment').val();
        var sendFrom = $('#ddlSenFrom').val();
        var factorydesc = $('#factorydesc').val();
        
        var paymentTypeIsRequired = null;

        if (paymentAmount === '0')
            paymentTypeIsRequired = "true";

        else if (paymentAmount !== '0' && paymentTypeId)
            paymentTypeIsRequired = "true";

        var img = getCookie('image');

        if (branchId && cellNumber && fullName && paymentTypeIsRequired && paymentTypeId) {
            $.ajax({
                type: "Post",
                url: "/Orders/PostFinalize",
                data: {
                    "branchId": branchId,
                    "orderDate": orderDate,
                    "recieveDate": recieveDate,
                    "fullName": fullName,
                    "phone": phone,
                    "address": address,
                    "shipmentTypeId": shipmentTypeId,
                    "addedAmount": addedAmount,
                    "decreasedAmount": decreasedAmount,
                    "desc": desc,
                    "cityId": cityId,
                    "regionId": regionId,
                    "paymentTypeId": paymentTypeId,
                    "paymentAmount": paymentAmount, "cellNumber": cellNumber,
                    "sendFrom": sendFrom,
                    "factorydesc": factorydesc,
                    "file": img
                },
                success: function (data) {
                    if (data.includes("true")) {
                        var orderCode = data.split('-')[1];
                        $('#submit-succes').css('display', 'block');
                        $('#submit-succes').html('فاکتور شماره '+ orderCode+' با موفقیت ثبت گردید.');
                        $('#submit-error').css('display', 'none');
                        clearForm();
                        window.location = "/orders/list";
                    } else {
                        $('#submit-succes').css('display', 'none');
                        $('#submit-error').css('display', 'block');
                        $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');

                    }

                },
                error: function () {
                    $('#submit-succes').css('display', 'none');
                    $('#submit-error').css('display', 'block');
                    $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                }
            });


        } else {
            $('#submit-succes').css('display', 'none');
            $('#submit-error').css('display', 'block');
            $('#submit-error').html('فیلدهای ستاره دار را تکمیل نمایید.');
            if (!paymentTypeIsRequired)
                $('#submit-error').html('نوع پرداخت را مشخص کنید.');

            if (cellNumber === '') {
                $('#CellNumber').css('border-color', 'red');
            }
            if (branchId === '') {
                $('#BranchId').css('border-color', 'red');
            }
            if (fullName === '') {
                $('#fullName').css('border-color', 'red');
            }
            if (paymentTypeId === '') {
                $('#PaymentTypeId').css('border-color', 'red');
            } 
        }
    } else {
        $('#submit-succes').css('display', 'none');
        $('#submit-error').css('display', 'block');
        $('#submit-error').html('محصولی انتخاب نشده است.');
    }
    unFreezePage();
}

function uploadFile() {
    if (window.FormData == undefined)
        alert("Error: FormData is undefined");
    else {
        var fileUpload = $("#FileUpload1").get(0);
        var files = fileUpload.files;

        var fileData = new FormData();

        fileData.append(files[0].name, files[0]);

        $.ajax({
            url: '/Orders/UploadFile',
            type: 'post',
            datatype: 'json',
            contentType: false,
            processData: false,
            async: false,
            data: fileData,
            success: function (response) {
                if (response.includes("true")) {
                    setCookie('image', response.split('_')[1]);
                    alert('فایل مورد نظر با موفقیت بارگزاری شد');
                } else {
                    alert(response);
                }
            }
        });
    }
}

function clearForm() {
    $('#CellNumber').val('');
    $('#fullName').val('');
    $('#address').val('');
    $('#Phone').val('');
    $('#ShipmentTypeId').val('');
    $('#addedAmount').val('0');
    $('#decreasedAmount').val('0');
    $('#payment').val('0');
    $('#remainAmount').val('0');
    $('#totalAmount').val('0');

    $('#factor tbody').html('');
    $('#total').html('0');
    setCookie("basket", '');

    $('.panel-body input').css('border-color', '#d9d9d9');
}

function changeTotalOrder() {
    $('#totalAmount').val('');
    var total = $('#total').html();
    var addedAmount = $('#addedAmount').val();
    var decreasedAmount = $('#decreasedAmount').val();
    var payment = $('#payment').val();

    total = clearAmount(total);
    total = clearAmount(total);
    total = clearAmount(total);

    addedAmount = clearAmount(addedAmount);
    addedAmount = clearAmount(addedAmount);
    addedAmount = clearAmount(addedAmount);

    decreasedAmount = clearAmount(decreasedAmount);
    decreasedAmount = clearAmount(decreasedAmount);
    decreasedAmount = clearAmount(decreasedAmount);

    payment = clearAmount(payment);
    payment = clearAmount(payment);
    payment = clearAmount(payment);



    var tot = parseInt(total) + parseInt(addedAmount) - parseInt(decreasedAmount);
    var remain = parseInt(tot) - parseInt(payment);

    $('#payment').val(commafy(payment));
    $('#decreasedAmount').val(commafy(decreasedAmount));
    $('#addedAmount').val(commafy(addedAmount));
    $('#totalAmount').val(commafy(tot));
    $('#remainAmount').val(commafy(remain));
}

function commafy(num) {
    var str = num.toString().split('.');
    if (str[0].length >= 5) {
        str[0] = str[0].replace(/(\d)(?=(\d{3})+$)/g, '$1,');
    }
    if (str[1] && str[1].length >= 5) {
        str[1] = str[1].replace(/(\d{3})/g, '$1 ');
    }
    return str.join('.');
}

function postEditOrder() {

    freezePage();

    var cookie = getCookie('basket');

    if (cookie) {
        var branchId = $('#BranchId').val();
        var orderDate = $('#OrderDate').val();
        var recieveDate = $('#RecieveDate').val();
        var cellNumber = $('#CellNumber').val();
        var fullName = $('#fullName').val();
        var phone = $('#Phone').val();
        var address = $('#address').val();
        var cityId = $('#CityId').val();
        var regionId = $('#RegionId').val();
        var addedAmount = $('#addedAmount').val();
        var decreasedAmount = $('#decreasedAmount').val();
        var desc = $('#desc').val();
        var shipmentTypeId = $('#ShipmentTypeId').val();
        var paymentTypeId = $('#PaymentTypeId').val();
        var paymentAmount = $('#payment').val();
        var sendFrom = $('#ddlSenFrom').val();
        var factorydesc = $('#factorydesc').val();

        var img = getCookie('image');

        var paymentTypeIsRequired = null;

        if (paymentAmount === '0')
            paymentTypeIsRequired = "true";

        else if (paymentAmount !== '0' && paymentTypeId)
            paymentTypeIsRequired = "true";

        var url = window.location.pathname;
        var id = url.substring(url.lastIndexOf('/') + 1);

        if (branchId && cellNumber && fullName && paymentTypeIsRequired) {
            $.ajax({
                type: "Post",
                url: "/Orders/PostEdit",
                data: {
                    "branchId": branchId,
                    "orderDate": orderDate,
                    "recieveDate": recieveDate,
                    "fullName": fullName,
                    "phone": phone,
                    "address": address,
                    "shipmentTypeId": shipmentTypeId,
                    "addedAmount": addedAmount,
                    "decreasedAmount": decreasedAmount,
                    "desc": desc,
                    "cityId": cityId,
                    "regionId": regionId,
                    "paymentTypeId": paymentTypeId,
                    "paymentAmount": paymentAmount,
                    "cellNumber": cellNumber,
                    "parentId": id,
                    "sendFrom": sendFrom,
                    "factorydesc": factorydesc,
                    "file": img

                },
                success: function (data) {
                    if (data === "true") {
                        $('#submit-succes').css('display', 'block');
                        $('#submit-error').css('display', 'none');
                    } else {
                        $('#submit-succes').css('display', 'none');
                        $('#submit-error').css('display', 'block');
                        $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                    }
                },
                error: function () {
                    $('#submit-succes').css('display', 'none');
                    $('#submit-error').css('display', 'block');
                    $('#submit-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                }
            });


        } else {
            $('#submit-succes').css('display', 'none');
            $('#submit-error').css('display', 'block');
            $('#submit-error').html('فیلدهای ستاره دار را تکمیل نمایید.');
            if (!paymentTypeIsRequired)
                $('#submit-error').html('نوع پرداخت را مشخص کنید.');
        }
    } else {
        $('#submit-succes').css('display', 'none');
        $('#submit-error').css('display', 'block');
        $('#submit-error').html('محصولی انتخاب نشده است.');
    }
    unFreezePage();
}






function SendOrderToFactory(type) {
    freezePage();
    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);

    var desc = $('#factorydesc').val();

    $.ajax({
        type: "Post",
        url: "/Orders/SendOrderToFactory",
        data: { "orderId": id, "desc": desc },
        success: function (data) {
            if (data === "true") {
                $('#submitsendFactoru-succes').css('display', 'block');
                $('#submitsendFactoru-error').css('display', 'none');
            } else {
                $('#submitsendFactoru-succes').css('display', 'none');
                $('#submitsendFactoru-error').css('display', 'block');
                $('#submitsendFactoru-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
            }
        },
        error: function () {
            $('#submitsendFactoru-succes').css('display', 'none');
            $('#submitsendFactoru-error').css('display', 'block');
            $('#submitsendFactoru-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
        }
    });




    unFreezePage();
}

function sendOrder() {

    var code = $('#order-code').html();

    $.ajax({
        type: "Post",
        url: "/Orders/ConfirmOrderSend",
        data: { "code": code },
        success: function (data) {
            if (data === "true") {
                $('#sendOrder-succes').css('display', 'block');
                $('#sendOrder-error').css('display', 'none');
            } else {
                if (data === "notAvailable") {
                    $('#sendOrder-succes').css('display', 'none');
                    $('#sendOrder-error').css('display', 'block');
                    $('#sendOrder-error').html('موجودی یکی از محصولات این فاکتور در انبار فروشگاه وجود ندارد.');
                } else {
                    $('#sendOrder-succes').css('display', 'none');
                    $('#sendOrder-error').css('display', 'block');
                    $('#sendOrder-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
                }
            }
        },
        error: function () {
            $('#sendOrder-succes').css('display', 'none');
            $('#sendOrder-error').css('display', 'block');
            $('#sendOrder-error').html('خطایی رخ داده است. لطفا دوباره تلاش کنید');
        }
    });
}