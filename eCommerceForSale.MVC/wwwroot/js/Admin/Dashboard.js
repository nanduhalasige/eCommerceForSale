var DashboadrData;
$(document).ready(function () {
    LoadDashBoardContent();
})

function LoadDashBoardContent() {
    $.ajax({
        type: "GET",
        url: "Dashboard/GetAllDashboardData",
        contentType: "application/json",
        success: function (data) {
            if (data) {
                DashboadrData = data;
                //data.sales.map(el => {
                loadSalesChart();
            }
        },
        error: function (er) {
            console.log(er);
        }
    });
}

function loadSalesChart(sales) {
    Highcharts.chart('chart-body', {
        title: {
            text: 'Sales (Fake)'
        },

        yAxis: {
            title: {
                text: 'Amount of sales/Day'
            }
        },

        xAxis: {
            accessibility: {
                rangeDescription: 'Range: 2010 to 2017'
            }
        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                pointStart: 2010
            }
        },

        series: [{
            name: 'Other',
            data: [12908, 5948, 8105, 11248, 8989, 11816, 18274, 18111]
        }],

        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
}