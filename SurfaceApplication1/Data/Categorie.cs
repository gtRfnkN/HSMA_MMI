using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstTest.Data
{
    public class Categorie
    {
        #region Fields
        public String Name;

        public Filter Filter { get; set; }

        private LinkedList<Attraction> _attractions = new LinkedList<Attraction>();
        public LinkedList<Attraction> Attractions
        {
            get { return this._attractions; }
            set { this._attractions = value ?? new LinkedList<Attraction>(); }
        }
        #endregion

        #region Methods
        #endregion
    }
}
