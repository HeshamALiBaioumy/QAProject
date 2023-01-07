using QA.Entities.Business_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.Session_Entities
{
    public class MapPointsMethods
    {
        public enum mapSelectionType { NA, Line, polyline, Rectangle, circle, pushpin, Polygon };

        public string getProjectShape(List<Ent_MapPoint> mapPoints)
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

        public string getTurfCoordinates(List<Ent_MapPoint> mapPoints)
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
    }
}