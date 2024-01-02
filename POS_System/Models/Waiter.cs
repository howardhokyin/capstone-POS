using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Models
{
    public class Waiter : User
    {
        private double totalSales;
        private double totalTip;

        public Waiter() { }

        public Waiter(double totalSales, double totalTip)
        {
            this.totalSales = totalSales;
            this.totalTip = totalTip;
        }
        public double TotalSales { get { return totalSales; } }
        public double TotalTip { get { return totalTip; } }

    }
}
