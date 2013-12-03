using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstTest.Data
{
    public class Filter : Categorie
    {
        #region Fields
        public String Color { get; set; }
        public int Radius { get; set; }

        private List<Categorie> _categories = new List<Categorie>();
        public List<Categorie> Categories
        {
            get { return this._categories; }
            set { this._categories = value ?? new List<Categorie>(); }
        }
        #endregion

        #region Methods
        #endregion
    }
}
