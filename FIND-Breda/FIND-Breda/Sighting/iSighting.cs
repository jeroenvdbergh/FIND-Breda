using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FIND_Breda.Sighting
{
    abstract class iSighting
    {
        protected String _name;
        protected String _shortInfo;
        protected String _adress;
        protected String _fullInfo;
        protected double _distanceFromUser;
        protected double _mapLocation;
        protected Image _image;

      //  public Image iSightings(String _name, String _shortInfo, String _adress, String _fullInfo, double _distanceFromUser, double _mapLocation, Image _image);
    }
}
