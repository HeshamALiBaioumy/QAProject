using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QA.Entities.Session_Entities;

namespace QA.Entities.Business_Entities
{
    public class Ent_MapPoint
    {
        public int mapID { get; set; }

        public int pointID { get; set; }

        public string pointLatitude { get; set; }

        public string pointLongitude { get; set; }

        public override string ToString()
        {
            return String.Concat(pointLatitude + "," + pointLongitude);
            //return String.Concat("mapID: ", mapID, "~ pointID: ", pointID, "~ pointLatitude: ", pointLatitude, "~ pointLongitude: ", pointLongitude);
        }

        public string ToGeometryString()
        {
            return String.Concat(pointLatitude + "," + pointLongitude);
        }

        public static List<Ent_MapPoint> convertToMapPoints(object[] coordinates)
        {
            try
            {
                List<Ent_MapPoint> mapPoints = new List<Ent_MapPoint>();

                foreach (object[] vals in coordinates)
                {
                    mapPoints.Add(new Ent_MapPoint()
                    {
                        pointLatitude = vals.GetValue(0).ToString(),
                        pointLongitude = vals.GetValue(1).ToString()
                    });
                }

                return mapPoints;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Ent_MapPoint> convertToPushpinMapPoint(object[] coordinates)
        {
            try
            {
                List<Ent_MapPoint> mapPoints = new List<Ent_MapPoint>();
                mapPoints.Add(new Ent_MapPoint()
                {
                    pointLatitude = coordinates[0].ToString(),
                    pointLongitude = coordinates[1].ToString()
                }) ;

                return mapPoints;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string prepareMapGeoJSON(List<Ent_MapPoint> lstMapPoints)
        {
            try
            {
                object[] lstPoints = new object[lstMapPoints.Count];
                for (int i = 0; i < lstMapPoints.Count; i++)
                {
                    lstPoints[i] = new object[] { decimal.Parse(lstMapPoints[i].pointLatitude)
                        , decimal.Parse(lstMapPoints[i].pointLongitude) };
                }

                List<object> lstCoordinates = new List<object>();
                lstCoordinates.Add(lstPoints);

                List<Feature> lstFeatures = new List<Feature>();
                lstFeatures.Add(new Feature()
                {
                    type = "Feature",
                    properties = new Properties(),
                    geometry = new Geometry()
                    {
                        type = "Polygon",
                        coordinates = lstCoordinates
                    }
                });

                MapGeoJSON mapJSON = new MapGeoJSON()
                {
                    type = "FeatureCollection",
                    features = lstFeatures
                };

                string geoJSON = JsonConvert.SerializeObject(mapJSON);

                return geoJSON;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}