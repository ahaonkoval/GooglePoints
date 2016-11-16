
$(document).ready(function () {
    osmap.init();
});

osmap = {
    start_lat: 50.486831,
    start_lng: 30.610115,

    map: null,

    color_market_points: 'FF6699',

    current_market: null,

    current_circle: null,
    //L.circle
    customer_points: [],
    //L.Polygon
    districts: [],
    //object
    regions: [],
    //L.Marker
    markets: [],

    init: function () {
        //var start_lat = 50.486831;
        //var start_lng = 30.610115;
        osmap.map = L.map('content').setView([osmap.start_lat, osmap.start_lng], 11);
        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpandmbXliNDBjZWd2M2x6bDk3c2ZtOTkifQ._QA7i5Mpkd_m30IGElHziw', {
            maxZoom: 28,
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
                '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
                'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.streets'
        }).addTo(osmap.map);

        osmap.fillRegions();        
        osmap.fillMarkets();       

        setEvent();
    },

    Radius: function () {
        var lradius = $("#lradius").val();
        if (lradius > 0) {
            return lradius;
        } else return 5;
    },

    // завантаження на клієнт та запонення селекта  <-- завантажуєтся при виборі області
    fillDistrict: function (region_id) {
        // знимаемо відображення на карті
        if (osmap.districts.length > 0) {
            $.each(osmap.districts, function (key, item) {
                if (item.visible) {

                    item.visible = false;
                }
            });
        }

        osmap.districts = [];

        $.ajax({
            url: 'api/district/getdistricts/' + region_id,
            type: 'get',
            success: function (districts) {
                // var cmbdistricts = $("#cmbdistricts");                
                // завантажуємо для кожного району список його координат
                $.each(districts, function (key, item) {
                    var district = {
                        district_id             : item.district_id,
                        potamus_district_id     : item.potamus_district_id,
                        region_id               : item.region_id,
                        name                    : item.name,
                        lat                     : item.lat,
                        lng                     : item.lng,
                        LeafLetPolygon          : null,
                        visible                 : false,
                        is_region_center        : item.is_region_center
                    };

                    osmap.getDistrictCoordinates(district);                    
                });
            }
        });
    },

    drawDistrict: function (district_id) {

        $.each(osmap.districts, function (key, item) {
            if (item.visible) {
                osmap.map.removeLayer(item.LeafLetPolygon);
                item.visible = false;
            }
        });

        $.each(osmap.districts, function (key, item) {
            if (item.district_id == district_id) {
                if (!item.visible) {
                    var position = new L.LatLng(item.lat, item.lng);
                    osmap.map.setView(position, 11);
                    osmap.map.addLayer(item.LeafLetPolygon);
                    item.visible = true;
                    if ($("#show_title").attr("checked") == "checked") {
                        var label = L.marker(item.LeafLetPolygon.getBounds().getCenter(), {
                            icon: L.divIcon({
                                className: 'district-label',
                                html: item.name,
                                iconSize: [100, 40]
                            })
                        }).addTo(osmap.map);
                    }
                }
            }
        });
    },

    drawDistrictAll: function () {
        $.each(osmap.districts, function (key, item) {
            if (item.visible) {
                osmap.map.removeLayer(item.LeafLetPolygon);
                item.visible = false;
            }
        });

        $.each(osmap.districts, function (key, item) {
            if (item.is_region_center) {
                var position = new L.LatLng(item.lat, item.lng);
                osmap.map.setView(position, 9);
            }

            osmap.map.addLayer(item.LeafLetPolygon);
            item.visible = true;

            
            if ($("#show_title").attr("checked") == "checked") {

                var label = L.marker(item.LeafLetPolygon.getBounds().getCenter(), {
                    icon: L.divIcon({
                        className: 'district-label',
                        html: item.name,
                        iconSize: [100, 40]
                    })
                }).addTo(osmap.map);
            }
        });
    },

    clearDistrictAll: function () {
        $.each(osmap.districts, function (key, item) {
            if (item.visible) {
                osmap.map.removeLayer(item.LeafLetPolygon);
                item.visible = false;
            }
        });

        var position = new L.LatLng(osmap.start_lat, osmap.start_lng);
        osmap.map.setView(position, 11);
    },

    getDistrictCoordinates: function (district) {        
        $.ajax({
            url: 'api/districtcoordinates/getdistrictcoordinates/' + district.district_id,
            type: 'get',
            success: function (district_coodrdinates) {
                var coordinates = [];
                $.each(district_coodrdinates, function (key, item) {
                    var point = new L.LatLng(item.lat, item.lng);
                    coordinates.push(point);
                });
                district.LeafLetPolygon = new L.Polygon(coordinates, {
                    interactive: false
                });
                district.LeafLetPolygon.bindPopup(district.name);
                osmap.districts.push(district);

                osmap.fillCmbDistrict();
            }
        });
    },

    fillCmbDistrict: function () {
        var cmbdistricts = $("#cmbdistricts");
        cmbdistricts.html('');
        $.each(osmap.districts, function (key, item) {
            cmbdistricts.prepend("<option value='" + item.district_id + "' >" + item.name + "</option>");
        });
        cmbdistricts.prepend("<option value='all' selected='selected'></option>");
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
        osmap.drawDistrictAll();
    },

    fillMarkets: function () {
        $.ajax({
            url: 'api/market/',
            type: 'get',
            success: function (markets) {
                $.each(markets, function (key, item) {

                    var market_location = new L.LatLng(item.lat, item.lng);
                    var icon = L.icon({
                        iconUrl: 'img/Epicentrk.svg.png',
                        iconSize: [40, 40]
                    });
                    var marker = new L.Marker(
                        market_location,
                        {
                            icon: icon
                        });

                    var market = {
                        marker: marker,
                        label: item.label,
                        name_short: item.name_short,
                        market_google_coordinates_id: item.market_google_coordinates_id,
                        address: item.address,
                        lat: item.lat,
                        lng: item.lng
                    }

                    osmap.markets.push(market);

                });
                osmap.fillMarketControls();
            }
        });
    },

    fillMarketControls: function () {
        var cmbmarket = $("#cmbmarket");
        $.each(osmap.markets, function (key, item) {
            cmbmarket.prepend("<option value='" + item.market_google_coordinates_id
                + "'>" + item.label + ' - ' + item.name_short + "</option>");
        });
        cmbmarket.prepend("<option value='all' selected='selected'></option>");

        osmap.drawMarkets();
    },

    drawMarkets: function () {
        if ($("#showmarket").attr("checked") == "checked") {
            $.each(osmap.markets, function (key, item) {
                item.marker.bindPopup(item.address).addTo(osmap.map);
            });
        } else {
            $.each(osmap.markets, function (key, item) {                
                osmap.map.removeLayer(item.marker);
            });
        }
    },

    showMarket: function (market_id) {
        $.ajax({
            url: 'api/market/' + market_id,
            type: 'get',
            success: function (market) {
                var latlng = new L.LatLng(market.lat, market.lng);
                osmap.map.setView(latlng, 11);

                if (osmap.current_circle != undefined) {
                    osmap.current_circle.removeFrom(osmap.map);
                }

                var radius = osmap.Radius();

                //if (market.name_short == 'Хмельницький' || market.name_short == 'Чернігів') {
                osmap.current_circle = L.circle([market.lat, market.lng], {
                    radius: radius * 1000,
                    fill: false
                });
                osmap.current_circle.addTo(osmap.map);
                //}
            }
        });
    },
    // з адресними точками працюємо тільки через БД
    drawCustomerPoints: function () {
        $.each(osmap.customer_points, function (key, item) {
            osmap.map.addLayer(item);
        });
    },

    clearCustomerPoints: function () {
        $.each(osmap.customer_points, function (key, item) {
            osmap.map.removeLayer(item);
        });

        osmap.customer_points = [];
    },

    // Загрузка данных на клиент
    fillMarketCustomerPoints: function () {
        var market_id = $("#cmbmarket").val();
        var visit = $("#cmbvisit").val();
        var distance = $("#lenkm").val();

        $.ajax({
            url: 'api/cardpoint/getpointsbymarketid/' + market_id,
            type: 'get',
            data: {
                visit: visit,
                distance: distance
            },
            success: function (points) {

                osmap.clearCustomerPoints();

                $.each(points, function (key, item) {
                    var point = new L.circleMarker(
                        new L.LatLng(item.lat, item.lng),
                        {
                            radius: 1,
                            interactive: false,
                            color: '#' + osmap.color_market_points
                        });
                    osmap.customer_points.push(point);
                });

                osmap.drawCustomerPoints();
            }
        });
    },
};
