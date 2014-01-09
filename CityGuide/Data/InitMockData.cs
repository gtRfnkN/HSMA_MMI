using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using CityGuide.ViewElements;
using Microsoft.Maps.MapControl.WPF;

namespace CityGuide.Data
{
    public class MockData
    {
        #region Singelten
        public static MockData MockDataInstance { get; private set; }

        public static MockData Init(Canvas drawCanvas, Canvas interactCanvas)
        {
            if (MockDataInstance == null)
            {
                var data = new MockData();
                data.InitFilter(drawCanvas, interactCanvas);
                data.InitCategories(data.Filters);
                data.InitAttractions();
                MockDataInstance = data;
            }
            return MockDataInstance;
        }

        public static MockData Init()
        {
            return MockDataInstance;
        }

        private MockData() { }
        #endregion

        #region Fields
        public List<Filter> Filters { get; set; }
        public List<Categorie> Categories { get; set; }
        public List<Attraction> Attractions { get; set; }
        public Dictionary<long, TagCircle> TagViewItems { get; set; }

        private readonly String[] _filterNames =
        {
            "Shoppen", 
            "Restaurants",
            "Nightlife", 
            "Sehenswürdigkeiten"
        };

        private readonly Dictionary<long, Color> _filterColors = new Dictionary<long, Color>
                {
                    {0x00,  Color.FromArgb(90, 255, 227, 159)},
                    {0x01, Color.FromArgb(90, 22, 134, 109)},
                    {0x02,  Color.FromArgb(90, 16, 143, 151)},
                    {0x03,Color.FromArgb(90, 255, 139, 107)}
                };

        private readonly String[] _shoppingCategorieNames = { "Kleidung", "Schmuck", "Schuhe", "Soveniers" };
        private readonly String[] _restaurantCategorieNames = { "Italienisch", "Französisch", "Griechisch", "Asiatisch", "Bürgerlich", "Fast Food" };
        private readonly String[] _nightlifeCategorieNames = { "Bars", "Clubs", "Diskos", "Andere" };
        private readonly String[] _sehenswuerdigkeitenCategorieNames = { "Musen", "Wahrzeichen", "Parks", "Andere" };
        #endregion

        #region Init Methods
        private void InitFilter(Canvas drawCanvas, Canvas interactCanvas)
        {
            this.Filters = _filterNames.Select((t, counter) => new Filter { Name = t, Color = _filterColors[counter], Radius = 200, TagID = _filterColors.Keys.ToArray()[counter]+1}).ToList();

            this.TagViewItems = new Dictionary<long, TagCircle>();
            foreach (var filter in Filters)
            {
                var tagViewItem = new TagCircle(filter)
                {
                    TagID = filter.TagID,
                    DrawCanvas = drawCanvas,
                    InteractCanvas = interactCanvas
                };

                this.TagViewItems.Add(filter.TagID, tagViewItem);
            }
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
                "Wasserturm", "Luisenpark", "Skyline", "Technoseum", "Planetarium",
                "Carl-Benz Stadion","Stars", "Vapiano", "Burger King", "Starbucks", "Barockschloss Mannheim", "Rheinterassen",
                "Hauptbahnhof", "Eisstadion Mannheim", "Alter Meßplatz", "Nationaltheater", "Reiss-Engelhorn-Museum", "SAP Arena",
                "Galeria Kaufhof", "Cineplex", "Subway", "Schillerplatz","Marktplatz","Citybeach", 
            };

            var shoppingFilter = this.Filters.First(f => f.Name.Equals(_filterNames[0]));
            var restaurantFilter = this.Filters.First(f => f.Name.Equals(_filterNames[1]));
            var nightlifeFilter = this.Filters.First(f => f.Name.Equals(_filterNames[2]));
            var sehenswürdigkeitenFilter = this.Filters.First(f => f.Name.Equals(_filterNames[3]));

