using System;

namespace FamilyBudget.Domain.Model
{
    //Create Table if not exists Purchases(PurchaseID INTEGER primary key autoincrement,ProductID int, Count int, PurchaseDate datetime, CostOverride int)
    public class Purchase
    {
        public int PurchaseID { get; set; }
        public int ProductID { get; set; }
        public int Count { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double? CostOverride { get; set; }
    }
}