using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceApplication1.Data
{
    public class Categorie
    {
        #region Fields
        public String Name;
        public Boolean IsSelected { get; set; }

        public Filter Filter { get; set; }

        private List<Attraction> _attractions = new List<Attraction>();
        public List<Attraction> Attractions
        {
            get { return this._attractions; }
            set { this._attractions = value ?? new List<Attraction>(); }
        }
        #endregion

        #region Methods
        #endregion
    }
}
