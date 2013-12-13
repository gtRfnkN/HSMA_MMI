using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using Microsoft.Maps.MapControl.WPF;
using SurfaceApplication1.Extensions;


namespace SurfaceApplication1.Data
{
    public class Filter
    {
        #region Fields
        public String Name;
        
        public Color Color { get; set; }
        public int Radius { get; set; }
        public Location LocationCenter { get; set; }
        public Location LocationHandel { get; set; }

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
           
            Location diff = LocationCenter.Subtract(LocationHandel);

            Location topLocation = LocationHandel;
            Location bottomLocation = LocationCenter.Subtract(diff);
            Location rightLocation = LocationCenter.Subtract(new Location{Longitude = LocationCenter.Longitude, Latitude = diff.Latitude});
            Location leftLocation = LocationCenter.Subtract(new Location { Longitude = diff.Longitude, Latitude = LocationCenter.Latitude });

            //TODO When Position Data are avalible, filter the attractions
            return attractions;
        }
        #endregion
    }
}
