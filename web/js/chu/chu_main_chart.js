// JavaScript source code

function chu_main_chart() { }


chu_main_chart.showchart = function (stock,big,type,start) {
    var tag = [];
    var totalshare = [];
    var sellshare = [];
    var buyshare = [];
    var incrementalBuyMoney = [];
    var incrementalSellMoney = [];
    var diff = [];
    var diff_share = [];
    var close = [];

    $.ajaxSetup({ async: false });
    var url = "/rest/rest/query/id/" + stock + "?big=" + big + "&type=" + type + "&start=" + start;
    var t = $.getJSON(url);
    for (var i in t.responseJSON) {
        tag[i] = t.responseJSON[i]["tag"];
        totalshare[i] = t.responseJSON[i]["totalshare"];
        sellshare[i] = t.responseJSON[i]["sellshare"];
        buyshare[i] = t.responseJSON[i]["buyshare"];
        incrementalBuyMoney[i] = t.responseJSON[i]["incrementalBuyMoney"];
        incrementalSellMoney[i] = t.responseJSON[i]["incrementalSellMoney"];
        diff[i] = incrementalBuyMoney[i] - incrementalSellMoney[i];
        diff_share[i] = t.responseJSON[i]["incrementalBuyShare"] - t.responseJSON[i]["incrementalSellShare"];
        close[i] = t.responseJSON[i]["close"];
    }


    seriesOptions = {
        name: "test",
        data: totalshare
    };

    $('#container').highcharts({
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: 'analyze chart',
            x: -20 //center
        },
        subtitle: {
            text: '',
            x: -20
        },
        xAxis: {
            categories: tag
        },
        yAxis: [{
            title: {
                text: 'trade data'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        }, //first Y
        {
            title: {
                text: 'price'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }],
            opposite: true
        } //second Y

        ],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor: '#FFFFFF'
        },
        series: [{
            name: 's',
            yAxis: 0,
            type: 'column',
            data: sellshare,
            color: 'green'
        }, {
            name: 'b',
            yAxis: 0,
            type: 'column',
            data: buyshare,
            color: 'red'
        },
        {
            name: 'close',
            yAxis: 1,
            type: 'line',
            data: close,
            color: 'blue'
        }]
    }); //high chart1

    $('#container2').highcharts({
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: 'analyze chart',
            x: -20 //center
        },
        subtitle: {
            text: '',
            x: -20
        },
        xAxis: {
            categories: tag
        },
        yAxis: [{
            title: {
                text: 'trade data'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        }, //first Y
        {
            title: {
                text: 'price'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }],
            opposite: true
        } //second Y

        ],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor: '#FFFFFF'
        },
        series: [
        {
            name: 'diff money',
            yAxis: 0,
            data: diff,
            type: 'line',
            color: 'blue'
        }
        ,
         {
             name: 'diff share',
             yAxis: 1,
             data: diff_share,
             type: 'line',
             color: 'red'
         }
        ]
    });//high chart 2
};
