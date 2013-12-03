using System;
using System.Collections.Generic;


namespace SurfaceApplication1.Data
{
    public class Filter
    {
        #region Fields
        public String Name;
        
        public String Color { get; set; }
        public int Radius { get; set; }
        public String Position { get; set; } //TODO: Change when Cords oder Position Data are avalible

        private List<Attraction> _attractions = new List<Attraction>();
        public List<Attraction> Attractions
        {
            get { return this._attractions; }
            set { this._attractions = value ?? new List<Attraction>(); }
        }

        private List<Categorie> _categories = new List<Categorie>();
        public List<Categorie> Categories
        {
            get { return this._categories; }
            set { this._categories = value ?? new List<Categorie>(); }
        }
        #endregion

        #region Methods

        public List<Attraction> GetAttrationsInFilterRange()
        {
            var result = new List<Attraction>();
            if (this._categories.Exists(c => c.IsSelected == true))
            {
                //Only the Selected Categories Attractions
                foreach (var category in this._categories)
                {
                    if (category.IsSelected)
                    {
                        result.AddRange(category.Attractions);
                    }
                }

                result = GetAttractionsInRange(result);
            }
            else
            {
                result = GetAttractionsInRange(this._attractions);
            }

            return result;
        }

        private List<Attraction> GetAttractionsInRange(List<Attraction> attractions)
        {
            //TODO When Position Data are avalible, filter the attractions
            return attractions;
        }
        #endregion
    }
}
