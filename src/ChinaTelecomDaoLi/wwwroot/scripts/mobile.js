var p = 0;
var lock = false;
var id = null

function Load()
{
    LoadInformations();
    LoadMachines();
    LoadLocals();
    LoadProducts();
    LoadOrders();
    LoadMyLocals();
    LoadProductComments();
    LoadMyServices();
}

function LoadMyServices() {
    if ($('#lstMyServices').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/Member/ServiceRaw', { p: p }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstMyServices').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0)
            {
                $('#lstMyServices').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstMyServices').append(data);
            p++;
            lock = false;
        });
    }
}

function LoadInformations() {
    if ($('#lstInformations').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/MInformation/ListRaw', { p: p, id: id, Title: $('#txtTitle').val() }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstInformations').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0) {
                $('#lstInformations').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstInformations').append(data);
            p++;
            lock = false;
        });
    }
}

function LoadMachines() {
    if ($('#lstMachines').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/MService/MachineRaw', { p: p }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstMachines').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0) {
                $('#lstMachines').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstMachines').append(data);
            p++;
            lock = false;
        });
    }
}

function LoadProductComments() {
    if ($('#lstProductComments').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/Mall/CommentRaw', { p: p, id: id }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstProductComments').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0) {
                $('#lstProductComments').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstProductComments').append(data);
            p++;
            lock = false;
        });
    }
}

function LoadLocals() {
    if ($('#lstLocals').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/MLocal/ListRaw', { p: p, id: id, Title: $('#txtTitle').val() }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstLocals').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0) {
                $('#lstLocals').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstLocals').append(data);
            p++;
            lock = false;
        });
    }
}

function LoadMyLocals() {
    if ($('#lstMyLocals').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/Member/LocalRaw', { p: p }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstMyLocals').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0) {
                $('#lstMyLocals').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstMyLocals').append(data);
            p++;
            lock = false;
        });
    }
}

function LoadOrders() {
    if ($('#lstOrders').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/Member/OrderRaw', { p: p }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstOrders').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0) {
                $('#lstOrders').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstOrders').append(data);
            p++;
            lock = false;
        });
    }
}

function LoadProducts() {
    if ($('#lstProducts').length > 0) {
        if (lock) return;
        lock = true;
        $.get('/Mobile/Mall/ListRaw', { p: p, id: id, Key: Key, Desc: Desc, Title: $('#txtProductTitle').val() }, function (data) {
            if (data.length < 5 && p == 0) {
                $('#lstProducts').html('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            else if (data.length < 5 && p != 0) {
                $('#lstProducts').append('<div class="no-more">没有更多信息了！</div>');
                return;
            }
            $('#lstProducts').append(data);
            p++;
            lock = false;
        });
    }
}

$(document).ready(function () {
    Load();
    $("img").click(function () {
        window.location = "/Mobile/Mobile/Image?src=" + $(this).attr("src");
    });
});

$(window).scroll(function () {
    var totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
    if ($(document).height() <= totalheight) {
        Load();
    }
});

var EarthRadiusKm = 6378.137;

function Distance(dis)
{
    if (dis > 1000)
        return (dis / 1000).toFixed(1) + "公里";
    else
        return dis.toFixed(1) + '米';
}

function getDistance(p1Lat, p1Lng, p2Lat, p2Lng) {
    var dLat1InRad = p1Lat * (Math.PI / 180);
    var dLong1InRad = p1Lng * (Math.PI / 180);
    var dLat2InRad = p2Lat * (Math.PI / 180);
    var dLong2InRad = p2Lng * (Math.PI / 180);
    var dLongitude = dLong2InRad - dLong1InRad;
    var dLatitude = dLat2InRad - dLat1InRad;
    var a = Math.pow(Math.sin(dLatitude / 2), 2) + Math.cos(dLat1InRad) * Math.cos(dLat2InRad) * Math.pow(Math.sin(dLongitude / 2), 2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var dDistance = EarthRadiusKm * c;
    return dDistance * 1000;
}