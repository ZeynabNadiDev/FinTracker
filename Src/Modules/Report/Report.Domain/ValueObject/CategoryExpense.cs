using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.ValueObject
{
    public class CategoryExpense
    {
        public int CategoryId { get; private set; }
        public string CategoryName { get; private set; }
        public decimal TotalAmount { get; private set; }

        // Private constructor for EF Core
        private CategoryExpense() { }

        public CategoryExpense(int categoryId, string categoryName, decimal totalAmount)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            TotalAmount = totalAmount;
        }
    }
}
