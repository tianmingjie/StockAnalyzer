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
