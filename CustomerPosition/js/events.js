
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

    $("#cmbmarket").change(function () {
        var market_id = $("#cmbmarket").val();
        osmap.showMarket(market_id);
    });

    $('#lradius').keyup(function (e) {
        if (e.keyCode == 13) {
            var market_id = $("#cmbmarket").val();
            if (market_id > 0) {
                osmap.showMarket(market_id);
            }
            //$(this).trigger("enterKey");
        }
    });

    $("#btnshow_visit_points").click(function () {
        
    });

    $("#btnshow_market_point").click(function () {
        osmap.fillMarketCustomerPoints();
    });
};

//
