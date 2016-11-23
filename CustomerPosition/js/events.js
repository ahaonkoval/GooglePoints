
$(document).ajaxStart(function () {
    hover.open({
        content: '<div>Завантаження данних...</div><div><p><img src="img/loading.gif"></p></div>'
    });
}).ajaxStop(function () {
    hover.close();
});

var setEvent = function () {
    $("#cmbregion").change(function () {
        var region_id = this.value;
        $('#showall_region').prop('checked', false);
        if (region_id == 'all') {
            osmap.clearDistrictAll();
            $('#showall_region').prop('disabled', true);
            
        } else {
            osmap.clearDistrictAll();
            $('#showall_region').prop('disabled', false);           
            osmap.fillDistrict(region_id);
        }
        
    });

    $("#cmbdistricts").change(function () {
        var district_id = this.value;
        if (district_id != 'all') {
            $('#showall_region').prop('checked', false);
            osmap.drawDistrict(district_id);
        }
    });

    $("#showall_region").change(function () {
        if (this.checked) {
            // Отображаем все регионы области (они уже на клиете)
            $("#cmbdistricts").val('all');        
            osmap.drawDistrictAll();
        } else {
            osmap.clearDistrictAll();
        }
    });

    $("#showmarket").change(function () {
        osmap.drawMarkets();
        //if (this.checked) {
            
        //} else {

        //}
    });

    $("#cmbmarket_getcard").change(function () {
        var market_id = $("#cmbmarket_getcard").val();
        osmap.showMarket(market_id);
    });

    $("#cmbmarket_visitcard").change(function () {
        var market_id = $("#cmbmarket_visitcard").val();
        osmap.showMarketVisited(market_id);
    });

    $('#lradius').keyup(function (e) {
        if (e.keyCode == 13) {
            var market_id = $("#cmbmarket_getcard").val();
            if (market_id > 0) {
                osmap.showMarket(market_id);
            }
        }
    });

    $("#btnshow_visit_points").click(function () {
        
    });

    $("#btnshow_market_point").click(function () {
        osmap.fillMarketCustomerPoints();
    });
};
