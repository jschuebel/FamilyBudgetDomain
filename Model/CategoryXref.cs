using System;

namespace FamilyBudget.Domain.Model
{
   //Create Table if not exists CategoryXrefs(ProductID int, CategoryID int)
    public class CategoryXref
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
    }
}