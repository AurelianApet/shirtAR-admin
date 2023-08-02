function selTopMenu(id) {
    $("#topmenu" + id).attr("src", "/img/top_m0" + id + "_r.png");
}

function setMainMenu(id) {
    $("#mainMenu" + id).css("background-color", "#363638");
}

function setSubMenu(id) {
    $("#subMenu" + id).css("background-color", "#42485a");
    $("#subMenu" + id).css("color", "white");
}


function logout() {
    document.location.href = "/Manager/Logout.aspx";
}

function showProgress() {
    showPopup("progress_div");
}

function hideProgress() {
    $("#progress_div").fadeOut("slow");
    $("#progress_div").fadeOut("slow");
}

function confirm() {
    location.reload(true);
}

function showPopup(pid) {
    var leftPos = (($(window).width() - $("#" + pid).outerWidth()) / 2) + $(window).scrollLeft();
    var topPos = (($(window).height() - $("#" + pid).outerHeight()) / 2) + $(window).scrollTop();

    if (topPos < 0) {
        topPos = $(window).scrollTop();
    }

    $("#" + pid).fadeIn("fast");
    $("#" + pid).css({
        "left": leftPos,
        "top": topPos
    });
    $("#popBack").css({
        "opacity": "0.7"
    });
    $("#popBack").fadeIn("fast");
}

function closePopup() {
    $(".clspopup").fadeOut("slow");
    $("#popBack").fadeOut("slow");
}

function na_restore_img_src(name) {
    var img = eval((navigator.appName.indexOf('Netscape', 0) != -1) ? 'document.' + name : 'document.all.' + name);
    if (name == '')
        return;
    if (img && img.altsrc) {
        img.src = img.altsrc;
        img.altsrc = null;
    }
}

function na_change_img_src(name, rpath) {
    var img = eval((navigator.appName.indexOf('Netscape', 0) != -1) ? 'document.' + name : 'document.all.' + name);
    if (name == '')
        return;
    if (img) {
        img.altsrc = img.src;
        img.src = rpath;
    }
}

function checkAll(obj) {
    var chkObj = document.getElementsByName("chkNo");
    for (var i = 0; i < chkObj.length; i++) {
        chkObj[i].checked = obj.checked;
    }
}

function showPopupPWD() {
    showPopup("divChangePWD");
}

function checkDateTime(dtObj, bEmpty) {
    if (!isValidDate(dtObj.value)) {
        if (bEmpty == true) {
            dtObj.value = "";
        } else {
            dtObj.value = getDateStrFromDateObject(new Date());
        }
    }
}

function isDateFormat(d) {
    var df = /[0-9]{4}-[0-9]{2}-[0-9]{2}/;
    return d.match(df);
}

function isLeaf(year) {
    var leaf = false;

    if (year % 4 == 0) {
        leaf = true;

        if (year % 100 == 0) {
            leaf = false;
        }

        if (year % 400 == 0) {
            leaf = true;
        }
    }

    return leaf;
}

function isValidDate(d) {
    // 포맷에 안맞으면 false리턴
    if (!isDateFormat(d)) {
        return false;
    }

    var month_day = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    var dateToken = d.split('-');
    var year = Number(dateToken[0]);
    var month = Number(dateToken[1]);
    var day = Number(dateToken[2]);

    // 날짜가 0이면 false
    if (day == 0) {
        return false;
    }

    var isValid = false;

    // 윤년일때
    if (isLeaf(year)) {
        if (month == 2) {
            if (day <= month_day[month - 1] + 1) {
                isValid = true;
            }
        } else {
            if (day <= month_day[month - 1]) {
                isValid = true;
            }
        }
    } else {
        if (day <= month_day[month - 1]) {
            isValid = true;
        }
    }

    return isValid;
}

function getDateStrFromDateObject(dateObject) {
    var str = null;

    var month = dateObject.getMonth() + 1;
    var day = dateObject.getDate();

    if (month < 10)
        month = '0' + month;
    if (day < 10)
        day = '0' + day;

    str = dateObject.getFullYear() + '-' + month + '-' + day;
    return str;
}
