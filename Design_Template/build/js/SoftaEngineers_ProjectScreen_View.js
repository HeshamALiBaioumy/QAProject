var centerLatVar = $('#MapCenterLatitude').val();
var centerLngVar = $('#MapCenterLongitude').val();
var zoomVar = $('#MapZoomLevel').val();
var mapJEOJSON = $('#MapJEOJSON').val();

var map = prepareLeafletMap('map', centerLatVar, centerLngVar, zoomVar);
var featureGroup = L.featureGroup().addTo(map);

// Draw Already Exist Map
var mapJEOJSONParse = JSON.parse(mapJEOJSON);
L.geoJson(mapJEOJSONParse, { onEachFeature: onEachFeature });
////Take advantage of the onEachFeature callback to initialize drawnItems
function onEachFeature(feature, layer) {
    featureGroup.addLayer(layer);
}