            this.Attractions = new List<Attraction>
            {
                new Attraction
                {
                    Titel = "Skyline",
                    DefaultDurationInMinutes = 90,
                    Location = new Location(49.486991, 8.491993, 0.0),
                    Filter = restaurantFilter,
                    Background = new SolidColorBrush(restaurantFilter.Color),
                    IsSpezialSunrise = true,
                    IsSpezialSunset = true,
                    Interest = 6,
                    Address = "Hans-Reschke-Ufer 2, 68165 Mannheim",
                    OpeningHours = "10.00 bis 24.00 Uhr\nKüchenzeiten: 11.30 - 14.00 und 18.00 - 22.00 Uhr",
                    Information =
                        "Tel.: +49 621 419290 Fax: +49 621 4192911 E-Mail: info@skyline-ma.de Internet: http://www.skyline-ma.de",
                    Teaser =
                        "Über dem Aussichtsgeschoss liegt in 125 m Höhe das Drehrestaurant Skyline, welches sich mit seinen 150 Sitzplätzen einmal in der Stunde um die eigene Achse dreht.\nMaximale Personenanzahl: 150 Personen\nAbgegrenzter Raucherbereich: Nein\nBewirtung im Freien möglich: Nein"
                },
                new Attraction
                {
                    Titel = "Technoseum",
                    DefaultDurationInMinutes = 120,
                    Location = new Location(49.476465, 8.496863, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    Interest = 8,
                    Address = "Museumsstraße 1,68165 Mannheim",
                    OpeningHours = "täglich 9.00 - 17.00 Uhr, (am 24. und 31. Dezember geschlossen)",
                    Information = "Tel.: +49 621 42989 Fax: +49 621 4298754 E-Mail: info@technoseum.de",
                    Teaser =
                        "Zu einer spannenden Zeitreise durch die Industrialisierung des deutschen Südwestens lädt das TECHNOSEUM ein.\nMehr als 16 Stationen des \"arbeitenden Museums\" machen den technisch-sozialen Wandel der letzten zweieinhalb Jahrhunderte hautnah erlebbar. Zum Anfassen, Experimentieren und Staunen zeigen die ausgestellten Werkstätten, Maschinen und Gerätschaften, wie technische Neuerungen die Arbeits- und Lebensbedingungen des Menschen veränder(te)n.\nKein Wunder, dass dieses technikgeschichtliche Museum gerade in Mannheim angesiedelt ist. Schließlich stammen viele herausragende Erfindungen und Innovationen der Technik aus der Region."
                },
                new Attraction
                {
                    Titel = "Planetarium",
                    DefaultDurationInMinutes = 90,
                    Location = new Location(49.477330, 8.492872, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    Interest = 8,
                    Address = "Wilhelm-Varnholt-Allee 1 (Europaplatz), 68165 Mannheim",
                    OpeningHours =
                        "Tageskasse öffnet jeweils eine Stunde vor Beginn der jeweiligen Vorstellung.\nTelefonische Kartenreserviereung:\nDi / Mi / Do / Fr 8.30 - 12.30 und 14.00 - 16.00 Uhr\nMi zusätzlich 17.00 - 19.00 Uhr\nSa / So / Feiertage 12.30 - 16.30 Uhr",
                    Information = "Tel.: +49 621 415692 Internet: http://www.planetarium-mannheim.de",
                    Teaser =
                        "Im Zweiten Weltkrieg fast völlig zerstört und erst 50 Jahre später unweit des alten Standortes auf dem Europaplatz neu errichtet. Heute erwarten den \"Sternengucker\" ein brillanter Himmelsanblick und modernste Multimedia-Sternenshows. \nGroße und kleine Besucher erfahren Wissenswertes über Sterne, Nebel und Galaxien. Spazieren auf der Milchstraße, die Geburtsstätten neuer Sterne kennen lernen oder die Magellan'schen Wolken betrachten - dies alles ermöglicht die innovative Glasfaseroptik von Carl Zeiss Jena. \nEinen Besuch bei den Gestirnen sollten Sie in Mannheim nicht versäumen!"
                },
                new Attraction
                {
                    Titel = "Stars",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.486585, 8.465822, 0.0),
                    Filter = nightlifeFilter,
                    Background = new SolidColorBrush(nightlifeFilter.Color),
                    IsSpezialSunrise = true,
                    IsSpezialSunset = true,
                    Interest = 4,
                    OpeningHours =
                        "Sonntag bis Donnerstag: 14:00 bis 1:00 Uhr\nFreitag & Samstag: 14:00 bis 3:00 Uhr\nvor Feiertagen: 14:00 bis 3:00 Uhr",
                    Address = "Stadthaus N1, 68161 Mannheim",
                    Teaser =
                        "Seit 1994 sind wir “die Cocktailmacher” hoch über den Dächern der Stadt!\nGenießen Sie bei uns neben klassischen Cocktails wie Pina Colada oder Caipirinha auch zahllose Eigenkreationen, die Sie ausschließlich bei uns finden werden. \nÜber 180 Cocktails mit und ohne Alkohol erwarten Sie bei uns, zudem natürlich auch erfrischende Biere, kühle Softdrinks und belebende Kaffeespezialitäten.",
                    Information = "Tel. 0621 / 21 600 E-Mail: info@turmcafe-stars.de"
                },
                new Attraction
                {
                    Titel = "Vapiano",
                    DefaultDurationInMinutes = 90,
                    Location = new Location(49.485106, 8.475653, 0.0),
                    Filter = restaurantFilter,
                    Background = new SolidColorBrush(restaurantFilter.Color),
                    Interest = 4,
                    Address = "Friedrichsplatz 1, 68165 Mannheim",
                    OpeningHours = "Mo - So 10.00 - 1.00 Küche 11.00 - 24.00",
                    Information =
                        "Tel.: +49 621 1259777 Fax: +49 621 1259779 E-Mail: mannheim1@vapiano.de Internet: http://www.vapiano.de",
                    Teaser =
                        "Vapiano, das heißt mediterranes Flair, frische Pasta und hausgemachte Pizza aus dem Steinofen, knackige Salate, leckere Dolci und duftende Kräuter in einem hellen, großzügigen Ambiente mit  einem stilvollen Loungebereich.\nMaximale Personenanzahl: 232 Sitzplätze plus 80 Sitzplätze auf der Terrasse\nAbgegrenzter Raucherbereich: Nein\nBewirtung im Freien möglich: Ja\n"
                },
                new Attraction
                {
                    Titel = "Barockschloss Mannheim",
                    DefaultDurationInMinutes = 120,
                    Location = new Location(49.483582, 8.462260, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    Interest = 10,
                    OpeningHours = "Dienstag bis Sonntag und an Feiertagen 10.00 – 17.00 Uhr letzter Einlass 16.30 Uhr",
                    Teaser =
                        "Als Kurfürst Carl Philipp im Jahr 1720 von Heidelberg nach Mannheim zog wurde die Stadt zur Residenz und zum strahlenden Mittelpunkt für Wissenschaften, Kunst und Kultur. Im selben Jahr legte er den Grundstein zum Neubau dieser imponierenden Schlossanlage. 450 Meter lang ist die Stadtfront, etwa sechs Hektar Fläche sind hier umbaut, und einst gab es ca. 1.000 Räume und Säle. Schloss Mannheim gehört zu den größten Barockanlagen Deutschlands.\nDas Mannheimer Schloss öffnet sich zur Stadt hin mit dem Ehrenhof. An den mittig gelegenen Haupttrakt schließen sich der Ost- und der Westflügel an. Hinter den Gebäuden liegt zum Rhein hin der weitläufige Schlossgarten. Im Ostflügel richtete Carl Theodor eine Bibliothek ein, die zu den bedeutendsten ihrer Zeit zählte. Seine Hofkapelle wurde zum Wegbereiter der europäischen Klassik und ging als „Mannheimer Schule“ in die Musikgeschichte ein. Heute ist das Schloss teilweise Sitz der Universität, die somit als eine der schönsten Deutschlands gelten darf.",
                    Address = "Bismarckstraße, 68161 Mannheim",
                    Information =
                        "http://www.schloss-mannheim.de/ Telefon:+49 621 - 2922891\nEintrittspreise\nBesichtigung mit Audioguide\nErwachsene 6,00 €\nErmäßigte 3,00 €\nFamilien 15,50 €\nGruppen pro Pers. 5,40 €\n(mit Audioguide)\n\nBesichtigung mit ca. einstündiger Schlossführung\nErwachsene 8,00 €\nErmäßigte 4,00 €\nFamilien 20,00 €\nGruppen bis 20 Personen 144,00 €\nGruppen pro Pers 7,20 €\n\nÖffentliche Führungen\ntägl. 11.00 und 15.00 Uhr\nSamstag 11.00, 13.00 und 15.00 Uhr\nSonntag und Feiertage\nstündl. 11.00 – 16.00 Uhr\n"
                },
                new Attraction
                {
                    Titel = "Hauptbahnhof",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.479886, 8.469992, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    Address = "Willy-Brandt-Platz, 68161 Mannheim",
                    Information = "Internet: http://www.bahn.de",
                    Teaser =
                        "In den Jahren 1871 bis 1876 als gemeinsames Projekt der Badischen und der Preußisch-Hessischen Bahn entstanden, ist der Mannheimer Hauptbahnhof heute einer der deutschlandweit bedeutendsten ICE-Knotenpunkte.\nTäglich erreichen den Hauptbahnhof über 500 Züge, davon fast 200 Fernzüge. Rund 70.000 Reisende passieren Tag für Tag den Bahnhof.\nEinst im ganzen Land bekannt für sein sehr vornehmes, im Renaissancestil erbautes Empfangsgebäude, erstrahlt der Mannheimer Hauptbahnhof heute - nach unzähligen Um- und Wiederaufbauten - als hochmodernes Reisezentrum mit traditionellem Charme."
                },
                new Attraction
                {
                    Titel = "Nationaltheater",
                    DefaultDurationInMinutes = 150,
                    Location = new Location(49.488370, 8.477753, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    Interest = 8,
                    Address = "Am Goetheplatz, 68161 Mannheim",
                    OpeningHours = "Abendverkauf an allen Vorstellungstagen von 18.00 - 20.00 Uhr",
                    Information = "Tel.: +49 621 1680310 Internet: http://www.nationaltheater-mannheim.de",
                    Teaser =
                        "Die \"Schillerbühne\", benannt nach der legendären Uraufführung von Schillers \"Die Räuber\" anno 1782, zählt zu den anerkanntesten Theatern Deutschlands.\nDie große Operntradition, mitreißendes Schauspiel, klassisches wie auch modernes Ballett sowie einzigartige Inszenierungen zeitgenössischer Werke machen das Vier-Sparten-Haus zu einem unvergleichlichen kulturellen Highlight der gesamten Region."
                },
                new Attraction
                {
                    Titel = "Reiss-Engelhorn-Museum",
                    DefaultDurationInMinutes = 150,
                    Location = new Location(49.488772, 8.461928, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    Interest = 8,
                    Address = "D 5, 68159 Mannheim",
                    OpeningHours = "11:00 - 18:00",
                    Information = "Tel.: +49 621 293 3150 Internet: http://www.rem-mannheim.de",
                    Teaser =
                        "Die Reiss-Engelhorn-Museen Mannheim (rem genannt) haben sich in den letzten Jahren zu einem international agierenden Museumskomplex entwickelt. Mit der Verbindung von vier Ausstellungshäusern und zahlreichen Forschungsstellen und Instituten sind sie an der Nahtstelle von Natur- und Geisteswissenschaften, Technik und Vermittlung tätig."
                },
                new Attraction
                {
                    Titel = "SAP Arena",
                    DefaultDurationInMinutes = 120,
                    Location = new Location(49.464134, 8.517818, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    Interest = 7,
                    Address = "Xaver-Fuhr-Str.150, 68163 Mannheim",
                    Information = "Tel.: +49 621 18190333 Internet: http://www.saparena.de",
                    Teaser =
                        "Spektakuläres vielfältigster Art erwartet Sie in der SAP Arena. Weithin sichtbares Kennzeichen der größten und modernsten Sport- und Veranstaltungshalle im Südwesten ist das, von Mannheimern gerne UFO genannte, Aluminiumdach.\nDie gigantische, 15.000 Besucher fassende Multifunktionshalle beheimatet den Eishockey Rekordmeister, die Mannheimer Adler, sowie den Handball Bundesligisten, die Rhein-Neckar-Löwen.\nDie Arena ist außerdem Veranstaltungsort für zahlreiche Großereignisse aus dem Musik-, Kultur- und Showbereich.",
                    OpeningHours = "Zu Veranstallungen"
                },
                new Attraction
                {
                    Titel = "Wasserturm",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.484076, 8.475525, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    IsSpezialSunrise = true,
                    IsSpezialSunset = true,
                    Interest = 10,
                    OpeningHours = "Immer, Im Winter sind die Brunnen ohne Wasser.",
                    Address = "Friedrichsplatz, 68161 Mannheim",
                    Teaser =
                        "Der Wasserturm ist das Wahrzeichen Mannheims. Erbaut 1889 von dem Stuttgarter Architekten Gustav Halmhuber, der auch am Bau des Berliner Reichstags mitwirkte. Als Herzstück der zentralen Trinkwasserversorgung war er bis zum Jahr 2000 in Betrieb. Der Wasserturm ist 60 Meter hoch, hat einen Durchmesser von 19 Metern und fasst 2000 Kubikmeter Wasser. Das Dach des Turmes bekrönt eine Statue der Amphitrite, der Gattin des Meeresgottes Poseidon. Auch der weitere Bildschmuck und die Figuren am kleinen und am großen Becken nehmen diese Thematik auf: Wasser ist Leben und – speziell für Mannheim – die Grundlage für Schifffahrt und Handel.\nDer Wasserturm erhebt sich an der höchsten Stelle des Friedrichsplatzes. Mit seinem Ensemble aus Turm, Garten, Wasserbecken und der angrenzenden Festhalle sowie der Kunsthalle gilt der Friedrichsplatz als eine der schönsten Jugendstilanlagen Deutschlands. Bei Anbruch der Dunkelheit sorgen die hell erleuchteten Wasserspiele für eine ganz besondere Atmosphäre."
                },
                new Attraction
                {
                    Titel = "Luisenpark",
                    DefaultDurationInMinutes = 120,
                    Location = new Location(49.478816, 8.496567, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    IsSpezialSunrise = true,
                    IsSpezialSunset = true,
                    Interest = 10,
                    OpeningHours = "Immer von 09:00. Kassenschlus 21:00",
                    Address = "Theodor-Heuss-Anlage 2, 68165 Mannheim",
                    Teaser =
                        "Sehen, Hören, Riechen, Fühlen – die Farben der Natur, das Singen der Vögel, den Duft der Blumen oder die Rinde uralter Bäume: Ein Besuch im Luisenpark aktiviert die Sinne, ist Erholungs- und Frischekur für Körper, Geist und Seele zugleich, ein Ort purer Entspannung und Entschleunigung. An 365 Tagen im Jahr genießen 1,2 Millionen Besucher aus der ganzen Republik das Angebot des größten Mannheimer Parks, der bekannt ist als eine der schönsten Parkanlagen Europas."
                },
                new Attraction
                {
                    Titel = "Burger King",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.485037, 8.473592, 0.0),
                    Filter = restaurantFilter,
                    Interest = 2,
                    Background = new SolidColorBrush(restaurantFilter.Color),
                    Categorie = Categories.FirstOrDefault(c => c.Name.Equals("Fast Food"))
                },
                new Attraction
                {
                    Titel = "Starbucks",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.484034, 8.473700, 0.0),
                    Filter = restaurantFilter,
                    Interest = 2,
                    Background = new SolidColorBrush(restaurantFilter.Color),
                    Categorie = Categories.FirstOrDefault(c => c.Name.Equals("Fast Food"))
                },
                new Attraction
                {
                    Titel = "Rheinterassen",
                    Location = new Location(49.479254, 8.461713, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 6,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                },
                new Attraction
                {
                    Titel = "Eisstadion Mannheim",
                    DefaultDurationInMinutes = 60,
                    Location = new Location(49.485462, 8.457977, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                },
                new Attraction
                {
                    Titel = "Alter Meßplatz",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.496267, 8.472735, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                },
                new Attraction
                {
                    Titel = "Galeria Kaufhof",
                    DefaultDurationInMinutes = 50,
                    Location = new Location(49.487844, 8.466678, 0.0),
                    Filter = shoppingFilter,
                    Interest = 4,
                    Background = new SolidColorBrush(shoppingFilter.Color)
                },
                new Attraction
                {
                    Titel = "Cineplex",
                    DefaultDurationInMinutes = 150,
                    Location = new Location(49.483477, 8.471519, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 4,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                },
                new Attraction
                {
                    Titel = "Subway",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.486639, 8.465338, 0.0),
                    Filter = restaurantFilter,
                    Interest = 2,
                    Background = new SolidColorBrush(restaurantFilter.Color),
                    Categorie = Categories.FirstOrDefault(c => c.Name.Equals("Fast Food"))
                },
                new Attraction
                {
                    Titel = "Carl-Benz Stadion",
                    DefaultDurationInMinutes = 75,
                    Location = new Location(49.479456, 8.502636, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                },
                new Attraction
                {
                    Titel = "Schillerplatz",
                    DefaultDurationInMinutes = 45,
                    Location = new Location(49.486379, 8.462053, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                },
                new Attraction
                {
                    Titel = "Marktplatz",
                    DefaultDurationInMinutes = 30,
                    Location = new Location(49.489706, 8.4674, 0.0),
                    Filter = shoppingFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(shoppingFilter.Color),
                },
                new Attraction
                {
                    Titel = "Citybeach",
                    DefaultDurationInMinutes = 75,
                    Location = new Location(49.492939,8.472303, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                    IsSpezialSunset = true,
                    OpeningHours = "18:00 - open end",
                    Address = "Friedrichsring 48‎, 68161 Mannheim",
                    Teaser =
                        "Ein sehr ruhig gelegenes Cafe mit einem schönen Ausblick auf den Neckar"
                },
                new Attraction
                {
                    Titel = "Carl-Benz Stadion",
                    DefaultDurationInMinutes = 75,
                    Location = new Location(49.479456, 8.502636, 0.0),
                    Filter = sehenswürdigkeitenFilter,
                    Interest = 5,
                    Background = new SolidColorBrush(sehenswürdigkeitenFilter.Color),
                }
            };

            var categorieFastFood = Categories.FirstOrDefault(c => c.Name.Equals("Fast Food"));
            categorieFastFood.Attractions.Add(Attractions.FirstOrDefault(a => a.Titel.Equals("Subway")));
            categorieFastFood.Attractions.Add(Attractions.FirstOrDefault(a => a.Titel.Equals("Starbucks")));
            categorieFastFood.Attractions.Add(Attractions.FirstOrDefault(a => a.Titel.Equals("Burger King")));
        }

        public void ResetMockingData(Canvas drawCanvas, Canvas interactCanvas)
        {
            var data = new MockData();
            data.InitFilter(drawCanvas, interactCanvas);
            data.InitCategories(data.Filters);
            data.InitAttractions();
            MockDataInstance = data;
        }
        
        #endregion
    }
}
