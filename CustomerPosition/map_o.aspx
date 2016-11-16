<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map_o.aspx.cs" Inherits="CustomerPosition.map" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Карти</title>
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.0.1/dist/leaflet.css" />
    <link rel="stylesheet" href="css/map.css" />

    <%--    <script src="Scripts/jquery-1.8.2.intellisense.js"></script>--%>
    <script src="Scripts/jquery-1.8.2.js"></script>
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery-ui-1.8.24.js"></script>
    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script src="Scripts/jquery.unobtrusive-ajax.js"></script>
    <script src="Scripts/jquery.unobtrusive-ajax.min.js"></script>
    <script src="Scripts/jquery.validate-vsdoc.js"></script>
    <script src="Scripts/jquery.validate.js"></script>
    <script src="Scripts/jquery.validate.min.js"></script>
    <script src="Scripts/jquery.validate.unobtrusive.js"></script>
    <script src="Scripts/jquery.validate.unobtrusive.min.js"></script>
    <script src="Scripts/knockout-2.2.0.debug.js"></script>
    <script src="Scripts/knockout-2.2.0.js"></script>
    <script src="Scripts/modernizr-2.6.2.js"></script>

    <script src="https://unpkg.com/leaflet@1.0.1/dist/leaflet.js"></script>
    <script src="js/map_o.js"></script>
    <script src="js/events.js"></script>
    <script src="Scripts/colorpicker/jscolor.js"></script>

</head>
<body>
    <div id="conteiner">
        <div id="head">
            <h1>Розміщення</h1>
        </div>
        <div id="menu">
            <div>
                <div class="with-all-label">
                    Відображення адміністративного поділу
                </div>

                <div class="left-label">
                    <label>Область:</label>
                </div>
                <div>
                    <select id="cmbregion" style="width: 170px;"></select>
                </div>
                <div class="left-label">
                    <label>Район:</label>
                </div>
                <div>
                    <select id="cmbdistricts" style="width: 170px;"></select>
                </div>

                <div class="left-label"></div>
                <div class="checkbox-no-select">
                    <input type="checkbox" name="showall" id="showall_region" value="0" disabled="disabled" />Показати всі
                </div>
                <div class="left-label"></div>
                <div class="middle-text">
                    <input type="checkbox" name="show_title" id="show_title" />Показати підписи
                </div>
            </div>
            <div class="line"></div>

            <div>
                <div class="with-all-label">
                    Статистика по магазину
                </div>

                <div class="with-all-label-caption">Маркет видачі карти:</div>
                <div>
                    <select id="cmbmarket" style="width: 100%;"></select>
                </div>

                <div class="left-label">
                    <div class="middle-text">
                        <label>Радіус (км):</label>
                    </div>
                </div>

                <input type="text" id="lradius" style="width: 100px;" />

                <div>
                    <div class="left-label-free" style="width:87px;">
                        <div class="middle-text">Колір точок:</div>
                    </div>                    
                    <button id="btncolor_market_points"
                        class="jscolor {valueElement:null, onFineChange:'setcolor_market_points(this)',value:'FF6699'}"
                        style="width: 50px; height: 20px;">
                    </button>
                    <script>
                        function setcolor_market_points(color) {
                            osmap.color_market_points = color;
                        }
                    </script>
                </div>

                <div class="left-label"></div>
                <div class="middle-text">
                    <input type="checkbox" name="show_market" id="showmarket" value="0" checked="checked" />Показати всі магазини
                </div>

                <div class="left-label"></div>
                <div>
                    <button id="btnshow_market_point">Показати на карті</button>
                </div>

                <div class="line"></div>
            </div>

            <div>

                <div class="with-all-label">
                    По анкетним данним покупців
                </div>

                <div class="with-all-label-caption">Маркет візиту карти:</div>
                <div>
                    <select id="cmbmarketvisited" style="width: 100%;"></select>
                </div>


                <div class="left-label">
                    <div class="middle-text">
                        <label>Відстань (км):</label>
                    </div>
                </div>

                <input type="text" id="lenkm" style="width: 45px;" />

                <div>
                    <div class="left-label-free" style="width:87px;">
                        <div class="middle-text">Колір точок:</div>
                    </div>                    
                    <button id="btncolor_visit_points"
                        class="jscolor {valueElement:null,value:'8140A1'}"
                        style="width: 50px; height: 20px;">
                    </button>
                </div>
                

                <div class="left-label">
                    <div class="middle-text">
                        <label>Візити:</label>
                    </div>
                </div>
                <div>
                    <select id="cmbvisit" style="width: 104px;">
                        <option value="-1"></option>
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        <option value="6">6</option>
                        <option value="7">7</option>
                        <option value="8">8</option>
                        <option value="9">9</option>
                        <option value="10">10</option>
                        <option value="0">більше десяти</option>
                    </select>
                </div>
                <div class="left-label"></div>
                <div>
                    <button id="btnshow_visit_points">Показати на карті</button>
                </div>
            </div>

            <div class="line"></div>
            <%--            <div>
                 <p><input type="checkbox" name="option1" value="a1" checked="checked"/>Відображати магазини</p>
            </div>--%>
        </div>
        <div id="content">
        </div>
        <div id="foot">
            foot
        </div>
    </div>
</body>
</html>
