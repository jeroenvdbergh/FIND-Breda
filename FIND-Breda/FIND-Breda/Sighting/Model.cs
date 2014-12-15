using FIND_Breda.GPSHandler;
using FIND_Breda.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIND_Breda.Sighting
{
    class Model
    {
        List<Route> _routes = new List<Route>();
        List<iSighting> _sightings = new List<iSighting>();
        iLanguage _language = new iLanguage();
        View _view = new View();
        public Model()
        {
        }

        public void updateScreen() { }
        public void setDistanceForSighting(iSighting sighting) { }

        public void changeLanguage(iLanguage language) { }
    }
}
