using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.DBHelper
{
    public class OrderByOperat
    {
        private string Orderby { get; set; }
        private string OrderbyNew { get; set; }
         
        public OrderByOperat(string orderby)
        {
            this.Orderby = orderby.ToUpper();
            Resolve();
        }

        private void Resolve()
        {
            string[] byBefore = this.Orderby.Split(',');
            foreach (var item in byBefore)
            {
                if (item.Contains("DESC"))
                {
                    OrderbyNew += item.Replace("DESC", "ASC");
                }
                else
                {
                    if (item.Contains("ASC"))
                    {
                        OrderbyNew += item.Replace("ASC", "DESC");
                    }
                    else
                    {
                        OrderbyNew += item + " DESC";
                    }
                }
                OrderbyNew += ",";
            }
            OrderbyNew = OrderbyNew.Remove(OrderbyNew.Length - 1, 1);
        }

        public string GetReverseOrder()
        {
            return this.OrderbyNew;
        }
    }
}
