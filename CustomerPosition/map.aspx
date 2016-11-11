<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map.aspx.cs" Inherits="CustomerPosition.map" %>

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
    <script src="js/map.js"></script>
</head>
<body onload="initialize();">
    <%--<div id="mapid" style="width: 1000px; height: 800px;"></div>--%>

    <%--    <form id="frm_map" runat="server">
        <div id="map_canvas" style="width:100%; height:100%"></div>
    </form>--%>
    <div id="conteiner">
        <div id="head">
            <h1>Розміщення</h1>
        </div>
        <div id="menu">
            <div>
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
                <div>
                    <button id="btn_showdistrict" onclick="btnshowdistrict();">Показати на карті</button>
                </div>
            </div>
            <div class="line"></div>
            <div>
                <div class="left-label">Маркет</div>
                <div>
                    <select id="cmbmarket" style="width: 170px;"></select>
                </div>

                <div class="left-label">
                    <label>Відстань(км):</label>
                </div>
                <input type="text" id="lenkm" style="width: 100px;" />

                <div class="left-label">
                    <label>Візити:</label>
                </div>
                <div>
                    <select id="visit_count" style="width: 104px;">
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
                    <button id="btn_showpoints" onclick="btnshowpoints();">Показати на карті</button>
                </div>
            </div>
            <div class="line"></div>
            <div>
                 <p><input type="checkbox" name="option1" value="a1" checked="checked"/>Відображати магазини</p>
            </div>
        </div>
        <div id="content">
        </div>
        <div id="foot">
            foot
        </div>
    </div>
</body>
</html>
