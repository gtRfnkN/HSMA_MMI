using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Maps.MapControl.WPF;

namespace SurfaceApplication1.Data
{
    public class InitMockData
    {
        public List<Filter> Filters { get; set; }
        public List<Categorie> Categories { get; set; }
        public List<Attraction> Attractions { get; set; }

        private readonly String[] _filterNames = { "Shoppen", "Restaurants", "Nightlife", "Sehenswürdigkeiten" };
        private readonly String[] _filterColors = { "Gelb", "Grün", "Blau", "Rot" };

        private readonly String[] _shoppingCategorieNames = { "Kleidung", "Schmuck", "Schuhe", "Soveniers" };
        private readonly String[] _restaurantCategorieNames = { "Italienisch", "Französisch", "Griechisch", "Asiatisch", "Bürgerlich" };
        private readonly String[] _nightlifeCategorieNames = { "Bars", "Clubs", "Diskos", "Andere" };
        private readonly String[] _sehenswuerdigkeitenCategorieNames = { "Musen", "Wahrzeichen", "Parks", "Andere" };

        private void InitFilter()
        {
            this.Filters = _filterNames.Select((t, counter) => new Filter { Name = t, Color = _filterColors[counter] }).ToList();
        }

        private void InitCategories(List<Filter> filters)
        {
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
            this.Categories = categroies;
        }

        private void InitAttractions()
        {
            String[] attrationNames =
            {
                "Wasserturm", "Fernsehturm", "Tech-Museum", "Planetarium",
                "Carl-Benz Stadion","Star", "Vapiano", "Burger King", "Starbucks", "Mannheimer Schloss", "Rheinterassen",
                "Hauptbahnhof", "Eisstadion Mannheim", "Alter Meßplatz", "Natzional Theather", "SAP Arena",
                "Galeria Kaufhof", "Cineplex", "Subway"
            };

            var geoCoordHashtable =
        new Dictionary<string, Location>
            {
                {"Wasserturm", new Location(49.484076,8.475525,0.0)},
                {"Fernsehturm", new Location(49.486991,8.491993,0.0)},
                {"Tech Museum", new Location(49.476465,8.496863,0.0)},
                {"Planetarium", new Location(49.477330,8.492872,0.0)},
                {"Carl-Benz Stadion", new Location(49.479456,8.502636,0.0)},
                {"Star", new Location(49.486585,8.465822,0.0)},
                {"Vapiano", new Location(49.485106,8.475653,0.0)},
                {"Burger King", new Location(49.485037,8.473592,0.0)},
                {"Starbucks", new Location(49.484034,8.473700,0.0)},
                {"Mannheimer Schloss", new Location(49.483582,8.462260,0.0)},
                {"Rheinterassen", new Location(49.479254,8.461713,0.0)},
                {"Hauptbahnhof", new Location(49.479886,8.469992,0.0)},
                {"Eisstadion Mannheim", new Location(49.485462,8.457977,0.0)},
                {"Alter Meßplatz", new Location(49.496267,8.472735,0.0)},
                {"Natzional Theather", new Location(49.488370,8.477753,0.0)},
                {"SAP Arena", new Location(49.464134,8.517818,0.0)},
                {"Galeria Kaufhof", new Location(49.487844,8.466678,0.0)},
                {"Cineplex", new Location(49.483477,8.471519,0.0)},
                {"Subway", new Location(49.486639,8.465338,0.0)}
            };

             this.Attractions = new List<Attraction>();

            //TODO: Init Mock data
            foreach (var attractionName in geoCoordHashtable.Keys)
            {
                var attraction = new Attraction { Titel = attractionName, Location = geoCoordHashtable[attractionName] };
                this.Attractions.Add(attraction);
            }
        }

        public void Init()
        {
            InitFilter();
            InitCategories(this.Filters);
            InitAttractions();
        }

    }
}
