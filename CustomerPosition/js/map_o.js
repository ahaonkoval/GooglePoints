
$(document).ready(function () {    
    osmap.init();
});

osmap = {
    start_lat: 50.486831,
    start_lng: 30.610115,

    map: null,

    color_market_points: 'FF6699',

    current_market: null,

    current_circle_getcard: null,

    current_circle_visit: null,
    //L.circle
    customer_points: [],
    //
    customer_used_viber: [],
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
        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoiYWhhb24iLCJhIjoiY2o1M2thdzFvMDR1cjJ3czMwOWY0M2ZyeiJ9.zc2uUm_WuMy8E6PHCeeFDQ', {
            maxZoom: 28,
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
                '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
                'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.streets'
        }).addTo(osmap.map);

        osmap.fillRegions();        
        osmap.fillMarkets();       

        // Пока отключим
        //osmap.fillDictVisited();

        //osmap.fillCustomerPointsUsedViber();

        setEvent();
    },

    Radius: function () {
        var lradius = $("#lradius").val();
        if (lradius > 0) {
            return lradius;
        } else return 5;
    },

    Distance: function(){
        var lenkm = $("#lenkm").val();
        if (lenkm > 0) {
            return lenkm;
        } else return 5;
    },

    fillDictVisited: function () {
        $.ajax({
            url: 'api/cardpoint/getsegmentbyvisited/1',
            type: 'get',
            success: function (dict) {
                var cmbvisit = $("#cmbvisit");
                cmbvisit.html('');
                $.each(dict, function (key, item) {
                    cmbvisit.prepend("<option value='" + item.id + "' >" + item.name + "</option>");
                });
                cmbvisit.prepend("<option value='' selected='selected'></option>");
            }
        });
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

    //завантаження на клієнт та запонення селекта  <-- завантажуєтся при старті сторінки
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

    //перемальовуємо всі райони області
    drawRegions: function () {
        osmap.drawDistrictAll();
    },

    fillMarkets: function () {
        $.ajax({
            url: 'api/market/',
            type: 'get',
            success: function (markets) {
                $.each(markets, function (key, item) {

                    var market_location = new L.LatLng(item.Lat, item.Lng);
                    var icon = L.icon({
                        iconUrl: 'img/epicentrk_market.png',
                        //shadowUrl: 'img/marker-shadow.png',
                        iconSize: [55, 65]
                    });
                    var marker = new L.Marker(
                        market_location,
                        {
                            icon: icon
                        });

                    var market = {
                        marker: marker,
                        label: item.Label,
                        name_short: item.NameShort,
                        market_coordinates_id: item.MarketId,
                        address: item.Address,
                        lat: item.Lat,
                        lng: item.Lng
                    }

                    osmap.markets.push(market);

                });
                osmap.fillMarketControls();
            }
        });
    },

    fillMarketControls: function () {
        var cmbmarket_getcard = $("#cmbmarket_getcard");
        var cmbmarket_visitcard = $("#cmbmarket_visitcard");

        $.each(osmap.markets, function (key, item) {
            cmbmarket_getcard.prepend("<option value='" + item.market_coordinates_id
                + "'>" + item.label + ' - ' + item.name_short + "</option>");

            cmbmarket_visitcard.prepend("<option value='" + item.market_coordinates_id
                + "'>" + item.label + ' - ' + item.name_short + "</option>");
        });
        cmbmarket_getcard.prepend("<option value='all' selected='selected'></option>");
        cmbmarket_visitcard.prepend("<option value='all' selected='selected'></option>");

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
                var latlng = new L.LatLng(market.Lat, market.Lng);
                osmap.map.setView(latlng, 11);

                if (osmap.current_circle_getcard != undefined) {
                    osmap.current_circle_getcard.removeFrom(osmap.map);
                }

                var radius = osmap.Radius();
                osmap.current_circle_getcard = L.circle([market.Lat, market.Lng], {
                    radius: radius * 1000,
                    fill: false
                });
                osmap.current_circle_getcard.addTo(osmap.map);
            }
        });
    },

    showMarketVisited: function (market_id) {
        $.ajax({
            url: 'api/market/' + market_id,
            type: 'get',
            success: function (market) {
                var latlng = new L.LatLng(market.lat, market.lng);
                osmap.map.setView(latlng, 11);

                if (osmap.current_circle_visit != undefined) {
                    osmap.current_circle_visit.removeFrom(osmap.map);
                }

                var distanse = osmap.Distance();
                osmap.current_circle_visit = L.circle([market.lat, market.lng], {
                    radius: distanse * 1000,
                    fill: false,
                    color: '#885FC1'
                });
                osmap.current_circle_visit.addTo(osmap.map);
            }
        });
    },
    // з адресними точками працюємо тільки через БД
    drawMarkertCustomerPoints: function () {

        var markers = L.markerClusterGroup();

        $.each(osmap.customer_points, function (key, item) {
            //osmap.map.addLayer(item);
            markers.addLayer(item);
        });

        osmap.map.addLayer(markers);
    },

    clearMarketCustomerPoints: function () {
        $.each(osmap.customer_points, function (key, item) {
            osmap.map.removeLayer(item);
        });

        osmap.customer_points = [];
    },

    // Загрузка данных на клиент
    fillMarketCustomerPoints: function () {
        var market_id = $("#cmbmarket_getcard").val();
        var radius = $("#lradius").val();

        if (market_id == undefined) {
            modal.open({
                content: '<div>Не вибраний магазин видачі карти</div>'
            });
            return;
        }

        $.ajax({
            url: 'api/cardpoint/getpointsbymarketradius/' + market_id,
            type: 'get',
            data: {
                radius: radius
            },
            success: function (points) {
                osmap.clearMarketCustomerPoints();
                $.each(points, function (key, item) {
                    //var point = new L.circleMarker(
                    //    new L.LatLng(item.lat, item.lng),
                    //    {
                    //        radius: 1,
                    //        interactive: false,
                    //        color: '#' + osmap.color_market_points
                    //    });

                    var point = new L.marker(
                        new L.LatLng(item.lat, item.lng));

                    osmap.customer_points.push(point);
                });
                osmap.drawMarkertCustomerPoints();

                osmap.getMarketCustomerCountPoints();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //osmap.clearCustomerPoints();
            }
        });
    },

    getMarketCustomerCountPoints: function () {
        var market_id = $("#cmbmarket_getcard").val();
        var radius = $("#lradius").val();

        $.ajax({
            url: 'api/cardpoint/getcountpointsbymarketradius/' + market_id,
            type: 'get',
            data: {
                radius: radius
            },
            success: function (count) {
                modal.open({
                    content: '<div class="with-all-label">Кількість карт з адресою: '+ count + '</div>'
                });
            }
        });
    },

    fillCustomerPointsUsedViber: function () {
        $.ajax({
            url: 'api/cardpoint/getpointscustomerusedviber/1',
            type: 'get',
            success: function (points) {
                $.each(points, function (key, item) {

                    var myIcon = L.icon({
                        iconUrl: 'img/viber.png',
                        iconRetinaUrl: 'img/viber.png',
                        iconSize: [15, 15],
                    });
                    var point = new L.marker(
                        new L.LatLng(item.lat, item.lng),
                        {
                            clickable: false,
                            icon: myIcon
                        });

                    osmap.customer_used_viber.push(point);
                });
                osmap.drawCustomerPointsUsedViber();
            }
        });
    },

    drawCustomerPointsUsedViber: function () {
        $.each(osmap.customer_used_viber, function (key, item) {
            osmap.map.addLayer(item);
        });
    }
};
