using System;
using System.Collections.Generic;
using System.Linq;
using Elements.Serialization.glTF;
using Hypar.Revit;
using RevitServices.Persistence;

namespace HyparDynamo.Hypar
{
    public static class Wall
    {
        /// <summary>
        /// Convert a Revit wall to Elements.WallByProfile(s) for use in Hypar models.
        /// Sometimes a single wall in Revit needs to be converted to multiple Hypar walls.
        /// </summary>
        /// <param name="RevitWall">The walls to be exported.</param>
        /// <returns name="WallByProfile">The Hypar walls.</param>
        public static Elements.WallByProfile[] FromRevitWall(this Revit.Elements.Wall revitWall)
        {
            var r_Wall = (Autodesk.Revit.DB.Wall)revitWall.InternalElement;

            // wrapped exception catching to deliver more meaningful message in Dynamo
            try
            {
                return Create.WallsFromRevitWall(r_Wall, DocumentManager.Instance.CurrentDBDocument);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public static class Floor
    {
        /// <summary>
        /// Convert a Revit floor to Elements.Floor(s) for use in Hypar models.
        /// Sometimes a single floor in Revit needs to be converted to multiple Hypar floors.
        /// </summary>
        /// <param name="revitFloor">The floor to be exported.</param>
        /// <returns name="floor">The Hypar floors.</param>
        public static Elements.Floor[] FromRevitFloor(this Revit.Elements.Floor revitFloor)
        {
            var r_Floor = (Autodesk.Revit.DB.Floor)revitFloor.InternalElement;

            // wrapped exception catching to deliver more meaningful message in Dynamo
            try
            {
                return Create.FloorsFromRevitFloor(DocumentManager.Instance.CurrentDBDocument, r_Floor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public static class Column
    {
        public static string FromRevitColumn(this Revit.Elements.Element column)
        {
            throw new NotImplementedException("Conversion of Revit columns is not yet supported.");
        }
    }
    public static class Model
    {
        public static void WriteJson(string filePath, Elements.Model model)
        {
            try
            {
                var json = model.ToJson();
                System.IO.File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void WriteGlb(string filePath, Elements.Model model)
        {
            try
            {
                model.ToGlTF(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Elements.Model FromElements(IList<object> elements)
        {
            var model = new Elements.Model();
            var elems = elements.Cast<Elements.Element>().Where(e => e != null);

            model.AddElements(elems);

            return model;
        }
    }
}