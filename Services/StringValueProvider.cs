using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using FamilyBudget.Domain.Model;

namespace FamilyBudget.Domain.Services
{
    public static class StringArrayNull {
        public static string First(this string [] vals) {
            if (vals.Length==0) return null;
            return vals[0];
        }
    }
    public class StringValueProvider
    {
        protected string [] stringvals {get; set;}
        public StringValueProvider(string value){ 
            if (value.StartsWith('?'))
                value=value.Substring(1);
            stringvals = value.Split('&');
        }
        public string [] ValueFor(string findstring) {
            List<string> retval = new List<string>();
            foreach(var sv in stringvals) {
                if (sv.IndexOf("=")==-1) continue;
                var keyonly = sv.Substring(0,sv.IndexOf("="));
                var valonly = sv.Substring(sv.IndexOf("=")+1);
                if (keyonly==findstring) {
                    retval.Add(valonly);
                }
            }
            return retval.ToArray();
        }
    }
}