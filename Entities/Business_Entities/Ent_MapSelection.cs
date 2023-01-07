using System;
using System.Collections.Generic;

namespace QA.Entities.Business_Entities
{
    public class Ent_MapSelection
    {
        public int mapID { get; set; }

        public bool displayOnUserLocation { get; set; }

        public int zoomLevel { get; set; }

        public string centerLatitude { get; set; }

        public string centerLongitude { get; set; }

        public enum mapSelectionType { NA, Line, polyline, Rectangle, circle, pushpin, Polygon };

        public mapSelectionType projectMapSelectionType { get; set; }

        // for Circle map Selection type only
        public double circleDiameter { get; set; }

        public List<Ent_MapPoint> mapPoints { get; set; }

        public string getProjectShape
        {
            get
            {
                string strResult = "";

                if (mapPoints.Count == 1)
                {
                    foreach (Ent_MapPoint mapPoint in mapPoints)
                    {
                        strResult += "[" + mapPoint.pointLongitude + ", " + mapPoint.pointLatitude + "] ";
                    }
                }
                else if (mapPoints.Count > 0)
                {
                    strResult = "[";

                    foreach (Ent_MapPoint mapPoint in mapPoints)
                    {
                        strResult += "[" + mapPoint.pointLongitude + ", " + mapPoint.pointLatitude + "], ";
                    }

                    strResult = strResult.Substring(0, strResult.Length - 2);
                    strResult += "]";
                }

                return strResult;
            }
        }

        public string getTurfCoordinates
        {
            get
            {
                string strResult = "";

                if (mapPoints.Count == 1)
                {
                    foreach (Ent_MapPoint mapPoint in mapPoints)
                    {
                        strResult += "[" + mapPoint.pointLatitude + ", " + mapPoint.pointLongitude + "] ";
                    }
                }
                else if (mapPoints.Count > 0)
                {
                    strResult = "[[";

                    foreach (Ent_MapPoint mapPoint in mapPoints)
                    {
                        strResult += "[" + mapPoint.pointLatitude + ", " + mapPoint.pointLongitude + "], ";
                    }

                    strResult = strResult.Substring(0, strResult.Length - 2);
                    strResult += "]]";
                }

                return strResult;
            }
        }

        public static mapSelectionType getDrawType(string drawType)
        {
            mapSelectionType userSelection = mapSelectionType.NA;

            switch (drawType.ToLower())
            {
                case "polygon":
                    userSelection = mapSelectionType.Polygon;
                    break;
                case "polyline":
                case "linestring":
                    userSelection = mapSelectionType.polyline;
                    break;
                case "pushpin":
                    userSelection = mapSelectionType.pushpin;
                    break;
            }

            return userSelection;
        }

        public string exportJEOJSON { get; set; }

        public string turfCoordinates { get; set; }

        public override string ToString()
        {
            return String.Concat("mapID: ", mapID, "~ zoomLevel: ", zoomLevel, "~ centerLatitude: ", centerLatitude
                , "~ centerLongitude: ", centerLongitude, "~ projectMapSelectionType: ", projectMapSelectionType
                , "~ circleDiameter: ", circleDiameter, "~ mapPoints: ", mapPoints.ToString());
        }
    }
}