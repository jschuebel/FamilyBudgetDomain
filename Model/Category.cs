using System;

namespace FamilyBudget.Domain.Model
{
  //Create Table if not exists Categories(CategoryID INTEGER primary key autoincrement, Title text)
    public class Category
    {
        public int CategoryID { get; set; }
        public string Title { get; set; }
    }

}