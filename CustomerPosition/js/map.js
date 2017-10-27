
var map;
var current_circle;

function _create_map(lat, lng) {
    //map = L.map('content').setView([49.3744385, 26.5376699], 10);
    map = L.map('content').setView([lat, lng], 11);
    //L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpandmbXliNDBjZWd2M2x6bDk3c2ZtOTkifQ._QA7i5Mpkd_m30IGElHziw', {
    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoiYWhhb24iLCJhIjoiY2o1M2thdzFvMDR1cjJ3czMwOWY0M2ZyeiJ9.zc2uUm_WuMy8E6PHCeeFDQ', {
        maxZoom: 28,
        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
			'<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
			'Imagery © <a href="http://mapbox.com">Mapbox</a>',
        id: 'mapbox.streets'
    }).addTo(map);
};
/* ----------------------------------------------------------------------------------------------------------------------------- */
function initialize() {

    _create_map(50.486831, 30.610115);

    $.ajax({
        url: 'api/region/',
        type: 'get',
        data: { request_id: 'regions'},
        success: function (response) {
            var cmbregion = $("#cmbregion");
            $.each(response, function (key, item) {
                cmbregion.prepend("<option value='" + item.region_id + "' selected='selected'>" + item.name + "</option>");
            });
            cmbregion.prepend("<option value='all' selected='selected'></option>");
        }
    });

    $("#cmbregion").change(function () {
        var region_id = this.value;
        cmbdistricts_fill(region_id);
    });

    var setmarket = function (address, lat, lng) {
        var marke_location = new L.LatLng(lat, lng);

        var greenIcon = L.icon({
            iconUrl: 'img/Epicentrk.svg.png',
            iconSize: [40, 40] // size of the icon
        });

        var market_marker = new L.Marker(
            marke_location,
            {
                icon: greenIcon
            });

        market_marker.bindPopup(address);
        map.addLayer(market_marker);
    };

    $.ajax({
        url: 'api/market/',
        type: 'get',
        success: function (resp) {
            var cmbmarket = $("#cmbmarket");
            $.each(resp, function (key, item) {
                cmbmarket.prepend("<option value='" + item.market_coordinates_id
                    + "' selected='selected'>" + item.label + ' - ' + item.name_short + "</option>");

                setmarket(item.address, item.lat, item.lng);
                
            });
            cmbmarket.prepend("<option value='all' selected='selected'></option>");
        }
    });

    $("#cmbmarket").change(function () {
        var market_id = $("#cmbmarket").val();
        map_showmarket(market_id);
    });
};
/* ----------------------------------------------------------------------------------------------------------------------------- */
function map_showmarket(market_id) {
    $.ajax({
        url: 'api/market/' + market_id,
        type: 'get',
        success: function (market) {
            var latlng = new L.LatLng(market.lat, market.lng);
            map.setView(latlng, 11);

            if (current_circle != undefined) {
                current_circle.removeFrom(map);
            }
            if (market.name_short == 'Хмельницький' || market.name_short == 'Чернігів') {
                current_circle = L.circle([market.lat, market.lng], {
                    radius: 5000,
                    fill: false
                });
                current_circle.addTo(map);
            }
        }
    });
}
/* ----------------------------------------------------------------------------------------------------------------------------- */
function cmbdistricts_fill(region_id) {
    $.ajax({
        url: 'api/district/getdistricts/' + region_id,
        type: 'get',
        success: function (response) {
            var cmbdistricts = $("#cmbdistricts");
            cmbdistricts.html('');
            $.each(response, function (key, item) {
                cmbdistricts.prepend("<option value='" + item.district_id + "' selected='selected'>" + item.name + "</option>");
            });
            cmbdistricts.prepend("<option value='all' selected='selected'></option>");
        }
    });
};
/* ----------------------------------------------------------------------------------------------------------------------------- */
function btnshowdistrict() {
    var value = $("#cmbdistricts").val()
    $.ajax({
        url: 'api/districtcoordinates/getdistrictcoordinates/' + value,
        type: 'get',
        success: function (response) {
            console.clear();
            var polypoints = [];
            $.each(response, function (key, item) {
                //console.info(item.lat);                
                var point = new L.LatLng(item.lat, item.lng);
                polypoints.push(point);
            });

            var polygon = new L.Polygon(polypoints, { interactive: false });
            polygon.bindPopup($("#cmbdistricts option:selected").text());
            map.addLayer(polygon);
            $("#cmbdistricts option[value='" + value + "']").remove();
        }
    });

    $.ajax({
        url: 'api/district/GetDistrictById/' + value,
        type: 'get',
        success: function (district) {
            var district = new L.LatLng(district.lat, district.lng);
            map.setView(district, 11);
        }
    });
};

function btnshowpoints() {
    var market_id = $("#cmbmarket").val();
    var visit_count = $("#visit_count").val();

    $.ajax({
        url: 'api/cardpoint/getpointsbymarketid/' + market_id , 
        type: 'get',
        data: { visit: visit_count },
        success: function (resp) {
            $.each(resp, function (key, item) {
                setpointsmap(item.lat, item.lng);
            });
        }
    });
};

var setpointsmap = function (lat, lng) {
    var marke_location = new L.LatLng(lat, lng);
    var market_marker = new L.circleMarker(marke_location, {
        radius: 1,
        interactive: true,
        color: '#ee1313'
    });

    map.addLayer(market_marker);
};

/*
                if (item.name_short == 'Хмельницький' || item.name_short == 'Чернігів') {
                    L.circle([item.lat, item.lng], {
                        radius: 5000,
                        fill: false
                    }).addTo(map);
                }
*/