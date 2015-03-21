// JavaScript source code

function chu_common() { }

chu_common.add0 = function(m) { return m < 10 ? '0' + m : m };

chu_common.formatdate = function(date) {
    //shijianchuo是整数，否则要parseInt转换
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + this.add0(m) + this.add0(d);
};

chu_common.formatnumber = function (number, digit) {
    var bb = Math.pow(10, digit);
    var number = number * bb;
    var number = Math.round(number) / bb;
    return number;
};

chu_common.parsepara = function (key_var) {
    var u = decodeURIComponent(window.location.href);
    var par = u.substring(u.lastIndexOf('?') + 1, u.length);
    var d = par.split('&');
    var ret = "";
    for (var i = 0; i < d.length; i++) {
        var keyValue = d[i].split('=');
        var key = keyValue[0];
        var value = keyValue[1];
        if (key == key_var) ret = value;

    }
    return ret;
};
