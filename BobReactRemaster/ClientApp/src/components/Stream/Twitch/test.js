
var InfoBoxCollection = []; function GetMap1() {
    var map1 = new Microsoft.Maps.Map('#MapContainer1', { center: new Microsoft.Maps.Location(0, 0), zoom: 5 });
    var Icon = 'http://wilmsol.local/assets/map-marker.png'; var locs = [];
    var Location_Marker_1 = new Microsoft.Maps.Location(48.293284, 7.813045);
    var pushpin1 = new Microsoft.Maps.Pushpin(Location_Marker_1, { icon: 'http://wilmsol.local/assets/map-marker.png' });
    var Location_InfoBox_1 = new Microsoft.Maps.Location(48.293284, 7.813045);
    var infobox1 = new Microsoft.Maps.Infobox(
        Location_InfoBox_1, {
        title: 'Hier ist der Titel',
        htmlContent: '<div class="infoBox typography">    <div class="infoBox_header">Hier ist der Titel<a class="infobox-close" href="javascript:closeInfoBox(1)">x</a></div>    <span>test</span></div><style>    .infoBox{        padding:0 20px 20px 20px;        background:white;        border:1px solid black;        border-radius:5px;    }</style>',
        visible: false
    }
    );
    InfoBoxCollection.push(infobox1);
    infobox1.setMap(map1);
    Microsoft.Maps.Events.addHandler(pushpin1, 'click', () => {
        infobox1.setOptions({ visible: true });
    });
    map1.entities.push(pushpin1);
    locs.push(new Microsoft.Maps.Location(48.293284, 7.813045));
    var Location_Marker_2 = new Microsoft.Maps.Location(48.296472, 7.818122);
    var pushpin2 = new Microsoft.Maps.Pushpin(Location_Marker_2, { icon: 'http://wilmsol.local/assets/map-marker.png' });
    map1.entities.push(pushpin2);
    locs.push(new Microsoft.Maps.Location(48.296472, 7.818122));
    map1.setView({
        bounds: Microsoft.Maps.LocationRect.fromLocations(locs),
        padding: 50
    });
}
function closeInfoBox(i) {
    InfoBoxCollection[i].setOptions({ visible: false });
}