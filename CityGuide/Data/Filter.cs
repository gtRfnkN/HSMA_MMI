﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using CityGuide.Extensions;
using Microsoft.Maps.MapControl.WPF;

namespace CityGuide.Data
{
    public class Filter
    {
        #region Fields
        public String Name { get; set; }
        public long TagID { get; set; }
        
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
            
            //TODO When Position Data are avalible, filter the attractions
            return attractions;
        }
        #endregion
    }
}
