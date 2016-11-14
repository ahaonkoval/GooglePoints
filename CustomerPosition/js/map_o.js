
$(document).ready(function () {
    osmap.init();
});

osmap = {

    map: null,
    current_region: null,
    current_district: null,
    current_market:null,
    current_circle: null,
    //L.circle
    custome_points: [],
    //L.Polygon
    district: [],
    //object
    regions: [],
    //L.Marker
    markets: [],

    init: function () {
        var start_lat = 50.486831;
        var start_lng = 30.610115;
        osmap.map = L.map('content').setView([start_lat, start_lng], 11);
        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpandmbXliNDBjZWd2M2x6bDk3c2ZtOTkifQ._QA7i5Mpkd_m30IGElHziw', {
            maxZoom: 28,
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
                '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
                'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.streets'
        }).addTo(osmap.map);

        osmap.fillRegions();        
        osmap.fillMarkets();        
    },
    // Загрузка данных на клиент
    fillCustomerPoints: function() {

    },
    // отображение точек
    drawCustomerPoints: function () {

    },
    // завантаження на клієнт та запонення селекта  <-- завантажуєтся при виборі області
    fillDistrict: function () {

    },

    drawDistrict: function () {

    },

    // завантаження на клієнт та запонення селекта  <-- завантажуєтся при старті сторінки
    fillRegions: function () {
        $.ajax({
            url: 'api/region/',
            type: 'get',
            data: { request_id: 'regions' },
            success: function (regions) {
                var cmbregion = $("#cmbregion");
                $.each(regions, function (key, item) {
                    osmap.regions.push(item);
                });
                osmap.fillRegionConrols();
            }
        });
    },

    fillRegionConrols: function () {
        if (osmap.regions.length > 0) {
            var cmbregion = $("#cmbregion");
            $.each(osmap.regions, function (key, item) {
                cmbregion.prepend("<option value='" + item.region_id + "' >" + item.name + "</option>");
            });
            cmbregion.prepend("<option value='all' selected='selected'></option>");
        }
    },

    // перемальовуємо всі райони області
    drawRegions: function () {

    },

    fillMarkets: function () {
        $.ajax({
            url: 'api/market/',
            type: 'get',
            success: function (markets) {
                $.each(markets, function (key, item) {
                    osmap.markets.push(item);
                });
                fillMarketControls();
            }
        });
    },

    fillMarketControls: function () {
        var cmbmarket = $("#cmbmarket");
        $.each(resp, function (key, item) {
            cmbmarket.prepend("<option value='" + item.market_google_coordinates_id
                + "' selected='selected'>" + item.label + ' - ' + item.name_short + "</option>");

            setmarket(item.address, item.lat, item.lng);

        });
        cmbmarket.prepend("<option value='all' selected='selected'></option>");
    },

    drawMarkets: function () {

    }
};
