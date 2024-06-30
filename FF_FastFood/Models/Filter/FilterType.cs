using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models.Filter
{
    public class FilterType
    {
        FF_FastFoodEntities1 db=new FF_FastFoodEntities1();
        public int? type { get; set; }
    
        public String search { get; set; }
        public FilterType(int? type,  string search)
        {
            this.type = type;
            
            this.search = search;
        }

        public List<Food> filterListFoodSearch(List<Food> foods)
        {
            if (search != null)
            {
                return foods.Where(x => x.name.Contains(search)).ToList();
            }
            return foods;
        }
        public List<Food> filterListFoodType( List<Food> foods ) {
            if (type.HasValue && type!=0)
            {
                return foods.Where(x => x.category_id.Equals(type.Value)).ToList();
            }
            return foods;
        }
       
    }
}