using System;

namespace FamilyBudget.Domain.Model
{
    //Create Table if not exists Products(ProductID INTEGER primary key autoincrement, Title text, Count int, Cost int)
    public class Product
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public double? Cost  { get; set; }
         public int Count { get; set; }
    }
}