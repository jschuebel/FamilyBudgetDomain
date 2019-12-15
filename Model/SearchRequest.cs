using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace FamilyBudget.Domain.Model
{
 //  [ModelBinder(typeof(SearchBinder))]
    public class SearchRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public FilterObjectWrapper FilterObjectWrapper { get; set; }
        public ICollection<SortObject> SortObjects { get; set; }



      public string GetSorting()
        {
            var expression = "";

            foreach (var sortObject in SortObjects)
            {
                expression += sortObject.Field + " " + sortObject.Direction + ", ";
            }

            if (expression.Length < 2)
                return "true";

            expression = expression.Substring(0, expression.Length - 2);

            return expression;
        }


        public string GetFiltering<T>()
        {
            var finalExpression = "";

            foreach (var filterObject in FilterObjectWrapper.FilterObjects)
            {
                if (finalExpression.Length > 0)
                    finalExpression += " " + FilterObjectWrapper.LogicToken + " ";


                if (filterObject.IsConjugate)
                {
                    var expression1 = GetExpression<T>(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    var expression2 = GetExpression<T>(filterObject.Field2, filterObject.Operator2, filterObject.Value2);
                    var combined = string.Format("({0} {1} {2})", expression1, FilterObjectWrapper.LogicToken, expression2);
                    finalExpression += combined;
                }
                else
                {
                    var expression = GetExpression<T>(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    finalExpression += expression;
                }
            }

            if (finalExpression.Length == 0)
                return "true";

            return finalExpression;
        }

        private static string GetExpression<T>(string field, string op, string param)
        {
            string caseMod = "";
            var p = typeof(T).GetProperty(field);
            if (p==null)
                throw new Exception($"GetExpression:field=({field}) did not exist.");
            var dataType = (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ?
                p.PropertyType.GetGenericArguments()[0].Name.ToLower() : p.PropertyType.Name.ToLower(); ;



            if (dataType == "string")
            {
                param = @"""" + param.ToLower() + @"""";
                caseMod = ".ToLower()";
            }

            if (dataType == "datetime")
            {
                var i = param.IndexOf("GMT");
                if (i > 0)
                {
                    param = param.Remove(i);
                }
                var date = DateTime.Parse(param, new CultureInfo("en-US"));

                var str = string.Format("DateTime({0}, {1}, {2})", date.Year, date.Month, date.Day);
                param = str;
            }

            string exStr;

            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0}{2} == {1}", field, param, caseMod);
                    break;

                case "neq":
                    exStr = string.Format("{0}{2} != {1}", field, param, caseMod);
                    break;

                case "contains":
                    exStr = string.Format("{0}!=null && {0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "startswith":
                    exStr = string.Format("{0}{2}.StartsWith({1})", field, param, caseMod);
                    break;

                case "endswith":
                    exStr = string.Format("{0}{2}.EndsWith({1})", field, param, caseMod);
                    break;
                case "gte":
                    exStr = string.Format("{0}{2} >= {1}", field, param, caseMod);
                    break;
                case "gt":
                    exStr = string.Format("{0}{2} > {1}", field, param, caseMod);
                    break;
                case "lte":
                    exStr = string.Format("{0}{2} <= {1}", field, param, caseMod);
                    break;
                case "lt":
                    exStr = string.Format("{0}{2} < {1}", field, param, caseMod);
                    break;
                default:
                    exStr = "";
                    throw new Exception($"GetExpression:op=({op}) has no handler.");
                    break;
            }

            return exStr;
        }

    }
}