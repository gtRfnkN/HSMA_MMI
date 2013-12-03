using System;
using System.Collections.Generic;
using System.Linq;

namespace SurfaceApplication1.Data
{
    public class InitMockData
    {
        private readonly String[] _filterNames = { "Shoppen", "Restaurants", "Nightlife", "Sehenswürdigkeiten" };
        private readonly String[] _filterColors = { "Gelb", "Grün", "Blau", "Rot" };

        private readonly String[] _shoppingCategorieNames = { "Kleidung", "Schmuck", "Schuhe", "Soveniers" };
        private readonly String[] _restaurantCategorieNames = { "Italienisch", "Französisch", "Griechisch", "Asiatisch", "Bürgerlich" };
        private readonly String[] _nightlifeCategorieNames = { "Bars", "Clubs", "Diskos", "Andere" };
        private readonly String[] _sehenswuerdigkeitenCategorieNames = { "Musen", "Wahrzeichen", "Parks", "Andere" };

        private List<Filter> InitFilter()
        {
            return _filterNames.Select((t, counter) => new Filter { Name = t, Color = _filterColors[counter] }).ToList();
        }

        private List<Categorie> InitCategories()
        {
            var filters = InitFilter();

            #region Shopping
            var shoppingFilter = filters.First(f => f.Name.Equals(_filterNames[0]));
            var shoppigCategories = _shoppingCategorieNames.Select(t => new Categorie { Name = t, Filter = shoppingFilter }).ToList();

            shoppingFilter.Categories = shoppigCategories;
            #endregion

            #region Restaurants
            var restaurantFilter = filters.First(f => f.Name.Equals(_filterNames[1]));
            var restaurantCategories = _restaurantCategorieNames.Select(t => new Categorie { Name = t, Filter = restaurantFilter }).ToList();

            restaurantFilter.Categories = restaurantCategories;
            #endregion

            #region Nightlife
            var nightlifeFilter = filters.First(f => f.Name.Equals(_filterNames[2]));
            var nightlifeCategories = _nightlifeCategorieNames.Select(t => new Categorie { Name = t, Filter = nightlifeFilter }).ToList();

            nightlifeFilter.Categories = nightlifeCategories;
            #endregion

            #region Sehensuerdigkeiten
            var sehenswürdigkeitenFilter = filters.First(f => f.Name.Equals(_filterNames[3]));
            var sehenswuerdigkeitenCategories = _sehenswuerdigkeitenCategorieNames.Select(t => new Categorie { Name = t, Filter = sehenswürdigkeitenFilter }).ToList();

            sehenswürdigkeitenFilter.Categories = sehenswuerdigkeitenCategories;
            #endregion

            var categroies = new List<Categorie>();
            categroies.AddRange(shoppigCategories);
            categroies.AddRange(restaurantCategories);
            categroies.AddRange(nightlifeCategories);
            categroies.AddRange(sehenswuerdigkeitenCategories);
            return categroies;
        }

        //TODO: Init Mock data
    }
}
